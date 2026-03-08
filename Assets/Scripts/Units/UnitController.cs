using DS7.Data;
using DS7.Grid;
using UnityEngine;

namespace DS7.Units
{
    /// <summary>
    /// Handles issuing commands to a Unit: Move, Attack, Resupply, Capture, etc.
    /// Acts as the bridge between UI/AI intent and the Unit's state.
    /// </summary>
    [RequireComponent(typeof(Unit))]
    public class UnitController : MonoBehaviour
    {
        private Unit       _unit;
        private HexGrid    _grid;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _grid = HexGrid.Instance;
        }

        // ── Move ──────────────────────────────────────────────────────────────
        /// <summary>
        /// Attempts to move the unit to the target cell at the given altitude.
        /// Returns true on success.
        /// </summary>
        public bool TryMove(HexCell targetCell, AltitudeLayer altitude, bool highSpeed)
        {
            if (_unit.HasMoved)
            {
                Debug.LogWarning($"{_unit.Data.unitName} has already moved.");
                return false;
            }

            if (targetCell.IsOccupied(altitude) &&
                targetCell.GetUnit(altitude)?.Owner != _unit.Owner)
            {
                Debug.LogWarning("Target hex/altitude is occupied by an enemy.");
                return false;
            }

            // Calculate fuel cost (simple: terrain cost × fuel rate)
            int range   = HexCoordinates.Distance(_unit.CurrentCoords, targetCell.Coordinates);
            int baseMove = highSpeed ? _unit.Data.highMove : _unit.Data.standardMove;
            int fuelRate = highSpeed ? _unit.Data.fuelPerHighMove : _unit.Data.fuelPerStandardMove;
            int fuelCost = range * fuelRate;

            if (_unit.CurrentFuel < fuelCost)
            {
                Debug.LogWarning($"Insufficient fuel for move ({_unit.CurrentFuel}/{fuelCost}).");
                return false;
            }

            // Update grid occupancy
            var currentCell = _grid.GetCell(_unit.CurrentCoords);
            currentCell?.RemoveUnit(_unit.CurrentAltitude);
            targetCell.TryPlace(_unit, altitude);

            // Update unit position
            _unit.MoveTo(targetCell.Coordinates, altitude, fuelCost, highSpeed);

            // Undeploy if deployed unit moves
            if (_unit.IsDeployed) _unit.Undeploy();

            // Move transform
            transform.position = targetCell.Coordinates.ToWorldPosition(_grid.hexSize)
                                  + Vector3.up * (int)altitude * 0.5f;

            Debug.Log($"{_unit.Data.unitName} moved to {targetCell.Coordinates}.");
            return true;
        }

        // ── Attack ────────────────────────────────────────────────────────────
        /// <summary>
        /// Resolves an attack against an enemy unit using the best available weapon.
        /// Delegates full resolution to CombatResolver.
        /// </summary>
        public void Attack(Unit target)
        {
            if (_unit.HasActed)
            {
                Debug.LogWarning($"{_unit.Data.unitName} has already acted.");
                return;
            }

            Combat.CombatResolver.ResolveAttack(_unit, target, _grid);
            _unit.MarkActed();
        }

        // ── Resupply ──────────────────────────────────────────────────────────
        /// <summary>
        /// Resupplies fuel and ammo. The calling system checks that a supply
        /// source is adjacent or the unit is on a facility first.
        /// </summary>
        public void Resupply()
        {
            _unit.RefillAmmo();
            _unit.RefillFuel();
            // Actual cost deduction is handled by SupplySystem
        }

        // ── Capture ───────────────────────────────────────────────────────────
        public void AttemptCapture(HexCell cell)
        {
            if (!_unit.Data.HasAbility(UnitAbility.Capture))
            {
                Debug.LogWarning($"{_unit.Data.unitName} cannot capture facilities.");
                return;
            }

            if (cell.Owner == _unit.Owner)
            {
                Debug.LogWarning("Already own this facility.");
                return;
            }

            cell.CaptureProgress++;
            int required = cell.Terrain.terrainType == TerrainType.Capital ? 3 : 2;
            if (cell.CaptureProgress >= required)
            {
                cell.Owner = _unit.Owner;
                cell.CaptureProgress = 0;
                Debug.Log($"{_unit.Owner} captured {cell.Terrain.terrainName}!");
            }

            _unit.MarkActed();
        }

        // ── Deploy ────────────────────────────────────────────────────────────
        public void Deploy()
        {
            if (!_unit.Data.HasAbility(UnitAbility.Dply))
            {
                Debug.LogWarning($"{_unit.Data.unitName} has no DPLY ability.");
                return;
            }

            _unit.Deploy();
        }

        // ── Jam ───────────────────────────────────────────────────────────────
        public void ActivateJam()
        {
            if (!_unit.Data.HasAbility(UnitAbility.Jam))
            {
                Debug.LogWarning($"{_unit.Data.unitName} has no JAM ability.");
                return;
            }

            _unit.ActivateJam();
        }

        // ── Equip ─────────────────────────────────────────────────────────────
        /// <summary>
        /// Changes weapon pack. Must be at an appropriate facility, full HP and supply.
        /// </summary>
        public void EquipPack(int packIndex, HexCell currentCell)
        {
            if (_unit.HasMoved || _unit.HasActed)
            {
                Debug.LogWarning("Cannot equip after moving or acting.");
                return;
            }

            if (_unit.CurrentEndurance < _unit.Data.maxEndurance ||
                _unit.CurrentFuel      < _unit.Data.maxFuel)
            {
                Debug.LogWarning("Unit must be at full supply and health to reequip.");
                return;
            }

            _unit.SetPack(packIndex);
            _unit.MarkActed();
        }

        // ── Load / Unload (Transport) ─────────────────────────────────────────
        public bool TryLoad(Unit cargo)
        {
            if (!_unit.CanLoad(cargo)) return false;
            _unit.Load(cargo);
            return true;
        }

        public void Unload(Unit cargo, HexCell destination)
        {
            _unit.Unload(cargo);
            // Position cargo adjacent to transport
            destination.TryPlace(cargo, AltitudeLayer.Ground);
            cargo.transform.position = destination.Coordinates.ToWorldPosition(_grid.hexSize);
        }

        // ── Combine Units ─────────────────────────────────────────────────────
        /// <summary>
        /// Merges a weak allied unit into this unit (restores endurance).
        /// The merged unit is then destroyed.
        /// </summary>
        public void CombineWith(Unit other)
        {
            if (other.Owner != _unit.Owner || other.Data != _unit.Data)
            {
                Debug.LogWarning("Can only combine units of the same type and nation.");
                return;
            }

            int replenish = other.CurrentEndurance;
            _unit.Repair(replenish);

            var otherCell = _grid.GetCell(other.CurrentCoords);
            otherCell?.RemoveUnit(other.CurrentAltitude);
            Destroy(other.gameObject);

            Debug.Log($"{_unit.Data.unitName} combined. Endurance now {_unit.CurrentEndurance}.");
        }
    }
}
