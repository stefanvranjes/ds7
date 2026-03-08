using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using DS7.Units;
using UnityEngine;

namespace DS7.Logistics
{
    /// <summary>
    /// Manages loading, unloading, and transporting of units by other units.
    /// Enforces DS7 transport rules: airports for fixed-wing, ports for ships, etc.
    /// </summary>
    public class TransportSystem : MonoBehaviour
    {
        public static TransportSystem Instance { get; private set; }

        private HexGrid _grid;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            _grid    = HexGrid.Instance;
        }

        // ── Load Rules Check ──────────────────────────────────────────────────
        /// <summary>Returns true if cargo can be loaded onto transport at the given cell.</summary>
        public bool CanLoad(Unit transport, Unit cargo, HexCell cell)
        {
            if (!transport.Data.HasAbility(UnitAbility.Trans)) return false;
            if (!transport.CanLoad(cargo)) return false;

            bool isTransportAircraft = transport.Data.unitType == UnitType.Air;
            bool isTransportHeli     = transport.Data.unitType == UnitType.Helicopter;
            bool isTransportVessel   = transport.Data.unitType == UnitType.Vessel;

            // Transport aircraft: load only at airports
            if (isTransportAircraft)
                return cell.Terrain.terrainType == TerrainType.Airport;

            // Transport helicopter / VTOL: load wherever they can land
            if (isTransportHeli)
                return CanHelicopterLand(transport, cell);

            // Ground transport (truck): load anywhere
            if (!isTransportAircraft && !isTransportHeli && !isTransportVessel)
                return true;

            // Carriers: load air units only while at sea
            if (isTransportVessel)
            {
                bool isCarrier = transport.Data.HasAbility(UnitAbility.Trans) &&
                                 (cargo.Data.unitType == UnitType.Air ||
                                  cargo.Data.unitType == UnitType.Helicopter);
                return isCarrier && (cell.Terrain.terrainType == TerrainType.Sea ||
                                     cell.Terrain.terrainType == TerrainType.Shallows);
            }

            return false;
        }

        // ── Unload Rules Check ────────────────────────────────────────────────
        /// <summary>Returns true if cargo can be unloaded from transport at the given cell.</summary>
        public bool CanUnload(Unit transport, Unit cargo, HexCell cell)
        {
            bool isVessel = transport.Data.unitType == UnitType.Vessel;
            bool isHeli   = transport.Data.unitType == UnitType.Helicopter;

            // Transport ships: must be at port
            if (isVessel && !transport.Data.HasAbility(UnitAbility.LAir))
                return cell.Terrain.terrainType == TerrainType.Port;

            // Assault ships: unload at ports or shallows
            if (isVessel && transport.Data.HasAbility(UnitAbility.LAir))
                return cell.Terrain.terrainType == TerrainType.Port ||
                       cell.Terrain.terrainType == TerrainType.Shallows;

            // Heli transport: unload wherever they can land
            if (isHeli)
                return CanHelicopterLand(transport, cell);

            // Ground transport: adjacent free hex
            return cell.GetUnit(AltitudeLayer.Ground) == null;
        }

        // ── Paratrooper Drop ──────────────────────────────────────────────────
        /// <summary>
        /// Checks if a paratrooper/special forces unit can be dropped from the given aircraft.
        /// Allowed from Medium or Low altitude (not High).
        /// </summary>
        public bool CanParadrop(Unit aircraft, Unit cargo, out string reason)
        {
            reason = string.Empty;

            if (aircraft.CurrentAltitude == AltitudeLayer.HighAir)
            { reason = "Cannot drop from High altitude."; return false; }

            bool isParatrooper = cargo.Data.HasAbility(UnitAbility.Capture); // proxy flag
            bool isLAir        = cargo.Data.HasAbility(UnitAbility.LAir);

            if (isLAir && aircraft.CurrentAltitude != AltitudeLayer.LowAir)
            { reason = "LAir units can only drop from Low altitude."; return false; }

            return true;
        }

        // ── Execute Load ──────────────────────────────────────────────────────
        public bool TryLoad(Unit transport, Unit cargo, HexCell cell)
        {
            if (!CanLoad(transport, cargo, cell)) return false;

            var cargoCell = _grid.GetCell(cargo.CurrentCoords);
            cargoCell?.RemoveUnit(cargo.CurrentAltitude);

            transport.Load(cargo);

            Debug.Log($"[Transport] {cargo.Data.unitName} loaded onto {transport.Data.unitName}.");
            return true;
        }

        // ── Execute Unload ────────────────────────────────────────────────────
        public bool TryUnload(Unit transport, Unit cargo, HexCell destinationCell)
        {
            if (!CanUnload(transport, cargo, destinationCell)) return false;

            transport.Unload(cargo);
            destinationCell.TryPlace(cargo, AltitudeLayer.Ground);
            cargo.transform.position = destinationCell.Coordinates.ToWorldPosition(_grid.hexSize);

            Debug.Log($"[Transport] {cargo.Data.unitName} unloaded to {destinationCell.Coordinates}.");
            return true;
        }

        // ── Execute Paradrop ──────────────────────────────────────────────────
        public bool TryParadrop(Unit aircraft, Unit cargo, HexCell target)
        {
            if (!CanParadrop(aircraft, cargo, out var reason))
            {
                Debug.LogWarning($"[Transport] Paradrop blocked: {reason}");
                return false;
            }

            aircraft.Unload(cargo);
            target.TryPlace(cargo, AltitudeLayer.Ground);
            cargo.transform.position = target.Coordinates.ToWorldPosition(_grid.hexSize);

            // Paratroopers use their turn during the drop
            cargo.MarkActed();

            Debug.Log($"[Transport] {cargo.Data.unitName} paradropped to {target.Coordinates}.");
            return true;
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private static bool CanHelicopterLand(Unit heli, HexCell cell)
        {
            // Helicopter can land on most terrain; not deep sea
            return cell.Terrain.terrainType != TerrainType.DeepSea &&
                   cell.Terrain.terrainType != TerrainType.Sea;
        }
    }
}
