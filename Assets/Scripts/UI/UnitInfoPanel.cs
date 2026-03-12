using DS7.Data;
using DS7.Units;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DS7.UI
{
    /// <summary>
    /// HUD panel showing selected unit stats: name, nation, altitude, fuel bar,
    /// endurance, and per-weapon ammo counts.
    /// Requires: TextMeshPro package and UGUI.
    /// </summary>
    public class UnitInfoPanel : MonoBehaviour
    {
        [Header("Header")]
        public TMP_Text unitNameText;
        public TMP_Text unitTypeText;
        public Image    flagIcon;

        [Header("Status")]
        public TMP_Text altitudeText;
        public TMP_Text enduranceText;
        public Slider   fuelBar;
        public TMP_Text fuelText;
        public TMP_Text levelText;

        [Header("Weapons")]
        public Transform weaponListParent;
        public GameObject weaponRowPrefab; // prefab with TMP_Text fields: name, ammo

        private void Awake() => gameObject.SetActive(false);

        // ── Show / Hide ───────────────────────────────────────────────────────
        public void Show(Unit unit)
        {
            gameObject.SetActive(true);

            if (unit?.Data == null) return;

            // Header
            if (unitNameText) unitNameText.text = unit.Data.unitName;
            if (unitTypeText) unitTypeText.text = unit.Data.unitType.ToString();
            if (flagIcon)
            {
                var factionData = FactionManager.Instance?.GetFactionData(unit.Owner);
                if (factionData != null)
                    flagIcon.color = factionData.factionColor;
            }

            // Status
            if (altitudeText)  altitudeText.text  = unit.CurrentAltitude.ToString();
            if (enduranceText) enduranceText.text = $"HP {unit.CurrentEndurance}/{unit.Data.maxEndurance}";
            if (levelText)     levelText.text     = $"Lv.{unit.Level}  XP:{unit.Experience}";

            if (fuelBar)
            {
                fuelBar.maxValue = unit.Data.maxFuel;
                fuelBar.value    = unit.CurrentFuel;
            }
            if (fuelText) fuelText.text = $"{unit.CurrentFuel}/{unit.Data.maxFuel}";

            // Weapons
            RefreshWeapons(unit);
        }

        public void Hide() => gameObject.SetActive(false);

        // ── Weapons List ──────────────────────────────────────────────────────
        private void RefreshWeapons(Unit unit)
        {
            if (weaponListParent == null || weaponRowPrefab == null) return;

            // Clear old rows
            foreach (Transform child in weaponListParent)
                Destroy(child.gameObject);

            foreach (var weapon in unit.Data.GetWeapons(unit.CurrentPackIndex))
            {
                var row = Instantiate(weaponRowPrefab, weaponListParent);
                var texts = row.GetComponentsInChildren<TMP_Text>();

                if (texts.Length >= 2)
                {
                    texts[0].text = weapon.weaponName;
                    texts[1].text = $"{unit.GetAmmo(weapon)}/{weapon.maxAmmo}";
                }
            }
        }
    }
}
