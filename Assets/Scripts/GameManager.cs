using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0xe8 - size
public class PTR_DAT_004a3660
{
    public bool DAT_02; //0x02
    public ushort DAT_48; //0x48
    public int DAT_4c; //0x4C
    public int DAT_50; //0x50
    public int DAT_54; //0x54
    public int DAT_58; //0x58
    public int DAT_5c; //0x5C
    public int DAT_60; //0x60
    public ushort DAT_72; //0x72
}

//0x40 - size
public class uGpffff838c
{
    public int DAT_00; //0x00
    public int DAT_10; //0x10
    public float DAT_20; //0x20
    public float DAT_24; //0x24
    public float DAT_28; //0x28
    public float DAT_30; //0x30
    public float DAT_34; //0x34
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int iGpfffff1a4;
    public int iGpfffff1a8;
    public int iGpfffff1ac;
    public int iGpfffff1b0;
    public int iGpfffff1b4;
    public int uGpfffff1b8;
    public int iGpfffff1bc;
    public int iGpfffff1c0;
    public int iGpfffff1c4;
    public int iGpfffff1c8;
    public int iGpfffff1cc;
    public int iGpfffff1d0;
    public int iGpfffff1d4;
    public int iGpfffff1d8;
    public int iGpfffff1dc;
    public int iGpfffff1e0;
    public int iGpfffff1e4;
    public int iGpfffff1f4;
    public int iGpfffff1f8;
    public int iGpfffff1fc;
    public int iGpfffff200;
    public int iGpfffff204;
    public sbyte[] iGpffffb07c;
    public uGpffff838c[] uGpffff838c;
    public int[] DAT_0035b200;
    public int[] DAT_0035b230;
    public int[] DAT_003db258;
    public int[] DAT_003db2b0;
    public PTR_DAT_004a3660[] DAT_004a3660;
    public int DAT_004a366c;
    public int DAT_0050ee88;
    public int DAT_0050ee8c;
    public int DAT_0050ee90;
    public int DAT_0050ee94;
    public int DAT_0050eec8;
    public int[] DAT_0050eff0;
    public int DAT_0050f808;
    public int DAT_0050f820;
    public int[] DAT_0050f838;
    public int DAT_0050f848;
    public int[] DAT_0050f850;
    public int DAT_0050f860;
    
    public void FUN_001a5668()
    {
        ushort uVar1;
        long lVar2;
        int iVar3;
        int[] puVar4;
        uint uVar5;
        int iVar6;
        PTR_DAT_004a3660 pVar6;
        int iVar7;
        int iVar8;
        PTR_DAT_004a3660 pVar8;
        int iVar9;
        sbyte sVar10;
        int iVar11;
        PTR_DAT_004a3660 pVar11;
        long lVar12;

        iVar11 = iGpfffff1a4;
        lVar12 = 0;
        DAT_0050eec8 = 1;
        iVar8 = DAT_0050ee88 + DAT_0050ee8c * 0x40;

        switch (iGpfffff1ac)
        {
            case 0:
                pVar11 = DAT_004a3660[DAT_004a366c];

                if (pVar11.DAT_60 != 0)
                {
                    iVar3 = 13;
                    iGpfffff1dc = iGpfffff1d4;
                    DAT_0050f808 = 1;
                    iGpfffff1e0 = iGpfffff1d8;
                    iGpfffff1ac = 2;
                    goto LAB_001a5764;
                }

                if (pVar11.DAT_5c != 0)
                {
                    DAT_0050f820 = 1;
                    iVar3 = 13;
                    iGpfffff1a8 = iGpfffff1a4;
                    iGpfffff1ac = 3;
                    goto LAB_001a5764;
                }

                if (pVar11.DAT_58 == 0)
                {
                    if ((pVar11.DAT_48 & 0x10) == 0)
                    {
                        if (!pVar11.DAT_02)
                        {
                            if (pVar11.DAT_54 == 0)
                            {
                                if (pVar11.DAT_50 != 0)
                                {
                                    if (iGpfffff200 == 0)
                                    {
                                        iVar3 = 6;
                                        //FUN_00144bb0
                                    }
                                    else
                                    {
                                        //...
                                    }
                                }
                            }
                            else
                            {
                                sVar10 = iGpffffb07c[iVar8 + 0x842];

                                if (sVar10 < 0x20)
                                    iGpfffff1d4 = DAT_003db2b0[sVar10];
                                else
                                {
                                    iGpfffff1e4 = sVar10 - 0x20;
                                    iGpfffff1d8 = iGpfffff1e4 / 6;
                                    iGpfffff1e4 %= 6;
                                    iGpfffff1d4 = iGpfffff1e4 + 0x16;
                                }

                                iVar3 = 4;
                                //FUN_00144bb0
                            }
                        }
                        else if (iGpfffff1d4 < 0x16)
                            lVar12 = DAT_003db258[iGpfffff1d4];
                        else
                            lVar12 = iGpfffff1e4 + iGpfffff1d8 * 6 + 0x20;

                        switch (iGpfffff1a4)
                        {
                            case 0:
                                if (lVar12 == 0)
                                    return;

                                if (iGpffffb07c[iVar8 + 0x842] == lVar12)
                                    return;

                                //FUN_0024c8d8
                                iGpfffff204 = 1;
                                iGpfffff200 = 1;
                                iGpfffff1fc = (iGpfffff1fc + 1) % 2;
                                uVar5 = FUN_001a19b8(iVar8, (sbyte)lVar12);
                                break;
                            case 1:
                            case 2:
                            case 4:
                            case 5:
                            case 6:
                                if (DAT_004a366c < 0)
                                    iVar11 = DAT_004a3660[4].DAT_4c;
                                else
                                    iVar11 = DAT_004a3660[DAT_004a366c].DAT_4c;

                                if (iVar11 == 0)
                                    return;

                                iGpfffff1f4 = 0;
                                iGpfffff1b0 = DAT_0050ee88;
                                uGpfffff1b8 = DAT_0050ee90;
                                iVar3 = 4;
                                iGpfffff1b4 = DAT_0050ee8c;
                                iGpfffff1ac = 1;
                                goto LAB_001a5764;
                            case 3:
                                if (lVar12 == 0)
                                    return;

                                if (iGpffffb07c[iVar8 + 0x842] == lVar12)
                                    return;

                                //FUN_0024c8d8
                                iGpfffff204 = 1;
                                iGpfffff200 = 1;
                                iGpfffff1fc = (iGpfffff1fc + 1) % 2;
                                uVar5 = (uint)FUN_001a1df8(DAT_0050ee88, DAT_0050ee8c, (sbyte)lVar12);
                                break;
                            case 7:
                                if (!DAT_004a3660[DAT_004a366c].DAT_02)
                                    return;

                                if (iGpffffb07c[iVar8 + 0x1842] == 0 && iGpffffb07c[iVar8 + 0x2842] == 0)
                                    return;

                                //FUN_0024c8d8
                                iGpfffff204 = 1;
                                uVar5 = 2;
                                iGpfffff200 = 1;
                                iGpfffff1fc = (iGpfffff1fc + 1) % 2;
                                iGpffffb07c[iVar8 + 0x1842] = 0;
                                iGpffffb07c[iVar8 + 0x2842] = 0;
                                break;
                            default:
                                goto switchD_001a56c4_caseD_8;
                        }

                        GridManager.instance.FUN_001a0620();
                        iVar3 = 1;
                        goto LAB_001a5764;
                    }

                    iGpfffff1ac = 5;
                    puVar4 = DAT_0050f850;
                }
                else
                {
                    iGpfffff1ac = 4;
                    puVar4 = DAT_0050f838;
                }

                FUN_0019d808(puVar4);
                iVar3 = 13;
                LAB_001a5764:
                //FUN_00144bb0
                return;
            case 1:
                switch (iGpfffff1a4)
                {
                    default:
                        goto switchD_001a56c4_caseD_8;
                    case 1:
                    case 2:
                    case 5:
                    case 6:
                        if (iGpfffff1a4 == 2)
                            FUN_001a0a58();
                        else
                            FUN_001a1308();

                        pVar11 = DAT_004a3660[DAT_004a366c];

                        if (pVar11.DAT_50 == 0)
                        {
                            if (DAT_004a366c < 0)
                                iVar11 = DAT_004a3660[4].DAT_4c;
                            else
                                iVar11 = pVar11.DAT_4c;

                            if (iVar11 == 0)
                                return;

                            //FUN_00144bb0

                            if (iGpfffff1d4 < 0x16)
                                sVar10 = (sbyte)DAT_003db258[iGpfffff1d4];
                            else
                                sVar10 = (sbyte)(iGpfffff1e4 + (sbyte)iGpfffff1d8 * 6 + 0x20);

                            if (iGpfffff1a4 == 2)
                                lVar12 = (long)FUN_001a1c10(sVar10);
                            else
                            {
                                if (2 < iGpfffff1a4)
                                {
                                    if (iGpfffff1a4 == 5)
                                        iVar11 = 0x1842;
                                    else
                                    {
                                        if (iGpfffff1a4 != 6)
                                        {
                                            iGpfffff1ac = 0;
                                            return;
                                        }

                                        iVar11 = 0x2842;
                                    }

                                    lVar2 = FUN_001a15d0(iVar11, iGpffffb07c);
                                    lVar12 = 2;

                                    if (lVar2 == 0)
                                    {
                                        iGpfffff1ac = 0;
                                        return;
                                    }

                                    GridManager.instance.FUN_001a0620();
                                    iGpfffff1ac = 0;
                                    return;
                                }

                                if (iGpfffff1a4 != 1)
                                {
                                    iGpfffff1ac = 0;
                                    return;
                                }

                                lVar12 = (long)FUN_001a1a80(sVar10);
                            }

                            if (lVar12 == 0)
                            {
                                iGpfffff1ac = 0;
                                return;
                            }

                            GridManager.instance.FUN_001a0620();
                            iGpfffff1ac = 0;
                            return;
                        }

                        break;
                    case 4:
                        FUN_001a0a58();
                        pVar11 = DAT_004a3660[DAT_004a366c];

                        if (pVar11.DAT_50 == 0)
                        {
                            if (DAT_004a366c < 0)
                                iVar11 = DAT_004a3660[4].DAT_4c;
                            else
                                iVar11 = pVar11.DAT_4c;

                            if (iVar11 == 0)
                                return;

                            //FUN_00144bb0

                            if (DAT_0050ee88 < iGpfffff1b0)
                            {
                                iGpfffff1bc = DAT_0050ee88;
                                iGpfffff1c4 = iGpfffff1b0 - DAT_0050ee88;
                            }
                            else
                            {
                                iGpfffff1bc = iGpfffff1b0;
                                iGpfffff1c4 = DAT_0050ee88 - iGpfffff1b0;
                            }

                            if (DAT_0050ee8c < iGpfffff1b4)
                            {
                                iGpfffff1c0 = DAT_0050ee8c;
                                iGpfffff1c8 = iGpfffff1b4 - DAT_0050ee8c;
                            }
                            else
                            {
                                iGpfffff1c0 = DAT_0050ee8c;
                                iGpfffff1c8 = DAT_0050ee8c - iGpfffff1b4;
                            }

                            iGpfffff1ac = 7;
                            iGpfffff1b0 = DAT_0050ee88;
                            iGpfffff1b4 = DAT_0050ee8c;
                            return;
                        }

                        break;
                }

                break;
            case 2:
                DAT_0050eec8 = 0;
                pVar11 = DAT_004a3660[DAT_004a366c];
                uVar1 = pVar11.DAT_72;

                if ((uVar1 & 1) == 0)
                {
                    if ((uVar1 & 2) == 0)
                    {
                        if ((uVar1 & 4) == 0)
                        {
                            if ((uVar1 & 8) == 0)
                            {
                                if (DAT_004a366c < 0)
                                    iVar11 = DAT_004a3660[4].DAT_4c;
                                else
                                    iVar11 = pVar11.DAT_4c;

                                if (iVar11 == 0)
                                {
                                    pVar11 = DAT_004a3660[DAT_004a366c];

                                    if (pVar11.DAT_60 == 0)
                                    {
                                        if (pVar11.DAT_5c == 0)
                                        {
                                            if (pVar11.DAT_50 != 0)
                                            {
                                                //FUN_00144bb0
                                                DAT_0050f808 = 0;
                                                iGpfffff1ac = 6;
                                                iGpfffff1d4 = iGpfffff1dc;
                                                iGpfffff1d8 = iGpfffff1e0;
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            DAT_0050f820 = 1;
                                            iGpfffff1a8 = iGpfffff1a4;
                                            iGpfffff1ac = 3;
                                            DAT_0050f808 = 0;
                                            //FUN_00144bb0
                                        }

                                        goto LAB_001a5e3c;
                                    }

                                    //FUN_00144bb0
                                    iGpfffff1ac = 6;
                                    DAT_0050f808 = 0;

                                    if (iGpfffff1d4 < 22) goto LAB_001a5e3c;

                                    if (iGpfffff1d4 < 28)
                                    {
                                        if (iGpfffff1d4 == 22)
                                        {
                                            iGpfffff1e4 = 0;

                                            if (iGpfffff1d8 == 0)
                                            {
                                                iGpfffff1d4 = 23;
                                                iGpfffff1cc++;
                                                iGpfffff1e4 = 1;
                                            }
                                        }
                                        else
                                            iGpfffff1e4 = iGpfffff1d4 - 0x16;

                                        goto LAB_001a5e3c;
                                    }
                                }
                                else
                                {
                                    if (iGpfffff1d4 < 22)
                                    {
                                        //FUN_00144bb0
                                        iGpfffff1ac = 6;
                                        DAT_0050f808 = 0;
                                        goto LAB_001a5e3c;
                                    }

                                    if (iGpfffff1d4 < 28)
                                    {
                                        if (iGpfffff1d4 == 22 && iGpfffff1d8 == 0)
                                        {
                                            //...
                                        }
                                        else
                                        {
                                            //FUN_00144bb0
                                            iGpfffff1ac = 6;
                                            iGpfffff1e4 = iGpfffff1d4 - 22;
                                            DAT_0050f808 = 0;
                                        }

                                        goto LAB_001a5e3c;
                                    }

                                    //FUN_00144bb0
                                }

                                iGpfffff1d8 = iGpfffff1d4 - 27;

                                if (iGpfffff1d8 == 5)
                                {
                                    iGpfffff1d8 = 0;

                                    if (iGpfffff1e4 == 0)
                                        iGpfffff1e4 = 1;
                                }

                                goto LAB_001a5e3c;
                            }

                            iGpfffff1cc++;
                        }
                        else
                            iGpfffff1cc += 10;

                        iGpfffff1cc %= 11;
                        goto LAB_001a5e3c;
                    }

                    iGpfffff1d0++;
                }
                else
                    iGpfffff1d0 += 2;

                iGpfffff1d0 %= 3;
                LAB_001a5e3c:
                iVar11 = iGpfffff1cc + iGpfffff1d0 * 11;

                if (iVar11 == iGpfffff1d4)
                    return;

                //FUN_00144bb0
                iGpfffff1d4 = iVar11;
                return;
            case 3:
                DAT_0050eec8 = 0;
                pVar8 = DAT_004a3660[DAT_004a366c];
                uVar1 = pVar8.DAT_72;

                if ((uVar1 & 4) == 0)
                {
                    iVar7 = iGpfffff1a4 + 8;

                    if ((uVar1 & 8) == 0)
                    {
                        if (DAT_004a366c < 0)
                            iVar8 = DAT_004a3660[4].DAT_4c;
                        else
                            iVar8 = pVar8.DAT_4c;

                        uVar5 = 4;

                        if (iVar8 == 0)
                        {
                            pVar8 = DAT_004a3660[DAT_004a366c];
                            uVar5 = 14;

                            if (pVar8.DAT_5c == 0)
                            {
                                if (pVar8.DAT_60 == 0)
                                {
                                    if (pVar8.DAT_50 != 0)
                                    {
                                        //FUN_00144bb0
                                        DAT_0050f820 = 0;
                                        iGpfffff1a4 = iGpfffff1a8;
                                        iGpfffff1ac = 6;
                                        return;
                                    }
                                }
                                else
                                {
                                    DAT_0050f808 = 1;
                                    iGpfffff1ac = 2;
                                    iGpfffff1dc = iGpfffff1d4;
                                    iGpfffff1e0 = iGpfffff1d8;
                                    DAT_0050f820 = 0;
                                    //FUN_00144bb0
                                }

                                goto LAB_001a6144;
                            }
                        }

                        //FUN_00144bb0
                        iGpfffff1ac = 6;
                        DAT_0050f820 = 0;
                        goto LAB_001a6144;
                    }

                    iVar11 = iGpfffff1a4 + 1;
                }
                else
                {
                    iVar11 = iGpfffff1a4 + 7;
                    iVar7 = iGpfffff1a4 + 14;
                }

                if (-1 < iVar11)
                    iVar7 = iVar11;

                iVar11 = iVar11 + (iVar7 >> 3) * -8;

                LAB_001a6144:
                if (iVar11 == iGpfffff1a4)
                    return;

                //FUN_00144bb0
                iGpfffff1a4 = iVar11;
                return;
            case 4:
                DAT_0050eec8 = 0;

                if (DAT_004a366c < 0)
                    iVar11 = DAT_004a3660[4].DAT_4c;
                else
                    iVar11 = DAT_004a3660[DAT_004a366c].DAT_4c;

                if (iVar11 != 0 || (DAT_004a3660[DAT_004a366c].DAT_48 & 0x10) != 0)
                {
                    //FUN_00144bb0
                    FUN_0019d820(DAT_0050f838);

                    if (DAT_0050f848 == 1)
                    {
                        //FUN_001a2778
                        iGpfffff1ac = 6;
                        return;
                    }

                    if (DAT_0050f848 < 2)
                    {
                        if (DAT_0050f848 != 0)
                        {
                            iGpfffff1ac = 6;
                            return;
                        }

                        //FUN_001a29a0
                        iGpfffff1ac = 6;
                        return;
                    }

                    if (DAT_0050f848 == 2)
                    {
                        //FUN_001a26a0
                        iGpfffff1ac = 6;
                        return;
                    }

                    if (DAT_0050f848 != 3)
                    {
                        iGpfffff1ac = 6;
                        return;
                    }

                    //FUN_001a2528
                    iGpfffff1ac = 6;
                    return;
                }

                pVar11 = DAT_004a3660[DAT_004a366c];

                if (pVar11.DAT_50 == 0)
                {
                    if ((pVar11.DAT_72 & 1) == 0)
                    {
                        if ((pVar11.DAT_72 & 2) == 0)
                        {
                            DAT_0050eec8 = 0;
                            return;
                        }

                        //FUN_00144bb0
                        iVar8 = DAT_0050f848 + 1;
                        iVar11 = DAT_0050f848 + 4;
                    }
                    else
                    {
                        //FUN_00144bb0
                        iVar8 = DAT_0050f848 + 3;
                        iVar11 = DAT_0050f848 + 6;
                    }

                    if (-1 < iVar8)
                        iVar11 = iVar8;

                    DAT_0050f848 = iVar8 + (iVar11 >> 2) * -4;
                    return;
                }

                //FUN_00144bb0
                puVar4 = DAT_0050f838;
                goto LAB_001a636c;
            case 5:
                DAT_0050eec8 = 0;

                if (DAT_004a366c < 0)
                    iVar11 = DAT_004a3660[4].DAT_4c;
                else
                    iVar11 = DAT_004a3660[DAT_004a366c].DAT_4c;

                if (iVar11 != 0 || (DAT_004a3660[DAT_004a366c].DAT_48 & 0x10) != 0)
                {
                    //FUN_00144bb0
                    FUN_0019d820(DAT_0050f850);

                    switch (DAT_0050f860)
                    {
                        case 0:
                            iGpfffff1ac = 9;
                            //FUN_001a4f58
                            iGpfffff1ac = 6;
                            return;
                        case 1:
                            goto switchD_001a64ac_caseD_1;
                        case 2:
                            DAT_0050ee94 ^= 1;
                            iGpfffff1ac = 6;
                            return;
                        case 3:
                            //FUN_001a4ac8
                            iGpfffff1ac = 6;
                            return;
                        case 4:
                            if (iGpfffff204 != 0)
                            {
                                //...
                                return;
                            }

                            //FUN_001a4238
                            iGpfffff1ac = 6;
                            return;
                        case 5:
                            if (iGpfffff204 != 0)
                            {
                                //...
                                return;
                            }

                            iGpfffff1ac = 10;
                            return;
                        default:
                            iGpfffff1ac = 6;
                            return;
                    }
                }

                pVar11 = DAT_004a3660[DAT_004a366c];

                if (pVar11.DAT_50 == 0)
                {
                    if ((pVar11.DAT_72 & 1) == 0)
                    {
                        if ((pVar11.DAT_72 & 2) == 0)
                        {
                            DAT_0050eec8 = 0;
                            return;
                        }

                        //FUN_00144bb0
                        iVar11 = DAT_0050f860 + 1;
                    }
                    else
                    {
                        //FUN_00144bb0
                        iVar11 = DAT_0050f860 + 5;
                    }

                    DAT_0050f860 = iVar11 % 6;
                    return;
                }

                //FUN_00144bb0
                puVar4 = DAT_0050f850;
                LAB_001a636c:
                FUN_0019d820(puVar4);
                iGpfffff1ac = 6;
                return;
            case 6:
                if (!DAT_004a3660[DAT_004a366c].DAT_02)
                {
                    DAT_0050eec8 = 1;
                    iGpfffff1ac = 0;
                    return;
                }

                DAT_0050eec8 = 1;
                return;
            case 7:
                iVar11 = DAT_0050ee88 - (iGpfffff1c4 + 1 >> 1);
                iVar8 = iVar11 + iGpfffff1c4;

                if (iVar11 < 0)
                {
                    iVar8 -= iVar11;
                    iVar11 = 0;
                }

                if (0x3f < iVar8)
                {
                    iVar11 = (iVar11 - iVar8) + 0x3f;
                    iVar8 = 0x3f;
                }

                iVar7 = DAT_0050ee8c - (iGpfffff1c8 + 1 >> 1);
                iVar9 = iVar7 + iGpfffff1c8;

                if (iVar7 < 0)
                {
                    iVar9 -= iVar7;
                    iVar7 = 0;
                }

                if (0x3f < iVar9)
                {
                    iVar7 = (iVar7 - iVar9) + 0x3f;
                    iVar9 = 0x3f;
                }

                //FUN_001a0bd0
                pVar6 = DAT_004a3660[DAT_004a366c];

                if (pVar6.DAT_50 == 0)
                {
                    if (DAT_004a366c < 0)
                        iVar6 = DAT_004a3660[4].DAT_4c;
                    else
                        iVar6 = pVar6.DAT_4c;

                    if (iVar6 == 0)
                        return;

                    //...
                    GridManager.instance.FUN_001a0620();
                    iGpfffff1ac = 0;
                    return;
                }

                break;
            default:
                goto switchD_001a56c4_caseD_8;
        }

        //FUN_00144bb0
        iGpfffff1ac = 0;
        switchD_001a56c4_caseD_8:
        return;

        switchD_001a64ac_caseD_1:
        if (0x7f < iGpfffff1f8)
        {
            //...
        }

        //FUN_001a2a78
        iGpfffff1ac = 6;
    }

    private void FUN_0019d808(int[] param1)
    {
        param1[2] = 0;
        param1[3] = 15;
        param1[0] = 1;
    }

    private void FUN_0019d820(int[] param1)
    {
        param1[3] = 15;
        param1[2] = 1;
    }

    private void FUN_001a0a58()
    {
        int iVar1;
        int iVar2;
        int iVar3;
        int piVar4;
        int iVar5;
        int iVar6;
        int iVar7;

        iVar1 = DAT_0050ee88;
        iVar3 = iGpfffff1b0;

        if (iGpfffff1b0 <= DAT_0050ee88)
        {
            iVar1 = iGpfffff1b0;
            iVar3 = DAT_0050ee88;
        }

        iVar2 = DAT_0050ee8c;
        iVar7 = iGpfffff1b4;

        if (iGpfffff1b4 <= DAT_0050ee8c)
        {
            iVar2 = iGpfffff1b4;
            iVar7 = DAT_0050ee8c;
        }

        iGpfffff1f4 = 0;

        if (iVar1 <= iVar3)
        {
            iGpfffff1f4 = 0;
            piVar4 = 0;
            iVar5 = iVar1;

            do
            {
                DAT_0050eff0[piVar4] = iVar5;
                iGpfffff1f4++;
                DAT_0050eff0[piVar4 + 1] = iVar2;
                iVar5++;
                piVar4 += 2;
            } while (iVar5 <= iVar3);
        }

        iVar5 = iVar7 - iVar2;

        if (iVar2 != iVar7 && iVar1 <= iVar3)
        {
            piVar4 = iGpfffff1f4 * 2;
            iVar5 = iVar1;

            do
            {
                DAT_0050eff0[piVar4] = iVar5;
                iGpfffff1f4++;
                DAT_0050eff0[piVar4 + 1] = iVar7;
                iVar5++;
                piVar4 += 2;
            } while (iVar5 <= iVar3);

            iVar5 = iVar7 - iVar2;
        }

        if (1 < iVar5)
        {
            while (++iVar2 < iVar7)
            {
                iVar6 = iGpfffff1f4 + 1;
                DAT_0050eff0[iGpfffff1f4 * 2 + 1] = iVar2;
                DAT_0050eff0[iGpfffff1f4 * 2] = iVar1;
                iVar5 = iGpfffff1f4 + 2;
                iGpfffff1f4 = iVar6;

                if (iVar1 != iVar3)
                {
                    DAT_0050eff0[iVar6 * 2 + 1] = iVar2;
                    DAT_0050eff0[iVar6 * 2] = iVar3;
                    iGpfffff1f4 = iVar5;
                }
            }
        }
    }

    private void FUN_001a1308()
    {
        bool bVar1;
        uint uVar2;
        uint uVar3;
        int iVar4;
        int iVar5;
        uint uVar6;
        uint uVar7;
        uint uVar8;
        int iVar9;
        int iVar10;
        int iVar11;
        uint uVar12;
        uint uVar13;
        uint uVar14;

        uVar7 = (uint)DAT_0050ee8c;
        iVar11 = DAT_0050ee88 * 2 + (DAT_0050ee8c & 1);
        bVar1 = (((uint)iGpfffff1b0 | (uint)iGpfffff1b4) & 0xffffffc0) == 0;
        iVar9 = iGpfffff1b0 * 2 + (iGpfffff1b4 & 1);

        if (bVar1)
        {
            DAT_0050eff0[0] = iGpfffff1b0;
            DAT_0050eff0[1] = iGpfffff1b4;
        }

        iGpfffff1f4 = bVar1 ? 1 : 0;
        uVar3 = 0xffffffff;

        if (iVar11 < iVar9)
            iVar5 = iVar9 - iVar11;
        else
        {
            uVar3 = (uint)(iVar9 < iVar11 ? 1 : 0);
            iVar5 = iVar11 - iVar9;
        }

        uVar2 = 0xffffffff;

        if (DAT_0050ee8c < iGpfffff1b4)
            iVar4 = iGpfffff1b4 - DAT_0050ee8c;
        else
        {
            uVar2 = (uint)(iGpfffff1b4 < DAT_0050ee8c ? 1 : 0);
            iVar4 = DAT_0050ee8c - iGpfffff1b4;
        }

        if (iVar4 * 2 < iVar5)
        {
            iVar5--;

            if (uVar3 == 1)
                iVar9++;
            else
                iVar11++;

            uVar7 = (uint)iGpfffff1b4 * 2;

            if (uVar2 != 1)
                uVar7 = (uint)iGpfffff1b4 * 2 + 1;

            iVar10 = iVar5 >> 1;
            uVar6 = (uint)iGpfffff1b4;
            uVar8 = (uint)iGpfffff1b0;

            while (iVar9 != iVar11)
            {
                iVar10 = iVar10 + iVar4 * 2 + 1;
                iVar9 = iVar9 + (int)uVar3;

                if (iVar5 <= iVar10)
                {
                    iVar10 = iVar10 - iVar5;
                    uVar7 = uVar7 + uVar2;
                }

                uVar13 = (uint)(iVar9 - ((int)(uVar7 & 2) >> 1));
                uVar14 = (uint)((int)uVar13 >> 1);
                uVar12 = (uint)((int)uVar7 >> 1);

                if (uVar8 != uVar14 || uVar6 != uVar12)
                {
                    uVar6 = uVar12;
                    uVar8 = uVar14;

                    if (((int)(uVar13 | uVar7) >> 1 & 0xffffffc0U) == 0)
                    {
                        DAT_0050eff0[iGpfffff1f4 * 2 + 1] = (int)uVar12;
                        DAT_0050eff0[iGpfffff1f4 * 2] = (int)uVar14;
                        iGpfffff1f4++;
                    }
                }
            }
        }
        else if (iVar4 < iVar5)
        {
            iVar10 = (iVar5 >> 1) + 1;

            if (uVar3 == 1)
                iVar11++;
            else
                iVar9++;

            uVar7 = (uint)iGpfffff1b4;
            uVar6 = (uint)iGpfffff1b0;
            uVar8 = (uint)iGpfffff1b4;

            if (iVar9 != iVar11)
            {
                do
                {
                    iVar10 = iVar10 + iVar4 - 1;
                    iVar9 = iVar9 + (int)uVar3;

                    if (iVar5 <= iVar10)
                    {
                        iVar10 -= iVar5;
                        uVar8 += uVar2;
                    }

                    uVar12 = (uint)((iVar9 - (int)(uVar8 & 1)) >> 1);

                    if (uVar6 != uVar12 || uVar7 != uVar8)
                    {
                        uVar7 = uVar8;
                        uVar6 = uVar12;

                        if (((uVar12 | uVar8) & 0xffffffc0) == 0)
                        {
                            DAT_0050eff0[iGpfffff1f4 * 2 + 1] = (int)uVar8;
                            DAT_0050eff0[iGpfffff1f4 * 2] = (int)uVar12;
                            iGpfffff1f4++;
                        }
                    }
                } while (iVar9 != iVar11);

                return;
            }
        }
        else
        {
            if (iVar5 != 0)
                iVar5--;

            if (uVar3 == 1)
                iVar9++;

            iVar11 = iVar4 >> 1;

            if (uVar3 == 0)
                iVar9 = (iVar9 - (iGpfffff1b4 & 1)) + 1;

            uVar6 = (uint)iGpfffff1b0;
            uVar8 = (uint)iGpfffff1b4;
            uVar12 = (uint)iGpfffff1b4;

            if (iGpfffff1b4 != DAT_0050ee8c)
            {
                do
                {
                    iVar11 += iVar5;
                    uVar8 += uVar2;

                    if (iVar4 <= iVar11)
                    {
                        iVar11 -= iVar4;
                        iVar9 += (int)uVar3;
                    }

                    uVar13 = (uint)((iVar9 - (int)(uVar8 & 1)) >> 1);

                    if (uVar6 != uVar13 || uVar12 != uVar8)
                    {
                        uVar12 = uVar8;
                        uVar6 = uVar13;

                        if (((uVar13 | uVar8) & 0xffffffc0) == 0)
                        {
                            DAT_0050eff0[iGpfffff1f4 * 2] = (int)uVar13;
                            DAT_0050eff0[iGpfffff1f4 * 2 + 1] = (int)uVar8;
                            iGpfffff1f4++;
                        }
                    }
                } while (uVar8 != uVar7);

                return;
            }
        }
    }

    private int FUN_001a15d0(int param1, sbyte[] param2)
    {
        uint uVar1;
        uint uVar2;
        uint uVar3;
        int iVar4;
        int iVar5;
        int iVar6;
        int pbVar7;
        byte bVar8;
        byte bVar9;
        int iVar10;
        uint uVar11;
        byte bVar12;
        int iVar13;
        byte[] auStack_3a60 = new byte[0x3a40];

        iVar13 = 0;
        //FUN_0024c8d8
        iVar5 = 0;

        if (0 < iGpfffff1f4 - 1)
        {
            do
            {
                bVar9 = 0;
                uVar1 = (uint)DAT_0050eff0[iVar5 * 2 + 1];
                uVar2 = (uint)DAT_0050eff0[iVar5 * 2 + 2];
                uVar3 = (uint)DAT_0050eff0[iVar5 * 2 + 3];
                iVar4 = DAT_0050eff0[iVar5 * 2];
                iVar6 = iVar4 + (int)uVar1 * 0x40;
                iVar10 = (int)uVar2 + (int)uVar3 * 0x40;

                if (((uVar2 | uVar3) & 0xffffffc0) == 0)
                {
                    pbVar7 = param1 + iVar10;
                    bVar8 = (byte)param2[pbVar7];

                    if (iVar4 - 1 == (int)uVar2 && uVar1 == uVar3)
                    {
                        bVar8 |= 8;
                        bVar9 = 1;
                    }

                    uVar11 = uVar1 & 1;

                    if ((iVar4 + (int)uVar11) - 1 == (int)uVar2 && uVar1 + 1 == uVar3)
                    {
                        bVar8 |= 0x10;
                        bVar9 |= 2;
                    }

                    if (iVar4 + (int)uVar11 == (int)uVar2 && uVar1 + 1 == uVar3)
                    {
                        bVar8 |= 0x10;
                        bVar9 |= 4;
                    }

                    if (iVar4 + 1 == (int)uVar2 && uVar1 == uVar3)
                    {
                        bVar8 |= 1;
                        bVar9 |= 8;
                    }

                    if (iVar4 + (int)uVar11 == (int)uVar2 && uVar1 - 1 == uVar3)
                    {
                        bVar8 |= 2;
                        bVar9 |= 0x10;
                    }

                    if ((iVar4 + (int)uVar11) - 1 == (int)uVar2)
                    {
                        bVar12 = (byte)param2[pbVar7];

                        if (uVar1 - 1 == uVar3)
                        {
                            bVar8 |= 4;
                            bVar9 |= 0x20;
                        }
                    }
                    else
                        bVar12 = (byte)param2[pbVar7];

                    if (bVar8 != bVar12)
                    {
                        switch (iGpffffb07c[iVar10 + 0x842] - 8)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 11:
                            case 12:
                            case 14:
                            case 15:
                            case 28:
                            case 34:
                            case 40:
                            case 46:
                            case 52:
                                break;
                            default:
                                param2[pbVar7] = (sbyte)bVar8;
                                iVar13 = 1;
                                break;
                        }
                    }
                }

                pbVar7 = param1 + iVar6;
                bVar8 = (byte)param2[pbVar7];
                bVar9 = (byte)(bVar8 | bVar9);

                if (bVar9 != bVar8)
                {
                    switch (iGpffffb07c[iVar6 + 0x842] - 8)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 11:
                        case 12:
                        case 14:
                        case 15:
                        case 28:
                        case 34:
                        case 40:
                        case 46:
                        case 52:
                            break;
                        default:
                            param2[pbVar7] = (sbyte)bVar9;
                            iVar13 = 1;
                            break;
                    }
                }

                iVar5++;
            } while (iVar5 < iGpfffff1f4 - 1);
        }

        if (iVar13 != 0)
        {
            //FUN_0024c8d8
            iGpfffff204 = 1;
            iGpfffff200 = 1;
            iGpfffff1fc = (iGpfffff1fc + 1) % 2;
        }

        return iVar13;
    }

    private uint FUN_001a19b8(int param1, sbyte param2)
    {
        bool bVar1;
        bool bVar2;
        uint uVar3;

        uVar3 = 1;

        switch (param2 - 8 & 0xff)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 11:
            case 12:
            case 14:
            case 15:
            case 28:
            case 34:
            case 40:
            case 46:
            case 52:
                bVar2 = false;
                bVar1 = false;
                break;
            default:
                bVar1 = 0x1f < param2;
                bVar2 = true;
                break;
        }

        iGpffffb07c[param1 + 0x842] = param2;

        if (!bVar2)
        {
            if (iGpffffb07c[param1 + 0x1842] != 0)
            {
                iGpffffb07c[param1 + 0x1842] = 0;
                uVar3 = 3;
            }

            if (iGpffffb07c[param1 + 0x2842] != 0)
            {
                iGpffffb07c[param1 + 0x2842] = 0;
                uVar3 |= 2;
            }
        }

        if (bVar1)
        {
            if (iGpffffb07c[param1 + 0x1842] == 0)
            {
                if (iGpffffb07c[param1 + 0x2842] != 0)
                    uVar3 |= 2;
            }
            else
                uVar3 |= 2;
        }

        return uVar3;
    }

    private ulong FUN_001a1a80(sbyte param1)
    {
        bool bVar1;
        bool bVar2;
        int iVar3;
        int piVar4;
        int iVar5;
        int iVar6;
        ulong uVar7;
        byte[] auStack_3a60 = new byte[0x3a40];

        iVar6 = param1;
        //FUN_0024c8d8
        uVar7 = 0;

        switch (iVar6 - 8 & 0xff)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 11:
            case 12:
            case 14:
            case 15:
            case 28:
            case 34:
            case 40:
            case 46:
            case 52:
                bVar1 = false;
                bVar2 = false;
                break;
            default:
                bVar2 = 0x1f < (long)iVar6;
                bVar1 = true;
                break;
        }

        iVar3 = 0;

        if (0 < iGpfffff1f4)
        {
            piVar4 = 0;

            do
            {
                iVar5 = DAT_0050eff0[piVar4] + DAT_0050eff0[piVar4 + 1] * 0x40;

                if (iGpffffb07c[iVar5 + 0x842] != iVar6)
                {
                    iGpffffb07c[iVar5 + 0x842] = param1;
                    uVar7 |= 1;
                }

                if (!bVar1)
                {
                    if (iGpffffb07c[iVar5 + 0x1842] != 0)
                    {
                        iGpffffb07c[iVar5 + 0x1842] = 0;
                        uVar7 |= 2;
                    }

                    if (iGpffffb07c[iVar5 + 0x2842] != 0)
                    {
                        iGpffffb07c[iVar5 + 0x2842] = 0;
                        uVar7 |= 2;
                    }
                }

                if (bVar2)
                {
                    if (iGpffffb07c[iVar5 + 0x1842] == 0)
                    {
                        if (iGpffffb07c[iVar5 + 0x2842] != 0)
                            uVar7 |= 2;
                    }
                    else
                        uVar7 |= 2;
                }

                iVar3++;
                piVar4 += 2;
            } while (iVar3 < iGpfffff1f4);
        }

        if (uVar7 != 0)
        {
            //FUN_0024c8d8
            iGpfffff204 = 1;
            iGpfffff200 = 1;
            iGpfffff1fc = (iGpfffff1fc + 1) % 2;
        }

        return uVar7;
    }

    private ulong FUN_001a1c10(sbyte param1)
    {
        bool bVar1;
        bool bVar2;
        int iVar3;
        int iVar4;
        int iVar5;
        int iVar6;
        int iVar7;
        int iVar8;
        ulong uVar9;
        int iVar10;
        byte[] auStack_3a60 = new byte[0x3a40];

        iVar10 = param1;
        //FUN_0024c8d8

        switch (iVar10 - 8 & 0xff)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 11:
            case 12:
            case 14:
            case 15:
            case 28:
            case 34:
            case 40:
            case 46:
            case 52:
                bVar2 = false;
                bVar1 = false;
                break;
            default:
                bVar1 = 0x1f < (long)iVar10;
                bVar2 = true;
                break;
        }

        iVar3 = DAT_0050ee88;
        iVar6 = iGpfffff1b0;

        if (iGpfffff1b0 <= DAT_0050ee88)
        {
            iVar3 = iGpfffff1b0;
            iVar6 = DAT_0050ee88;
        }

        iVar4 = iGpfffff1b4;
        iVar5 = DAT_0050ee8c;

        if (iGpfffff1b4 <= DAT_0050ee8c)
        {
            iVar4 = DAT_0050ee8c;
            iVar5 = iGpfffff1b4;
        }

        uVar9 = 0;

        for (; iVar5 <= iVar4; iVar5++)
        {
            if (iVar3 <= iVar6)
            {
                iVar7 = iVar3;

                do
                {
                    iVar8 = iVar7 + iVar5 * 0x40;

                    if (iGpffffb07c[iVar8 + 0x842] != iVar10)
                    {
                        iGpffffb07c[iVar8 + 0x842] = param1;
                        uVar9 |= 1;
                    }

                    if (!bVar2)
                    {
                        if (iGpffffb07c[iVar8 + 0x1842] != 0)
                        {
                            iGpffffb07c[iVar8 + 0x1842] = 0;
                            uVar9 |= 2;
                        }

                        if (iGpffffb07c[iVar8 + 0x2842] != 0)
                        {
                            iGpffffb07c[iVar8 + 0x2842] = 0;
                            uVar9 |= 2;
                        }
                    }

                    if (bVar1)
                    {
                        if (iGpffffb07c[iVar8 + 0x1842] == 0)
                        {
                            if (iGpffffb07c[iVar8 + 0x2842] != 0)
                                uVar9 |= 2;
                        }
                        else
                            uVar9 |= 2;
                    }

                    iVar7++;
                } while (iVar7 <= iVar6);
            }
        }

        if (uVar9 != 0)
        {
            //FUN_0024c8d8
            iGpfffff204 = 1;
            iGpfffff200 = 1;
            iGpfffff1fc = (iGpfffff1fc + 1) % 2;
        }

        return uVar9;
    }

    private ulong FUN_001a1df8(int param1, int param2, sbyte param3)
    {
        sbyte sVar1;
        bool bVar2;
        bool bVar3;
        bool bVar4;
        int iVar5;
        int iVar6;
        int iVar7;
        int[] piVar7;
        uint uVar8;
        int iVar9;
        uint uVar10;
        int iVar11;
        ulong uVar12;
        sbyte[] local_2040 = new sbyte[4096];
        sbyte[] local_1040 = new sbyte[4096];

        iVar11 = param3;
        uVar12 = 0;

        switch (iVar11 - 8 & 0xff)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 11:
            case 12:
            case 14:
            case 15:
            case 28:
            case 34:
            case 40:
            case 46:
            case 52:
                bVar3 = false;
                bVar2 = false;
                break;
            default:
                bVar2 = 0x1f < (long)iVar11;
                bVar3 = true;
                break;
        }

        //FUN_0024c988 -- Array.Fill ??
        //FUN_0024c8d8 -- Array.Copy??
        iVar9 = param1 + param2 * 0x40;
        sVar1 = iGpffffb07c[iVar9 + 0x842];
        iGpffffb07c[iVar9 + 0x842] = param3;

        while (true)
        {
            bVar4 = false;
            uVar8 = 0;
            uVar10 = 0;

            do
            {
                iVar9 = 0;

                do
                {
                    iVar5 = iVar9 + (int)uVar8 * 0x40;

                    if (iGpffffb07c[iVar5 + 0x842] == (long)iVar11 &&
                        local_1040[iVar5] == sVar1 && local_2040[iVar5] == -1)
                    {
                        local_2040[iVar5] = 1;
                        piVar7 = DAT_0035b230;

                        if (uVar10 == 0)
                            piVar7 = DAT_0035b200;

                        iVar5 = 5;
                        iVar7 = 0;

                        do
                        {
                            if (((iVar9 + piVar7[iVar7] | (int)uVar8 + piVar7[iVar7 + 1]) & 0xffffffc0) == 0)
                            {
                                iVar6 = iVar9 + piVar7[iVar7] + ((int)uVar8 + piVar7[iVar7 + 1]) * 0x40;

                                if (iGpffffb07c[iVar6 + 0x842] == sVar1 && local_1040[iVar6] == sVar1)
                                {
                                    iGpffffb07c[iVar6 + 0x842] = param3;
                                    bVar4 = true;

                                    if (!bVar3)
                                    {
                                        if (iGpffffb07c[iVar6 + 0x1842] != 0)
                                        {
                                            iGpffffb07c[iVar6 + 0x1842] = 0;
                                            uVar12 |= 2;
                                        }

                                        if (iGpffffb07c[iVar6 + 0x2842] != 0)
                                        {
                                            iGpffffb07c[iVar6 + 0x2842] = 0;
                                            uVar12 |= 2;
                                        }

                                        if (bVar2)
                                        {
                                            if (iGpffffb07c[iVar6 + 0x1842] == 0)
                                            {
                                                if (iGpffffb07c[iVar6 + 0x2842] != 0)
                                                    uVar12 |= 2;
                                            }
                                            else
                                                uVar12 |= 2;
                                        }
                                    }
                                }
                            }

                            iVar5--;
                            iVar7 += 2;
                        } while (-1 < iVar5);
                    }

                    iVar9++;
                } while (iVar9 < 0x40);

                uVar8++;
                uVar10 &= 1;
            } while ((int)uVar8 < 0x40);

            if (!bVar4) break;

            uVar12 |= 1;
        }

        return uVar12;
    }
}
