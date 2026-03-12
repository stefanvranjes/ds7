using System.Collections.Generic;
using DS7.Data;
using DS7.Grid;
using UnityEngine;

namespace DS7.Combat
{
    /// <summary>
    /// Resolves attack and counter-attack calculations between two units.
    /// </summary>
    public static class CombatResolver
    {
        // ── Main Attack Entry Point ───────────────────────────────────────────
        /// <summary>
        /// Resolves a full attack: attacker fires, and if the defender survives
        /// and has a valid counter weapon, they fire back.
        /// </summary>
        public static void ResolveAttack(Units.Unit attacker, Units.Unit defender, HexGrid grid)
        {
            int distance = HexCoordinates.Distance(attacker.CurrentCoords, defender.CurrentCoords);

            // Select best weapon for attack
            var atkWeapon = SelectWeapon(attacker, defender, distance,
                                         attacker.HasMoved, isCounter: false);
            if (atkWeapon == null)
            {
                Debug.LogWarning($"{attacker.Data.unitName} has no valid weapon to attack {defender.Data.unitName}.");
                return;
            }

            // Resolve attacker → defender
            int atkDamage = CalculateDamage(attacker, defender, atkWeapon, grid);
            Debug.Log($"[Combat] {attacker.Data.unitName} → {defender.Data.unitName}: {atkDamage} damage ({atkWeapon.weaponName})");

            attacker.ConsumeAmmo(atkWeapon);

            bool atkMegahex = atkWeapon.isMegahex;
            if (atkMegahex)
                ApplyMegahexDamage(defender, atkDamage, grid, attacker.Owner);
            else
                ApplyDamage(defender, atkDamage);

            // Award experience
            attacker.AddExperience(atkDamage * 2);

            // Counter-attack (defender fires back if alive and in range)
            if (!defender.IsAlive) return;

            var defWeapon = SelectWeapon(defender, attacker, distance,
                                         hasMoved: false, isCounter: true);
            if (defWeapon == null) return;

            int defDamage = CalculateDamage(defender, attacker, defWeapon, grid);
            Debug.Log($"[Combat] {defender.Data.unitName} ↩ {attacker.Data.unitName}: {defDamage} damage ({defWeapon.weaponName})");

            defender.ConsumeAmmo(defWeapon);
            ApplyDamage(attacker, defDamage);
            defender.AddExperience(defDamage);
        }

        // ── Weapon Selection ──────────────────────────────────────────────────
        /// <summary>
        /// Chooses the highest-firepower valid weapon from the attacker against the defender.
        /// </summary>
        public static WeaponData SelectWeapon(Units.Unit attacker, Units.Unit defender,
                                              int distance, bool hasMoved, bool isCounter)
        {
            WeaponData best      = null;
            int        bestPower = -1;

            foreach (var weapon in attacker.Data.GetWeapons(attacker.CurrentPackIndex))
            {
                if (!IsWeaponValid(weapon, attacker, defender, distance, hasMoved, isCounter))
                    continue;
                if (attacker.GetAmmo(weapon) <= 0) continue;

                if (weapon.firePower > bestPower)
                {
                    best      = weapon;
                    bestPower = weapon.firePower;
                }
            }

            return best;
        }

        private static bool IsWeaponValid(WeaponData w, Units.Unit attacker, Units.Unit defender,
                                          int distance, bool hasMoved, bool isCounter)
        {
            // Attacker / defender flags
            if (isCounter && !w.canDefend) return false;
            if (!isCounter && !w.canAttack) return false;

            // Move & fire restriction
            if (hasMoved && !w.canMoveAndFire) return false;

            // Deploy restriction
            if (attacker.Data.HasAbility(UnitAbility.Dply) && !attacker.IsDeployed) return false;

            // Range check
            int range = w.GetRange(defender.CurrentAltitude);
            if (distance > range) return false;
            if (!w.canAdj && distance == 1) return false;

            // Elevation check
            int altDiff = (int)defender.CurrentAltitude - (int)attacker.CurrentAltitude;
            if (altDiff > w.elevationUp)   return false;
            if (-altDiff > w.elevationDown) return false;

            return true;
        }

        // ── Damage Calculation ────────────────────────────────────────────────
        /// <summary>
        /// Calculates damage from attacker to defender.
        /// Formula: (FirePower × HitRate × AttackerStrength × TerrainMod) / divisor
        /// Clamped to 1–10 (which maps to endurance points).
        /// </summary>
        public static int CalculateDamage(Units.Unit attacker, Units.Unit defender,
                                           WeaponData weapon, HexGrid grid)
        {
            float firePower  = weapon.firePower;
            float hitRate    = weapon.GetHitRate(defender.Data.unitType) / 100f;
            float strength   = attacker.CurrentEndurance / (float)attacker.Data.maxEndurance;
            float experience = 1f + (attacker.Level - 1) * 0.05f; // +5% per level

            // Terrain defense bonus
            float terrainDef = 1f;
            var defCell = grid?.GetCell(defender.CurrentCoords);
            if (defCell?.Terrain != null)
                terrainDef = 1f - defCell.Terrain.defenseBonus / 100f;

            float rawDamage = firePower * hitRate * strength * experience * terrainDef;

            // Scale to 1–10 endurance range (firePower ~100 at full strength → ~1 point damage)
            int damage = Mathf.RoundToInt(rawDamage / 10f);
            return Mathf.Clamp(damage, 0, 10);
        }

        // ── Apply Damage ──────────────────────────────────────────────────────
        private static void ApplyDamage(Units.Unit target, int damage)
        {
            if (!target.IsAlive) return;
            bool destroyed = target.TakeDamage(damage);
            if (destroyed)
            {
                Debug.Log($"[Combat] {target.Data.unitName} destroyed!");
                Object.Destroy(target.gameObject);
            }
        }

        /// <summary>Applies damage to all units in and adjacent to the target hex (7-hex area).</summary>
        private static void ApplyMegahexDamage(Units.Unit primaryTarget, int damage,
                                                HexGrid grid, Faction attackerFaction)
        {
            var affected = new HashSet<Units.Unit> { primaryTarget };
            foreach (var neighborCoords in primaryTarget.CurrentCoords.AllNeighbors())
            {
                if (!grid.TryGetCell(neighborCoords, out var cell)) continue;
                foreach (var u in cell.AllUnits())
                    if (u.Owner != attackerFaction)
                        affected.Add(u);
            }

            int splash = Mathf.Max(1, damage / 2);
            foreach (var u in affected)
                ApplyDamage(u, u == primaryTarget ? damage : splash);
        }
    }
}
