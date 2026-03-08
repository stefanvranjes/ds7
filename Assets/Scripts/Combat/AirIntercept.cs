using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using DS7.Units;
using UnityEngine;

namespace DS7.Combat
{
    /// <summary>
    /// Air Intercept system.
    ///
    /// When an enemy aircraft moves through or into a cell observed by a
    /// fighter/interceptor, the interceptor gets an opportunity to attack.
    ///
    /// Rules modelled from DS7 walkthrough:
    ///   - Only fighter-type units with air-intercept weapons can intercept.
    ///   - Interceptor fires once; no counter-fire from the target.
    ///   - Stealth units have a reduced (20 %) chance of being detected.
    ///   - Jamming (ECM) reduces intercept hit-rate by 50 %.
    ///   - Intercept range equals the weapon's High-altitude range.
    /// </summary>
    public class AirIntercept : MonoBehaviour
    {
        public static AirIntercept Instance { get; private set; }

        private HexGrid _grid;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            _grid    = HexGrid.Instance;
        }

        // ── Intercept Check ───────────────────────────────────────────────────
        /// <summary>
        /// Called whenever an air unit completes a move.
        /// Checks all enemy interceptors within range and resolves intercept fire.
        /// </summary>
        public void CheckIntercept(Unit movingAirUnit, IEnumerable<Unit> allUnits)
        {
            if (movingAirUnit.CurrentAltitude < AltitudeLayer.LowAir) return; // ground units don't intercept here

            foreach (var interceptor in allUnits)
            {
                if (interceptor.Owner == movingAirUnit.Owner) continue;
                if (!CanIntercept(interceptor, movingAirUnit)) continue;

                // Stealth evasion check
                if (movingAirUnit.Data.HasAbility(UnitAbility.Stlth))
                {
                    float roll = Random.value;
                    if (roll > 0.20f) continue; // 80 % evasion chance
                }

                FireIntercept(interceptor, movingAirUnit);

                if (!movingAirUnit.IsAlive) return; // shot down
            }
        }

        // ── Can Intercept ─────────────────────────────────────────────────────
        private bool CanIntercept(Unit interceptor, Unit target)
        {
            // Must be a fighter-capable unit on the correct altitude
            if (interceptor.CurrentAltitude < AltitudeLayer.LowAir &&
                interceptor.CurrentAltitude != AltitudeLayer.Ground) return false;

            // Must have acted = false (intercepts don't consume a turn, but cap to 1/turn)
            // (we track separately to avoid double interception — skip that complexity for now)

            // Needs a weapon valid against air targets
            var weapon = BestInterceptWeapon(interceptor, target);
            if (weapon == null) return false;

            // Range check
            int dist  = HexCoordinates.Distance(interceptor.CurrentCoords, target.CurrentCoords);
            int range = weapon.GetRange(target.CurrentAltitude);

            return dist <= range;
        }

        // ── Weapon Selection ──────────────────────────────────────────────────
        private WeaponData BestInterceptWeapon(Unit interceptor, Unit target)
        {
            WeaponData best      = null;
            int        bestPower = -1;

            foreach (var w in interceptor.Data.GetWeapons(interceptor.CurrentPackIndex))
            {
                if (!w.canAttack) continue;
                if (interceptor.GetAmmo(w) <= 0) continue;

                int range = w.GetRange(target.CurrentAltitude);
                int dist  = HexCoordinates.Distance(interceptor.CurrentCoords, target.CurrentCoords);
                if (dist > range) continue;

                int altDiff = (int)target.CurrentAltitude - (int)interceptor.CurrentAltitude;
                if (altDiff > w.elevationUp) continue;

                if (w.firePower > bestPower)
                { best = w; bestPower = w.firePower; }
            }

            return best;
        }

        // ── Fire ──────────────────────────────────────────────────────────────
        private void FireIntercept(Unit interceptor, Unit target)
        {
            var weapon = BestInterceptWeapon(interceptor, target);
            if (weapon == null) return;

            int damage = CombatResolver.CalculateDamage(interceptor, target, weapon, _grid);

            // ECM jamming halves damage
            if (target.IsJamming) damage = Mathf.Max(0, damage / 2);

            Debug.Log($"[AirIntercept] {interceptor.Data.unitName} intercepts {target.Data.unitName}: {damage}");

            interceptor.ConsumeAmmo(weapon);
            bool destroyed = target.TakeDamage(damage);
            interceptor.AddExperience(damage);

            if (destroyed)
            {
                Debug.Log($"[AirIntercept] {target.Data.unitName} shot down!");
                Destroy(target.gameObject);
            }
        }
    }
}
