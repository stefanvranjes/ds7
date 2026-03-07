using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAT_0027905c
{
    public sbyte[] DAT_842; //0x842
    public sbyte[] DAT_1842; //0x1842
    public sbyte[] DAT_2842; //0x2842
    public sbyte DAT_3842;
    public sbyte[] DAT_3843;
}

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public float[] DAT_002648e0 = new float[0x1000];
    public int[] DAT_002688e0 = new int[0x1000];
    public int[] DAT_0035b200;
    public int[] DAT_0035b230;
    public Vector3 DAT_003694b4;
    public Vector3 DAT_003694cc;
    public float DAT_003694d0;
    public int DAT_003694d4;
    public Vector4[] DAT_00369598 = new Vector4[4];
    public Vector4[] DAT_003695d8 = new Vector4[4];
    public Vector4[] DAT_00369618 = new Vector4[4];
    public Vector4[] DAT_00369658 = new Vector4[4];
    public int DAT_00369848;
    public int DAT_00369850;
    public int DAT_00369864;
    public int DAT_003da1e0;
    public int DAT_003da1e4;
    public int DAT_003da1e8;
    public int DAT_003da1ec;
    public int[] DAT_003da3b0 = new int[]
    {
        -1, -1, 1, 0, -1, 1, -1, 0, 0, 1, 0, 0, -1, 1, 1, 0, 1, 1
    };
    public int[] DAT_003db258;
    public int[] DAT_003db2b0;
    public float[] DAT_00484878 = new float[]
    {
        3.2f, 1.6f, 0f, -1.6f, -3.2f, -4.8f
    };
    public float[] DAT_00484930 = new float[]
    {
        0f, 0.0625f, 0.125f, 0.1875f, 0.25f, 0.3125f, 0.375f, 0.4375f,
        0.5f, 0.5625f, 0.625f, 0.6875f, 0.75f, 0.8125f, 0.875f, 0.9375f, 1f
    };
    public float DAT_0049d0ec = 3.1415925f;
    public float DAT_0049d0f0 = 0.15915494f;
    public float DAT_0049d0f4 = 6.283185f;
    public short DAT_0049d878 = 0;
    public short DAT_0049d87a = 0;
    public float DAT_0049eb00 = 0;
    public float DAT_0049eb04 = 2.3999999f;
    public float DAT_0049eb08 = 0;
    public float DAT_0049eb0c = 76.799995f;
    public float[] DAT_0049eb20 = new float[]
    {
        0, 0.5235988f, 3.2f, 0.5f, 1.6f, 76.7999954f, 1f
    }; //...
    public float DAT_0049ec88 = 0.03125f;
    public float DAT_0049ec8c = 0.125f;
    public float DAT_0049ec90 = 0.03125f;
    public float DAT_0049ec94 = 0.03125f;
    public float DAT_0049ec98 = 1.6f;
    public float DAT_0049ec9c = -1.6f;
    public float DAT_0049eca0 = 3.2f;
    public float DAT_0049ef18 = 108.125f;
    public DAT_0027905c PTR_DAT_004a00ec;
    public int DAT_004a00fc = 0;
    public ushort DAT_004a0100 = 0;
    public float DAT_004a0104 = 100f;
    public float DAT_004a0108 = 26f;
    public float DAT_004a010c = 230f;
    public float DAT_004a0110 = 1.5707964f;
    public float DAT_004a0114 = -1.5707964f;
    public float DAT_004a0118 = 3.1415927f;
    public float DAT_004a011c = 2.0943952f;
    public float DAT_004a0120 = 1.0471976f;
    public float DAT_004a0124 = -2.0943952f;
    public float DAT_004a0128 = -1.0471976f;
    public float DAT_004a012c = 0.6f;
    public float DAT_004a0130 = 1f;
    public float DAT_004a0134 = 240f;
    public float DAT_004a0138 = 320f;
    public float DAT_004a013c = 200f;
    public float DAT_004a0140 = 32f;
    public float DAT_004a0144 = 608f;
    public float DAT_004a0148 = 64f;
    public float DAT_004a014c = 416f;
    public float DAT_004a0460 = 0.03125f;
    public int DAT_004a420c;
    public int DAT_004a4210;
    public int DAT_004a4258;
    public int DAT_004a4268;
    public float DAT_004a6b1c;
    public int DAT_0050ee88;
    public int DAT_0050ee8c;
    public int DAT_0050ee90;
    public int DAT_0050ee94;
    public Vector3 DAT_0050ee98;
    public int DAT_0050eea8;
    public float DAT_0050eeac;
    public float DAT_0050eeb0;
    public float DAT_0050eeb4;
    public float DAT_0050eeb8;
    public Vector2 DAT_0050eebc;
    public int DAT_0050eec4;
    public int DAT_0050eec8;
    public int[] DAT_0050ef98;
    public int[] DAT_0050eff0;
    public short[] DAT_0050efb0;
    public int[] DAT_0050f7f0;
    public int DAT_0050f808;
    public int DAT_0050f820;
    public int[] DAT_0050f838;
    public int DAT_0050f848;
    public int[] DAT_0050f850;
    public int DAT_0050f860;
    public int[] DAT_0050f868;

    private void FUN_00181698(DAT_0027905c param1)
    {
        FUN_00181b08(param1);
        FUN_00181c90(param1);
        FUN_001824e0(param1);
        FUN_00182020(param1);
    }

    private int FUN_00181ac0(int param1)
    {
        switch (param1)
        {
            case 9:
            case 20:
                return 3;
            case 10:
            case 12:
            case 13:
            case 22:
            case 23:
            case 36:
            case 42:
            case 48:
            case 54:
            case 60:
                return 1;
            default:
                return 2;
            case 14:
                return 0;
        }
    }

    private void FUN_00181b08(DAT_0027905c param1)
    {
        sbyte sVar1;
        int iVar2;
        uint uVar3;
        uint uVar4;
        float fVar5;
        float fVar6;
        float fVar7;
        float fVar8;

        uVar4 = 0;
        fVar7 = DAT_0049ec88;

        while (true)
        {
            sVar1 = param1.DAT_842[uVar4];
            iVar2 = FUN_00181ac0(sVar1);
            fVar5 = DAT_00484878[iVar2];
            DAT_002688e0[uVar4] = iVar2;
            DAT_002648e0[uVar4] = fVar5;

            if (sVar1 < 0x20)
            {
                fVar6 = DAT_00484930[sVar1 & 0xf];
                fVar8 = DAT_00484930[sVar1 >> 4];
            }
            else
            {
                iVar2 = Utilities.FUN_001451b0(sVar1);
                fVar6 = DAT_00484930[iVar2];
                fVar8 = DAT_0049ec8c;
            }

            uVar3 = uVar4 & 0x3f;
            iVar2 = (int)uVar4 >> 6;
            uVar4++;
            RenderQueue.FUN_00104a10(-fVar5, uVar3, iVar2, 0);
            RenderQueue.FUN_00104aa0(fVar6 + fVar7, fVar8 + fVar7, uVar3, iVar2, 0, 0, 1);

            if (0xfff < (int)uVar4) break;
        }
    }

    private void FUN_00181c90(DAT_0027905c param1)
    {
        int iVar1;
        int iVar2;
        int iVar3;
        int iVar4;
        sbyte sVar5;
        sbyte sVar6;
        uint uVar7;
        uint uVar8;
        long lVar9;
        int iVar10;
        uint uVar11;
        float fVar12;
        float fVar13;
        float fVar14;
        float fVar15;

        iVar10 = 0;
        iVar1 = 0;
        iVar4 = 0;

        do
        {
            sVar6 = param1.DAT_842[iVar4];
            iVar1++;

            if (sVar6 < 0x20)
            {
                sVar5 = param1.DAT_1842[iVar4];

                if (sVar5 == 0)
                {
                    if (param1.DAT_2842[iVar4] == 0 && sVar6 != 10 && sVar6 != 22 && sVar6 != 23 && sVar6 != 12)
                        goto LAB_00181d54;

                    sVar5 = param1.DAT_1842[iVar4];
                }

                iVar2 = iVar10 + 1;
                iVar3 = iVar10 + 2;
                iVar10 = iVar2;

                if (sVar5 != 0 && param1.DAT_2842[iVar4] != 0)
                    iVar10 = iVar3;
            }

            LAB_00181d54:
            iVar4 = iVar1;
        } while (iVar1 < 0x1000);

        DAT_003da1e4 = iVar10 * 6;
        DAT_003da1e8 = iVar10 << 1;

        if (iVar10 == 0)
        {
            uVar11 = 0;

            do
            {
                uVar7 = uVar11 & 0x3f;
                iVar4 = (int)uVar11 >> 6;
                uVar11++;
                RenderQueue.FUN_00104aa0(0, 0, uVar7, iVar4, 1, 0, 0);
                RenderQueue.FUN_00104aa0(0, 0, uVar7, iVar4, 2, 0, 0);
            } while ((int)uVar11 < 0x1000);
        }
        else
        {
            fVar12 = 0.0f;
            uVar11 = 0;
            fVar14 = 0.0f;
            fVar15 = 0.0f;
            fVar13 = DAT_0049ec90;

            do
            {
                uVar7 = uVar11 & 0x3f;
                iVar10 = (int)uVar11 >> 6;
                RenderQueue.FUN_00104aa0(0, 0, uVar7, iVar10, 1, 0, 0);
                RenderQueue.FUN_00104aa0(0, 0, uVar7, iVar10, 2, 0, 0);
                iVar4 = (int)uVar11;
                lVar9 = param1.DAT_1842[iVar4];
                sVar6 = param1.DAT_842[iVar4];
                sVar5 = param1.DAT_2842[iVar4];

                if (sVar6 == 12)
                    lVar9 = -36;
                else if (sVar6 < 13)
                {
                    if (sVar6 == 10)
                        lVar9 = -40;
                }
                else if (sVar6 == 22)
                    lVar9 = -39;
                else if (sVar6 == 23)
                    lVar9 = -38;

                if (lVar9 == 0)
                {
                    if (sVar5 != 0)
                    {
                        sVar6 = param1.DAT_842[iVar4];

                        if (sVar6 < 0x20)
                        {
                            if (sVar5 != 0)
                            {
                                fVar12 = DAT_00484930[(sVar5 >> 4) + 8];
                                fVar14 = DAT_00484930[sVar5 & 0xf];
                                RenderQueue.FUN_00104a10(-0, uVar7, iVar10, 2); //0x80000000
                                RenderQueue.FUN_00104aa0(fVar14 + fVar13, fVar12 + fVar13, uVar7, iVar10, 2, 0, 1);
                            }

                            uVar8 = (uint)lVar9;

                            if (lVar9 < 1)
                            {
                                if (lVar9 < 0)
                                {
                                    uVar8 += 0x40;
                                    lVar9 = (long)(int)uVar8;
                                    fVar14 = DAT_00484930[uVar8 & 0xf];
                                    fVar12 = DAT_00484930[(int)uVar8 >> 4];
                                }
                            }
                            else
                            {
                                fVar14 = DAT_00484930[uVar8 & 0xf];
                                fVar12 = DAT_00484930[((int)uVar8 >> 4) + 4];
                            }

                            if (lVar9 != 0)
                            {
                                RenderQueue.FUN_00104a10(-fVar15, uVar7, iVar10, 1);
                                RenderQueue.FUN_00104aa0(fVar14 + DAT_0049ec90, fVar12 + DAT_0049ec90, uVar7, iVar10, 1, 0, 1);
                            }
                        }
                    }
                }
                else
                {
                    sVar6 = param1.DAT_842[iVar4];

                    if (sVar6 < 0x20)
                    {
                        if (sVar5 != 0)
                        {
                            fVar12 = DAT_00484930[(sVar5 >> 4) + 8];
                            fVar14 = DAT_00484930[sVar5 & 0xf];
                            RenderQueue.FUN_00104a10(-0, uVar7, iVar10, 2); //0x80000000
                            RenderQueue.FUN_00104aa0(fVar14 + fVar13, fVar12 + fVar13, uVar7, iVar10, 2, 0, 1);
                        }

                        uVar8 = (uint)lVar9;

                        if (lVar9 < 1)
                        {
                            if (lVar9 < 0)
                            {
                                uVar8 += 0x40;
                                lVar9 = (long)(int)uVar8;
                                fVar14 = DAT_00484930[uVar8 & 0xf];
                                fVar12 = DAT_00484930[(int)uVar8 >> 4];
                            }
                        }
                        else
                        {
                            fVar14 = DAT_00484930[uVar8 & 0xf];
                            fVar12 = DAT_00484930[((int)uVar8 >> 4) + 4];
                        }

                        if (lVar9 != 0)
                        {
                            RenderQueue.FUN_00104a10(-fVar15, uVar7, iVar10, 1);
                            RenderQueue.FUN_00104aa0(fVar14 + DAT_0049ec90, fVar12 + DAT_0049ec90, uVar7, iVar10, 1, 0, 1);
                        }
                    }
                }

                uVar11++;
            } while ((int)uVar11 < 0x1000);
        }
    }

    private void FUN_00182020(DAT_0027905c param1)
    {
        float fVar1;
        int iVar2;
        int iVar3;
        int iVar4;
        uint uVar5;
        uint uVar6;
        float fVar7;

        fVar1 = DAT_0049ec94;
        iVar4 = 0;
        iVar2 = 0;

        do
        {
            iVar3 = iVar2;
            iVar2++;

            if (param1.DAT_842[iVar3] == 14)
                iVar4++;
        } while (iVar2 < 0x1000);

        DAT_003da1ec = iVar4 << 1;

        if (iVar4 != 0)
        {
            uVar6 = 0;
            fVar7 = DAT_0049ec98;

            do
            {
                uVar5 = uVar6 & 0x3f;
                iVar2 = (int)uVar6 >> 6;
                RenderQueue.FUN_00104aa0(fVar1, fVar1, uVar5, iVar2, 3, 0, 0);
                iVar4 = (int)uVar6;
                uVar6++;

                if (param1.DAT_842[iVar4] == 14)
                {
                    RenderQueue.FUN_00104a10(-fVar7, uVar5, iVar2, 3);
                    RenderQueue.FUN_00104aa0(fVar1, fVar1, uVar5, iVar2, 3, 0, 1);
                }
            } while ((int)uVar6 < 0x1000);
        }
    }

    private void FUN_00182148(DAT_0027905c param1)
    {
        sbyte sVar1;
        byte bVar2;
        long lVar3;
        int[] piVar4;
        int ppiVar4;
        uint uVar5;
        int iVar6;
        int puVar7;
        int iVar8;
        int iVar9;
        int puVar10;
        float[] local_100 = new float[8];
        Vector4[] auStack_e0 = new Vector4[4];
        Vector4[] auStack_a0 = new Vector4[4];

        puVar10 = 0;
        local_100[0] = 4.1887903f;
        local_100[1] = 3.1415927f;
        local_100[2] = 2.0943952f;
        local_100[3] = 1.0471976f;
        local_100[4] = 0;
        local_100[5] = 5.2359877f;
        FUN_001e14b8();
        FUN_001e14b8();

        if (0 < param1.DAT_3842)
        {
            do
            {
                sVar1 = param1.DAT_3843[puVar10 + 2];
                bVar2 = (byte)param1.DAT_3843[puVar10 + 3];
                lVar3 = Utilities.FUN_001451b0(param1.DAT_3843[puVar10]);
                iVar9 = sVar1;

                if (lVar3 == 4)
                {
                    if ((bVar2 & 1) == 0)
                        piVar4 = DAT_0035b200;
                    else
                        piVar4 = DAT_0035b230;

                    iVar8 = 0;
                    ppiVar4 = 0;
                    iVar6 = piVar4[ppiVar4];

                    while (true)
                    {
                        uVar5 = (uint)((sbyte)bVar2 + piVar4[ppiVar4 + 1]);
                        ppiVar4 += 2;

                        if ((((iVar9 + iVar6 | (int)uVar5) & 0xffffffc0) == 0 &&
                            param1.DAT_842[iVar9 + iVar6 + (int)uVar5 * 0x40] - 13 < 2) || 5 < ++iVar8) break;

                        iVar6 = piVar4[ppiVar4];
                    }

                    if (iVar8 == 6)
                    {
                        if ((uVar5 & 1) == 0)
                            piVar4 = DAT_0035b200;
                        else
                            piVar4 = DAT_0035b230;

                        iVar8 = 0;
                        ppiVar4 = 0;
                        iVar6 = piVar4[ppiVar4];

                        while (true)
                        {
                            uVar5 = (uint)((sbyte)bVar2 + piVar4[ppiVar4 + 1]);
                            ppiVar4 += 2;

                            if ((((iVar9 + iVar6 | (int)uVar5) & 0xffffffc0) == 0 &&
                                param1.DAT_842[iVar9 + iVar6 + (int)uVar5 * 0x40] == 15) || 5 < ++iVar8) break;

                            iVar6 = piVar4[ppiVar4];
                        }

                        if (iVar8 == 6)
                            iVar8 = 0;
                    }

                    FUN_001e1058(local_100[iVar8], auStack_a0);
                    puVar7 = (iVar9 + bVar2 * 0x40) * 2;
                    FUN_001e10b8(DAT_0049eb20[puVar7], DAT_0049eb20[puVar7 + 1], 0, auStack_e0);
                }
            }
        }
    }

    private void FUN_001824e0(DAT_0027905c param1)
    {
        float fVar1;
        int iVar2;
        uint uVar3;
        uint uVar4;
        int puVar5;
        int piVar6;
        uint uVar7;
        float fVar8;
        float fVar9;
        float fVar10;
        int local_70;
        int local_6c;

        fVar1 = DAT_0049eca0;
        uVar7 = 0;
        fVar9 = DAT_0049ec9c;
        RenderQueue.FUN_00103f90(10);
        local_6c = 0;
        local_70 = 0;
        iVar2 = 0;

        do
        {
            piVar6 = 0;
            iVar2 = FUN_00181ac0(param1.DAT_842[iVar2]);
            fVar10 = DAT_00484878[iVar2];
            iVar2 = DAT_003da3b0[0];

            while (true)
            {
                uVar4 = (uVar7 & 0x3f) + (uint)iVar2 + (uVar7 >> 6 & (uint)DAT_003da3b0[piVar6 / 4 + 2]);
                uVar3 = (uint)((int)uVar7 >> 6) + (uint)DAT_003da3b0[piVar6 / 4 + 1];
                piVar6 += 12;
                fVar8 = DAT_0049eca0;

                if (((uVar4 | uVar3) & 0xffffffc0) == 0)
                {
                    iVar2 = FUN_00181ac0(param1.DAT_842[uVar4 + uVar3 * 0x40]);
                    fVar8 = DAT_00484878[iVar2];
                }

                if (fVar10 < fVar8) break;

                if (0x47 < (int)piVar6) goto LAB_00182630;

                iVar2 = DAT_003da3b0[piVar6 / 4];
            }

            local_70++;
            local_6c += 4;
            puVar5 = (int)uVar7 * 2;
            RenderQueue.FUN_001025e8(DAT_0049eb20[puVar5], -fVar10, DAT_0049eb20[puVar5 + 1], -fVar9, -fVar1);
            LAB_00182630:
            uVar7++;
            iVar2 = (int)uVar7;

            if (0xfff < (int)uVar7)
            {
                if (local_70 == 0)
                    DAT_003da1e0 = 0;
                else
                {
                    if (local_6c < 0)
                        local_6c += 3;

                    DAT_003da1e0 = local_6c >> 2;
                }

                return;
            }
        } while (true);
    }

    private void FUN_0019d838()
    {
        ushort uVar1;
        int iVar2;
        int iVar3;
        PTR_DAT_004a3660 pVar3;
        int piVar4;
        ushort uVar5;
        float fVar6;
        int iVar7;
        int iVar8;
        float fVar9;
        float fVar10;
        float fVar11;
        float fVar12;
        float fVar13;
        uint uVar14;
        int[] local_a0;
        int[] local_80;

        piVar4 = 0;
        iVar7 = DAT_0050ee88 + DAT_0050ee8c * 0x40;
        pVar3 = InputManager.instance.DAT_004a3660[InputManager.instance.DAT_004a366c];
        fVar12 = (float)(pVar3.DAT_08 - pVar3.DAT_09) / DAT_004a0104;

        if (fVar12 != 0.0f)
        {
            DAT_0050eeb0 += fVar12;

            if (DAT_0050eeb0 < DAT_004a0108)
                DAT_0050eeb0 = DAT_004a0108;

            if (DAT_004a010c < DAT_0050eeb0)
                DAT_0050eeb0 = DAT_004a010c;
        }

        if (fVar12 != 0.0f)
        {
            DAT_0050eea8 = 0;
            DAT_0050eeac = -DAT_0050eeb0;
        }

        uVar1 = InputManager.instance.DAT_004a3660[InputManager.instance.DAT_004a366c].DAT_72;
        uVar5 = (ushort)(uVar1 & 0xf);

        if (DAT_0050eec8 == 0 || (uVar1 & 0xf) == 0)
        {
            DAT_004a00fc++;

            if (0x78 < DAT_004a00fc)
            {
                DAT_0050eeb4 = DAT_0049eb20[(iVar7 * 8) / 4];
                DAT_0050eeb8 = DAT_0049eb20[(iVar7 * 8) / 4 + 1];
            }

            goto LAB_0019dc48;
        }

        if (DAT_004a0100 != uVar5)
        {
            DAT_0050eeb4 = DAT_0049eb20[(iVar7 * 8) / 4];
            DAT_0050eeb8 = DAT_0049eb20[(iVar7 * 8) / 4 + 1];
        }

        fVar12 = DAT_0050eeb4;
        DAT_0050eec4 = 0;
        uVar14 = 0;

        switch (uVar5)
        {
            default:
                goto switchD_0019d9d8_caseD_0;
            case 1:
                fVar6 = DAT_004a0110;
                break;
            case 2:
                fVar6 = DAT_004a0114;
                goto LAB_0019e030;
            case 4:
                fVar6 = DAT_004a0118;
                break;
            case 5:
                fVar6 = DAT_004a011c;
                goto LAB_0019e030;
            case 6:
                fVar6 = DAT_004a0124;
                LAB_0019e030:
                uVar14 = BitConverter.ToUInt32(BitConverter.GetBytes(fVar6), 0);
                goto switchD_0019d9d8_caseD_0;
            case 9:
                fVar6 = DAT_004a0120;
                break;
            case 10:
                fVar6 = DAT_004a0128;
                break;
        }

        uVar14 = BitConverter.ToUInt32(BitConverter.GetBytes(fVar6), 0);
        switchD_0019d9d8_caseD_0:
        iVar8 = -1;
        fVar13 = DAT_0050eeb8;
        DAT_004a0100 = uVar5;
        fVar10 = Utilities.FUN_0022bc48(uVar14);
        fVar9 = DAT_0049eb00;
        fVar12 = fVar12 + DAT_0049eb00 * fVar10;
        fVar10 = Utilities.FUN_0022bd10(uVar14);
        fVar13 = fVar13 + fVar9 * fVar10;
        fVar10 = fVar12 - DAT_0049eb20[iVar7 * 2];
        fVar9 = fVar13 - DAT_0049eb20[iVar7 * 2 + 1];
        fVar9 = fVar9 * fVar9 + fVar10 * fVar10;
        local_a0 = new int[6] { -0x41, -0x40, -1, 1, 0x3f, 0x40 };
        local_80 = new int[6] { -0x40, -0x3f, -1, 1, 0x40, 0x41 };

        if ((DAT_0050ee8c & 1) == 0)
        {
            iVar3 = 5;

            do
            {
                iVar2 = local_a0[piVar4] + iVar7;
                fVar10 = fVar9;
                uVar14 = (uint)iVar8;

                if (iVar2 < 0x1000)
                {
                    fVar11 = fVar12 - DAT_0049eb20[iVar2 * 2];
                    fVar10 = fVar13 - DAT_0049eb20[iVar2 * 2 + 1];
                    fVar10 = fVar10 * fVar10 + fVar11 * fVar11;
                    uVar14 = (uint)iVar2;

                    if (fVar9 <= fVar10)
                    {
                        fVar10 = fVar9;
                        uVar14 = (uint)iVar8;
                    }
                }

                iVar8 = (int)uVar14;
                iVar3--;
                piVar4++;
                fVar9 = fVar10;
            } while (-1 < iVar3);
        }
        else
        {
            iVar3 = 0;
            piVar4 = 0;

            do
            {
                iVar2 = local_80[piVar4] + iVar7;
                fVar10 = fVar9;
                uVar14 = (uint)iVar8;

                if (iVar2 < 0x1000)
                {
                    fVar11 = fVar12 - DAT_0049eb20[iVar2 * 2];
                    fVar10 = fVar13 - DAT_0049eb20[iVar2 * 2 + 1];
                    fVar10 = fVar10 * fVar10 + fVar11 * fVar11;
                    uVar14 = (uint)iVar2;

                    if (fVar9 <= fVar10)
                    {
                        fVar10 = fVar9;
                        uVar14 = (uint)iVar8;
                    }
                }

                iVar8 = (int)uVar14;
                iVar3++;
                piVar4++;
                fVar9 = fVar10;
            } while (iVar3 < 6);
        }

        if (iVar8 < 0)
        {
            DAT_0050eeb4 = DAT_0049eb20[iVar7 * 2];
            DAT_0050eeb8 = DAT_0049eb20[iVar7 * 2 + 1];
        }
        else
        {
            DAT_0050ee8c = iVar8 >> 6;
            DAT_0050ee88 = iVar8 + DAT_0050ee8c * -0x40;

            if (DAT_0049eb00 * DAT_0049eb00 * DAT_004a012c <= fVar10)
            {
                fVar12 = DAT_0050eeb4;
                fVar13 = DAT_0050eeb8;
            }

            DAT_0050eeb8 = fVar13;
            DAT_0050eeb4 = fVar12;
            DAT_004a00fc = 0;
            DAT_0050eebc = Utilities.FUN_001e08d8(DAT_0049eb20[iVar8 * 2], DAT_0049eb20[iVar8 * 2 + 1], DAT_00369618);
            fVar13 = (DAT_004a0130 - DAT_0050eebc.y) * DAT_004a0134;
            fVar9 = (DAT_0050eebc.x + DAT_004a0130) * DAT_004a0138;
            fVar12 = DAT_004a0140;

            if (DAT_0050f7f0[0] != 0)
                fVar12 = DAT_004a013c;

            if (fVar9 < fVar12 || DAT_004a0144 < fVar9 || fVar13 < DAT_004a0148 || DAT_004a014c < fVar13)
            {
                DAT_0050ee98.x = DAT_0050eeb4;
                DAT_0050ee98.y = DAT_0050eeb8;
            }
        }

        LAB_0019dc48:
        fVar12 = DAT_004a0130;
        pVar3 = InputManager.instance.DAT_004a3660[InputManager.instance.DAT_004a366c];
        fVar9 = DAT_0049eb20[0];
        fVar13 = DAT_0050ee98.x + pVar3.DAT_14;
        DAT_0050ee98.y += pVar3.DAT_18;

        if (fVar9 <= fVar13)
            fVar9 = fVar13;

        DAT_0050ee98.x = fVar9;

        if (DAT_0049ef18 < fVar9)
            DAT_0050ee98.x = DAT_0049ef18;

        if (DAT_0049eb20[1] < DAT_0050ee98.y)
            DAT_0050ee98.y = DAT_0049eb20[1];

        if (DAT_0050ee98.y < DAT_004a6b1c)
            DAT_0050ee98.y = DAT_004a6b1c;

        DAT_003694b4.y = DAT_0050eea8;
        DAT_003694b4.z = DAT_0050eeac;
        DAT_003694b4.x = 0;
        DAT_003694d4 = 0;
        DAT_003694cc.x = 0;
        DAT_003694d0 = DAT_004a0130;
        Utilities.FUN_001e07f0(out DAT_003694b4, DAT_003694b4, DAT_0050ee98);
        Utilities.FUN_001e0c38(DAT_00369598, DAT_003694b4, DAT_0050ee98, DAT_003694cc);
        Utilities.FUN_001e0e40(DAT_00369618, DAT_00369598, DAT_003695d8);
        Utilities.FUN_001e0eb0(DAT_00369658, DAT_00369618);

        if (DAT_0050eec8 != 0)
        {
            DAT_0050eebc = Utilities.FUN_001e08d8(DAT_0049eb20[(DAT_0050ee88 + DAT_0050ee8c * 0x40) * 2], 
                                                  DAT_0049eb20[(DAT_0050ee88 + DAT_0050ee8c * 0x40) * 2 + 1], DAT_00369618);
            fVar9 = (DAT_0050eebc.x + fVar12) * DAT_004a0138;
            fVar12 = (fVar12 - DAT_0050eebc.y) * DAT_004a0134;

            if (DAT_0050eec4 == 0)
            {
                fVar13 = DAT_004a0140;

                if (DAT_0050f7f0[0] != 0)
                    fVar13 = DAT_004a013c;

                if (fVar13 <= fVar9 && fVar9 <= DAT_004a0144 && DAT_004a0148 <= fVar12 && fVar12 <= DAT_004a014c)
                    goto LAB_0019df18;
            }

            DAT_0050eec4 = 1;
            DAT_0050ee88 = (int)((DAT_0050ee98.x - DAT_0049eb08) / DAT_0049eb00);

            if (DAT_0050ee88 < 0)
                DAT_0050ee88 = 0;

            if (0x3f < DAT_0050ee88)
                DAT_0050ee88 = 0x3f;

            DAT_0050ee8c = (int)((DAT_0049eb0c - DAT_0050ee98.x) / DAT_0049eb04);

            if (DAT_0050ee8c < 0)
                DAT_0050ee8c = 0;

            if (0x3f < DAT_0050ee8c)
                DAT_0050ee8c = 0x3f;

            iVar3 = DAT_0050ee88 + DAT_0050ee8c * 0x40;
            DAT_0050eebc = Utilities.FUN_001e08d8(DAT_0049eb20[iVar3 * 2], DAT_0049eb20[iVar3 * 2 + 1], DAT_00369618);
            DAT_0050eeb4 = DAT_0049eb20[iVar3 * 2];
            DAT_0050eeb8 = DAT_0049eb20[iVar3 * 2 + 1];
        }

        LAB_0019df18:
        DAT_0049d878 = (short)DAT_0050ee88;
        DAT_0050ee90 = (short)DAT_0050ee88 + (short)DAT_0050ee8c * 0x40;
        DAT_0049d87a = (short)DAT_0050ee8c;
    }

    public void FUN_001a83f8(DAT_0027905c param1)
    {
        //...
    }

    public void FUN_001a2a78()
    {
        int iVar1;
        PTR_DAT_004a3660 pVar2;
        uint uVar3;

        iVar1 = DAT_0050f7f0[2];

        if (DAT_0050f7f0[2] == 0)
            GameManager.instance.FUN_0019d820(DAT_0050f7f0);

        DAT_0050f7f0[1] = 0;
        DAT_00369848 = 0;

        do
        {
            DAT_00369864 = DAT_00369864 - 0x14;

            if (DAT_00369864 < 0)
                DAT_00369864 = 0;

            FUN_0019d838();
            //FUN_001a6a10
        } while (DAT_0050f7f0[0] != 0 || DAT_0050f850[0] != 0);

        GameManager.instance.iGpfffff1ac = 8;
        //FUN_00181698
        DAT_00369850 = 0;
        GameManager.instance.FUN_0019d808(DAT_0050f868);

        do
        {
            //FUN_001a6a10
            pVar2 = InputManager.instance.DAT_004a3660[InputManager.instance.DAT_004a366c];

            if ((pVar2.DAT_48 & 0x20) != 0) break;
        } while (pVar2.DAT_50 == 0);

        SoundManager.instance.FUN_00144bb0(5);
        DAT_00369850 = 0;
        GameManager.instance.FUN_0019d820(DAT_0050f868);

        do
        {
            DAT_00369864 -= 0x14;

            if (DAT_00369864 < 0)
                DAT_00369864 = 0;

            //FUN_001a6a10
        } while (DAT_0050f868[0] != 0);

        uVar3 = 0;

        do
        {
            //FUN_00104a10
            uVar3++;
        } while ((int)uVar3 < 0x1000);

        FUN_001a0620();
        GameManager.instance.iGpfffff1ac = 6;

        do
        {
            DAT_00369864 += 0x14;

            if (0xff < DAT_00369864)
                DAT_00369864 = 0xff;

            FUN_0019d838();
            //FUN_001a6a10
        } while (DAT_00369864 < 0xff);

        DAT_0050f7f0[1] = 1;

        if (iVar1 != 0)
            return;

        GameManager.instance.FUN_0019d808(DAT_0050f7f0);
    }

    public void FUN_001a0620()
    {
        float fVar1;
        sbyte sVar2;
        bool bVar3;
        float fVar4;
        int iVar5;
        int iVar6;
        uint uVar7;
        sbyte sVar8;
        uint uVar9;
        long lVar10;
        int puVar11;
        int pcVar12;
        uint uVar13;
        int pcVar14;
        int pcVar15;
        float fVar16;
        float fVar17;
        float fVar18;
        float fVar19;
        float fVar20;
        int local_80;

        fVar19 = DAT_004a0460;
        puVar11 = 0;
        DAT_0050ef98[0] = 0;
        DAT_0050ef98[4] = 0; //0x50efa8
        DAT_0050ef98[3] = 0; //0x50efa4
        DAT_0050ef98[2] = 0; //0x50efa0
        DAT_0050ef98[1] = 0; //0x50ef9c
        DAT_004a420c = (DAT_004a420c + 1) % 2;
        uVar13 = 0;
        DAT_004a4268 = 0;
        pcVar12 = 0;
        //FUN_0024c988
        sVar8 = PTR_DAT_004a00ec.DAT_842[pcVar12];

        while (true)
        {
            pcVar12++;

            if (0x1f < sVar8)
            {
                DAT_004a4268++;
                iVar5 = Utilities.FUN_00145150(sVar8);
                iVar6 = Utilities.FUN_001451b0(sVar8);
                DAT_0050ef98[iVar5]++;
                DAT_0050efb0[iVar5 * 6 + iVar6]++;
            }

            fVar16 = DAT_0049eb20[puVar11];
            fVar1 = DAT_0049eb20[puVar11 + 1];
            fVar17 = DAT_00484930[sVar8 & 0xf];
            fVar18 = DAT_00484930[sVar8 >> 4];
            uVar7 = uVar13 & 0x3f;
            iVar5 = (int)uVar13 >> 6;
            uVar13++;
            puVar11 += 2;
            RenderQueue.FUN_00104b80(fVar16, 0, fVar1, fVar17 + fVar19, fVar18 + fVar19, uVar7, iVar5, 0, 1);
            fVar4 = DAT_004a0460;

            if (0xfff < (int)uVar13) break;

            sVar8 = PTR_DAT_004a00ec.DAT_842[pcVar12];
        }

        local_80 = 0;
        fVar20 = 0.5f;
        fVar19 = 0.25f;
        pcVar15 = 0;
        pcVar12 = 0;
        pcVar14 = 0;
        DAT_004a4210 = (DAT_004a4210 + 1) % 2;
        uVar13 = 0;
        puVar11 = 0;

        do
        {
            lVar10 = PTR_DAT_004a00ec.DAT_1842[pcVar14];
            sVar8 = PTR_DAT_004a00ec.DAT_842[pcVar12];
            sVar2 = PTR_DAT_004a00ec.DAT_2842[pcVar15];

            if (sVar8 == 22)
                lVar10 = -39;
            else if (sVar8 < 23)
            {
                if (sVar8 == 10)
                    lVar10 = -40;
            }
            else if (sVar8 == 23)
                lVar10 = -38;

            uVar7 = uVar13 & 0x3f;
            iVar5 = (int)uVar13 >> 6;
            RenderQueue.FUN_00104aa0(0, 0, uVar7, iVar5, 1, 0, 0);
            RenderQueue.FUN_00104aa0(0, 0, uVar7, iVar5, 2, 0, 0);

            if (lVar10 == 0)
            {
                if (sVar2 != 0)
                {
                    sVar8 = PTR_DAT_004a00ec.DAT_842[pcVar12];

                    if (sVar8 < 0x20)
                    {
                        if (sVar2 != 0)
                        {
                            local_80++;
                            fVar18 = DAT_00484930[sVar2 >> 4] + fVar20;
                            fVar17 = DAT_00484930[sVar2 & 0xf];
                            RenderQueue.FUN_00104b80(DAT_0049eb20[puVar11], 0, DAT_0049eb20[puVar11 + 1], fVar17 + fVar4, fVar18 + fVar4, uVar7, iVar5, 2, 1);
                        }

                        bVar3 = local_80 < 0x1000;

                        if (!bVar3) break;

                        uVar9 = (uint)lVar10;

                        if (lVar10 < 1)
                        {
                            if (lVar10 < 0)
                            {
                                uVar9 += 0x40;
                                lVar10 = (long)(int)uVar9;
                                fVar17 = DAT_00484930[uVar9 & 0xf];
                                fVar18 = DAT_00484930[(int)uVar9 >> 4];
                            }
                        }
                        else
                        {
                            fVar18 = DAT_00484930[(int)uVar9 >> 4] + fVar19;
                            fVar17 = DAT_00484930[uVar9 & 0xf];
                        }

                        if (lVar10 != 0)
                        {
                            local_80++;
                            RenderQueue.FUN_00104b80(DAT_0049eb20[puVar11], 0, DAT_0049eb20[puVar11 + 1], fVar17 + fVar4, fVar18 + fVar4, uVar7, iVar5, 1, 1);
                            bVar3 = local_80 < 0x1000;
                        }

                        if (!bVar3) break;
                    }
                }
            }
            else
            {
                sVar8 = PTR_DAT_004a00ec.DAT_842[pcVar12];

                if (sVar8 < 0x20)
                {
                    if (sVar2 != 0)
                    {
                        local_80++;
                        fVar18 = DAT_00484930[sVar2 >> 4] + fVar20;
                        fVar17 = DAT_00484930[sVar2 & 0xf];
                        RenderQueue.FUN_00104b80(DAT_0049eb20[puVar11], 0, DAT_0049eb20[puVar11 + 1], fVar17 + fVar4, fVar18 + fVar4, uVar7, iVar5, 2, 1);
                    }

                    bVar3 = local_80 < 0x1000;

                    if (!bVar3) break;

                    uVar9 = (uint)lVar10;

                    if (lVar10 < 1)
                    {
                        if (lVar10 < 0)
                        {
                            uVar9 += 0x40;
                            lVar10 = (long)(int)uVar9;
                            fVar17 = DAT_00484930[uVar9 & 0xf];
                            fVar18 = DAT_00484930[(int)uVar9 >> 4];
                        }
                    }
                    else
                    {
                        fVar18 = DAT_00484930[(int)uVar9 >> 4] + fVar19;
                        fVar17 = DAT_00484930[uVar9 & 0xf];
                    }

                    if (lVar10 != 0)
                    {
                        local_80++;
                        RenderQueue.FUN_00104b80(DAT_0049eb20[puVar11], 0, DAT_0049eb20[puVar11 + 1], fVar17 + fVar4, fVar18 + fVar4, uVar7, iVar5, 1, 1);
                        bVar3 = local_80 < 0x1000;
                    }

                    if (!bVar3) break;
                }
            }

            uVar13++;
            pcVar12++;
            pcVar14++;
            pcVar15++;
            puVar11 += 2;
        } while ((int)uVar13 < 0x1000);

        DAT_004a4258 = local_80 << 1;
        //FUN_001a83f8 --draw mini screen
    }

    private void FUN_001e08d8()
    {
        //...
    }

    

    private void FUN_001e1058(float param1, Vector4[] param2)
    {
        float fVar1;
        Vector4[] auStack_60 = new Vector4[4];

        Utilities.FUN_00200aa0(auStack_60);
        fVar1 = FUN_001ff048(param1);
        Utilities.FUN_00240938(fVar1, auStack_60, auStack_60);
        Utilities.FUN_001e0778(param2, auStack_60);
    }

    private void FUN_001e10b8(float param1, float param2, float param3, Vector4[] param4)
    {
        Utilities.FUN_001e0af0(param4);
        param4[3].z = param3;
        param4[3].x = param1;
        param4[3].y = param2;
    }

    private void FUN_001e14b8()
    {
        return;
    }

    private float FUN_001ff048(float param1)
    {
        float fVar1;

        fVar1 = (param1 + DAT_0049d0ec) * DAT_0049d0f0;
        return param1 - (float)((int)fVar1 - (BitConverter.ToInt32(BitConverter.GetBytes(fVar1), 0) >> 0x1f)) * DAT_0049d0f4; //need to check
    }
}
