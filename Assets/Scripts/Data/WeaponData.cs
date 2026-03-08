using UnityEngine;

namespace DS7.Data
{
    /// <summary>
    /// Defines one weapon loadout entry. A unit may carry several.
    /// Range table: indices map to AltitudeLayer enum values.
    /// </summary>
    [CreateAssetMenu(menuName = "DS7/Weapon Data", fileName = "NewWeapon")]
    public class WeaponData : ScriptableObject
    {
        [Header("Identity")]
        public string weaponName;
        [Tooltip("Type label, e.g. 'Cannon', 'Missile', 'Torpedo'")]
        public string weaponType;

        [Header("Firepower & Ammo")]
        [Range(0, 200)] public int firePower;
        public int maxAmmo;

        [Header("Hit Rates (0-100)")]
        [Range(0, 100)] public int hitRateAir;
        [Range(0, 100)] public int hitRateHeli;
        [Range(0, 100)] public int hitRateVehicle;
        [Range(0, 100)] public int hitRateInfantry;
        [Range(0, 100)] public int hitRateVessel;
        [Range(0, 100)] public int hitRateSub;
        [Range(0, 100)] public int hitRateFacility;

        [Header("Range Table (hexes per altitude band)")]
        [Tooltip("Max lateral range when firing at High altitude targets")]
        public int rangeHigh;
        [Tooltip("Max lateral range when firing at Medium altitude targets")]
        public int rangeMed;
        [Tooltip("Max lateral range when firing at Low altitude targets")]
        public int rangeLow;
        [Tooltip("Max lateral range when firing at Ground/Surface targets")]
        public int rangeGround;
        [Tooltip("Max lateral range when firing at Surface (sea) targets")]
        public int rangeSurface;
        [Tooltip("Max lateral range when firing at Submerged targets")]
        public int rangeDeep;

        [Tooltip("How many altitude levels UP this weapon can fire")]
        public int elevationUp;
        [Tooltip("How many altitude levels DOWN this weapon can fire")]
        public int elevationDown;

        [Header("Weapon Flags")]
        [Tooltip("Can this weapon fire at adjacent (range-1) hex?")]
        public bool canAdj;
        [Tooltip("Can this weapon be fired after the unit has moved this turn?")]
        public bool canMoveAndFire;
        [Tooltip("Can this weapon be used for normal attacks?")]
        public bool canAttack = true;
        [Tooltip("Can this weapon be used for counter-attacks / opportunity fire?")]
        public bool canDefend = true;
        [Tooltip("Can this weapon bomb (damage) a facility?")]
        public bool canBomb;
        [Tooltip("Is this a missile-type weapon (affected by Jam)?")]
        public bool isMissile;
        [Tooltip("Does this weapon deal Megahex (AoE to all 7 hexes) damage?")]
        public bool isMegahex;

        // ── Helpers ──────────────────────────────────────────────────────────

        /// <summary>Returns the lateral range of this weapon against a target at the given altitude.</summary>
        public int GetRange(AltitudeLayer targetAlt)
        {
            switch (targetAlt)
            {
                case AltitudeLayer.HighAir:
                    return rangeHigh;
                case AltitudeLayer.MedAir:
                    return rangeMed;
                case AltitudeLayer.LowAir:
                    return rangeLow;
                case AltitudeLayer.Ground:
                    return rangeGround;
                case AltitudeLayer.Surface:
                    return rangeSurface;
                case AltitudeLayer.DeepSea:
                    return rangeDeep;
                default:
                    return 0;
            }
        }

        /// <summary>Returns the hit rate of this weapon against the given target unit type.</summary>
        public int GetHitRate(UnitType targetType)
        {
            switch (targetType)
            {
                case UnitType.Air:
                    return hitRateAir;
                case UnitType.Helicopter:
                    return hitRateHeli;
                case UnitType.Vehicle:
                    return hitRateVehicle;
                case UnitType.Infantry:
                    return hitRateInfantry;
                case UnitType.Vessel:
                    return hitRateVessel;
                case UnitType.Submarine:
                    return hitRateSub;
                default:
                    return 0;
            }
        }
    }
}
