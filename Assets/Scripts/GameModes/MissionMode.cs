using System.Collections.Generic;
using DS7.Data;
using DS7.Units;
using UnityEngine;

namespace DS7.GameModes
{
    /// <summary>
    /// Mission Mode: fixed scenario, S–F grading, unlockables.
    /// Attach this to the Battlefield scene root alongside TurnManager.
    /// </summary>
    public class MissionMode : MonoBehaviour
    {
        [Header("Mission Config")]
        public int missionIndex;
        public string missionName;
        [TextArea] public string objective;

        [Header("Turn Limit")]
        [Tooltip("0 = no turn limit")]
        public int turnLimit = 0;

        [Header("Victory Condition")]
        public VictoryConditionType victoryCondition;
        public int targetFacilityCount;  // for FacilityCount condition
        public int targetFundAmount;     // for FundRace condition
        public int minAliveUnits;        // for UnitSurvival condition

        [Header("Grading (S/A/B/C/D/F)")]
        public int turnsForS = 10;
        public int turnsForA = 15;
        public int turnsForB = 20;

        [Header("Unlockables")]
        public List<string> unlockedUnits;
        public List<string> unlockedMaps;

        private TurnManager _turns;
        private int         _startTurn;
        private bool        _missionComplete;

        private void Start()
        {
            _turns     = TurnManager.Instance;
            _startTurn = _turns?.TurnNumber ?? 1;

            if (_turns != null)
            {
                _turns.OnTurnEnd += OnTurnEnd;
                _turns.OnVictory += OnVictory;
            }
        }

        private void OnTurnEnd(Nation nation)
        {
            if (_missionComplete) return;

            switch (victoryCondition)
            {
                case VictoryConditionType.CapitalCapture:
                    // Handled by TurnManager/GameManager default
                    break;

                case VictoryConditionType.EliminateAll:
                    // Check if all enemy units are gone
                    break;

                case VictoryConditionType.FundRace:
                    var funds = _turns.NationFunds;
                    foreach (var kv in funds)
                        if (kv.Value >= targetFundAmount)
                            TriggerVictory(kv.Key);
                    break;

                case VictoryConditionType.TurnSurvival:
                    int elapsed = (_turns?.TurnNumber ?? 1) - _startTurn;
                    if (elapsed >= turnLimit)
                        TriggerVictory(Nation.USA); // Blue = human nation (scenario-specific)
                    break;
            }
        }

        private void OnVictory(Nation winner)
        {
            if (_missionComplete) return;
            TriggerVictory(winner);
        }

        private void TriggerVictory(Nation winner)
        {
            _missionComplete = true;

            int turnsUsed = (_turns?.TurnNumber ?? 1) - _startTurn;
            var grade     = GetGrade(turnsUsed);

            Debug.Log($"[Mission {missionIndex}] Complete! Winner: {winner}, Grade: {grade}");

            // Unlock items
            foreach (var u in unlockedUnits) Debug.Log($"Unlocked unit: {u}");
            foreach (var m in unlockedMaps)  Debug.Log($"Unlocked map: {m}");
        }

        private MissionGrade GetGrade(int turnsUsed)
        {
            if (turnsUsed <= turnsForS) return MissionGrade.S;
            if (turnsUsed <= turnsForA) return MissionGrade.A;
            if (turnsUsed <= turnsForB) return MissionGrade.B;
            return MissionGrade.C;
        }
    }

    public enum VictoryConditionType
    {
        CapitalCapture,
        EliminateAll,
        FacilityCount,
        FundRace,
        TurnSurvival,
        UnitSurvival
    }
}
