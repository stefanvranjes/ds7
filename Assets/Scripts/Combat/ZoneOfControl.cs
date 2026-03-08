using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using UnityEngine;

namespace DS7.Combat
{
    /// <summary>
    /// Manages Zone of Control: rebuilds the ZOC map each turn and
    /// handles movement interruption when a unit enters enemy ZOC.
    /// </summary>
    public class ZoneOfControl : MonoBehaviour
    {
        public static ZoneOfControl Instance { get; private set; }

        private HexGrid _grid;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            _grid    = HexGrid.Instance;
        }

        // ── Rebuild ZOC map ───────────────────────────────────────────────────
        /// <summary>
        /// Called at the start of each nation's turn.
        /// Clears old ZOC and recalculates based on all ground/surface units.
        /// </summary>
        public void RebuildZOC(IEnumerable<Units.Unit> allUnits)
        {
            // Clear all ZOC
            if (_grid == null) _grid = HexGrid.Instance;

            for (int col = 0; col < _grid.width; col++)
            for (int row = 0; row < _grid.height; row++)
            {
                var cell = _grid.GetCell(col, row);
                cell?.ZocNations.Clear();
            }

            // Apply ZOC for all ground and surface units
            foreach (var unit in allUnits)
            {
                if (!ExertsZOC(unit)) continue;

                foreach (var neighborCoords in unit.CurrentCoords.AllNeighbors())
                {
                    if (!_grid.TryGetCell(neighborCoords, out var cell)) continue;
                    cell.ZocNations.Add(unit.Owner);
                }
            }
        }

        // ── ZOC Exertion Rules ────────────────────────────────────────────────
        /// <summary>Only ground units and surface vessels exert ZOC (not air/sub).</summary>
        private static bool ExertsZOC(Units.Unit unit)
        {
            return unit.Data.unitType is UnitType.Infantry
                                      or UnitType.Vehicle
                                      or UnitType.Vessel;
        }

        // ── Opportunity Fire ──────────────────────────────────────────────────
        /// <summary>
        /// Called when a unit enters a cell in enemy ZOC during movement.
        /// Triggers immediate fire from every adjacent enemy capable of firing.
        /// The moving unit cannot return fire.
        /// </summary>
        public void TriggerOpportunityFire(Units.Unit movingUnit, HexCell enteredCell,
                                           IEnumerable<Units.Unit> allEnemyUnits)
        {
            foreach (var enemy in allEnemyUnits)
            {
                if (enemy.Owner == movingUnit.Owner) continue;
                if (HexCoordinates.Distance(enemy.CurrentCoords, enteredCell.Coordinates) > 1)
                    continue;

                var weapon = CombatResolver.SelectWeapon(enemy, movingUnit,
                    HexCoordinates.Distance(enemy.CurrentCoords, enteredCell.Coordinates),
                    hasMoved: false, isCounter: true);

                if (weapon == null) continue;

                int damage = CombatResolver.CalculateDamage(enemy, movingUnit, weapon, _grid);
                Debug.Log($"[ZOC] Opportunity fire: {enemy.Data.unitName} → {movingUnit.Data.unitName}: {damage}");
                enemy.ConsumeAmmo(weapon);
                movingUnit.TakeDamage(damage);

                if (!movingUnit.IsAlive)
                {
                    Destroy(movingUnit.gameObject);
                    return;
                }
            }
        }
    }
}
