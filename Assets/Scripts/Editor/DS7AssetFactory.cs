#if UNITY_EDITOR
using System.IO;
using DS7.Data;
using UnityEditor;
using UnityEngine;

namespace DS7.Editor
{
    /// <summary>
    /// Unity Editor utility: DS7 > Create Starter Assets
    /// Auto-generates a baseline set of TerrainData, NationData, WeaponData,
    /// and UnitData ScriptableObjects into Assets/DS7Data/.
    /// Run once to bootstrap the project without clicking through menus manually.
    /// </summary>
    public static class DS7AssetFactory
    {
        private const string RootPath = "Assets/DS7Data";

        [MenuItem("DS7/Create Starter Assets")]
        public static void CreateAll()
        {
            EnsureDir(RootPath);
            CreateTerrainAssets();
            CreateNationAssets();
            CreateWeaponAssets();
            CreateUnitAssets();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[DS7Factory] Starter assets created in Assets/DS7Data/");
        }

        // ── Terrain ───────────────────────────────────────────────────────────
        private static void CreateTerrainAssets()
        {
            EnsureDir($"{RootPath}/Terrain");

            CreateTerrain("Plain",     TerrainType.Plain,     defBonus:0,  income:0,   tint:new Color(0.56f,0.73f,0.35f));
            CreateTerrain("City",      TerrainType.City,      defBonus:20, income:200, tint:new Color(0.75f,0.75f,0.75f), facility:true, produce:true, resupply:true);
            CreateTerrain("Capital",   TerrainType.Capital,   defBonus:30, income:500, tint:new Color(1f,0.84f,0.0f),    facility:true, produce:true, resupply:true);
            CreateTerrain("Airport",   TerrainType.Airport,   defBonus:10, income:100, tint:new Color(0.95f,0.95f,0.8f), facility:true, produce:true, resupply:true, equip:true);
            CreateTerrain("Port",      TerrainType.Port,      defBonus:10, income:100, tint:new Color(0.4f,0.6f,0.9f),  facility:true, produce:true, resupply:true);
            CreateTerrain("Forest",    TerrainType.Forest,    defBonus:30, income:0,   tint:new Color(0.18f,0.49f,0.18f));
            CreateTerrain("Mountain",  TerrainType.Mountain,  defBonus:40, income:0,   tint:new Color(0.55f,0.45f,0.35f));
            CreateTerrain("Sea",       TerrainType.Sea,       defBonus:0,  income:0,   tint:new Color(0.25f,0.45f,0.80f));
            CreateTerrain("Shallows",  TerrainType.Shallows,  defBonus:0,  income:0,   tint:new Color(0.4f,0.7f,0.95f));
            CreateTerrain("DeepSea",   TerrainType.DeepSea,   defBonus:0,  income:0,   tint:new Color(0.1f,0.2f,0.6f));
            CreateTerrain("Road",      TerrainType.Road,      defBonus:0,  income:0,   tint:new Color(0.70f,0.65f,0.55f));
        }

        private static void CreateTerrain(string assetName, TerrainType type, int defBonus, int income, Color tint,
                                           bool facility=false, bool produce=false, bool resupply=false, bool equip=false)
        {
            string path = $"{RootPath}/Terrain/{assetName}.asset";
            if (File.Exists(Path.Combine(Application.dataPath, "..", path))) return;

            var t = ScriptableObject.CreateInstance<DS7.Data.TerrainData>();
            t.terrainName   = assetName;
            t.terrainType   = type;
            t.mapColor      = tint;
            t.editorTint    = tint;
            t.defenseBonus  = defBonus;
            t.incomePerTurn = income;
            t.isFacility    = facility;
            t.canProduce    = produce;
            t.canResupply   = resupply;
            t.canEquip      = equip;

            // Standard movement costs
            t.movementCosts = new MovementCostEntry[]
            {
                new() { category = MovementCategory.Foot,     cost = type == TerrainType.Mountain ? 2 : 1 },
                new() { category = MovementCategory.Wheeled,   cost = type == TerrainType.Mountain ? -1 : (type == TerrainType.Forest ? 3 : 1) },
                new() { category = MovementCategory.Tracked,   cost = type == TerrainType.Mountain ? 2 : 1 },
                new() { category = MovementCategory.Air,       cost = 1 },
                new() { category = MovementCategory.Naval,     cost = IsLand(type) ? -1 : 1 },
            };

            AssetDatabase.CreateAsset(t, path);
        }

        private static bool IsLand(TerrainType t) =>
            t != TerrainType.Sea && t != TerrainType.Shallows && t != TerrainType.DeepSea;

        // ── Nations ───────────────────────────────────────────────────────────
        private static void CreateNationAssets()
        {
            EnsureDir($"{RootPath}/Nations");
            CreateNation("USA",    Nation.USA,    new Color(0.2f,0.4f,0.8f),  500);
            CreateNation("Japan",  Nation.Japan,  new Color(0.9f,0.2f,0.2f),  500);
            CreateNation("Russia", Nation.Russia, new Color(0.1f,0.6f,0.1f),  500);
        }

        private static void CreateNation(string assetName, Nation nation, Color flag, int funds)
        {
            string path = $"{RootPath}/Nations/{assetName}.asset";
            if (File.Exists(Path.Combine(Application.dataPath, "..", path))) return;

            var n = ScriptableObject.CreateInstance<NationData>();
            n.nation       = nation;
            n.displayName  = assetName;
            n.flagColor    = flag;
            n.baseFundRate = funds;
            AssetDatabase.CreateAsset(n, path);
        }

        // ── Weapons ───────────────────────────────────────────────────────────
        private static void CreateWeaponAssets()
        {
            EnsureDir($"{RootPath}/Weapons");

            // Ground weapons
            MakeWeapon("Rifle",         firePower:30,  maxAmmo:99, rangeGround:1, rangeHigh:0, canAdj:true,  canAttack:true,  canDefend:true,  canMoveAndFire:true,  elevUp:0, elevDown:0);
            MakeWeapon("AT_Missile",    firePower:80,  maxAmmo:6,  rangeGround:3, rangeHigh:0, canAdj:false, canAttack:true,  canDefend:true,  canMoveAndFire:false, elevUp:0, elevDown:0);
            MakeWeapon("MainGun_120mm", firePower:100, maxAmmo:20, rangeGround:3, rangeHigh:0, canAdj:true,  canAttack:true,  canDefend:true,  canMoveAndFire:true,  elevUp:0, elevDown:0);
            MakeWeapon("Arty_155mm",    firePower:90,  maxAmmo:20, rangeGround:8, rangeHigh:0, canAdj:false, canAttack:true,  canDefend:false, canMoveAndFire:false, elevUp:0, elevDown:0, isDply:true);

            // Air weapons
            MakeWeapon("Vulcan_20mm",   firePower:60,  maxAmmo:99, rangeLow:2,  rangeMed:0, rangeHigh:0, canAdj:true,  canAttack:true, canDefend:true,  canMoveAndFire:true,  elevUp:2, elevDown:2);
            MakeWeapon("Sidewinder",    firePower:100, maxAmmo:4,  rangeLow:4,  rangeMed:3, rangeHigh:2, canAdj:false, canAttack:true, canDefend:true,  canMoveAndFire:true,  elevUp:3, elevDown:1);
            MakeWeapon("AMRAAM",        firePower:120, maxAmmo:4,  rangeLow:6,  rangeMed:6, rangeHigh:4, canAdj:false, canAttack:true, canDefend:true,  canMoveAndFire:true,  elevUp:3, elevDown:1, isMissile:true);
            MakeWeapon("LGB_500",       firePower:150, maxAmmo:8,  rangeLow:3,  rangeMed:0, rangeHigh:0, rangeGround:0, canAdj:false, canAttack:true, canDefend:false, canMoveAndFire:false, elevUp:0, elevDown:3, canBomb:true);

            // Naval
            MakeWeapon("NavalGun_5in",  firePower:80, maxAmmo:99, rangeSurface:5, rangeGround:5, canAdj:true, canAttack:true, canDefend:true, canMoveAndFire:true, elevUp:0, elevDown:0);
            MakeWeapon("Torpedo",       firePower:140, maxAmmo:6, rangeSurface:3, rangeDeep:3, canAdj:false, canAttack:true, canDefend:true, canMoveAndFire:true, elevUp:0, elevDown:1);
        }

        private static void MakeWeapon(string name, int firePower, int maxAmmo,
            int rangeGround=0, int rangeLow=0, int rangeMed=0, int rangeHigh=0,
            int rangeSurface=0, int rangeDeep=0,
            bool canAdj=true, bool canAttack=true, bool canDefend=true,
            bool canMoveAndFire=false, int elevUp=0, int elevDown=0,
            bool isMissile=false, bool canBomb=false, bool isDply=false)
        {
            string path = $"{RootPath}/Weapons/{name}.asset";
            if (File.Exists(Path.Combine(Application.dataPath, "..", path))) return;

            var w = ScriptableObject.CreateInstance<WeaponData>();
            w.weaponName       = name;
            w.firePower        = firePower;
            w.maxAmmo          = maxAmmo;
            w.rangeGround      = rangeGround;
            w.rangeLow         = rangeLow;
            w.rangeMed         = rangeMed;
            w.rangeHigh        = rangeHigh;
            w.rangeSurface     = rangeSurface;
            w.rangeDeep        = rangeDeep;
            w.canAdj           = canAdj;
            w.canAttack        = canAttack;
            w.canDefend        = canDefend;
            w.canMoveAndFire   = canMoveAndFire;
            w.elevationUp      = elevUp;
            w.elevationDown    = elevDown;
            w.isMissile        = isMissile;
            w.canBomb          = canBomb;
            // default hit rates
            w.hitRateInfantry  = 70;
            w.hitRateVehicle   = 60;
            w.hitRateAir       = isMissile ? 80 : (rangeLow > 0 ? 65 : 5);
            w.hitRateVessel    = rangeSurface > 0 ? 70 : 10;

            AssetDatabase.CreateAsset(w, path);
        }

        // ── Units ─────────────────────────────────────────────────────────────
        private static void CreateUnitAssets()
        {
            EnsureDir($"{RootPath}/Units/USA");
            EnsureDir($"{RootPath}/Units/Japan");

            // ── USA Units
            MakeUnit("Infantry_USA",    Nation.USA,  UnitType.Infantry,  MovementCategory.Foot, prod:100, endurance:10, fuel:99, stdMv:3, highMv:5,  fuelPerMv:0, abilities: UnitAbility.Capture,
                      weapons: new[]{"Rifle","AT_Missile"});
            MakeUnit("M1A2_Abrams",     Nation.USA,  UnitType.Vehicle,   MovementCategory.Tracked,  prod:400, endurance:15, fuel:60, stdMv:4, highMv:7,  fuelPerMv:2,
                      weapons: new[]{"MainGun_120mm","Vulcan_20mm"});
            MakeUnit("M109_Paladin",    Nation.USA,  UnitType.Vehicle,   MovementCategory.Tracked,  prod:300, endurance:10, fuel:50, stdMv:3, highMv:0,  fuelPerMv:2, abilities: UnitAbility.Dply,
                      weapons: new[]{"Arty_155mm"});
            MakeUnit("M2_Bradley",      Nation.USA,  UnitType.Vehicle,   MovementCategory.Tracked,  prod:250, endurance:12, fuel:55, stdMv:4, highMv:6,  fuelPerMv:2, abilities: UnitAbility.Trans,
                      weapons: new[]{"MainGun_120mm"});
            MakeUnit("AH64_Apache",     Nation.USA,  UnitType.Helicopter,MovementCategory.Air,      prod:500, endurance:10, fuel:40, stdMv:5, highMv:8,  fuelPerMv:3,
                      weapons: new[]{"Vulcan_20mm","AT_Missile","Sidewinder"});
            MakeUnit("F15C_Eagle",      Nation.USA,  UnitType.Air,       MovementCategory.Air,      prod:700, endurance:10, fuel:80, stdMv:7, highMv:10, fuelPerMv:4,
                      weapons: new[]{"Vulcan_20mm","Sidewinder","AMRAAM"});
            MakeUnit("F16_Falcon",      Nation.USA,  UnitType.Air,       MovementCategory.Air,      prod:600, endurance:10, fuel:70, stdMv:7, highMv:10, fuelPerMv:4,
                      weapons: new[]{"Vulcan_20mm","Sidewinder","LGB_500"});
            MakeUnit("Arleigh_Burke",   Nation.USA,  UnitType.Vessel,    MovementCategory.Naval,    prod:800, endurance:15, fuel:150,stdMv:4, highMv:6,  fuelPerMv:2,
                      weapons: new[]{"NavalGun_5in","AMRAAM"});
            MakeUnit("M88_Recovery",    Nation.USA,  UnitType.Vehicle,   MovementCategory.Tracked,  prod:150, endurance:10, fuel:50, stdMv:3, highMv:5,  fuelPerMv:2, abilities: UnitAbility.Sup,
                      weapons: new[]{"Rifle"});

            // ── Japan Units
            MakeUnit("Infantry_JPN",    Nation.Japan,UnitType.Infantry,  MovementCategory.Foot, prod:100, endurance:10, fuel:99, stdMv:3, highMv:5,  fuelPerMv:0, abilities: UnitAbility.Capture,
                      weapons: new[]{"Rifle","AT_Missile"});
            MakeUnit("Type90_Tank",     Nation.Japan,UnitType.Vehicle,   MovementCategory.Tracked,  prod:420, endurance:15, fuel:60, stdMv:4, highMv:7,  fuelPerMv:2,
                      weapons: new[]{"MainGun_120mm","Vulcan_20mm"});
            MakeUnit("OH1_Ninja",       Nation.Japan,UnitType.Helicopter,MovementCategory.Air,      prod:450, endurance:10, fuel:40, stdMv:5, highMv:8,  fuelPerMv:3,
                      weapons: new[]{"Vulcan_20mm","Sidewinder"});
            MakeUnit("F2_Viper",        Nation.Japan,UnitType.Air,       MovementCategory.Air,      prod:650, endurance:10, fuel:70, stdMv:7, highMv:10, fuelPerMv:4,
                      weapons: new[]{"Vulcan_20mm","AMRAAM","LGB_500"});
            MakeUnit("Kongo_Destroyer", Nation.Japan,UnitType.Vessel,    MovementCategory.Naval,    prod:750, endurance:14, fuel:150,stdMv:4, highMv:6,  fuelPerMv:2,
                      weapons: new[]{"NavalGun_5in","Torpedo"});
        }

        private static void MakeUnit(string name, Nation owner, UnitType type, MovementCategory mvCat,
            int prod, int endurance, int fuel, int stdMv, int highMv, int fuelPerMv,
            UnitAbility abilities = 0, string[] weapons = null)
        {
            string path = $"{RootPath}/Units/{owner}/{name}.asset";
            if (File.Exists(Path.Combine(Application.dataPath, "..", path))) return;

            var u = ScriptableObject.CreateInstance<UnitData>();
            u.unitName         = name;
            u.nation           = owner;
            u.unitType         = type;
            u.movementCategory = mvCat;
            u.productionCost   = prod;
            u.maxEndurance     = endurance;
            u.maxFuel          = fuel;
            u.standardMove     = stdMv;
            u.highMove         = highMv;
            u.fuelPerStandardMove = fuelPerMv;
            u.abilities        = abilities;
            u.detectionRange   = 3;

            // Assign weapon pack 0
            if (weapons != null && weapons.Length > 0)
            {
                var wepList = new WeaponData[weapons.Length];
                for (int i = 0; i < weapons.Length; i++)
                    wepList[i] = AssetDatabase.LoadAssetAtPath<WeaponData>($"{RootPath}/Weapons/{weapons[i]}.asset");
                u.weaponPacks = new WeaponPack[]
                {
                    new() { packName = "Default", weapons = wepList }
                };
            }

            AssetDatabase.CreateAsset(u, path);
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private static void EnsureDir(string path)
        {
            string full = Path.Combine(Application.dataPath, "..", path);
            if (!Directory.Exists(full)) Directory.CreateDirectory(full);
        }
    }
}
#endif
