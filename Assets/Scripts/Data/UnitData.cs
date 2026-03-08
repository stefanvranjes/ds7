using UnityEngine;

namespace DS7.Data
{
    /// <summary>
    /// Blueprint for a unit type – shared across all instances of that unit.
    /// Runtime state lives in the Unit component.
    /// </summary>
    [CreateAssetMenu(menuName = "DS7/Unit Data", fileName = "NewUnit")]
    public class UnitData : ScriptableObject
    {
        [Header("Identity")]
        public string unitName;
        public Nation nation;
        public UnitType unitType;
        [TextArea] public string description;
        public Sprite icon;

        [Header("Economy")]
        public int productionCost;

        [Header("Core Stats")]
        [Tooltip("Total hit points (endurance). 10 = full strength.")]
        [Range(1, 10)] public int maxEndurance = 10;
        public int maxFuel;
        [Tooltip("Fuel consumed per hex of standard movement on ground.")]
        public int fuelPerStandardMove = 1;
        [Tooltip("Fuel consumed per hex of high-speed movement (2x standard).")]
        public int fuelPerHighMove = 2;

        [Header("Movement")]
        [Tooltip("Standard movement allowance in hexes per turn.")]
        public int standardMove;
        [Tooltip("High-speed movement allowance in hexes per turn.")]
        public int highMove;
        [Tooltip("Movement category governs terrain cost table.")]
        public MovementCategory movementCategory;

        [Header("Load & Transport")]
        public LoadType loadType;
        [Tooltip("Transport slots this unit provides. Each entry is a max load type that can fill the slot.")]
        public LoadType[] transportSlots;

        [Header("Abilities")]
        public UnitAbility abilities;

        [Header("Weapons (by Pack)")]
        [Tooltip("Each Pack is a loadout. Pack 0 is the default.")]
        public WeaponPack[] weaponPacks;

        [Header("Detection")]
        [Tooltip("Base detection range in hexes (at ground level).")]
        public int detectionRange = 2;

        // ── Helpers ──────────────────────────────────────────────────────────

        public bool HasAbility(UnitAbility flag) => (abilities & flag) != 0;

        public WeaponData[] GetWeapons(int packIndex)
        {
            if (weaponPacks == null || weaponPacks.Length == 0) return System.Array.Empty<WeaponData>();
            packIndex = Mathf.Clamp(packIndex, 0, weaponPacks.Length - 1);
            return weaponPacks[packIndex].weapons;
        }
    }

    // ── Movement Category ─────────────────────────────────────────────────────
    public enum MovementCategory
    {
        Foot,       // infantry, special forces
        Wheeled,    // trucks, APCs
        Tracked,    // tanks, SPH
        Air,        // fixed-wing
        Helicopter,
        Naval,      // surface ships
        Submarine
    }

    // ── Weapon Pack ───────────────────────────────────────────────────────────
    [System.Serializable]
    public class WeaponPack
    {
        public string packName;
        public WeaponData[] weapons;
        [Tooltip("Facility types where this pack can be equipped.")]
        public TerrainType[] equipAt;
    }
}
