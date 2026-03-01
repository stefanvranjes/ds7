using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQueue : MonoBehaviour
{
    public delegate int FUN_0047c068(uint param1, int param2);
    public static FUN_0047c068[] PTR_FUN_0047c068 = new FUN_0047c068[] 
    {
        FUN_00103668, FUN_0010367c
    };

    public static int FUN_001035e0(uint param1)
    {
        long lVar1;

        lVar1 = (long)((int)param1 >> 6) * 0x1070;
        return (int)(((uint)lVar1 | (uint)((ulong)lVar1 >> 0x20)) +
            ((int)param1 >> 5 & 1) * 0x810 + (param1 & 0x1f) * 0x40) / 0x40;
    }

    public static int FUN_00103618(uint param1, int param2)
    {
        return FUN_001035e0((uint)param2 << 6 | param1);
    }

    public static int FUN_00103668(uint param1, int param2)
    {
        return FUN_00103618(param1, param2);
    }

    public static int FUN_0010367c(uint param1, int param2)
    {
        return FUN_00103618(param1, param2) + 0x1071;
    }

    public static int FUN_00103690(uint param1, int param2)
    {
        return FUN_00103618(param1, param2) + 0x20e2;
    }

    public static int FUN_00103638(ulong param1, uint param2, int param3)
    {
        if (param1 < 13)
        {
            return PTR_FUN_0047c068[(int)param1](param2, param3);
        }

        return -1; //null ptr
    }

    public static void FUN_00104aa0(float param1, float param2, uint param3, int param4, long param5, 
                                    int param6, int param7)
    {
        uGpffff838c puVar1;

        puVar1 = GameManager.instance.uGpffff838c[FUN_00103638((ulong)param5, param3, param4)];
        puVar1.DAT_00 = param7;
        puVar1.DAT_30 = param1;
        puVar1.DAT_34 = param2;
        puVar1.DAT_10 = param6;

        if (param5 == 5)
        {
            puVar1 = GameManager.instance.uGpffff838c[FUN_00103638(11, param3, param4)];
            puVar1.DAT_10 = param6;
            puVar1.DAT_00 = param7;
            puVar1.DAT_30 = param1;
            puVar1.DAT_34 = param2;
        }
        else if (param5 == 6)
        {
            puVar1 = GameManager.instance.uGpffff838c[FUN_00103638(12, param3, param4)];
            puVar1.DAT_00 = param7;
            puVar1.DAT_30 = param1;
            puVar1.DAT_34 = param2;
            puVar1.DAT_10 = param6;
        }
    }

    public static void FUN_00104b80(float param1, float param2, float param3, float param4, float param5, 
                                    uint param6, int param7, long param8, int param9)
    {
        uGpffff838c puVar1;

        puVar1 = GameManager.instance.uGpffff838c[FUN_00103638((ulong)param8, param6, param7)];
        puVar1.DAT_00 = param9;
        puVar1.DAT_20 = param1;
        puVar1.DAT_24 = param2;
        puVar1.DAT_28 = param3;
        puVar1.DAT_30 = param4;
        puVar1.DAT_34 = param5;
        puVar1.DAT_10 = 0;

        if (param8 == 5)
        {
            puVar1 = GameManager.instance.uGpffff838c[FUN_00103638(11, param6, param7)];
            puVar1.DAT_10 = 0;
            puVar1.DAT_00 = param9;
            puVar1.DAT_20 = param1;
            puVar1.DAT_24 = param2;
            puVar1.DAT_28 = param3;
            puVar1.DAT_30 = param4;
            puVar1.DAT_34 = param5;
        }
        else if (param8 == 6)
        {
            puVar1 = GameManager.instance.uGpffff838c[FUN_00103638(12, param6, param7)];
            puVar1.DAT_00 = param9;
            puVar1.DAT_20 = param1;
            puVar1.DAT_24 = param2;
            puVar1.DAT_28 = param3;
            puVar1.DAT_30 = param4;
            puVar1.DAT_34 = param5;
            puVar1.DAT_10 = 0;
        }
    }
}
