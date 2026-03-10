using System;
using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



namespace DS7.Map
{
    /// <summary>
    /// Runtime + Editor map painting tool.
    /// Attach to a Manager GO in the MapEditor scene (or activate via GameManager.OpenMapEditor).
    ///
    /// Controls:
    ///   Left-click  → paint selected terrain onto hovered hex
    ///   Right-click → sample terrain from hovered hex (eye-dropper)
    ///   Middle-drag → pan camera
    ///   Scroll      → zoom
    /// </summary>
    public class MapEditor : MonoBehaviour
    {
        // ── Paint Mode ────────────────────────────────────────────────────────
        public enum BrushMode { Terrain, Nation, Facility, Erase }
        public enum BrushShape { Point, Line, Box }

        [Header("Mode")]
        public BrushMode brushMode = BrushMode.Terrain;
        public BrushShape brushShape = BrushShape.Point;

        [Header("Brush")]
        public DS7.Data.TerrainData selectedTerrain;
        public Nation      selectedNation = Nation.Neutral;
        public bool        setFacilityActive = true;
        [Range(1, 7)]
        [Tooltip("1 = single hex, 7 = megahex (centre + ring)")]
        public int brushRadius = 1;

        [Header("UI Integration")]
        public List<TerrainToggleMapping> terrainToggles;

        [Serializable]
        public struct TerrainToggleMapping
        {
            public Toggle toggle;
            public DS7.Data.TerrainData terrain;
        }

        [Header("Camera Controls")]
        public float panSpeed = 20f;
        public float rotationSpeed = 300f;

        [Header("References")]
        public HexGrid grid;
        public Camera targetCamera;
        public LayerMask hexLayer;

        // ── History for Undo ──────────────────────────────────────────────────
        private readonly Stack<MapEditAction> _undoStack = new();
        private const int MaxHistory = 200;

        // ── State ─────────────────────────────────────────────────────────────
        private HexCell _lastPainted;
        private HexCell _hoveredCell;
        private HexCell _dragStartCell;
        private readonly Dictionary<HexCell, GameObject> _activeHighlightObjects = new();

        // ── Map I/O ───────────────────────────────────────────────────────────
        [Header("Map Serialisation")]
        [Tooltip("File name written to Application.persistentDataPath")]
        public string mapFileName = "custom_map.json";

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            if (grid == null) grid = HexGrid.Instance;
            if (targetCamera == null) targetCamera = Camera.main;
        }

        // ── Update ────────────────────────────────────────────────────────────
        private void Update()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            HandleZoom();
            HandleCameraMovement();
            HandleCameraRotation();
            UpdateSelectedTerrainFromToggles();

            UpdateHoverAndHighlight();

            if (Input.GetMouseButtonDown(0))
            {
                _dragStartCell = _hoveredCell;
            }

            if (brushShape == BrushShape.Point)
            {
                if (Input.GetMouseButton(0)) TryPaint();
            }
            else
            {
                if (Input.GetMouseButtonUp(0)) TryPaint();
            }

            if (Input.GetKeyDown(KeyCode.Z) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)))
                Undo();
        }

        private void UpdateSelectedTerrainFromToggles()
        {
            if (terrainToggles == null) return;
            foreach (var mapping in terrainToggles)
            {
                if (mapping.toggle != null && mapping.toggle.isOn)
                {
                    selectedTerrain = mapping.terrain;
                    return;
                }
            }
        }

        private void UpdateHoverAndHighlight()
        {
            // Find hovered cell
            var ray = targetCamera.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = hexLayer.value == 0 ? ~0 : hexLayer;
            
            HexCell newHover = null;
            if (Physics.Raycast(ray, out var hit, 1000f, mask))
            {
                newHover = hit.collider.GetComponentInParent<HexCell>();
            }
            _hoveredCell = newHover;

            // Determine current target cells for highlighting
            HashSet<HexCell> targets = new();
            if (_hoveredCell != null)
            {
                List<HexCell> cells;
                if (Input.GetMouseButton(0) && (brushShape == BrushShape.Line || brushShape == BrushShape.Box) && _dragStartCell != null)
                {
                    cells = GetShapeCells(_dragStartCell, _hoveredCell);
                }
                else
                {
                    cells = BrushCells(_hoveredCell);
                }
                foreach (var c in cells) if (c != null) targets.Add(c);
            }

            // 1. Identify cells to add and remove
            List<HexCell> cellsToAdd = new();
            foreach (var t in targets) if (!_activeHighlightObjects.ContainsKey(t)) cellsToAdd.Add(t);

            List<HexCell> cellsToRemove = new();
            foreach (var cell in _activeHighlightObjects.Keys)
            {
                if (cell == null || !targets.Contains(cell)) cellsToRemove.Add(cell);
            }

            // 2. Handoff: Reuse objects from removed cells for added cells
            // This prevents SetActive(false) -> SetActive(true) which resets Animators.
            int handoffCount = Mathf.Min(cellsToAdd.Count, cellsToRemove.Count);
            for (int i = 0; i < handoffCount; i++)
            {
                var fromCell = cellsToRemove[i];
                var toCell   = cellsToAdd[i];
                var obj      = _activeHighlightObjects[fromCell];

                _activeHighlightObjects.Remove(fromCell);
                if (obj != null)
                {
                    obj.transform.position = toCell.transform.position + Vector3.up * toCell.Terrain.visualHeight;
                    _activeHighlightObjects[toCell] = obj;
                }
            }

            // 3. Cleanup remaining toRemove
            for (int i = handoffCount; i < cellsToRemove.Count; i++)
            {
                var cell = cellsToRemove[i];
                var obj  = _activeHighlightObjects[cell];
                if (obj != null) grid.ReturnHighlight(obj, HexCell.HighlightMode.Selected);
                _activeHighlightObjects.Remove(cell);
            }

            // 4. Instantiate remaining toAdd from pool
            for (int i = handoffCount; i < cellsToAdd.Count; i++)
            {
                var cell = cellsToAdd[i];
                var obj  = grid.GetHighlightFromPool(HexCell.HighlightMode.Selected);
                if (obj != null)
                {
                    obj.transform.position = cell.transform.position + Vector3.up * cell.Terrain.visualHeight;
                    _activeHighlightObjects[cell] = obj;
                }
            }
        }

        // ── Paint ────────────────────────────────────────────────────────────
        private void TryPaint()
        {
            if (_hoveredCell == null) return;

            // For Point brush, avoid re-painting the same cell every frame
            if (brushShape == BrushShape.Point && _hoveredCell == _lastPainted && brushRadius == 1) return;
            _lastPainted = _hoveredCell;

            // Collect brush hexes
            List<HexCell> cells;
            if ((brushShape == BrushShape.Line || brushShape == BrushShape.Box) && _dragStartCell != null)
            {
                cells = GetShapeCells(_dragStartCell, _hoveredCell);
            }
            else
            {
                cells = BrushCells(_hoveredCell);
            }

            foreach (var cell in cells)
                ApplyBrush(cell);
        }

        private List<HexCell> GetShapeCells(HexCell start, HexCell end)
        {
            var results = new List<HexCell>();
            if (start == null || end == null) return results;

            if (brushShape == BrushShape.Line)
            {
                var coords = HexCoordinates.GetLine(start.Coordinates, end.Coordinates);
                foreach (var c in coords)
                {
                    if (grid.TryGetCell(c, out var cell))
                    {
                        // Apply radius to each point on the line? 
                        // For now keep it simple: just the line itself.
                        results.AddRange(BrushCells(cell));
                    }
                }
            }
            else if (brushShape == BrushShape.Box)
            {
                var startOffset = start.Coordinates.ToOffsetCoords();
                var endOffset = end.Coordinates.ToOffsetCoords();

                int minX = Mathf.Min(startOffset.x, endOffset.x);
                int maxX = Mathf.Max(startOffset.x, endOffset.x);
                int minY = Mathf.Min(startOffset.y, endOffset.y);
                int maxY = Mathf.Max(startOffset.y, endOffset.y);

                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        var cell = grid.GetCell(x, y);
                        if (cell != null) results.Add(cell);
                    }
                }
            }

            // Deduplicate
            var unique = new HashSet<HexCell>(results);
            return new List<HexCell>(unique);
        }

        private List<HexCell> BrushCells(HexCell centre)
        {
            var result = new List<HexCell> { centre };
            if (brushRadius <= 1) return result;

            foreach (var neighborCoords in centre.Coordinates.AllNeighbors())
            {
                if (!grid.TryGetCell(neighborCoords, out var n)) continue;
                result.Add(n);
            }

            return result;
        }

        private void ApplyBrush(HexCell cell)
        {
            // Record for undo
            var action = new MapEditAction(cell, brushMode, selectedTerrain, selectedNation);
            PushUndo(action);

            switch (brushMode)
            {
                case BrushMode.Terrain:
                    if (selectedTerrain != null)
                        grid.ReplaceCell(cell, selectedTerrain);
                    break;

                case BrushMode.Nation:
                    cell.Owner = selectedNation;
                    cell.RefreshVisuals();
                    break;

                case BrushMode.Facility:
                    cell.SetFacilityActive(setFacilityActive);
                    break;

                case BrushMode.Erase:
                    // Reset to default terrain (first element in project or null)
                    var neutralizedCell = grid.ReplaceCell(cell, null);
                    if (neutralizedCell != null)
                    {
                        neutralizedCell.Owner = Nation.Neutral;
                        neutralizedCell.RefreshVisuals();
                    }
                    break;
            }
        }

        // ── Undo ──────────────────────────────────────────────────────────────
        private void PushUndo(MapEditAction action)
        {
            _undoStack.Push(action);
            if (_undoStack.Count > MaxHistory) { /* trim oldest – Stack doesn't support remove-bottom, use a deque in a real impl */ }
        }

        private void Undo()
        {
            if (_undoStack.Count == 0) return;
            var action = _undoStack.Pop();
            action.Revert();
        }

        // ── Camera Controls ───────────────────────────────────────────────────
        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) < 0.001f) return;
            
            if (targetCamera.orthographic)
            {
                targetCamera.orthographicSize = Mathf.Clamp(
                    targetCamera.orthographicSize - scroll * 5f, 2f, 80f);
            }
            else
            {
                targetCamera.transform.position += targetCamera.transform.forward * scroll * 15f;
            }
        }

        private Plane   _groundPlane = new Plane(Vector3.up, Vector3.zero);
        private Vector3 _panOriginWorld;
        private bool    _rotating;

        private void HandleCameraMovement()
        {
            Vector3 movement = Vector3.zero;

            // Get rotation only on the Y axis
            Quaternion yRotation = Quaternion.Euler(0, targetCamera.transform.eulerAngles.y, 0);
            
            Vector3 forward = yRotation * Vector3.forward;
            Vector3 right = yRotation * Vector3.right;

            if (Input.GetKey(KeyCode.W)) movement += forward;
            if (Input.GetKey(KeyCode.S)) movement -= forward;
            if (Input.GetKey(KeyCode.A)) movement -= right;
            if (Input.GetKey(KeyCode.D)) movement += right;

            if (movement != Vector3.zero)
            {
                targetCamera.transform.position += movement.normalized * panSpeed * Time.deltaTime;
                // Debug.Log($"Pan: {targetCamera.transform.position}");
            }
        }

        private void HandleCameraRotation()
        {
            if (Input.GetMouseButtonDown(2)) 
            { 
                Ray ray = new Ray(targetCamera.transform.position, targetCamera.transform.forward);
                if (_groundPlane.Raycast(ray, out float enter))
                {
                    _panOriginWorld = ray.GetPoint(enter);
                }
                else
                {
                    // Fallback if looking up/away from the ground
                    _panOriginWorld = new Vector3(targetCamera.transform.position.x, 0, targetCamera.transform.position.z);
                }
                _rotating = true;
            }
            if (Input.GetMouseButtonUp(2))   _rotating = false;

            if (!_rotating) return;
            
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            
            if (Mathf.Abs(mouseX) > 0.001f)
            {
                targetCamera.transform.RotateAround(_panOriginWorld, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
            }
            
            if (Mathf.Abs(mouseY) > 0.001f)
            {
                // Rotate vertically around the camera's local right axis relative to the pivot
                targetCamera.transform.RotateAround(_panOriginWorld, targetCamera.transform.right, -mouseY * rotationSpeed * Time.deltaTime);
            }
        }

        // ── Map Save / Load ───────────────────────────────────────────────────
        [ContextMenu("Save Map")]
        public void SaveMap()
        {
            var data = new MapSaveData(grid);
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            string path = System.IO.Path.Combine(Application.persistentDataPath, mapFileName);
            System.IO.File.WriteAllText(path, json);
            Debug.Log($"[MapEditor] Saved to {path}");
        }

        [ContextMenu("Load Map")]
        public void LoadMap()
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, mapFileName);
            if (!System.IO.File.Exists(path)) { Debug.LogWarning($"[MapEditor] No map file found at {path}"); return; }

            string json = System.IO.File.ReadAllText(path);
            var data = JsonUtility.FromJson<MapSaveData>(json);
            data.ApplyToGrid(grid);
            Debug.Log($"[MapEditor] Loaded {mapFileName}");
        }
    }

    // ── Undo Action ───────────────────────────────────────────────────────────
    [Serializable]
    internal class MapEditAction
    {
        private readonly DS7.Grid.HexCoordinates  _coords;
        private readonly MapEditor.BrushMode _mode;
        private readonly DS7.Data.TerrainData _prevTerrain;
        private readonly Nation          _prevNation;
        private readonly DS7.Data.TerrainData _newTerrain;
        private readonly Nation          _newNation;

        public MapEditAction(HexCell cell, MapEditor.BrushMode mode,
                             DS7.Data.TerrainData newTerrain, Nation newNation)
        {
            _coords      = cell.Coordinates;
            _mode        = mode;
            _prevTerrain = cell.Terrain;
            _prevNation  = cell.Owner;
            _newTerrain  = newTerrain;
            _newNation   = newNation;
        }

        public void Revert()
        {
            var grid = HexGrid.Instance;
            if (grid == null || !grid.TryGetCell(_coords, out var cell)) return;

            switch (_mode)
            {
                case MapEditor.BrushMode.Terrain: 
                    grid.ReplaceCell(cell, _prevTerrain); 
                    break;
                case MapEditor.BrushMode.Nation:  
                    cell.Owner = _prevNation; 
                    cell.RefreshVisuals(); 
                    break;
                case MapEditor.BrushMode.Facility:
                    // We didn't track previous facility state, this could be extended later
                    break;
                case MapEditor.BrushMode.Erase:
                    var revertedCell = grid.ReplaceCell(cell, _prevTerrain);
                    if (revertedCell != null)
                    {
                        revertedCell.Owner = _prevNation;
                        revertedCell.RefreshVisuals();
                    }
                    break;
            }
        }
    }

    // ── Map Serialisation ─────────────────────────────────────────────────────
    [Serializable]
    public class MapSaveData
    {
        public int width, height;
        public List<CellSaveData> cells = new();

        public MapSaveData() { }

        public MapSaveData(HexGrid grid)
        {
            width  = grid.width;
            height = grid.height;
            for (int col = 0; col < width; col++)
            for (int row = 0; row < height; row++)
            {
                var cell = grid.GetCell(col, row);
                if (cell == null) continue;
                cells.Add(new CellSaveData
                {
                    col           = col,
                    row           = row,
                    terrainName   = cell.Terrain?.name ?? string.Empty,
                    owner         = (int)cell.Owner,
                    facilityHealth = cell.facilityHealth
                });
            }
        }

        public void ApplyToGrid(HexGrid grid)
        {
            foreach (var cd in cells)
            {
                var cell = grid.GetCell(cd.col, cd.row);
                if (cell == null) continue;

                // Re-link terrain by name from Resources
                if (!string.IsNullOrEmpty(cd.terrainName))
                {
                    var td = Resources.Load<DS7.Data.TerrainData>($"TerrainData/{cd.terrainName}");
                    if (td != null) cell = grid.ReplaceCell(cell, td);
                }

                if (cell != null)
                {
                    cell.Owner          = (Nation)cd.owner;
                    cell.facilityHealth = cd.facilityHealth;
                    cell.RefreshVisuals();
                }
            }
        }
    }

    [Serializable]
    public class CellSaveData
    {
        public int    col, row;
        public string terrainName;
        public int    owner;
        public int    facilityHealth;
    }
}
