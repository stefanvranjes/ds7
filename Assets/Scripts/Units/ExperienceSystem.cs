using DS7.Units;
using UnityEngine;

namespace DS7.Progression
{
    /// <summary>
    /// Dedicated helper for experience-related rules.
    /// Unit.AddExperience() handles the core level-up, but this class
    /// manages bonus XP (retreat kills, facility capture, battle end) and
    /// the persistent max-level cap from Campaign saves.
    /// </summary>
    public static class ExperienceSystem
    {
        // ── Constants ─────────────────────────────────────────────────────────
        /// <summary>XP needed per level. Level = 1 + XP / PerLevel.</summary>
        public const int XPPerLevel      = 100;

        /// <summary>Hard cap on unit level. DS7 has 5 tiers (green→veteran).</summary>
        public const int MaxLevel        = 5;

        public const int KillBonus       = 30;
        public const int CaptureBonus    = 20;
        public const int SurvivalBonus   = 10;  // awarded at scenario end

        // ── Level Names ───────────────────────────────────────────────────────
        private static readonly string[] LevelNames =
        {
            "Recruit",    // Lv 1
            "Regular",    // Lv 2
            "Veteran",    // Lv 3
            "Elite",      // Lv 4
            "Ace"         // Lv 5
        };

        // ── Helpers ───────────────────────────────────────────────────────────
        public static string GetLevelName(int level)
            => (level >= 1 && level <= MaxLevel) ? LevelNames[level - 1] : "Recruit";

        /// <summary>
        /// Awards kill bonus XP to the destroyer.
        /// Call this after a unit is confirmed destroyed.
        /// </summary>
        public static void AwardKill(Unit killer)
        {
            if (killer == null || !killer.IsAlive) return;
            killer.AddExperience(KillBonus);
            Debug.Log($"[XP] {killer.Data.unitName} awarded {KillBonus} XP for kill. ({killer.Experience} total)");
        }

        /// <summary>
        /// Awards capture bonus XP to a unit that just captured a facility.
        /// </summary>
        public static void AwardCapture(Unit captor)
        {
            if (captor == null || !captor.IsAlive) return;
            captor.AddExperience(CaptureBonus);
            Debug.Log($"[XP] {captor.Data.unitName} awarded {CaptureBonus} XP for capture.");
        }

        /// <summary>
        /// Awards survival XP to all surviving units of a nation at scenario end.
        /// </summary>
        public static void AwardSurvival(System.Collections.Generic.IEnumerable<Unit> units, Data.Nation nation)
        {
            foreach (var u in units)
            {
                if (u.Owner != nation || !u.IsAlive) continue;
                u.AddExperience(SurvivalBonus);
            }
        }

        /// <summary>
        /// Calculates the combat hit-rate bonus granted by experience.
        /// +2 % per level above 1.
        /// </summary>
        public static float GetHitRateBonus(int level)
            => (level - 1) * 0.02f;

        /// <summary>
        /// Calculates damage reduction for a seasoned defender.
        /// +1 % defence per level above 1.
        /// </summary>
        public static float GetDefenceBonus(int level)
            => (level - 1) * 0.01f;
    }
}
