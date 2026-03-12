using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using DS7.Units;
using UnityEngine;

namespace DS7.AI
{
    /// <summary>
    /// Basic AI controller for computer-controlled nations.
    /// Respects Fog of War (no intel cheating). Difficulty adjusts aggressiveness.
    /// Priority: Resupply → Attack → Capture → March toward objective.
    /// </summary>
    public class AIController : MonoBehaviour
    {
        [Range(1, 3)]
        [Tooltip("1=Easy, 2=Normal, 3=Hard")]
        public int difficulty = 1;

        public Faction controlledFaction;

        private HexGrid           _grid;
        private List<Unit>        _allUnits;
        private Logistics.SupplySystem _supply;

        private void Awake()
        {
            _grid    = HexGrid.Instance;
            _supply  = Logistics.SupplySystem.Instance;
        }

        // ── Execute AI Turn ───────────────────────────────────────────────────
        /// <summary>Called by TurnManager when this AI faction's turn begins.</summary>
        public void ExecuteTurn(List<Unit> allUnits, Dictionary<Faction, int> factionFunds)
        {
            _allUnits = allUnits;

            var myUnits = new List<Unit>();
            foreach (var u in allUnits)
                if (u.Owner == controlledFaction && u.IsAlive) myUnits.Add(u);

            foreach (var unit in myUnits)
            {
                if (!unit.IsAlive) continue;
                ActWithUnit(unit, factionFunds);
            }

            // End turn automatically
            GameModes.TurnManager.Instance?.EndFactionTurn();
        }

        // ── Unit Decision ─────────────────────────────────────────────────────
        private void ActWithUnit(Unit unit, Dictionary<Faction, int> funds)
        {
            // 1. Resupply if needed
            if (ShouldResupply(unit) && _supply.CanResupply(unit))
            {
                _supply.Resupply(unit, funds);
                return;
            }

            // 2. Attack adjacent/ranged enemy if possible
            var attackTargets = _grid.GetAttackTargets(unit, hasMoved: false);
            if (attackTargets.Count > 0)
            {
                Unit target = PickBestTarget(unit, attackTargets);
                if (target != null)
                {
                    unit.GetComponent<Units.UnitController>()?.Attack(target);

                    // After attacking, try to move if hasMoved is still false (no ZOC)
                    if (!unit.HasMoved) TryAdvance(unit);
                    return;
                }
            }

            // 3. Move toward nearest enemy or objective
            TryAdvance(unit);

            // 4. After moving, try to attack again if weapon allows move+fire
            if (!unit.HasActed)
            {
                attackTargets = _grid.GetAttackTargets(unit, hasMoved: true);
                if (attackTargets.Count > 0)
                {
                    Unit target = PickBestTarget(unit, attackTargets);
                    unit.GetComponent<Units.UnitController>()?.Attack(target);
                }
            }

            // 5. Capture if on neutral/enemy facility
            if (!unit.HasActed && unit.Data.HasAbility(UnitAbility.Capture))
            {
                var cell = _grid.GetCell(unit.CurrentCoords);
                if (cell != null && cell.IsFacility && cell.Owner != controlledFaction)
                    unit.GetComponent<Units.UnitController>()?.AttemptCapture(cell);
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private bool ShouldResupply(Unit unit)
        {
            // Resupply if below 30% fuel or any weapon fully depleted
            if (unit.CurrentFuel < unit.Data.maxFuel * 0.3f) return true;
            foreach (var w in unit.Data.GetWeapons(unit.CurrentPackIndex))
                if (unit.GetAmmo(w) == 0) return true;
            return false;
        }

        private Unit PickBestTarget(Unit attacker, List<HexCell> targetCells)
        {
            Unit   best    = null;
            int    bestHP  = int.MaxValue; // prefer weakest target on Easy

            foreach (var cell in targetCells)
            {
                foreach (var u in cell.AllUnits())
                {
                    if (u.Owner == controlledFaction) continue;

                    // Hard mode: pick highest-value target (lowest endurance → easiest kill)
                    if (difficulty >= 2)
                    {
                        if (u.CurrentEndurance < bestHP)
                        { best = u; bestHP = u.CurrentEndurance; }
                    }
                    else
                    {
                        if (best == null) best = u;
                    }
                }
            }

            return best;
        }

        private void TryAdvance(Unit unit)
        {
            // Find nearest visible enemy capital or enemy unit
            HexCoordinates? target = FindBestMoveTarget(unit);
            if (target == null) return;

            unit.SetMarchDestination(target.Value);

            var movRange = _grid.GetMovementRange(unit);
            HexCell bestCell = null;
            int     bestDist = int.MaxValue;

            foreach (var cell in movRange.StandardRange)
            {
                int d = HexCoordinates.Distance(cell.Coordinates, target.Value);
                if (d < bestDist && !cell.IsOccupied(unit.CurrentAltitude))
                { bestDist = d; bestCell = cell; }
            }

            if (bestCell != null)
                unit.GetComponent<Units.UnitController>()?.TryMove(bestCell, unit.CurrentAltitude, false);
        }

        private HexCoordinates? FindBestMoveTarget(Unit unit)
        {
            // Prioritise enemy capitals, then nearest enemy unit
            HexCoordinates? bestCapital = null;
            HexCoordinates? nearestEnemy = null;
            int minDist = int.MaxValue;

            for (int col = 0; col < _grid.width; col++)
            for (int row = 0; row < _grid.height; row++)
            {
                var cell = _grid.GetCell(col, row);
                if (cell == null || !cell.IsVisibleTo(controlledFaction)) continue;

                if (cell.Terrain?.terrainType == TerrainType.Capital &&
                    cell.Owner != controlledFaction)
                {
                    bestCapital = cell.Coordinates;
                }

                foreach (var u in cell.AllUnits())
                {
                    if (u.Owner == controlledFaction) continue;
                    int d = HexCoordinates.Distance(unit.CurrentCoords, cell.Coordinates);
                    if (d < minDist) { minDist = d; nearestEnemy = cell.Coordinates; }
                }
            }

            return bestCapital ?? nearestEnemy;
        }
    }
}
