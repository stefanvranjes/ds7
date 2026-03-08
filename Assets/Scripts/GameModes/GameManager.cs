using System.Collections.Generic;
using DS7.Data;
using DS7.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DS7.GameModes
{
    /// <summary>
    /// Central singleton managing game state, mode, save/load, and scene transitions.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        // ── State ─────────────────────────────────────────────────────────────
        public GameMode ActiveMode { get; private set; }

        [Header("Scene Names")]
        public string battlefieldScene = "Battlefield";
        public string mainMenuScene    = "MainMenu";
        public string mapEditorScene   = "MapEditor";

        // ── Save Slots ────────────────────────────────────────────────────────
        public const int SaveSlots = 8;
        private GameSaveData[] _saves = new GameSaveData[SaveSlots];

        // ── Campaign Persistent Units ─────────────────────────────────────────
        public List<SavedUnitState> CampaignSavedUnits { get; } = new();

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // ── Mode Entry Points ─────────────────────────────────────────────────
        public void StartMission(int missionIndex)
        {
            ActiveMode = GameMode.Mission;
            PlayerPrefs.SetInt("MissionIndex", missionIndex);
            SceneManager.LoadScene(battlefieldScene);
        }

        public void StartCampaign(int campaignIndex, int battleIndex)
        {
            ActiveMode = GameMode.Campaign;
            PlayerPrefs.SetInt("CampaignIndex", campaignIndex);
            PlayerPrefs.SetInt("BattleIndex", battleIndex);
            SceneManager.LoadScene(battlefieldScene);
        }

        public void StartFreePlay()
        {
            ActiveMode = GameMode.FreePlay;
            SceneManager.LoadScene(battlefieldScene);
        }

        public void OpenMapEditor()
        {
            ActiveMode = GameMode.MapEditor;
            SceneManager.LoadScene(mapEditorScene);
        }

        public void ReturnToMainMenu()
            => SceneManager.LoadScene(mainMenuScene);

        // ── Save / Load ───────────────────────────────────────────────────────
        public void SaveGame(int slot)
        {
            if (slot < 0 || slot >= SaveSlots) return;
            _saves[slot] = GameSaveData.Capture(TurnManager.Instance, ActiveMode);
            string json = JsonUtility.ToJson(_saves[slot]);
            PlayerPrefs.SetString($"Save_{slot}", json);
            Debug.Log($"[GameManager] Game saved to slot {slot}.");
        }

        public bool LoadGame(int slot)
        {
            if (slot < 0 || slot >= SaveSlots) return false;
            string json = PlayerPrefs.GetString($"Save_{slot}", null);
            if (string.IsNullOrEmpty(json)) return false;
            _saves[slot] = JsonUtility.FromJson<GameSaveData>(json);
            // Full state restoration would happen in OnSceneLoaded
            Debug.Log($"[GameManager] Game loaded from slot {slot}.");
            return true;
        }

        // ── Victory ───────────────────────────────────────────────────────────
        public void CheckVictory(HashSet<Nation> owningNations, List<Unit> allUnits)
        {
            // Handled by active game mode script
            // This is the fallback: if only one living nation exists → victory
            var livingNations = new HashSet<Nation>();
            foreach (var u in allUnits)
                if (u.IsAlive) livingNations.Add(u.Owner);

            if (livingNations.Count == 1)
            {
                foreach (var n in livingNations)
                    Debug.Log($"[GameManager] VICTORY: {n} wins!");
            }
        }
    }

    // ── Save Data Container ───────────────────────────────────────────────────
    [System.Serializable]
    public class GameSaveData
    {
        public GameMode mode;
        public int      turnNumber;
        public Nation   activeNation;

        public static GameSaveData Capture(TurnManager tm, GameMode mode)
        {
            if (tm == null) return new GameSaveData { mode = mode };
            return new GameSaveData
            {
                mode         = mode,
                turnNumber   = tm.TurnNumber,
                activeNation = tm.ActiveNation
            };
        }
    }
}
