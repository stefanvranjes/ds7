namespace DS7.Data
{
    // ── Altitude ──────────────────────────────────────────────────────────────
    /// <summary>Six altitude layers per hex, from deep sea to high atmosphere.</summary>
    public enum AltitudeLayer
    {
        DeepSea  = -2,  // submarines
        Surface  =  -1,  // ground units, surface vessels, ground-level water flight
        Ground   =  0,  // alias used for land units (same layer as Surface)
        LowAir   =  1,
        MedAir   =  2,
        HighAir  =  3
    }

    // ── Unit Types ────────────────────────────────────────────────────────────
    public enum UnitType
    {
        Infantry,
        Vehicle,    // light/heavy armour, APCs, trucks
        Helicopter,
        Air,        // fixed-wing aircraft
        Vessel,     // surface ships
        Submarine
    }

    // ── Load Types (transport capacity matching) ──────────────────────────────
    public enum LoadType
    {
        None,
        Inf,   // infantry
        Veh,   // vehicle
        Lght,  // light
        Hev,   // heavy
        Hel,   // helicopter
        Air    // fixed-wing
    }

    // ── Terrain ───────────────────────────────────────────────────────────────
    public enum TerrainType
    {
        Plain, Road, River, Desert, Wasteland, Wetland,
        Grove, Forest, Hill, Mountain, Peaks,
        Lake, Shallows, Sea, DeepSea,
        BridgeL, BridgeR, BridgeS,
        SnowField, SnowyGrove, SnowyForest, SnowyHill, SnowyMountain, SnowyPeaks,
        // Facilities
        Capital, City, Airport, Port, Factory, Refinery
    }

    // ── Nations ───────────────────────────────────────────────────────────────
    public enum Nation
    {
        Neutral,
        Japan,
        USA,
        Russia,
        Germany,
        UK,
        France,
        Israel,
        China
    }

    // ── Game Modes ────────────────────────────────────────────────────────────
    public enum GameMode
    {
        Mission,
        Campaign,
        FreePlay,
        Tutorial,
        MapEditor
    }

    // ── Special Unit Abilities ────────────────────────────────────────────────
    [System.Flags]
    public enum UnitAbility
    {
        None    = 0,
        Trans   = 1 << 0,   // can transport other units
        Sup     = 1 << 1,   // supply / resupply capability
        Bmb     = 1 << 2,   // can bomb facilities
        Rpr     = 1 << 3,   // can repair facilities (engineer)
        Jam     = 1 << 4,   // ECM jamming
        Int     = 1 << 5,   // air intercept
        Dply    = 1 << 6,   // deploy/mobile restriction
        LAir    = 1 << 7,   // can be dropped from low altitude
        Capture = 1 << 8,   // can capture facilities
        VTOL    = 1 << 9,   // can land anywhere like a helicopter
        Stlth   = 1 << 10,  // stealth capability
    }

    // ── Mission Grade ─────────────────────────────────────────────────────────
    public enum MissionGrade { S, A, B, C, D, F }

    // ── Player Type ───────────────────────────────────────────────────────────
    public enum PlayerType { Human, AI }

    // ── Resupply Mode ─────────────────────────────────────────────────────────
    public enum ResupplyMode { Auto, Ask, Manual }
}
