using System.Collections.Generic;
using DS7.Data;
using DS7.Units;
using UnityEngine;

namespace DS7.GameModes
{
    /// <summary>
    /// Campaign Mode: branching battle map, experience carries between battles,
    /// saved unit roster (up to 20 per nation).
    /// </summary>
    public class CampaignMode : MonoBehaviour
    {
        [Header("Campaign Config")]
        public int campaignIndex;
        public string campaignName;
        public int battleIndex;
        [TextArea] public string objective;
        public VictoryConditionType victoryCondition;

        [Header("Branch Grades")]
        [Tooltip("Grade required to take the 'higher path'. S/A → higher, B/C/D → lower.")]
        public MissionGrade branchThreshold = MissionGrade.B;
        public int higherBranchBattleIndex;
        public int lowerBranchBattleIndex;

        private TurnManager _turns;
        private bool        _battleComplete;

        private void Start()
        {
            _turns = TurnManager.Instance;
            if (_turns != null)
                _turns.OnVictory += OnVictory;
        }

        private void OnVictory(Faction winner)
        {
            if (_battleComplete) return;
            _battleComplete = true;

            int turnsUsed = _turns?.TurnNumber ?? 1;
            var grade     = EvaluateGrade(turnsUsed);

            Debug.Log($"[Campaign {campaignIndex}:{battleIndex}] Complete! Grade: {grade}");

            // Allow player to save up to 20 leveled units
            OpenSaveUnitsScreen(winner);

            // Determine next battle based on grade
            int nextBattle = grade <= branchThreshold
                ? higherBranchBattleIndex
                : lowerBranchBattleIndex;

            PlayerPrefs.SetInt("BattleIndex", nextBattle);
            Debug.Log($"[Campaign] Branching to battle {nextBattle}.");
        }

        private MissionGrade EvaluateGrade(int turnsUsed)
        {
            if (turnsUsed <= 10) return MissionGrade.S;
            if (turnsUsed <= 15) return MissionGrade.A;
            if (turnsUsed <= 20) return MissionGrade.B;
            if (turnsUsed <= 30) return MissionGrade.C;
            return MissionGrade.D;
        }

        // ── Save Units Screen ─────────────────────────────────────────────────
        private void OpenSaveUnitsScreen(Faction winner)
        {
            // In a full implementation this opens a UI letting the player pick
            // which leveled units to save (up to 20). For now, auto-save all
            // surviving player units with XP.
            var savedList = GameManager.Instance?.CampaignSavedUnits;
            if (savedList == null) return;

            // This would be populated by a UI picker in the full game
            Debug.Log("[Campaign] Save units screen would open here.");
        }
    }
}
