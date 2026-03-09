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
                    cell.Initialize(coords, terrain);
                    _cells[col, row] = cell;
                }
            }
        }

        private void ClearGrid()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            _cells = null;
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
            if (_cells == null) return;
            foreach (var cell in _cells)
                cell?.SetHighlight(HexCell.HighlightMode.None);
        }

        public void HighlightMovementRange(MovementResult range)
        {
            foreach (var cell in range.StandardRange)
                cell.SetHighlight(HexCell.HighlightMode.MoveRange);
            foreach (var cell in range.HighSpeedRange)
                cell.SetHighlight(HexCell.HighlightMode.AttackRange); // red = high-speed
        }

        public void HighlightAttackTargets(List<HexCell> targets)
        {
            foreach (var cell in targets)
                cell.SetHighlight(HexCell.HighlightMode.Enemy);
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
