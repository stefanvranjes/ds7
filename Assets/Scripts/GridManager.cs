using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAT_0027905c
{
    public sbyte[] DAT_842; //0x842
    public sbyte[] DAT_1842; //0x1842
    public sbyte[] DAT_2842; //0x2842
}

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public float[] DAT_00484930 = new float[]
    {
        0f, 0.0625f, 0.125f, 0.1875f, 0.25f, 0.3125f, 0.375f, 0.4375f,
        0.5f, 0.5625f, 0.625f, 0.6875f, 0.75f, 0.8125f, 0.875f, 0.9375f, 1f
    };
    public float[] DAT_0049eb20 = new float[]
    {
        0, 0.5235988f, 3.2f, 0.5f, 1.6f, 76.7999954f, 1f
    }; //...
    public DAT_0027905c PTR_DAT_004a00ec;
    public float DAT_004a0460 = 0.03125f;
    public int DAT_004a420c;
    public int DAT_004a4210;
    public int DAT_004a4258;
    public int DAT_004a4268;
    public int[] DAT_0050ef98;
    public short[] DAT_0050efb0;

    public void FUN_001a83f8(DAT_0027905c param1)
    {
        //...
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
}
