using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using DS7.Units;
using UnityEngine;

namespace DS7.Map
{
    /// <summary>
    /// Manages per-nation fog of war. Updates each turn based on unit detection ranges.
    /// </summary>
    public class FogOfWar : MonoBehaviour
    {
        public static FogOfWar Instance { get; private set; }

        [Header("Enable Fog")]
        public bool fogEnabled = true;

        private HexGrid _grid;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            _grid    = HexGrid.Instance;
        }

        // ── Rebuild Visibility ────────────────────────────────────────────────
        /// <summary>Rebuilds the visibility map for all nations each turn.</summary>
        public void RebuildVisibility(List<Unit> allUnits)
        {
            if (!fogEnabled) return;
            if (_grid == null) _grid = HexGrid.Instance;

            // Clear all visibility
            for (int col = 0; col < _grid.width; col++)
            for (int row = 0; row < _grid.height; row++)
                _grid.GetCell(col, row)?.VisibleTo.Clear();

            // Apply vision from each unit
            foreach (var unit in allUnits)
                RevealAroundUnit(unit);

            // Update tile rendering (show/hide)
            ApplyToTiles(Nation.Japan); // human player — extend for multiplayer
        }

        private void RevealAroundUnit(Unit unit)
        {
            int detRange = unit.Data.detectionRange;

            // Altitude bonus: higher altitude = more detection range
            if (unit.CurrentAltitude >= AltitudeLayer.MedAir) detRange += 2;
            else if (unit.CurrentAltitude == AltitudeLayer.LowAir) detRange += 1;

            // Terrain modifier on unit's own hex
            var ownCell = _grid?.GetCell(unit.CurrentCoords);
            if (ownCell?.Terrain != null) detRange += ownCell.Terrain.detectionModifier;

            for (int dx = -detRange; dx <= detRange; dx++)
            for (int dz = -detRange; dz <= detRange; dz++)
            {
                var coords = new HexCoordinates(unit.CurrentCoords.X + dx,
                                                unit.CurrentCoords.Z + dz);
                if (HexCoordinates.Distance(unit.CurrentCoords, coords) > detRange) continue;
                if (!_grid.TryGetCell(coords, out var cell)) continue;

                cell.VisibleTo.Add(unit.Owner);
            }
        }

        private void ApplyToTiles(Nation viewingNation)
        {
            for (int col = 0; col < _grid.width; col++)
            for (int row = 0; row < _grid.height; row++)
            {
                var cell = _grid.GetCell(col, row);
                if (cell == null) continue;

                bool visible = cell.IsVisibleTo(viewingNation);

                // Gray out or hide non-visible tiles
                var rend = cell.GetComponent<MeshRenderer>();
                if (rend != null)
                    rend.material.color = visible ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f);

                // Hide enemy units in fog
                foreach (var unit in cell.AllUnits())
                    if (unit.Owner != viewingNation)
                        unit.gameObject.SetActive(visible);
            }
        }
    }
}
