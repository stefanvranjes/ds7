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
        public enum BrushMode { Terrain, Nation, Facility, Erase, CopyPaste, Fill }
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
        public List<BrushModeToggleMapping> modeToggles;
        public List<BrushShapeToggleMapping> shapeToggles;

        [Serializable]
        public struct TerrainToggleMapping
        {
            public Toggle toggle;
            public DS7.Data.TerrainData terrain;
        }

        [Serializable]
        public struct BrushModeToggleMapping
        {
            public Toggle toggle;
            public BrushMode mode;
        }

        [Serializable]
        public struct BrushShapeToggleMapping
        {
            public Toggle toggle;
            public BrushShape shape;
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
        private Vector3 _dragStartPos;
        private readonly Dictionary<HexCell, GameObject> _activeHighlightObjects = new();
        private BrushMode _lastBrushMode;
        private BrushMode _lastPaintBrushMode = BrushMode.Terrain;

        // ── Clipboard ─────────────────────────────────────────────────────────
        [Serializable]
        public struct ClipboardCell
        {
            public Vector2Int offset; // Obsolete, keeping for compatibility if needed elsewhere
            public HexCoordinates axialOffset; // Relative to root cell
            public DS7.Data.TerrainData terrain;
            public Nation owner;
            public bool hasFacility;
            public int facilityHealth;
        }
        private readonly Dictionary<HexCoordinates, ClipboardCell> _clipboard = new();

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
            
            if (Input.GetMouseButtonDown(0))
            {
                _dragStartCell = _hoveredCell;
                _dragStartPos = Input.mousePosition;
            }

            UpdateBrushFromToggles();
            UpdateHoverAndHighlight();

            bool isShapeMode = brushShape != BrushShape.Point || brushMode == BrushMode.CopyPaste || brushMode == BrushMode.Fill;
            if (isShapeMode)
            {
                if (Input.GetMouseButtonUp(0)) TryPaint();
            }
            else
            {
                if (Input.GetMouseButton(0)) TryPaint();
            }

            if (Input.GetKeyDown(KeyCode.Z) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)))
                Undo();
        }

        private void UpdateBrushFromToggles()
        {
            if (terrainToggles != null)
            {
                foreach (var mapping in terrainToggles)
                {
                    if (mapping.toggle != null && mapping.toggle.isOn)
                    {
                        selectedTerrain = mapping.terrain;
                        break;
                    }
                }
            }

            if (modeToggles != null)
            {
                foreach (var mapping in modeToggles)
                {
                    if (mapping.toggle != null && mapping.toggle.isOn)
                    {
                        brushMode = mapping.mode;
                        if (brushMode != BrushMode.Fill && brushMode != BrushMode.CopyPaste)
                        {
                            _lastPaintBrushMode = brushMode;
                        }
                        break;
                    }
                }
            }

            if (shapeToggles != null)
            {
                foreach (var mapping in shapeToggles)
                {
                    if (mapping.toggle != null && mapping.toggle.isOn)
                    {
                        brushShape = mapping.shape;
                        break;
                    }
                }
            }

            if (brushMode != _lastBrushMode)
            {
                if (brushMode == BrushMode.CopyPaste)
                {
                    SetBrushShape(BrushShape.Box);
                }
                _lastBrushMode = brushMode;
            }
        }

        public void SetBrushShape(BrushShape shape)
        {
            brushShape = shape;
            if (shapeToggles == null) return;
            
            foreach (var mapping in shapeToggles)
            {
                if (mapping.toggle != null)
                {
                    mapping.toggle.isOn = (mapping.shape == shape);
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

            // Determine current target cells and their intended highlight modes
            Dictionary<HexCell, HexCell.HighlightMode> targetModes = new();
            
            float dragDist = _dragStartCell != null ? Vector3.Distance(Input.mousePosition, _dragStartPos) : 0f;
            bool isDragging = Input.GetMouseButton(0) && _dragStartCell != null && (dragDist > 5f || _dragStartCell != _hoveredCell);
            bool isDraggingShape = isDragging && (brushShape == BrushShape.Line || brushShape == BrushShape.Box || brushMode == BrushMode.CopyPaste);

            if (_hoveredCell != null)
            {
                // 1. Determine shape cells
                List<HexCell> shapeCells;
                bool isPastePreview = brushMode == BrushMode.CopyPaste && !isDragging && brushShape == BrushShape.Point && _clipboard.Count > 0;

                if (isPastePreview)
                {
                    shapeCells = new List<HexCell>();
                    foreach (var rel in _clipboard.Keys)
                    {
                        var target = grid.GetCell(_hoveredCell.Coordinates + rel);
                        if (target != null) shapeCells.Add(target);
                    }
                }
                else if (isDraggingShape)
                {
                    // In CopyPaste mode, default to Box if Point is selected, otherwise respect Line/Box
                    BrushShape shapeToUse = brushShape;
                    if (brushMode == BrushMode.CopyPaste && brushShape == BrushShape.Point)
                    {
                        shapeToUse = BrushShape.Box;
                    }
                    shapeCells = GetShapeCells(_dragStartCell, _hoveredCell, shapeToUse, outlineOnly: true);
                }
                else
                {
                    shapeCells = BrushCells(_hoveredCell);
                }

                // 2. Assign modes
                foreach (var cell in shapeCells)
                {
                    if (cell == null) continue;
                    
                    // Mouse cell always gets Selected highlight
                    if (cell == _hoveredCell)
                    {
                        targetModes[cell] = HexCell.HighlightMode.Selected;
                    }
                    else
                    {
                        // Other cells in the shape get Brush highlight if dragging a shape, or Selected if point brush
                        // For CopyPaste mode - Point shape, we use Brush highlight for the paste preview
                        bool useBrushHighlight = isDraggingShape || isPastePreview;
                        targetModes[cell] = useBrushHighlight ? HexCell.HighlightMode.Brush : HexCell.HighlightMode.Selected;
                    }
                }
            }

            // 3. Identify and remove stale or incorrect highlights
            List<HexCell> cellsToRemove = new();
            foreach (var pair in _activeHighlightObjects)
            {
                var cell = pair.Key;
                var obj  = pair.Value;
                
                if (cell == null || !targetModes.TryGetValue(cell, out var targetMode) || grid.GetActiveHighlightMode(obj) != targetMode)
                {
                    cellsToRemove.Add(cell);
                }
            }

            foreach (var cell in cellsToRemove)
            {
                var obj = _activeHighlightObjects[cell];
                if (obj != null) grid.ReturnHighlight(obj);
                _activeHighlightObjects.Remove(cell);
            }

            // 4. Add missing highlights
            foreach (var pair in targetModes)
            {
                var cell = pair.Key;
                var mode = pair.Value;

                if (!_activeHighlightObjects.ContainsKey(cell))
                {
                    var obj = grid.GetHighlightFromPool(mode);
                    if (obj != null)
                    {
                        float height = (cell.Terrain != null) ? cell.Terrain.visualHeight : 0.5f;
                        obj.transform.position = cell.transform.position + Vector3.up * height;
                        _activeHighlightObjects[cell] = obj;
                    }
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
            float dragDist = _dragStartCell != null ? Vector3.Distance(Input.mousePosition, _dragStartPos) : 0f;
            bool isDragging = _dragStartCell != null && (dragDist > 5f || _dragStartCell != _hoveredCell);
            bool isDraggingShape = (brushShape != BrushShape.Point || brushMode == BrushMode.CopyPaste) && isDragging;

            if (isDraggingShape)
            {
                BrushShape shapeToUse = brushShape;
                if (brushMode == BrushMode.CopyPaste && brushShape == BrushShape.Point)
                {
                    shapeToUse = BrushShape.Box;
                }
                cells = GetShapeCells(_dragStartCell, _hoveredCell, shapeToUse, outlineOnly: false);
            }
            else if (brushMode == BrushMode.Fill)
            {
                cells = GetFillCells(_hoveredCell);
            }
            else
            {
                cells = BrushCells(_hoveredCell);
            }

            if (brushMode == BrushMode.CopyPaste)
            {
                // If we didn't drag at all, it's a Paste.
                // If we did drag, it's a Copy.
                
                if (isDragging)
                {
                    CopySelection(cells);
                    SetBrushShape(BrushShape.Point); // Ready to paste
                }
                else if (brushShape == BrushShape.Point)
                {
                    ApplyPaste(_hoveredCell);
                    SetBrushShape(BrushShape.Box); // Ready to copy more
                }
                return;
            }

            foreach (var cell in cells)
            {
                // If we are in Fill mode, we want to apply the "last paint mode" to the area
                if (brushMode == BrushMode.Fill)
                {
                    ApplyFill(cell);
                }
                else
                {
                    ApplyBrush(cell);
                }
            }
        }

        private void ApplyFill(HexCell cell)
        {
            // Record for undo - notice we use _lastPaintBrushMode for the undo action's "intent"
            var action = new MapEditAction(cell, _lastPaintBrushMode, selectedTerrain, selectedNation);
            PushUndo(action);

            // Apply based on the context of the fill
            switch (_lastPaintBrushMode)
            {
                case BrushMode.Terrain:
                    if (selectedTerrain != null)
                        grid.ReplaceCell(cell, selectedTerrain);
                    break;
                case BrushMode.Nation:
                    cell.Owner = selectedNation;
                    cell.RefreshVisuals();
                    break;
                case BrushMode.Erase:
                    var neutralizedCell = grid.ReplaceCell(cell, null);
                    if (neutralizedCell != null)
                    {
                        neutralizedCell.Owner = Nation.Neutral;
                        neutralizedCell.RefreshVisuals();
                    }
                    break;
                case BrushMode.Facility:
                    cell.SetFacilityActive(setFacilityActive);
                    break;
            }
        }

        private void CopySelection(List<HexCell> cells)
        {
            if (cells == null || cells.Count == 0) return;
            _clipboard.Clear();

            // Use the first cell as the relative root for axial offsets
            HexCoordinates root = cells[0].Coordinates;

            foreach (var cell in cells)
            {
                HexCoordinates rel = cell.Coordinates - root;
                _clipboard[rel] = new ClipboardCell
                {
                    axialOffset = rel,
                    terrain = cell.Terrain,
                    owner = cell.Owner,
                    hasFacility = cell.IsFacility,
                    facilityHealth = cell.facilityHealth
                };
            }
            Debug.Log($"[MapEditor] Copied {_clipboard.Count} cells to clipboard using axial offsets.");
        }

        private void ApplyPaste(HexCell root)
        {
            if (root == null || _clipboard.Count == 0) return;

            // Record for undo - we should probably record the entire paste operation as one block
            // For now, ApplyBrush handles individual undos. Let's make Paste use ApplyBrush if possible,
            // or implement a BulkUndoAction.
            
            foreach (var pair in _clipboard)
            {
                var rel = pair.Key;
                var data = pair.Value;
                
                var targetCoords = root.Coordinates + rel;
                if (grid.TryGetCell(targetCoords, out var targetCell))
                {
                    // Record for undo manually since we're not using BrushMode
                    var action = new MapEditAction(targetCell, BrushMode.Terrain, data.terrain, data.owner);
                    PushUndo(action);

                    // Apply
                    var replaced = grid.ReplaceCell(targetCell, data.terrain);
                    if (replaced != null)
                    {
                        replaced.Owner = data.owner;
                        replaced.facilityHealth = data.facilityHealth;
                        replaced.RefreshVisuals();
                    }
                }
            }
        }

        private List<HexCell> GetFillCells(HexCell startCell)
        {
            var results = new List<HexCell>();
            if (startCell == null) return results;

            // Determine what we're matching against based on current sub-settings
            bool matchTerrain = (_lastPaintBrushMode != BrushMode.Nation);
            
            var visited = new HashSet<HexCell>();
            var queue = new Queue<HexCell>();
            queue.Enqueue(startCell);
            visited.Add(startCell);

            DS7.Data.TerrainData targetTerrain = startCell.Terrain;
            Nation targetNation = startCell.Owner;

            while (queue.Count > 0)
            {
                var cell = queue.Dequeue();
                results.Add(cell);

                foreach (var coord in cell.Coordinates.AllNeighbors())
                {
                    if (grid.TryGetCell(coord, out var neighbor))
                    {
                        if (neighbor == null || visited.Contains(neighbor)) continue;

                        bool isMatch = false;
                        if (matchTerrain) isMatch = (neighbor.Terrain == targetTerrain);
                        else isMatch = (neighbor.Owner == targetNation);

                        if (isMatch)
                        {
                            visited.Add(neighbor);
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }

            return results;
        }

        private List<HexCell> GetShapeCells(HexCell start, HexCell end, bool outlineOnly = false)
        {
            return GetShapeCells(start, end, brushShape, outlineOnly);
        }

        private List<HexCell> GetShapeCells(HexCell start, HexCell end, BrushShape shape, bool outlineOnly = false)
        {
            var results = new List<HexCell>();
            if (start == null || end == null) return results;

            if (shape == BrushShape.Line)
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
                        if (outlineOnly)
                        {
                            // Only add if on the boundary of the box
                            if (x != minX && x != maxX && y != minY && y != maxY)
                                continue;
                        }

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
