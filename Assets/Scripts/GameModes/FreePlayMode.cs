using System.Collections.Generic;
using DS7.Data;
using UnityEngine;

namespace DS7.GameModes
{
    /// <summary>
    /// Free Play Mode: customizable parameters per nation, hot-seat multiplayer (up to 4),
    /// screen blackout between turns, saved leveled units from Campaign.
    /// </summary>
    public class FreePlayMode : MonoBehaviour
    {
        [Header("Player Slots (up to 4)")]
        public List<FreePlaySlot> playerSlots = new();

        [Header("Hot-Seat")]
        [Tooltip("Show a blank 'pass the controller' screen between human turns.")]
        public bool hotSeatBlackout = true;

        public GameObject blackoutPanel;

        private TurnManager _turns;
        private Faction     _lastFaction;

        private void Start()
        {
            _turns = TurnManager.Instance;
            if (_turns != null)
            {
                _turns.OnTurnStart += OnTurnStart;
                _turns.OnTurnEnd   += OnTurnEnd;
            }

            // Configure turn order from active slots
            _turns.turnOrder.Clear();
            foreach (var slot in playerSlots)
                if (slot.active && slot.factionData != null)
                    _turns.turnOrder.Add(slot.factionData);

            _turns.StartGame();
        }

        private void OnTurnStart(Faction faction)
        {
            // Hide blackout when new turn begins
            if (blackoutPanel != null)
                blackoutPanel.SetActive(false);

            _lastFaction = faction;
        }

        private void OnTurnEnd(Faction faction)
        {
            // If next player is also human, show blackout
            var slot = GetSlot(faction);
            if (hotSeatBlackout && slot?.playerType == PlayerType.Human)
            {
                if (blackoutPanel != null)
                    blackoutPanel.SetActive(true);
            }
        }

        private FreePlaySlot GetSlot(Faction faction)
        {
            foreach (var s in playerSlots)
                if (s.factionData?.faction == faction) return s;
            return null;
        }
    }

    [System.Serializable]
    public class FreePlaySlot
    {
        public bool        active;
        public FactionData factionData;
        public PlayerType playerType;
        public int        startingFunds;
        [Tooltip("Used for alliances: nations with same alliance ID are allied.")]
        public int        allianceGroup;
    }
}
