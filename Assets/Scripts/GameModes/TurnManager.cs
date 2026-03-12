using System;
using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using DS7.Units;
using DS7.Combat;
using DS7.Logistics;
using UnityEngine;

namespace DS7.GameModes
{
    /// <summary>
    /// Manages the turn order, per-turn resource income, ZOC rebuild, and end-of-turn resets.
    /// Also checks victory conditions after each turn.
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance { get; private set; }

        // ── References ────────────────────────────────────────────────────────
        private HexGrid     _grid;
        private ZoneOfControl _zoc;
        private SupplySystem  _supply;

        // ── State ─────────────────────────────────────────────────────────────
        [Header("Turn Order")]
        public List<FactionData> turnOrder = new();

        private int _currentNationIndex;
        private int _turnNumber = 1;

        public Faction ActiveFaction => turnOrder.Count > 0
            ? turnOrder[_currentNationIndex].faction
            : Faction.Neutral;

        public int TurnNumber => _turnNumber;

        // ── Events ────────────────────────────────────────────────────────────
        public event Action<Faction> OnTurnStart;
        public event Action<Faction> OnTurnEnd;
        public event Action<int>     OnTurnNumberChanged;
        public event Action<Faction> OnVictory;

        // ── Runtime unit list ─────────────────────────────────────────────────
        private List<Unit> _allUnits = new();

        public void RegisterUnit(Unit unit)   => _allUnits.Add(unit);
        public void UnregisterUnit(Unit unit) => _allUnits.Remove(unit);

        // ── Faction funds (runtime) ────────────────────────────────────────────
        public Dictionary<Faction, int> FactionFunds { get; } = new();

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            _grid    = HexGrid.Instance;
            _zoc     = ZoneOfControl.Instance;
            _supply  = SupplySystem.Instance;
        }

        public void StartGame()
        {
            _currentNationIndex = 0;
            _turnNumber         = 1;

            foreach (var f in turnOrder)
                FactionFunds[f.faction] = f.selectedNation != null ? f.selectedNation.baseFundRate : 100;

            BeginFactionTurn();
        }

        // ── Turn Flow ─────────────────────────────────────────────────────────
        private void BeginFactionTurn()
        {
            Faction active = ActiveFaction;

            // 1. Rebuild ZOC
            _zoc?.RebuildZOC(_allUnits);

            // 2. Collect facility income
            CollectIncome(active);

            // 3. Execute march orders
            ExecuteMarchOrders(active);

            // 4. Reset unit turn flags
            foreach (var unit in _allUnits)
                if (unit.Owner == active)
                    unit.StartTurn();

            // 5. Auto-resupply if mode is Auto
            if (_supply != null && _supply.defaultMode == ResupplyMode.Auto)
                _supply.ResupplyAll(active, _allUnits, FactionFunds);

            OnTurnStart?.Invoke(active);

            Debug.Log($"[Turn {_turnNumber}] {active}'s turn begins. Funds: {FactionFunds.GetValueOrDefault(active, 0)}");
        }

        /// <summary>Called by the player (or AI) when they press End Turn.</summary>
        public void EndFactionTurn()
        {
            Faction active = ActiveFaction;
            OnTurnEnd?.Invoke(active);

            CheckVictoryConditions();

            // Advance to next faction
            _currentNationIndex = (_currentNationIndex + 1) % turnOrder.Count;

            // If we wrapped back to the first nation, a new full turn begins
            if (_currentNationIndex == 0)
            {
                _turnNumber++;
                OnTurnNumberChanged?.Invoke(_turnNumber);
            }

            BeginFactionTurn();
        }

        // ── Income ────────────────────────────────────────────────────────────
        private void CollectIncome(Faction faction)
        {
            int income = 0;

            for (int col = 0; col < _grid.width; col++)
            for (int row = 0; row < _grid.height; row++)
            {
                var cell = _grid.GetCell(col, row);
                if (cell?.Owner == faction && cell.Terrain != null)
                {
                    // Apply bombardment damage reduction
                    float healthFactor = cell.facilityHealth / 100f;
                    income += Mathf.RoundToInt(cell.Terrain.incomePerTurn * healthFactor);
                }
            }

            if (!FactionFunds.ContainsKey(faction)) FactionFunds[faction] = 0;
            FactionFunds[faction] += income;
        }

        // ── March Orders ──────────────────────────────────────────────────────
        private void ExecuteMarchOrders(Faction faction)
        {
            foreach (var unit in _allUnits)
            {
                if (unit.Owner != faction) continue;
                if (unit.MarchDestination == null) continue;

                // Move one step toward destination each turn
                // (simplified: direct move toward destination within move allowance)
                var dest = unit.MarchDestination.Value;
                if (unit.CurrentCoords == dest)
                {
                    unit.SetMarchDestination(null);
                    continue;
                }

                // Use UnitController to move toward destination
                var controller = unit.GetComponent<Units.UnitController>();
                if (controller == null) continue;

                var movRange = _grid.GetMovementRange(unit);
                // Find the reachable cell closest to destination
                HexCell bestCell = null;
                int bestDist = int.MaxValue;

                foreach (var cell in movRange.StandardRange)
                {
                    int d = HexCoordinates.Distance(cell.Coordinates, dest);
                    if (d < bestDist) { bestDist = d; bestCell = cell; }
                }

                if (bestCell != null)
                    controller.TryMove(bestCell, unit.CurrentAltitude, highSpeed: false);

                if (unit.CurrentCoords == dest)
                    unit.SetMarchDestination(null);
            }
        }

        // ── Unit Production ───────────────────────────────────────────────────
        /// <summary>
        /// Attempts to produce a unit at the given facility cell.
        /// Deducts production cost from faction funds.
        /// </summary>
        public Unit ProduceUnit(HexCell cell, UnitData unitData, Faction faction, GameObject unitPrefab)
        {
            if (!cell.Terrain.canProduce)
            {
                Debug.LogWarning("This facility cannot produce units.");
                return null;
            }

            if (cell.IsOccupied(AltitudeLayer.Ground))
            {
                Debug.LogWarning("Production cell is occupied.");
                return null;
            }

            if (!FactionFunds.TryGetValue(faction, out int funds) || funds < unitData.productionCost)
            {
                Debug.LogWarning($"Insufficient funds to produce {unitData.unitName}.");
                return null;
            }

            FactionFunds[faction] = funds - unitData.productionCost;

            var go   = Instantiate(unitPrefab, cell.Coordinates.ToWorldPosition(_grid.hexSize), Quaternion.identity);
            var unit = go.GetComponent<Unit>();
            unit.Initialize(unitData, faction, cell.Coordinates);
            cell.TryPlace(unit, AltitudeLayer.Ground);
            _allUnits.Add(unit);

            Debug.Log($"[Production] {unitData.unitName} produced at {cell.Coordinates}.");
            return unit;
        }

        // ── Victory Conditions ────────────────────────────────────────────────
        private void CheckVictoryConditions()
        {
            // Default: capital capture → victory
            var capturedCapitals = new HashSet<Faction>();

            for (int col = 0; col < _grid.width; col++)
            for (int row = 0; row < _grid.height; row++)
            {
                var cell = _grid.GetCell(col, row);
                if (cell == null) continue;
                if (cell.Terrain?.terrainType != TerrainType.Capital) continue;
                if (cell.Owner != Faction.Neutral)
                    capturedCapitals.Add(cell.Owner);
            }

            // If only one faction has all capitals (others captured), signal victory
            // (Scenario-specific logic is handled by MissionMode/CampaignMode)
            GameManager.Instance?.CheckVictory(capturedCapitals, _allUnits);
        }
    }
}
