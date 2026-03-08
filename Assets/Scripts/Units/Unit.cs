using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using UnityEngine;

namespace DS7.Units
{
    /// <summary>
    /// Runtime representation of a unit on the battlefield.
    /// Holds all mutable state: position, fuel, ammo, endurance, experience.
    /// </summary>
    public class Unit : MonoBehaviour
    {
        // ── Blueprint ─────────────────────────────────────────────────────────
        public UnitData Data { get; private set; }

        // ── Ownership ─────────────────────────────────────────────────────────
        public Nation Owner { get; private set; }

        // ── Position ──────────────────────────────────────────────────────────
        public HexCoordinates CurrentCoords   { get; private set; }
        public AltitudeLayer  CurrentAltitude { get; private set; } = AltitudeLayer.Ground;

        // ── Combat Stats ──────────────────────────────────────────────────────
        public int CurrentEndurance { get; private set; }
        public int CurrentFuel      { get; private set; }

        /// <summary>Current weapon pack index (0 = default).</summary>
        public int CurrentPackIndex { get; private set; } = 0;

        private readonly Dictionary<WeaponData, int> _ammo = new();

        // ── Turn State ────────────────────────────────────────────────────────
        public bool HasMoved   { get; private set; }
        public bool HasActed   { get; private set; }
        public bool IsDeployed { get; private set; }

        /// <summary>Destination set for March automation. Null = no march order.</summary>
        public HexCoordinates? MarchDestination { get; private set; }

        // ── Transport ─────────────────────────────────────────────────────────
        /// <summary>Units loaded into this transport (if it has the Trans ability).</summary>
        public List<Unit> CargoUnits { get; } = new();

        /// <summary>The transport this unit is currently loaded onto (null if free).</summary>
        public Unit TransportParent { get; private set; }

        // ── Experience ────────────────────────────────────────────────────────
        public int Experience { get; private set; }
        public int Level      { get; private set; } = 1;

        // ── ECM ───────────────────────────────────────────────────────────────
        public bool IsJamming { get; private set; }
        private int _ecmAmmo;

        // ── Initializer ───────────────────────────────────────────────────────
        public void Initialize(UnitData data, Nation owner, HexCoordinates startCoords,
                               AltitudeLayer startAlt = AltitudeLayer.Ground)
        {
            Data             = data;
            Owner            = owner;
            CurrentCoords    = startCoords;
            CurrentAltitude  = startAlt;
            CurrentEndurance = data.maxEndurance;
            CurrentFuel      = data.maxFuel;

            // Initialize ammo for default pack
            SetPack(0);
        }

        // ── Fuel & Ammo ───────────────────────────────────────────────────────
        public int GetAmmo(WeaponData weapon)
            => _ammo.TryGetValue(weapon, out int a) ? a : 0;

        public void ConsumeAmmo(WeaponData weapon, int amount = 1)
        {
            if (_ammo.TryGetValue(weapon, out int a))
                _ammo[weapon] = Mathf.Max(0, a - amount);
        }

        public void RefillAmmo()
        {
            foreach (var w in Data.GetWeapons(CurrentPackIndex))
                _ammo[w] = w.maxAmmo;
        }

        public void ConsumeFuel(int amount)
            => CurrentFuel = Mathf.Max(0, CurrentFuel - amount);

        public void RefillFuel()
            => CurrentFuel = Data.maxFuel;

        public bool IsOutOfFuel => CurrentFuel <= 0;

        // ── Damage ────────────────────────────────────────────────────────────
        /// <summary>Apply damage. Returns true if unit is destroyed.</summary>
        public bool TakeDamage(int amount)
        {
            CurrentEndurance = Mathf.Max(0, CurrentEndurance - amount);
            return CurrentEndurance <= 0;
        }

        public bool IsAlive => CurrentEndurance > 0;

        public void Repair(int amount)
            => CurrentEndurance = Mathf.Min(Data.maxEndurance, CurrentEndurance + amount);

        // ── Equipment Pack ────────────────────────────────────────────────────
        public void SetPack(int index)
        {
            CurrentPackIndex = index;
            _ammo.Clear();
            foreach (var w in Data.GetWeapons(index))
                _ammo[w] = w.maxAmmo;
        }

        // ── Movement ──────────────────────────────────────────────────────────
        public void MoveTo(HexCoordinates destination, AltitudeLayer altitude,
                           int fuelConsumed, bool highSpeed)
        {
            CurrentCoords   = destination;
            CurrentAltitude = altitude;
            ConsumeFuel(fuelConsumed);
            HasMoved = true;
        }

        public void SetAltitude(AltitudeLayer alt)
        {
            CurrentAltitude = alt;
        }

        // ── Turn Reset ────────────────────────────────────────────────────────
        public void StartTurn()
        {
            HasMoved   = false;
            HasActed   = false;
            IsJamming  = false;
        }

        public void MarkActed() => HasActed = true;

        // ── March ─────────────────────────────────────────────────────────────
        public void SetMarchDestination(HexCoordinates? dest)
            => MarchDestination = dest;

        // ── Deploy ────────────────────────────────────────────────────────────
        public void Deploy()
        {
            IsDeployed = true;
            HasActed   = true;
        }

        public void Undeploy() => IsDeployed = false;

        // ── Jam (ECM) ─────────────────────────────────────────────────────────
        public void ActivateJam()
        {
            if (_ecmAmmo <= 0) return;
            _ecmAmmo--;
            IsJamming = true;
            HasActed  = true;
        }

        // ── Transport ─────────────────────────────────────────────────────────
        public bool CanLoad(Unit cargo)
        {
            if (!Data.HasAbility(UnitAbility.Trans)) return false;
            // Count occupied slots and check load type compatibility
            // (simplified: just check if slot count is not exceeded)
            return CargoUnits.Count < (Data.transportSlots?.Length ?? 0);
        }

        public void Load(Unit cargo)
        {
            CargoUnits.Add(cargo);
            cargo.TransportParent = this;
            cargo.gameObject.SetActive(false);
        }

        public void Unload(Unit cargo)
        {
            CargoUnits.Remove(cargo);
            cargo.TransportParent = null;
            cargo.gameObject.SetActive(true);
        }

        // ── Experience ────────────────────────────────────────────────────────
        public void AddExperience(int xp)
        {
            Experience += xp;
            int newLevel = 1 + Experience / 100;
            if (newLevel > Level)
            {
                Level = newLevel;
                Debug.Log($"{Data.unitName} leveled up to {Level}!");
            }
        }

        // ── Serialization (Campaign save) ────────────────────────────────────
        public SavedUnitState ToSaveState() => new()
        {
            unitDataName  = Data.name,
            owner         = Owner,
            experience    = Experience,
            level         = Level,
            endurance     = CurrentEndurance,
            fuel          = CurrentFuel
        };
    }

    // ── Saved Unit State (for Campaign persistence) ───────────────────────────
    [System.Serializable]
    public class SavedUnitState
    {
        public string unitDataName;
        public Nation owner;
        public int    experience;
        public int    level;
        public int    endurance;
        public int    fuel;
    }
}
