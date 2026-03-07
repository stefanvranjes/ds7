using System;
using UnityEngine;

public class RenderQueue : MonoBehaviour
{
    public delegate int FUN_0047c068(uint param1, int param2);
    public static FUN_0047c068[] PTR_FUN_0047c068 = new FUN_0047c068[] 
    {
        FUN_00103668, FUN_0010367c
    };
    public static float DAT_0049d44c = 0.03125f;

    private static int FUN_001021a0(int param1)
    {
        long lVar1;

        lVar1 = (param1 / 0xde) * 0x1c10;
        return (int)(((uint)lVar1 | (uint)((ulong)lVar1 >> 0x20)) +
            (param1 / 0x6f & 1) * 0xdf0 + (param1 % 0x6f) * 0x20) / 0x40;
    }

    private static int FUN_00102580(DAT_0049d400 param1, ref int param2)
    {
        int iVar1;
        int iVar2;

        iVar2 = FUN_001021a0(param2);
        param1.DAT_18da00[iVar2].DAT_0c = 0;
        param2++;
        iVar1 = FUN_001021a0(param2);
        param1.DAT_18da00[iVar1].DAT_0c = BitConverter.ToSingle(BitConverter.GetBytes(0xfff), 0);
        return iVar2;
    }

    public static void FUN_001025e8(float param1, float param2, float param3, float param4, float param5)
    {
        float fVar1;
        int puVar2;

        puVar2 = FUN_00102580(GameManager.instance.DAT_0049d400, ref GameManager.instance.DAT_0049d400.DAT_1ad320);
        GameManager.instance.DAT_0049d400.DAT_18da00[puVar2].DAT_00 = param1;
        fVar1 = DAT_0049d44c;
        GameManager.instance.DAT_0049d400.DAT_18da00[puVar2].DAT_08 = param3;
        GameManager.instance.DAT_0049d400.DAT_18da00[puVar2].DAT_04 = param2;
        GameManager.instance.DAT_0049d400.DAT_18da00[puVar2].DAT_10 = fVar1;
        GameManager.instance.DAT_0049d400.DAT_18da00[puVar2].DAT_0c = param5 - param2;
        GameManager.instance.DAT_0049d400.DAT_18da00[puVar2].DAT_14 = ((param2 - param4) * fVar1) / (param5 - param4);
    }

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

    public static int FUN_00103638(ulong param1, uint param2, int param3)
    {
        if (param1 < 13)
        {
            return PTR_FUN_0047c068[(int)param1](param2, param3);
        }

        return -1; //null ptr
    }

    private static int FUN_001035a0(int param1)
    {
        return (param1 + 2) % 3;
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

    private static void FUN_00103f30(int param1, int param2, int param3, DAT_0049d3fc param4)
    {
        if (0 < param3)
        {
            do
            {
                param4.DAT_10[param1 / 0x40].DAT_10 = 0xfff;
                param3--;
                param1 += 0x1070;
            } while (0 < param3);
        }

        param4.DAT_10[param2 / 0x40].DAT_04 = 0;
    }

    public static void FUN_00103f90(int param1)
    {
        int iVar1;
        int iVar2;
        DAT_0049d3fc pVar2;
        int puVar3;
        int iVar4;
        int iVar5;
        DAT_0049d3fc pVar5;
        int iVar6;
        int iVar7;
        int iVar8;

        pVar2 = GameManager.instance.DAT_0049d400;
        pVar5 = GameManager.instance.DAT_0049d3fc;

        switch (param1)
        {
            case 4:
                iVar2 = FUN_001035a0(pVar5.DAT_10[0x44d9].DAT_00);
                iVar6 = pVar5.DAT_10[0x44d9].DAT_10;
                iVar1 = iVar2 * 0x41c0;
                iVar4 = iVar2 * 4;
                iVar7 = 0x113644;
                iVar8 = 0x107100;
                break;
            case 5:
                iVar2 = FUN_001035a0(pVar5.DAT_10[0x5443].DAT_00);
                FUN_00103f30(iVar2 * 0x148c0 + 0x113680, iVar2 * 4 + 0x1510c4, pVar5.DAT_10[0x5443].DAT_10, pVar5);
                iVar1 = FUN_001035a0(pVar5.DAT_10[0x7002].DAT_00);
                iVar5 = iVar1 * 0x148c0;
                iVar6 = pVar5.DAT_10[0x7002].DAT_10;
                iVar1 = iVar1 * 4;
                iVar8 = 0x182640;
                puVar3 = 0x1c0084;
                goto LAB_001040b0;
            case 6:
                iVar2 = FUN_001035a0(pVar5.DAT_10[0x6098].DAT_00);
                FUN_00103f30(iVar2 * 0x10700 + 0x151100, iVar2 * 4 + 0x182604, pVar5.DAT_10[0x6098].DAT_10, pVar5);
                iVar1 = FUN_001035a0(pVar5.DAT_10[0x7c57].DAT_00);
                iVar5 = iVar1 * 0x10700;
                iVar6 = pVar5.DAT_10[0x7c57].DAT_10;
                iVar1 = iVar1 * 4;
                iVar8 = 0x1c00c0;
                puVar3 = 0x1f15c4;
                LAB_001040b0:
                puVar3 += iVar1;
                iVar8 = iVar5 + iVar8;
                goto LAB_00104014;
            case 7:
                iVar5 = FUN_001035a0(pVar2.DAT_10[0x3150].DAT_00);
                iVar6 = pVar2.DAT_10[0x3150].DAT_10;
                puVar3 = iVar5 * 4 + 0xc5404;
                iVar8 = iVar5 * 0x41c00;
                pVar5 = pVar2;
                goto LAB_00104014;
            case 8:
                iVar5 = FUN_001035a0(pVar2.DAT_10[0x62a1].DAT_00);
                iVar6 = pVar2.DAT_10[0x62a1].DAT_10;
                iVar1 = iVar5 * 0x41c00;
                iVar4 = iVar5 * 4;
                iVar7 = 0x18a844;
                iVar8 = 0xc5440;
                pVar5 = pVar2;
                break;
            case 9:
                iVar5 = FUN_001035a0(pVar2.DAT_10[0x6367].DAT_00);
                iVar6 = pVar2.DAT_10[0x6367].DAT_10;
                iVar1 = iVar5 * 0x1070;
                iVar4 = iVar5 * 4;
                iVar7 = 0x18d9d4;
                iVar8 = 0x18a880;
                pVar5 = pVar2;
                break;
            case 10:
                iVar2 = 0x6368;

                for (iVar5 = pVar2.DAT_34; 0 < iVar5; iVar5--)
                {
                    //...
                }

                pVar2.DAT_30 = 0;
                return;
            default:
                return;
        }

        puVar3 = iVar4 + iVar7;
        iVar8 = iVar1 + iVar8;

        LAB_00104014:
        FUN_00103f30(iVar8, puVar3, iVar6, pVar5);
    }

    public static void FUN_00104a10(float param1, uint param2, int param3, long param4)
    {
        int puVar1;
        uint uVar2;

        puVar1 = FUN_00103638((ulong)param4, param2, param3);
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_24 = param1;
        uVar2 = 11;

        if (param4 != 5)
        {
            if (param4 != 6)
                return;

            uVar2 = 12;
        }

        puVar1 = FUN_00103638(uVar2, param2, param3);
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_24 = param1;
    }

    public static void FUN_00104aa0(float param1, float param2, uint param3, int param4, long param5, 
                                    int param6, int param7)
    {
        int puVar1;

        puVar1 = FUN_00103638((ulong)param5, param3, param4);
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_00 = param7;
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_30 = param1;
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_34 = param2;
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_10 = param6;

        if (param5 == 5)
        {
            puVar1 = FUN_00103638(11, param3, param4);
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_10 = param6;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_00 = param7;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_30 = param1;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_34 = param2;
        }
        else if (param5 == 6)
        {
            puVar1 = FUN_00103638(12, param3, param4);
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_00 = param7;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_30 = param1;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_34 = param2;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_10 = param6;
        }
    }

    public static void FUN_00104b80(float param1, float param2, float param3, float param4, float param5, 
                                    uint param6, int param7, long param8, int param9)
    {
        int puVar1;

        puVar1 = FUN_00103638((ulong)param8, param6, param7);
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_00 = param9;
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_20 = param1;
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_24 = param2;
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_28 = param3;
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_30 = param4;
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_34 = param5;
        GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_10 = 0;

        if (param8 == 5)
        {
            puVar1 = FUN_00103638(11, param6, param7);
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_10 = 0;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_00 = param9;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_20 = param1;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_24 = param2;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_28 = param3;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_30 = param4;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_34 = param5;
        }
        else if (param8 == 6)
        {
            puVar1 = FUN_00103638(12, param6, param7);
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_00 = param9;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_20 = param1;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_24 = param2;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_28 = param3;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_30 = param4;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_34 = param5;
            GameManager.instance.DAT_0049d3fc.DAT_10[puVar1].DAT_10 = 0;
        }
    }
}
