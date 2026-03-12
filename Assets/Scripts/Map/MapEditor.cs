using System;
using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using UnityEngine;
using DS7.Units;
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
        public enum BrushMode { Pen, Line, Box, Eraser, Copy, Paint, Road, River, Unit }

        [Header("Mode")]
        public BrushMode brushMode = BrushMode.Pen;

        [Header("Paint Settings")]
        public DS7.Data.TerrainData brushTerrain;
        public Faction     brushOwner = Faction.Neutral;
        public bool        setFacilityActive = true;
        [Range(1, 7)]
        [Tooltip("1 = single hex, 7 = megahex (centre + ring)")]
        public int brushRadius = 1;

        [Header("UI Integration")]
        public List<TerrainToggleMapping> terrainToggles;
        public List<BrushModeToggleMapping> modeToggles;
        public Toggle[] factionToggles; // Added for Faction selection

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
        private BrushMode _lastPaintBrushMode = BrushMode.Pen;

        // ── Clipboard ─────────────────────────────────────────────────────────
        [Serializable]
        public struct ClipboardCell
        {
            public Vector2Int offset; // Obsolete, keeping for compatibility if needed elsewhere
            public HexCoordinates axialOffset; // Relative to root cell
            public DS7.Data.TerrainData terrain;
            public Faction owner; // Changed from Nation to Faction
            public bool hasFacility;
            public int facilityHealth;
            public int roadMask;
            public int riverMask;
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

            SetupToggleListeners();
        }

        private void SetupToggleListeners()
        {
            if (terrainToggles != null)
            {
                foreach (var mapping in terrainToggles)
                {
                    if (mapping.toggle != null)
                    {
                        mapping.toggle.onValueChanged.AddListener(isOn =>
                        {
                            if (isOn)
                            {
                                brushTerrain = mapping.terrain; // Changed from selectedTerrain
                                // If we are in Erase/Road/River/Unit, switch back to Pen mode
                                if (brushMode == BrushMode.Eraser || brushMode == BrushMode.Road || brushMode == BrushMode.River || brushMode == BrushMode.Unit)
                                {
                                    ForceSwitchToPenMode();
                                }
                            }
                        });

                        // Also catch clicks to handle already-on toggles
                        var trigger = mapping.toggle.GetComponent<EventTrigger>();
                        if (trigger == null) trigger = mapping.toggle.gameObject.AddComponent<EventTrigger>();
                        var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
                        entry.callback.AddListener((data) => 
                        {
                            brushTerrain = mapping.terrain; // Changed from selectedTerrain
                            // Only force switch if we are in a mode that doesn't use terrain (Eraser, Road, River, Unit)
                            if (brushMode == BrushMode.Eraser || brushMode == BrushMode.Road || brushMode == BrushMode.River || brushMode == BrushMode.Unit)
                            {
                                ForceSwitchToPenMode();
                            }
                        });
                        trigger.triggers.Add(entry);
                    }
                }
            }

            if (modeToggles != null)
            {
                foreach (var mapping in modeToggles)
                {
                    if (mapping.toggle != null)
                    {
                        mapping.toggle.onValueChanged.AddListener(isOn =>
                        {
                            if (isOn)
                            {
                                brushMode = mapping.mode;
                                // Only track as the "last paint mode" if it's a real painting mode —
                                // not Paint (overlay), Copy, Eraser, Road, River, Unit.
                                if (brushMode != BrushMode.Paint
                                    && brushMode != BrushMode.Copy
                                    && brushMode != BrushMode.Eraser
                                    && brushMode != BrushMode.Road
                                    && brushMode != BrushMode.River
                                    && brushMode != BrushMode.Unit)
                                {
                                    _lastPaintBrushMode = brushMode;
                                }
                            }
                        });
                    }
                }
            }

            // Faction selectors
            if (factionToggles != null)
            {
                for (int i = 0; i < factionToggles.Length; i++)
                {
                    int index = i;
                    if (factionToggles[i] != null)
                    {
                        factionToggles[i].onValueChanged.AddListener(on => {
                            if (on) brushOwner = (Faction)(index); // 0=None, 1=Red, etc.
                        });
                    }
                }
            }
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
                _lastPainted  = null;
            }

            UpdateHoverAndHighlight();

            bool isShapeMode = brushMode == BrushMode.Line || brushMode == BrushMode.Box || brushMode == BrushMode.Copy || brushMode == BrushMode.Paint || brushMode == BrushMode.Road || brushMode == BrushMode.River;
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

        private void ForceSwitchToPenMode()
        {
            brushMode = BrushMode.Pen;
            if (modeToggles == null) return;
            foreach (var mapping in modeToggles)
            {
                if (mapping.toggle != null)
                {
                    mapping.toggle.isOn = (mapping.mode == BrushMode.Pen);
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
            bool isDraggingShape = isDragging && (brushMode == BrushMode.Line || brushMode == BrushMode.Box || brushMode == BrushMode.Copy || brushMode == BrushMode.Road || brushMode == BrushMode.River);

            if (_hoveredCell != null)
            {
                // 1. Determine shape cells
                List<HexCell> shapeCells;
                bool isPastePreview = brushMode == BrushMode.Copy && !isDragging && _clipboard.Count > 0;

                if (isPastePreview)
                {
                    shapeCells = new List<HexCell>();
                    var clipOffsets = new HashSet<HexCoordinates>(_clipboard.Keys);
                    
                    foreach (var rel in clipOffsets)
                    {
                        // A cell is on the outline if any of its 6 neighbors is NOT in the clipboard
                        bool isOutline = false;
                        for (int d = 0; d < 6; d++)
                        {
                            if (!clipOffsets.Contains(rel.Neighbor(d)))
                            {
                                isOutline = true;
                                break;
                            }
                        }
                        if (!isOutline) continue;

                        var target = grid.GetCell(_hoveredCell.Coordinates + rel);
                        if (target != null) shapeCells.Add(target);
                    }
                }
                else if (isDraggingShape)
                {
                    shapeCells = GetShapeCells(_dragStartCell, _hoveredCell, outlineOnly: true);
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
                        // For Copy mode, we use Brush highlight for the paste preview
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

            // For Pen brush, avoid re-painting the same cell every frame
            if (brushMode == BrushMode.Pen && _hoveredCell == _lastPainted && brushRadius == 1) return;
            
            HexCell prevHub = _lastPainted;
            _lastPainted = _hoveredCell;

            // Collect brush hexes
            float dragDist = _dragStartCell != null ? Vector3.Distance(Input.mousePosition, _dragStartPos) : 0f;
            bool isDragging = _dragStartCell != null && (dragDist > 5f || _dragStartCell != _hoveredCell);
            bool isDraggingShape = (brushMode == BrushMode.Line || brushMode == BrushMode.Box || brushMode == BrushMode.Copy || brushMode == BrushMode.Road || brushMode == BrushMode.River) && isDragging;

            bool isLinePaintMode = (brushMode == BrushMode.Road || brushMode == BrushMode.River);

            if (isDraggingShape)
            {
                var cells = GetShapeCells(_dragStartCell, _hoveredCell, outlineOnly: false);

                if (brushMode == BrushMode.Copy)
                {
                    // This is a drag — copy the selection
                    CopySelection(cells);
                    return;
                }

                if (isLinePaintMode)
                {
                    HexCell pathPrev = null;
                    foreach (var hub in cells)
                    {
                        ApplyBrush(hub, pathPrev);
                        pathPrev = hub;
                    }
                }
                else
                {
                    foreach (var cell in cells) ApplyBrush(cell);
                }
                return;
            }

            // Copy: single click → paste
            if (brushMode == BrushMode.Copy && _clipboard.Count > 0)
            {
                ApplyPaste(_hoveredCell);
                _clipboard.Clear(); // Clear the clipboard after pasting to go back to box drag selection
                return;
            }
            if (brushMode == BrushMode.Paint)
            {
                var cells = GetFillCells(_hoveredCell);
                foreach (var cell in cells) ApplyFill(cell);
                return;
            }
            
            // Continuous Path (Line Paint - connecting items as brush moves over them)
            if (isLinePaintMode && !isDraggingShape && prevHub != null)
            {
                var pathCoords = HexCoordinates.GetLine(prevHub.Coordinates, _hoveredCell.Coordinates);
                HexCell pathPrev = prevHub;
                
                // path[0] is prevHub (already processed last frame). We start from path[1].
                for (int i = 1; i < pathCoords.Count; i++)
                {
                    if (grid.TryGetCell(pathCoords[i], out var hub))
                    {
                        // Apply to the hub (center) with connectivity
                        ApplyBrush(hub, pathPrev);

                        // If radius > 1, apply to surrounding area (isolated)
                        if (brushRadius > 1)
                        {
                            foreach (var sc in BrushCells(hub))
                            {
                                if (sc != hub) ApplyBrush(sc);
                            }
                        }
                        pathPrev = hub;
                    }
                }
            }
            else
            {
                // Regular single-point or radius brush
                var hubs = BrushCells(_hoveredCell);
                foreach (var cell in hubs) ApplyBrush(cell);
            }
        }

        private void ApplyFill(HexCell cell)
        {
            // Record for undo - notice we use _lastPaintBrushMode for the undo action's "intent"
            var action = new MapEditAction(cell, _lastPaintBrushMode, brushTerrain, brushOwner); // Changed selectedTerrain, selectedNation
            PushUndo(action);

            // Apply based on the context of the fill
            switch (_lastPaintBrushMode)
            {
                case BrushMode.Pen:
                case BrushMode.Line:
                case BrushMode.Box:
                    if (brushTerrain != null) // Changed selectedTerrain
                    {
                        grid.ReplaceCell(cell, brushTerrain); // Changed selectedTerrain
                    }
                    cell.Owner = brushOwner; // Added
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
                    facilityHealth = cell.facilityHealth,
                    roadMask = cell.RoadMask,
                    riverMask = cell.RiverMask
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
                    var action = new MapEditAction(targetCell, BrushMode.Pen, data.terrain, data.owner); // Changed Nation to Faction
                    PushUndo(action);

                    // Apply
                    var replaced = grid.ReplaceCell(targetCell, data.terrain);
                    if (replaced != null)
                    {
                        replaced.Owner = data.owner;
                        replaced.facilityHealth = data.facilityHealth;
                        replaced.SetRoadMask(data.roadMask);
                        replaced.SetRiverMask(data.riverMask);
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
            bool matchTerrain = true; // Paint only compares terrain now, not nations
            
            var visited = new HashSet<HexCell>();
            var queue = new Queue<HexCell>();
            queue.Enqueue(startCell);
            visited.Add(startCell);

            DS7.Data.TerrainData targetTerrain = startCell.Terrain;
            Faction targetNation = startCell.Owner; // Changed Nation to Faction

            while (queue.Count > 0)
            {
                var cell = queue.Dequeue();
                results.Add(cell);

                foreach (var coord in cell.Coordinates.AllNeighbors())
                {
                    if (grid.TryGetCell(coord, out var neighbor))
                    {
                        if (neighbor == null || visited.Contains(neighbor)) continue;

                        bool isMatch = (neighbor.Terrain == targetTerrain);

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
            var results = new List<HexCell>();
            if (start == null || end == null) return results;

            if (brushMode == BrushMode.Line || brushMode == BrushMode.Road || brushMode == BrushMode.River)
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
            else if (brushMode == BrushMode.Box || brushMode == BrushMode.Copy)
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

        private void ApplyBrush(HexCell cell, HexCell fromCell = null)
        {
            // Record for undo - only if something might change
            bool stateChanged = false;
            switch (brushMode)
            {
                case BrushMode.Pen:
                case BrushMode.Line:
                case BrushMode.Box:
                case BrushMode.Paint:
                    stateChanged = cell.Terrain != brushTerrain || cell.Owner != brushOwner || cell.IsFacility != setFacilityActive; // Changed selectedTerrain, selectedNation
                    break;
                case BrushMode.Eraser:   
                    stateChanged = cell.RoadMask >= 0 || cell.RiverMask >= 0 || cell.AllUnits().GetEnumerator().MoveNext(); 
                    break;
                case BrushMode.Road:    stateChanged = cell.RoadMask < 0 || fromCell != null; break;
                case BrushMode.River:   stateChanged = cell.RiverMask < 0 || fromCell != null; break;
            }

            if (stateChanged)
            {
                var action = new MapEditAction(cell, brushMode, brushTerrain, brushOwner); // Changed selectedTerrain, selectedNation
                PushUndo(action);
            }

            switch (brushMode)
            {
                case BrushMode.Pen:
                case BrushMode.Line:
                case BrushMode.Box:
                case BrushMode.Paint:
                    if (brushTerrain != null) // Changed selectedTerrain
                    {
                        grid.ReplaceCell(cell, brushTerrain); // Changed selectedTerrain
                    }
                    cell.Owner = brushOwner; // Added
                    cell.SetFacilityActive(setFacilityActive);
                    break;

                case BrushMode.Eraser:
                    cell.ClearOverlays();
                    
                    // Remove units
                    var layersToClear = new List<AltitudeLayer>();
                    foreach (var u in cell.AllUnits())
                    {
                        layersToClear.Add(u.CurrentAltitude);
                    }
                    foreach (var layer in layersToClear)
                    {
                        // In practice, call unit.Die() or similar, based on how Units are despawned
                        if (cell.GetUnit(layer) is Unit targetUnit)
                        {
                            if (targetUnit.gameObject != null) Destroy(targetUnit.gameObject);
                            cell.RemoveUnit(layer);
                        }
                    }
                    break;

                case BrushMode.Road:
                case BrushMode.River:
                    ApplyOverlayBrush(cell, fromCell);
                    break;
            }
        }

        /// <summary>Removes neighbor connection stubs that point back to a cell being erased.</summary>
        private void CleanNeighborConnections(HexCell cell)
        {
            for (int dir = 0; dir < 6; dir++)
            {
                var nbCoords = cell.Coordinates.Neighbor(dir);
                if (!grid.TryGetCell(nbCoords, out var nb)) continue;
                int oppDir = ConnectivityHelper.GetOppositeDirection(dir);
                if (nb.RoadMask >= 0)
                    nb.SetRoadMask(ConnectivityHelper.RemoveConnection(nb.RoadMask, oppDir));
                if (nb.RiverMask >= 0)
                    nb.SetRiverMask(ConnectivityHelper.RemoveConnection(nb.RiverMask, oppDir));
            }
        }

        private void ApplyOverlayBrush(HexCell cell, HexCell fromCell = null)
        {
            bool isRoad = brushMode == BrushMode.Road;
            
            // Ensure isolated node exists
            if (isRoad)
            {
                if (cell.RoadMask < 0) cell.SetRoadMask(0);
            }
            else
            {
                if (cell.RiverMask < 0) cell.SetRiverMask(0);
            }

            // Connection logic
            if (fromCell != null && fromCell != cell)
            {
                if (fromCell.Coordinates.IsNeighbor(cell.Coordinates))
                {
                    int dirToCell = ConnectivityHelper.GetDirection(fromCell.Coordinates, cell.Coordinates);
                    int dirToLast = ConnectivityHelper.GetOppositeDirection(dirToCell);

                    if (isRoad)
                    {
                        int fromMask = fromCell.RoadMask < 0 ? 0 : fromCell.RoadMask;
                        int cellMask = cell.RoadMask < 0 ? 0 : cell.RoadMask;
                        fromCell.SetRoadMask(ConnectivityHelper.AddConnection(fromMask, dirToCell));
                        cell.SetRoadMask(ConnectivityHelper.AddConnection(cellMask, dirToLast));
                    }
                    else
                    {
                        int fromMask = fromCell.RiverMask < 0 ? 0 : fromCell.RiverMask;
                        int cellMask = cell.RiverMask < 0 ? 0 : cell.RiverMask;
                        fromCell.SetRiverMask(ConnectivityHelper.AddConnection(fromMask, dirToCell));
                        cell.SetRiverMask(ConnectivityHelper.AddConnection(cellMask, dirToLast));
                    }
                }
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
        private readonly Faction          _prevNation;
        private readonly DS7.Data.TerrainData _newTerrain;
        private readonly Faction          _newNation;
        private readonly int             _prevRoadMask;
        private readonly int             _prevRiverMask;

        public MapEditAction(HexCell cell, MapEditor.BrushMode mode,
                             DS7.Data.TerrainData newTerrain, Faction newNation)
        {
            _coords       = cell.Coordinates;
            _mode         = mode;
            _prevTerrain  = cell.Terrain;
            _prevNation   = cell.Owner;
            _newTerrain   = newTerrain;
            _newNation    = newNation;
            _prevRoadMask = cell.RoadMask;
            _prevRiverMask = cell.RiverMask;
        }

        public void Revert()
        {
            var grid = HexGrid.Instance;
            if (grid == null || !grid.TryGetCell(_coords, out var cell)) return;

            switch (_mode)
            {
                case MapEditor.BrushMode.Pen:
                case MapEditor.BrushMode.Line:
                case MapEditor.BrushMode.Box:
                case MapEditor.BrushMode.Paint:
                case MapEditor.BrushMode.Copy:
                    var tc = grid.ReplaceCell(cell, _prevTerrain);
                    if (tc != null)
                    {
                        tc.SetFacilityActive(_prevTerrain != null && _prevTerrain.isFacility && cell.facilityHealth > 0);
                        tc.Owner = _prevNation;
                        tc.SetRoadMask(_prevRoadMask);
                        tc.SetRiverMask(_prevRiverMask);
                        tc.RefreshVisuals();
                    }
                    break;
                case MapEditor.BrushMode.Eraser:
                    cell.SetRoadMask(_prevRoadMask);
                    cell.SetRiverMask(_prevRiverMask);
                    break;
                case MapEditor.BrushMode.Road:
                    cell.SetRoadMask(_prevRoadMask);
                    break;
                case MapEditor.BrushMode.River:
                    cell.SetRiverMask(_prevRiverMask);
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
                    col            = col,
                    row            = row,
                    terrainName    = cell.Terrain?.name ?? string.Empty,
                    owner          = (int)cell.Owner,
                    facilityHealth = cell.facilityHealth,
                    roadMask       = cell.RoadMask,
                    riverMask      = cell.RiverMask
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
                    cell.Owner          = (Faction)cd.owner;
                    cell.facilityHealth = cd.facilityHealth;
                    cell.SetRoadMask(cd.roadMask);
                    cell.SetRiverMask(cd.riverMask);
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
        public int    roadMask;
        public int    riverMask;
    }
}
