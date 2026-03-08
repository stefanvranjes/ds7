using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using DS7.Units;
using UnityEngine;

namespace DS7.Scene
{
    /// <summary>
    /// Bootstraps the Battlefield scene: generates the grid, spawns starting units,
    /// hooks up all singleton systems, and starts the first turn.
    /// Attach this to the root Manager GameObject in the Battlefield scene.
    /// </summary>
    public class BattlefieldBootstrap : MonoBehaviour
    {
        [Header("Grid")]
        public HexGrid   hexGrid;
        public int       gridWidth  = 20;
        public int       gridHeight = 20;

        [Header("Default Terrain")]
        [Tooltip("Used for every cell when no map file is loaded.")]
        public DS7.Data.TerrainData defaultTerrain;

        [Header("Starting Units")]
        public List<StartingUnit> startingUnits = new();

        [Header("Optional Map")]
        [Tooltip("Leave empty to use a blank map.")]
        public string mapFileToLoad = string.Empty;

        [Header("Game Mode")]
        public GameModes.TurnManager turnManager;
        public List<NationData>      turnOrder = new();

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private IEnumerator<YieldInstruction> Start()
        {
            // One-frame delay so all Awake() singletons are ready.
            yield return null;

            // 1. Configure and generate grid
            hexGrid.width  = gridWidth;
            hexGrid.height = gridHeight;
            hexGrid.GenerateGrid(defaultTerrain);

            // 2. Load map from file if specified
            if (!string.IsNullOrEmpty(mapFileToLoad))
            {
                var editor = GetComponent<Map.MapEditor>();
                if (editor != null)
                {
                    editor.mapFileName = mapFileToLoad;
                    editor.LoadMap();
                }
            }

            // 3. Spawn starting units
            foreach (var su in startingUnits)
                SpawnUnit(su);

            // 4. Configure turn order
            turnManager.turnOrder.Clear();
            turnManager.turnOrder.AddRange(turnOrder);

            // 5. Rebuild ZOC before first turn
            Combat.ZoneOfControl.Instance?.RebuildZOC(CollectAllUnits());

            // 6. Rebuild FOW
            Map.FogOfWar.Instance?.RebuildVisibility(CollectAllUnits());

            // 7. Start the game
            turnManager.StartGame();
        }

        // ── Unit Spawning ─────────────────────────────────────────────────────
        private void SpawnUnit(StartingUnit su)
        {
            if (su.unitData == null || su.prefab == null) return;

            var cell = hexGrid.GetCell(su.col, su.row);
            if (cell == null)
            {
                Debug.LogWarning($"[Bootstrap] Invalid spawn cell ({su.col},{su.row}) for {su.unitData.unitName}");
                return;
            }

            if (cell.IsOccupied(su.altitude))
            {
                Debug.LogWarning($"[Bootstrap] Cell ({su.col},{su.row}) altitude {su.altitude} already occupied.");
                return;
            }

            var go   = Instantiate(su.prefab, cell.Coordinates.ToWorldPosition(hexGrid.hexSize), Quaternion.identity);
            var unit = go.GetComponent<Unit>();
            if (unit == null) { Destroy(go); return; }

            unit.Initialize(su.unitData, su.nation, cell.Coordinates);
            unit.SetAltitude(su.altitude);
            cell.TryPlace(unit, su.altitude);

            turnManager.RegisterUnit(unit);

            Debug.Log($"[Bootstrap] Spawned {su.unitData.unitName} ({su.nation}) at ({su.col},{su.row})");
        }

        private List<Unit> CollectAllUnits()
        {
            var result = new List<Unit>();
            for (int col = 0; col < hexGrid.width; col++)
            for (int row = 0; row < hexGrid.height; row++)
            {
                var cell = hexGrid.GetCell(col, row);
                if (cell == null) continue;
                foreach (var u in cell.AllUnits()) result.Add(u);
            }
            return result;
        }
    }

    // ── Spawn Descriptor ──────────────────────────────────────────────────────
    [System.Serializable]
    public class StartingUnit
    {
        public UnitData    unitData;
        public Nation      nation;
        public int         col, row;
        public AltitudeLayer altitude = AltitudeLayer.Ground;
        public GameObject  prefab;
    }
}
