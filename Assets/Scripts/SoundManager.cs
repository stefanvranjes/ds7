using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0x100 - size
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

//0x204 - size
public class DAT_0043ddc0
{
    public int DAT_00;
    public short[] DAT_24;
    public int DAT_28;
    public int DAT_2c;
    public int[] DAT_40;
    public int DAT_200;
}

//0x18 - size
public class DAT_0043e9d8
{
    public int DAT_00;
    public int DAT_04;
    public int DAT_08;
    public int DAT_0c;
    public int DAT_10;
    public int DAT_14;
}

//0x0C - size
public class DAT_0043f720
{
    public bool DAT_00;
    public sbyte DAT_01;
    public sbyte DAT_02;
    public sbyte DAT_03;
    public short DAT_06;
    public uint DAT_08;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public int iGpffff82c4;
    public DAT_0043ddc0[] DAT_0043ddc0;
    public DAT_0043e9d8[] DAT_0043e9d8;
    public DAT_0043efd8[] DAT_0043efd8;
    public int DAT_0043f71c;
    public DAT_0043f720[] DAT_0043f720;
    public int DAT_0043f8a0;
    public int DAT_0043fda8;
    public int DAT_0043fdb0;
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

            if (4 < (int)uVar5) goto LAB_001fb994;
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

    private int FUN_001fafe8(int param1, short param2, int param3, sbyte param4, sbyte param5)
    {
        bool bVar1;
        bool bVar2;
        int piVar3;
        int pcVar4;
        uint uVar5;
        int iVar6;
        int iVar7;
        int pcVar8;
        int iVar9;

        iVar9 = 0;

        if (DAT_0043ddc0[param1].DAT_40[0] == -1)
        {
            bVar2 = true;

            if (bVar2)
            {
                iVar6 = 0;
                iVar7 = 0;
                pcVar8 = 0;
                pcVar4 = pcVar8;

                do
                {
                    bVar1 = DAT_0043f720[pcVar4].DAT_00;
                    pcVar4++;

                    if (!bVar1)
                    {
                        DAT_0043ddc0[param1].DAT_40[iVar9] = DAT_0043f71c | 0x1000000;
                        iVar6 = DAT_0043ddc0[param1].DAT_200;

                        if (0x7f < param3)
                            param3 = 0x7f;

                        DAT_0043f720[pcVar8].DAT_06 = param2;
                        DAT_0043ddc0[param1].DAT_200 = iVar6 + 1;
                        uVar5 = (uint)DAT_0043f71c;
                        DAT_0043f720[pcVar8].DAT_00 = true;
                        uVar5 = (uint)param1 << 0x18 | (uint)iVar9 << 0x10 | uVar5;
                        DAT_0043f720[pcVar8].DAT_01 = (sbyte)param3;
                        DAT_0043f720[pcVar8].DAT_02 = param4;
                        DAT_0043f720[pcVar8].DAT_03 = param5;
                        DAT_0043f720[iVar7].DAT_08 = uVar5;
                        DAT_0043f71c = DAT_0043f71c + 1 & 0x7fff;
                        DAT_0043f8a0++;
                        return (int)uVar5;
                    }

                    iVar6++;
                    pcVar8++;
                    iVar7++;
                } while (iVar6 < 0x20);
            }
        }
        else
        {
            piVar3 = 0;
            iVar9 = 1;

            while (true)
            {
                bVar2 = iVar9 < 0x40;
                piVar3++;

                if (!bVar2) break;

                if (DAT_0043ddc0[param1].DAT_40[piVar3] == -1)
                {
                    if (bVar2)
                    {
                        iVar6 = 0;
                        iVar7 = 0;
                        pcVar8 = 0;
                        pcVar4 = pcVar8;

                        do
                        {
                            bVar1 = DAT_0043f720[pcVar4].DAT_00;
                            pcVar4++;

                            if (!bVar1)
                            {
                                DAT_0043ddc0[param1].DAT_40[iVar9] = DAT_0043f71c | 0x1000000;
                                iVar6 = DAT_0043ddc0[param1].DAT_200;

                                if (0x7f < param3)
                                    param3 = 0x7f;

                                DAT_0043f720[pcVar8].DAT_06 = param2;
                                DAT_0043ddc0[param1].DAT_200 = iVar6 + 1;
                                uVar5 = (uint)DAT_0043f71c;
                                DAT_0043f720[pcVar8].DAT_00 = true;
                                uVar5 = (uint)param1 << 0x18 | (uint)iVar9 << 0x10 | uVar5;
                                DAT_0043f720[pcVar8].DAT_01 = (sbyte)param3;
                                DAT_0043f720[pcVar8].DAT_02 = param4;
                                DAT_0043f720[pcVar8].DAT_03 = param5;
                                DAT_0043f720[iVar7].DAT_08 = uVar5;
                                DAT_0043f71c = DAT_0043f71c + 1 & 0x7fff;
                                DAT_0043f8a0++;
                                return (int)uVar5;
                            }

                            iVar6++;
                            pcVar8++;
                            iVar7++;
                        } while (iVar6 < 0x20);
                    }
                }
                else
                    iVar9++;
            }
        }

        return -1;
    }

    private int FUN_001fad50(long param1, long param2, int param3, long param4, long param5)
    {
        bool bVar1;
        int iVar2;
        int iVar3;
        long lVar4;
        int iVar5;
        int iVar6;
        int puVar7;
        int iVar8;
        int iVar9;
        int[] local_80 = new int[2];
        int[] local_70 = new int[2];
        int local_60;
        int local_5c;
        int local_58;

        puVar7 = 0;
        local_58 = -1;

        if (DAT_0043fdb0 != 0)
            return -2;

        iVar9 = 0;
        local_5c = param3;
        //WaitSema

        do
        {
            lVar4 = FUN_001f8fb8(DAT_0043ddc0[puVar7], (short)param1, param5, out local_60);
            iVar2 = local_58;

            if (-1 < lVar4)
            {
                iVar8 = 1;

                if (-1 < local_60)
                    iVar8 = 2;

                iVar5 = iVar8;

                if (DAT_0043ddc0[puVar7].DAT_40[0] == -1)
                {
                    iVar5 = iVar8 - 1;
                    bVar1 = true;

                    if (iVar5 < 1) goto LAB_001fae50;
                }

                iVar6 = 1;
                goto LAB_001fae1c;
            }

            iVar9++;
            puVar7++;
        } while (iVar9 < 6);

        goto LAB_001fafa8;

        LAB_001fae50:
        if (bVar1)
        {
            iVar5 = iVar8;

            if (!DAT_0043f720[0].DAT_00)
            {
                iVar5 = iVar8 - 1;
                bVar1 = true;

                if (iVar5 < 1)
                {
                    if (bVar1)
                    {
                        iVar5 = 0;
                        iVar6 = 0;

                        do
                        {
                            if (DAT_0043e9d8[iVar6].DAT_00 == 0)
                            {
                                DAT_0043fda8++;
                                DAT_0043e9d8[iVar6].DAT_00 = 1;
                                DAT_0043e9d8[iVar6].DAT_04 = iVar8;
                                DAT_0043e9d8[iVar6].DAT_10 = (int)param2;
                                DAT_0043e9d8[iVar6].DAT_14 = (int)param4;
                                FUN_001facd8(iVar8, (int)param2, (int)param4, local_80, local_70);
                                iVar3 = FUN_001fafe8(iVar9, (short)lVar4, local_80[0], (sbyte)local_5c, (sbyte)local_70[0]);
                                DAT_0043e9d8[iVar6].DAT_08 = iVar3;
                                iVar2 = iVar5;

                                if (iVar8 == 2)
                                {
                                    iVar3 = FUN_001fafe8(iVar9, (short)local_60, local_80[1], (sbyte)local_5c, (sbyte)local_70[1]);
                                    DAT_0043e9d8[iVar6].DAT_0c = iVar3;
                                }

                                break;
                            }

                            iVar5++;
                            iVar6++;
                        } while (iVar5 < 0x40);
                    }

                    goto LAB_001fafa8;
                }
            }

            iVar6 = 1;

            while (bVar1 = iVar6 < 0x20)
            {
                if (!DAT_0043f720[iVar6].DAT_00)
                {
                    iVar5--;

                    if (iVar5 < 1)
                    {
                        if (bVar1)
                        {
                            iVar5 = 0;
                            iVar6 = 0;

                            do
                            {
                                if (DAT_0043e9d8[iVar6].DAT_00 == 0)
                                {
                                    DAT_0043fda8++;
                                    DAT_0043e9d8[iVar6].DAT_00 = 1;
                                    DAT_0043e9d8[iVar6].DAT_04 = iVar8;
                                    DAT_0043e9d8[iVar6].DAT_10 = (int)param2;
                                    DAT_0043e9d8[iVar6].DAT_14 = (int)param4;
                                    FUN_001facd8(iVar8, (int)param2, (int)param4, local_80, local_70);
                                    iVar3 = FUN_001fafe8(iVar9, (short)lVar4, local_80[0], (sbyte)local_5c, (sbyte)local_70[0]);
                                    DAT_0043e9d8[iVar6].DAT_08 = iVar3;
                                    iVar2 = iVar5;

                                    if (iVar8 == 2)
                                    {
                                        iVar3 = FUN_001fafe8(iVar9, (short)local_60, local_80[1], (sbyte)local_5c, (sbyte)local_70[1]);
                                        DAT_0043e9d8[iVar6].DAT_0c = iVar3;
                                    }

                                    break;
                                }

                                iVar5++;
                                iVar6++;
                            } while (iVar5 < 0x40);
                        }

                        goto LAB_001fafa8;
                    }
                    else
                        iVar6++;
                }
                else
                    iVar6++;
            }
        }

        goto LAB_001fafa8;

        LAB_001fae1c:
        bVar1 = iVar6 < 0x40;

        if (bVar1)
        {
            if (DAT_0043ddc0[puVar7].DAT_40[iVar6] == 0)
            {
                iVar5--;

                if (iVar5 < 1) goto LAB_001fae50;

                iVar6++;
            }
            else
                iVar6++;

            goto LAB_001fae1c;
        }

        LAB_001fafa8:
        local_58 = iVar2;
        //SignalSema
        return local_58;
    }

    private void FUN_001facd8(int param1, int param2, int param3, int[] param4, int[] param5)
    {
        int iVar1;

        if (param1 == 2)
        {
            iVar1 = param2 * (0x80 - param3) >> 6;
            param4[0] = iVar1;

            if (0x7f < iVar1)
                param4[0] = 0x7f;

            iVar1 = param2 * param3 >> 6;
            param4[1] = iVar1;

            if (0x7f < iVar1)
                param4[1] = 0x7f;

            param5[0] = 0;
            param5[1] = 0x7f;
            return;
        }

        param4[0] = param2;
        param4[1] = 0;
        param5[1] = 0x40;
        param5[0] = param3;
    }

    private int FUN_001f8fb8(DAT_0043ddc0 param1, short param2, long param3, out int param4)
    {
        short sVar1;
        int iVar2;
        int psVar3;
        int iVar4;
        int iVar5;

        iVar2 = -1;
        param4 = -1;

        if (param1.DAT_00 == 2)
        {
            iVar2 = -2;

            if (param1.DAT_24 != null)
            {
                iVar2 = param1.DAT_28;
                iVar5 = 0;
                iVar4 = iVar2;

                if (0 < iVar2)
                {
                    while (true)
                    {
                        iVar4 >>= 1;
                        psVar3 = iVar4 * 2;
                        sVar1 = param1.DAT_24[psVar3];

                        if (sVar1 == param2)
                        {
                            sVar1 = param1.DAT_24[psVar3 + 1];
                            param4 = sVar1;

                            if (param3 != 0)
                            {
                                iVar4 += param1.DAT_2c;

                                if (-1 < sVar1)
                                    param4 = sVar1 + param1.DAT_2c;
                            }

                            return iVar4;
                        }

                        if (sVar1 <= param2)
                        {
                            iVar5 = iVar4 + 1;
                            iVar4 = iVar2;
                        }

                        if (iVar4 - iVar5 < 1) break;

                        iVar2 = iVar4;
                        iVar4 += iVar5;
                    }
                }

                return -3;
            }
        }

        return iVar2;
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
        long lVar1;

        if (iGpffff82c4 >> 5 != 0)
        {
            lVar1 = FUN_001fb878(0x47c048, (int)param1, 1);
            FUN_001fba30((uint)lVar1, iGpffff82c4 >> 5);
        }
    }
}
