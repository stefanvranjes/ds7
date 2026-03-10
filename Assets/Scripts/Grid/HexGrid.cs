using System.Collections.Generic;
using DS7.Data;
using DS7.Units;
using UnityEngine;

namespace DS7.Grid
{
    /// <summary>
    /// Manages the hex grid: creation, pathfinding, movement range, and attack range queries.
    /// </summary>
    public class HexGrid : MonoBehaviour
    {
        // ── Inspector ─────────────────────────────────────────────────────────
        [Header("Grid Dimensions")]
        public int width  = 20;
        public int height = 20;

        [Header("Layers")]
        [Tooltip("Layer assigned to every hex cell to ensure Raycasts hit in the MapEditor.")]
        public int hexLayerIndex = 0; // Default to Default layer

        [Header("Hex Size")]
        [Tooltip("Outer radius of each hex tile (center to corner).")]
        public float hexSize = 1f;

        [System.Serializable]
        public struct TerrainPrefabMapping
        {
            public TerrainType type;
            public HexCell prefab;
        }

        [Header("Prefabs")]
        public TerrainPrefabMapping[] cellPrefabs;
        public HexCell fallbackCellPrefab;

        [Header("Highlight Prefabs")]
        public GameObject moveRangeHighlightPrefab;
        public GameObject attackRangeHighlightPrefab;
        public GameObject selectedHighlightPrefab;
        public GameObject enemyHighlightPrefab;

        [Header("Highlight Root")]
        public Transform highlightRoot;

        private readonly Dictionary<HexCell.HighlightMode, List<GameObject>> _highlightPool = new();
        private readonly List<GameObject> _activeHighlights = new();

        private HexCell GetPrefabForTerrain(TerrainType type)
        {
            if (cellPrefabs != null)
            {
                foreach (var mapping in cellPrefabs)
                {
                    if (mapping.type == type && mapping.prefab != null)
                        return mapping.prefab;
                }
            }
            return fallbackCellPrefab;
        }

        [Header("Default Terrain")]
        public DS7.Data.TerrainData defaultTerrain;

        // ── Runtime ───────────────────────────────────────────────────────────
        private HexCell[,] _cells;

        public static HexGrid Instance { get; private set; }

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start() => GenerateGrid();

        // ── Grid Generation ───────────────────────────────────────────────────
        /// <summary>Generates a fresh grid using the inspector-assigned default terrain.</summary>
        public void GenerateGrid() => GenerateGrid(defaultTerrain);

        /// <summary>Generates a fresh grid, overriding default terrain at runtime (used by BattlefieldBootstrap).</summary>
        public void GenerateGrid(DS7.Data.TerrainData terrainOverride)
        {
            if (_cells != null) ClearGrid();

            _cells = new HexCell[width, height];

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    var coords  = HexCoordinates.FromOffsetCoords(col, row);
                    var worldPos = coords.ToWorldPosition(hexSize);

                    var terrain = terrainOverride ?? defaultTerrain;
                    var prefab = GetPrefabForTerrain(terrain != null ? terrain.terrainType : TerrainType.Plain);

                    var cell = Instantiate(prefab, worldPos, Quaternion.identity, transform);
                    cell.name = $"Hex_{col}_{row}";
                    SetLayerRecursive(cell.gameObject, hexLayerIndex);
                    cell.Initialize(coords, terrain);
                    
                    _cells[col, row] = cell;
                }
            }
        }

        public GameObject GetHighlight(HexCell.HighlightMode mode)
        {
            if (mode == HexCell.HighlightMode.None) return null;

            if (!_highlightPool.ContainsKey(mode))
                _highlightPool[mode] = new List<GameObject>();

            GameObject highlight = null;
            if (_highlightPool[mode].Count > 0)
            {
                highlight = _highlightPool[mode][0];
                _highlightPool[mode].RemoveAt(0);
            }
            else
            {
                var prefab = mode switch
                {
                    HexCell.HighlightMode.MoveRange   => moveRangeHighlightPrefab,
                    HexCell.HighlightMode.AttackRange => attackRangeHighlightPrefab,
                    HexCell.HighlightMode.Selected    => selectedHighlightPrefab,
                    HexCell.HighlightMode.Enemy       => enemyHighlightPrefab,
                    _                                 => null
                };

                if (prefab != null)
                {
                    highlight = Instantiate(prefab, highlightRoot != null ? highlightRoot : transform);
                }
            }

            if (highlight != null)
            {
                highlight.SetActive(true);
                _activeHighlights.Add(highlight);
            }
            return highlight;
        }

        public void ReturnAllHighlights()
        {
            foreach (var h in _activeHighlights)
            {
                if (h == null) continue;
                h.SetActive(false);
                
                // Identify which pool this belongs to
                // we can store a script on the prefab to identify its mode, 
                // or just check name/prefab reference. 
                // For now, let's assume we can determine it or use a default list to clear.
            }
            // A better way is to track which mode each active highlight belongs to.
            // Let's refine this in a second pass if needed, but for now we'll just 
            // clear the active list and we might need to recreate pools if we don't know the mode.
            // Actually, let's store them by mode in active highlights too.
        }

        // Refined pooling structure
        private readonly Dictionary<GameObject, HexCell.HighlightMode> _activeHighlightModes = new();

        public void ReturnAllActiveHighlights()
        {
            foreach (var pair in _activeHighlightModes)
            {
                var h = pair.Key;
                var mode = pair.Value;
                if (h != null)
                {
                    h.SetActive(false);
                    if (!_highlightPool.ContainsKey(mode)) _highlightPool[mode] = new List<GameObject>();
                    _highlightPool[mode].Add(h);
                }
            }
            _activeHighlightModes.Clear();
            _activeHighlights.Clear();
        }

        public GameObject GetHighlightFromPool(HexCell.HighlightMode mode)
        {
            GameObject h = GetHighlight(mode);
            if (h != null) _activeHighlightModes[h] = mode;
            return h;
        }

        private void InstantiateHighlights(HexCell cell)
        {
            // Removed for pooling logic
        }

        public void ReturnHighlight(GameObject highlight, HexCell.HighlightMode mode)
        {
            if (highlight == null) return;
            highlight.SetActive(false);
            
            _activeHighlightModes.Remove(highlight);
            _activeHighlights.Remove(highlight);

            if (!_highlightPool.ContainsKey(mode))
                _highlightPool[mode] = new List<GameObject>();
            
            _highlightPool[mode].Add(highlight);
        }

        private void SetLayerRecursive(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
            }
        }

        private void ClearGrid()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            _cells = null;
        }

        public HexCell ReplaceCell(HexCell oldCell, DS7.Data.TerrainData newTerrain)
        {
            if (oldCell == null) return null;

            var coords = oldCell.Coordinates;
            var offset = coords.ToOffsetCoords();
            int col = offset.x;
            int row = offset.y;

            var oldType = oldCell.Terrain != null ? oldCell.Terrain.terrainType : TerrainType.Plain;
            var newType = newTerrain != null ? newTerrain.terrainType : TerrainType.Plain;

            if (oldType == newType)
            {
                oldCell.SetTerrain(newTerrain);
                return oldCell;
            }

            var prefab = GetPrefabForTerrain(newType);
            var worldPos = oldCell.transform.position;

            var newCell = Instantiate(prefab, worldPos, Quaternion.identity, transform);
            newCell.name = $"Hex_{col}_{row}";
            SetLayerRecursive(newCell.gameObject, hexLayerIndex);

            newCell.Initialize(coords, newTerrain);
            newCell.Owner = oldCell.Owner;
            newCell.facilityHealth = oldCell.facilityHealth;

            foreach (var alt in System.Enum.GetValues(typeof(AltitudeLayer)))
            {
                AltitudeLayer layer = (AltitudeLayer)alt;
                if (oldCell.IsOccupied(layer))
                {
                    newCell.TryPlace(oldCell.GetUnit(layer), layer);
                }
            }

            _cells[col, row] = newCell;
            
            newCell.RefreshVisuals();

            if (Application.isPlaying)
                Destroy(oldCell.gameObject);
            else
                DestroyImmediate(oldCell.gameObject);

            return newCell;
        }

        // ── Cell Access ───────────────────────────────────────────────────────
        public HexCell GetCell(int col, int row)
        {
            if (col < 0 || col >= width || row < 0 || row >= height) return null;
            return _cells[col, row];
        }

        public HexCell GetCell(HexCoordinates coords)
        {
            var offset = coords.ToOffsetCoords();
            return GetCell(offset.x, offset.y);
        }

        public bool TryGetCell(HexCoordinates coords, out HexCell cell)
        {
            cell = GetCell(coords);
            return cell != null;
        }

        // ── Movement Range (Dijkstra) ─────────────────────────────────────────
        /// <summary>
        /// Returns all HexCells reachable within the unit's move allowance,
        /// split into standard-speed and high-speed sets.
        /// Respects terrain costs, fuel, and ZOC.
        /// </summary>
        public MovementResult GetMovementRange(Unit unit)
        {
            var result = new MovementResult();

            if (unit == null) return result;

            var origin   = GetCell(unit.CurrentCoords);
            if (origin == null) return result;

            var cat       = unit.Data.movementCategory;
            int stdMove   = unit.Data.standardMove;
            int highMv    = unit.Data.highMove;
            int fuel      = unit.CurrentFuel;
            Nation nation = unit.Owner;

            // Dijkstra: stores minimum cost to reach each cell
            var dist = new Dictionary<HexCoordinates, float>();
            var open = new SortedSet<(float cost, HexCoordinates coords)>(
                Comparer<(float, HexCoordinates)>.Create((a, b) =>
                    a.Item1 != b.Item1 ? a.Item1.CompareTo(b.Item1)
                    : a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode())));

            dist[unit.CurrentCoords] = 0;
            open.Add((0, unit.CurrentCoords));

            while (open.Count > 0)
            {
                var (currentCost, current) = open.Min;
                open.Remove(open.Min);

                if (!TryGetCell(current, out var currentCell)) continue;

                // If we already found a better path, skip
                if (dist.TryGetValue(current, out float bestCost) && currentCost > bestCost)
                    continue;

                // Check ZOC stop: if enemy ZOC, we can enter but not move further
                bool stoppedByZoc = current != unit.CurrentCoords && currentCell.IsInZocOf(nation);

                if (!stoppedByZoc)
                {
                    foreach (var neighborCoords in current.AllNeighbors())
                    {
                        if (!TryGetCell(neighborCoords, out var neighborCell)) continue;

                        // Can't enter hex occupied by enemy at same altitude
                        var occupant = neighborCell.GetUnit(unit.CurrentAltitude);
                        if (occupant != null && occupant.Owner != nation) continue;

                        int terrainCost = neighborCell.Terrain?.GetMovementCost(cat) ?? -1;
                        if (terrainCost < 0) continue; // impassable

                        float newCost = currentCost + terrainCost;
                        int fuelCost  = terrainCost * unit.Data.fuelPerStandardMove;

                        if (newCost > highMv || fuelCost > fuel) continue;

                        if (!dist.TryGetValue(neighborCoords, out float oldCost) || newCost < oldCost)
                        {
                            dist[neighborCoords] = newCost;
                            open.Add((newCost, neighborCoords));
                        }
                    }
                }

                if (current == unit.CurrentCoords) continue;

                if (currentCost <= stdMove)
                    result.StandardRange.Add(currentCell);
                else if (currentCost <= highMv)
                    result.HighSpeedRange.Add(currentCell);
            }

            return result;
        }

        // ── Attack Range ──────────────────────────────────────────────────────
        /// <summary>
        /// Returns all cells containing enemy units that the given unit can attack
        /// with at least one of its weapons.
        /// </summary>
        public List<HexCell> GetAttackTargets(Unit unit, bool hasMoved)
        {
            var targets = new List<HexCell>();
            if (unit == null) return targets;

            var weapons = unit.Data.GetWeapons(unit.CurrentPackIndex);
            var origin  = GetCell(unit.CurrentCoords);
            if (origin == null) return targets;

            foreach (var weapon in weapons)
            {
                if (!weapon.canAttack) continue;
                if (hasMoved && !weapon.canMoveAndFire) continue;
                if (unit.GetAmmo(weapon) <= 0) continue;

                int maxRange = GetMaxRange(weapon);

                // Gather all cells within max range
                for (int dx = -maxRange; dx <= maxRange; dx++)
                for (int dz = -maxRange; dz <= maxRange; dz++)
                {
                    var coords = new HexCoordinates(
                        unit.CurrentCoords.X + dx,
                        unit.CurrentCoords.Z + dz);

                    int dist = HexCoordinates.Distance(unit.CurrentCoords, coords);
                    if (dist == 0) continue;
                    if (!weapon.canAdj && dist == 1) continue;
                    if (!TryGetCell(coords, out var cell)) continue;

                    int range = weapon.GetRange(unit.CurrentAltitude);
                    if (dist > range) continue;

                    // Check each altitude layer for enemy units
                    foreach (var occupant in cell.AllUnits())
                    {
                        if (occupant.Owner == unit.Owner) continue;

                        int weapRange = weapon.GetRange(occupant.CurrentAltitude);
                        if (dist > weapRange) continue;

                        int altDiff = (int)occupant.CurrentAltitude - (int)unit.CurrentAltitude;
                        if (altDiff > weapon.elevationUp)   continue;
                        if (-altDiff > weapon.elevationDown) continue;

                        if (!targets.Contains(cell)) targets.Add(cell);
                    }
                }
            }

            return targets;
        }

        private static int GetMaxRange(WeaponData w)
            => Mathf.Max(w.rangeHigh, w.rangeMed, w.rangeLow,
                         w.rangeGround, w.rangeSurface, w.rangeDeep);

        // ── Highlight Helpers ─────────────────────────────────────────────────
        public void ClearAllHighlights()
        {
            ReturnAllActiveHighlights();
        }

        public void HighlightMovementRange(MovementResult range)
        {
            foreach (var cell in range.StandardRange)
                ApplyPooledHighlight(cell, HexCell.HighlightMode.MoveRange);
            foreach (var cell in range.HighSpeedRange)
                ApplyPooledHighlight(cell, HexCell.HighlightMode.AttackRange); // red = high-speed
        }

        public void HighlightAttackTargets(List<HexCell> targets)
        {
            foreach (var cell in targets)
                ApplyPooledHighlight(cell, HexCell.HighlightMode.Enemy);
        }

        private void ApplyPooledHighlight(HexCell cell, HexCell.HighlightMode mode)
        {
            var obj = GetHighlightFromPool(mode);
            if (obj != null && cell != null)
            {
                float height = (cell.Terrain != null) ? cell.Terrain.visualHeight : 0.5f;
                obj.transform.position = cell.transform.position + Vector3.up * height;
            }
        }
    }

    // ── Movement Result ───────────────────────────────────────────────────────
    public class MovementResult
    {
        /// <summary>Cells reachable within standard movement allowance.</summary>
        public List<HexCell> StandardRange  { get; } = new();
        /// <summary>Cells reachable only with high-speed movement (costs 2x fuel).</summary>
        public List<HexCell> HighSpeedRange { get; } = new();

        public bool IsEmpty => StandardRange.Count == 0 && HighSpeedRange.Count == 0;
    }
}
