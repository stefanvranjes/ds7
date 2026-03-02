using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAT_0043efd8
{
    public int DAT_00; //0x00
    public int DAT_50; //0x50
    public int DAT_54; //0x54
    public int DAT_58; //0x58
    public int DAT_5c;
    public int DAT_60;
    public int DAT_64; //0x64
    public int DAT_68; //0x68
    public int DAT_70; //0x70
    public int DAT_74; //0x74
    public int[] DAT_a0; //0xA0
    public int DAT_a8; //0xA8
    public int DAT_ac; //0xAC
    public int DAT_fc; //0xFC
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public int iGpffff82c4;
    public DAT_0043efd8[] DAT_0043efd8;
    public int DAT_0043fdc4;
    public int DAT_0043fdc8;

    public void FUN_00144bb0(long param1)
    {
        if (-1 < param1)
            FUN_001e68c0((int)param1);
    }

    private int FUN_001fba30(uint param1, long param2)
    {
        uint uVar1;
        uint uVar2;
        int iVar3;

        uVar2 = param1 & 0xff;
        iVar3 = -1;

        if (uVar2 < 5)
        {
            uVar1 = (uint)DAT_0043efd8[uVar2].DAT_fc;

            if (uVar1 == param1)
            {
                //WaitSema
                if (DAT_0043efd8[uVar2].DAT_fc == uVar1 && DAT_0043efd8[uVar2].DAT_a8 != 0)
                {
                    FUN_001fb9d8((int)uVar2, (int)param2);
                    iVar3 = 0;
                    DAT_0043efd8[uVar2].DAT_64 = 1;
                }
                //SignalSema
            }
            else
                iVar3 = -1;
        }

        return iVar3;
    }

    private void FUN_001fba18(int param1, int param2)
    {
        DAT_0043efd8[param1].DAT_60 = param2;
    }

    private void FUN_001fba00(int param1, int param2)
    {
        DAT_0043efd8[param1].DAT_5c = param2;
    }

    private void FUN_001fb9d8(int param1, int param2)
    {
        DAT_0043efd8[param1].DAT_58 = param2 * DAT_0043fdc4 >> 12;
    }

    private int FUN_001fb878(long param1, int param2, int param3)
    {
        int puVar1;
        int iVar2;
        int iVar3;
        int puVar4;
        uint uVar5;

        uVar5 = 0;
        iVar3 = -1;
        puVar4 = 0;
        //WaitSema

        while (DAT_0043efd8[puVar4].DAT_a8 != 0)
        {
            uVar5++;
            puVar4++;

            if (4 < (int)uVar5) goto LAB_1fb994;
        }

        //FUN_0024d510
        DAT_0043efd8[puVar4].DAT_50 = param2;
        DAT_0043efd8[puVar4].DAT_54 = param3;
        iVar2 = 1;
        puVar1 = 1;

        do
        {
            iVar2--;
            DAT_0043efd8[puVar4].DAT_a0[puVar1] = -1;
            puVar1--;
        } while (-1 < iVar2);

        DAT_0043efd8[puVar4].DAT_70 = 0;
        DAT_0043efd8[puVar4].DAT_74 = 0;
        DAT_0043efd8[puVar4].DAT_64 = 0;
        DAT_0043efd8[puVar4].DAT_68 = 0;
        DAT_0043efd8[puVar4].DAT_ac = 0;
        FUN_001fb9d8((int)uVar5, 0x7f);
        FUN_001fba00((int)uVar5, 0x40);
        FUN_001fba18((int)uVar5, 0x2000);
        DAT_0043fdc8 = DAT_0043fdc8 + 1 & 0x7fffff;
        iVar3 = (int)uVar5 | DAT_0043fdc8 << 8;
        DAT_0043efd8[puVar4].DAT_a8 = 1;
        DAT_0043efd8[puVar4].DAT_fc = iVar3;
        LAB_001fb994:
        //SignalSema
        return iVar3;
    }

    private void FUN_001fad50(long param1, long param2, int param3, long param4, long param5)
    {
        //...
    }

    private void FUN_001e68c0(int param1)
    {
        long lVar1;

        switch (param1)
        {
            case 11:
                lVar1 = 29;
                break;
            case 12:
                lVar1 = 30;
                break;
            default:
                FUN_00101628(param1);
                return;
            case 63:
                lVar1 = 24;
                break;
            case 64:
                lVar1 = 25;
                break;
            case 65:
                lVar1 = 26;
                break;
            case 66:
                lVar1 = 27;
                break;
            case 67:
                lVar1 = 20;
                break;
            case 68:
                lVar1 = 21;
                break;
            case 69:
                lVar1 = 22;
                break;
            case 70:
                lVar1 = 23;
                break;
        }

        FUN_001014f8(lVar1);
    }

    private void FUN_00101628(long param1)
    {
        if (iGpffff82c4 >> 5 != 0)
            FUN_001fad50(param1, iGpffff82c4 >> 5, 60, 64, 0);
    }

    private void FUN_001014f8(long param1)
    {
        //...
    }
}
