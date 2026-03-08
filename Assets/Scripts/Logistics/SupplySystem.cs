using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using DS7.Units;
using UnityEngine;

namespace DS7.Logistics
{
    /// <summary>
    /// Manages resupply and refueling of units.
    /// Handles costs, supply truck stock, and Auto/Ask/Manual modes.
    /// </summary>
    public class SupplySystem : MonoBehaviour
    {
        public static SupplySystem Instance { get; private set; }

        private HexGrid _grid;

        [Tooltip("Default resupply mode for human players.")]
        public ResupplyMode defaultMode = ResupplyMode.Manual;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            _grid    = HexGrid.Instance;
        }

        // ── Can Resupply Check ────────────────────────────────────────────────
        /// <summary>
        /// Returns true if the unit can be resupplied right now:
        /// either it's on a supply facility or adjacent to a supply capable unit.
        /// </summary>
        public bool CanResupply(Unit unit)
        {
            // On a resupply facility?
            var cell = _grid?.GetCell(unit.CurrentCoords);
            if (cell?.Terrain != null && cell.Terrain.canResupply)
                return true;

            // Adjacent to a supply unit?
            foreach (var neighborCoords in unit.CurrentCoords.AllNeighbors())
            {
                if (!_grid.TryGetCell(neighborCoords, out var neighborCell)) continue;
                var supplyUnit = neighborCell.GetUnit(unit.CurrentAltitude);
                if (supplyUnit == null)
                    supplyUnit = neighborCell.GetUnit(AltitudeLayer.Ground);

                if (supplyUnit != null &&
                    supplyUnit.Owner == unit.Owner &&
                    supplyUnit.Data.HasAbility(UnitAbility.Sup))
                {
                    return true; // supply truck adjacent
                }
            }

            return false;
        }

        // ── Execute Resupply ──────────────────────────────────────────────────
        /// <summary>
        /// Resupplies and refuels a unit. Deducts cost from the nation's funds
        /// and, if resupplied by a supply truck, consumes one supply stock point.
        /// </summary>
        public bool Resupply(Unit unit, Dictionary<Nation, int> nationFunds)
        {
            if (!CanResupply(unit))
            {
                Debug.LogWarning($"Cannot resupply {unit.Data.unitName}: no supply source.");
                return false;
            }

            // Deduct resupply cost from nation funds
            int cost = CalculateResupplyCost(unit);
            if (!nationFunds.TryGetValue(unit.Owner, out int funds) || funds < cost)
            {
                Debug.LogWarning($"Insufficient funds to resupply {unit.Data.unitName} (cost {cost}).");
                return false;
            }
            nationFunds[unit.Owner] = funds - cost;

            // Consume supply truck stock if applicable
            TryConsumeSupplyTruckStock(unit);

            unit.RefillAmmo();
            unit.RefillFuel();

            Debug.Log($"[Supply] {unit.Data.unitName} resupplied. Cost: {cost}. Funds left: {nationFunds[unit.Owner]}");
            return true;
        }

        // ── Resupply All ──────────────────────────────────────────────────────
        /// <summary>Attempts to resupply all units of the given nation that need it.</summary>
        public void ResupplyAll(Nation nation, List<Unit> allUnits, Dictionary<Nation, int> nationFunds)
        {
            foreach (var unit in allUnits)
            {
                if (unit.Owner != nation) continue;
                if (unit.CurrentFuel >= unit.Data.maxFuel) continue; // not needed
                Resupply(unit, nationFunds);
            }
        }

        // ── Cost Calculation ──────────────────────────────────────────────────
        private static int CalculateResupplyCost(Unit unit)
        {
            // Each unit type has a base resupply cost (simplified: production cost / 10).
            return Mathf.Max(10, unit.Data.productionCost / 10);
        }

        // ── Supply Truck Stock ────────────────────────────────────────────────
        private void TryConsumeSupplyTruckStock(Unit unit)
        {
            // Find adjacent supply truck and consume 1 supply point
            foreach (var neighborCoords in unit.CurrentCoords.AllNeighbors())
            {
                if (!_grid.TryGetCell(neighborCoords, out var cell)) continue;
                var supplyUnit = cell.GetUnit(AltitudeLayer.Ground);
                if (supplyUnit?.Owner == unit.Owner &&
                    supplyUnit.Data.HasAbility(UnitAbility.Sup))
                {
                    // Supply trucks track stock via fuel (re-using the fuel field).
                    supplyUnit.ConsumeFuel(1);
                    Debug.Log($"[Supply] {supplyUnit.Data.unitName} supply stock: {supplyUnit.CurrentFuel}");
                    return;
                }
            }
        }
    }
}
