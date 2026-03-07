using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCSX2;
using static PCSX2.VUops;
using static PCSX2.R5900;

public class Utilities
{
    public static int[] DAT_00374718 = new int[]
    {
        -1, 89, 89, 88, 88, 71, 71, 71, 91, 88, 88, 88, -1, -1, 83,
        83, 83, -1, -1, 82, 82, -1, 84, -1, 84, -1, 84, 84, -1, 85
    };
    public static int[] DAT_00374808 = new int[]
    {
        3, 3, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1,
        2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 0, 1, 2, 3, 4, 5, 0, 1,
        2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3,
        4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5,
        0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5
    };
    public static Vector4[] DAT_00443920 = new Vector4[]
    {
        new Vector4(1f, 0, 0, 0), new Vector4(0, 1f, 0, 0),
        new Vector4(0, 0, 1f, 0), new Vector4(0, 0, 0, 1f)
    };
    public static Vector4 DAT_004760f0 = new Vector4(0.000002601887f, -0.00019807414f, 0.0083330255f, -0.16666657f);
    public static float[] DAT_0049a9c4 = new float[]
    {
        1.499389E-43f, 1.57077f, 3.141541f, 4.71228f, 6.283081f,
        7.853882f, 9.424561f, 10.99536f, 12.56616f, 14.13696f,
        15.70776f, 17.27832f, 18.84912f, 20.41992f, 21.99072f,
        23.56152f, 25.13232f, 26.70313f, 28.27393f, 29.84473f,
        31.41553f, 32.98633f, 34.55664f, 36.12793f, 37.69824f,
        39.26953f, 40.83984f, 42.41113f, 43.98145f, 45.55273f,
        47.12305f, 48.69434f
    };
    public static float DAT_0049d3f8 = -1f;
    public static float DAT_004a372c = 1f;
    public static float DAT_004a3730 = 1f;
    public static float DAT_004a3738 = 1f;
    public static float DAT_004a3744 = 1f;
    public static Vector4[] DAT_004a3f30 = new Vector4[4];
    public static Vector4[] DAT_004a3f34 = new Vector4[4];
    public static Vector4[] DAT_004a3f38 = new Vector4[4];
    public static Vector4[] DAT_004a3f3c = new Vector4[4];
    public static Vector4[] DAT_004a3f40 = new Vector4[4];
    public static Vector4[] DAT_004a3f44 = new Vector4[4];
    public static Vector4[] DAT_004a3f48 = new Vector4[4];
    public static Vector4[] DAT_004a3f4c = new Vector4[4];
    public static Vector4[] DAT_004a3f50 = new Vector4[4];
    public static Vector4[] DAT_004a48d0;
    public static Vector4[] DAT_004a4910;

    private static void FUN_00101da8(Vector4 param1)
    {
        FUN_002405a0(DAT_004a4910, DAT_004a48d0, ref param1);
    }

    private static void FUN_00101e78(Vector4 param1, Vector3 param2, Vector4 param3)
    {
        Vector4[] aVar1;
        Vector4[] aVar2;
        Vector4[] aVar3;
        Vector4[] aVar4;
        Vector4[] aVar5;
        Vector4[] aVar6;
        Vector4[] local_b0 = new Vector4[4];
        Vector4 local_70;
        Vector4[] local_60;

        aVar1 = FUN_0021e8c8();
        aVar2 = FUN_0021e8a8();
        aVar3 = FUN_0021e8b8();
        aVar4 = FUN_0021e8d0();
        local_60 = FUN_0021e8d8();
        aVar5 = FUN_0021e8e0();
        aVar6 = FUN_0021e8e8();
        local_70 = new Vector3(param2.x - param1.x, param2.y - param1.y, param2.z - param1.z);
        FUN_00240ba8(aVar1, ref param1, ref local_70, ref param3);
        FUN_00240898(local_b0);
        local_b0[0].x = DAT_0049d3f8;
        FUN_002405a0(aVar1, local_b0, ref aVar1[0]);
        FUN_002405a0(aVar3, aVar2, ref aVar1[0]);
        FUN_002405a0(aVar5, aVar4, ref aVar1[0]);
        FUN_002405a0(aVar6, local_60, ref aVar1[0]);
        FUN_00101da8(aVar1[0]);
    }

    public static int FUN_00145150(int param1)
    {
        return DAT_00374718[param1];
    }

    public static int FUN_001451b0(int param1)
    {
        return DAT_00374808[param1];
    }

    private static void FUN_001e0738(Vector4[] param1, Vector4[] param2)
    {
        int iVar1;
        int iVar2;
        int iVar3;

        iVar1 = 0;
        iVar3 = 0;

        while (true)
        {
            iVar3 = 0;
            iVar2 = 3;

            do
            {
                iVar2--;
                param1[iVar1][iVar3] = param2[iVar1][iVar3];
                iVar3++;
            } while (-1 < iVar2);

            iVar1++;

            if (3 < iVar1) break;
        }
    }

    public static void FUN_001e0778(Vector4[] param1, Vector4[] param2)
    {
        int iVar1;
        int iVar2;
        int iVar3;

        iVar1 = 0;
        iVar3 = 0;

        while (true)
        {
            iVar3 = 0;
            iVar2 = 3;

            do
            {
                iVar2--;
                param1[iVar1][iVar3] = param2[iVar1][iVar3];
                iVar3++;
            } while (-1 < iVar2);

            iVar1++;

            if (3 < iVar1) break;
        }
    }

    public static void FUN_001e07f0(out Vector3 param1, Vector3 param2, Vector3 param3)
    {
        float fVar2;
        float fVar3;
        float fVar4;
        float fVar5;

        fVar2 = param2.x;
        fVar5 = param3.x;
        fVar3 = param2.y;
        fVar4 = param3.y;
        param1.z = param2.z + param3.z;
        param1.x = fVar2 + fVar5;
        param1.y = fVar3 + fVar4;
    }

    public static Vector4 FUN_001e0848(float param1, float param2, Vector4[] param3)
    {
        Vector4 local_70;
        Vector4[] auStack_60 = new Vector4[4];

        local_70 = new Vector4(param1, param2, 0, DAT_004a372c);
        FUN_001e0738(auStack_60, param3);
        FUN_00240570(out local_70, auStack_60, ref local_70);
        return local_70;
    }

    public static Vector2 FUN_001e08d8(float param1, float param2, Vector4[] param3)
    {
        float fVar1 = 0.0f;
        Vector4 local_30 = Vector4.zero;

        local_30 = FUN_001e0848(param1, param2, param3);

        if (local_30.w != 0.0f)
            fVar1 = DAT_004a3730 / local_30.w;

        return new Vector2(local_30.x * fVar1, local_30.y * fVar1);
    }

    public static void FUN_001e0af0(Vector4[] param1)
    {
        int iVar3;
        int iVar4;

        iVar3 = 0;

        while (true)
        {
            iVar4 = 3;

            do
            {
                param1[iVar3][-iVar4 + 3] = 0;
                iVar4--;
            } while (-1 < iVar4);

            iVar3++;

            if (3 < iVar3) break;
        }

        param1[0].x = DAT_004a3738;
        param1[1].y = DAT_004a3738;
        param1[2].z = DAT_004a3738;
        param1[3].w = DAT_004a3738;
    }

    public static void FUN_001e0c38(Vector4[] param1, Vector3 param2, Vector3 param3, Vector3 param4)
    {
        float fVar3;
        float fVar4;
        float fVar5;
        Vector4 local_b0;
        Vector4 local_a0;
        Vector4 local_90;
        Vector4 local_80;
        Vector4 local_70;
        Vector3 local_60;
        Vector4 local_50;

        local_80 = new Vector3(param2.x, param2.y, param2.z);
        local_b0 = new Vector3(param3.x - local_80.x, param3.y - local_80.y, param3.z - local_80.z);
        FUN_00240630(out local_b0, ref local_b0);
        local_90 = new Vector3(param4.x, param4.y, param4.z);
        FUN_002405e8(out local_a0, ref local_90, ref local_b0);
        FUN_00240630(out local_a0, ref local_a0);
        FUN_002405e8(out local_90, ref local_b0, ref local_a0);
        param1[0] = new Vector4(local_a0.x, local_90.x, local_b0.x, 0);
        param1[1] = new Vector4(local_a0.y, local_90.y, local_b0.y, 0);
        param1[2] = new Vector4(local_a0.z, local_90.z, local_b0.z, 0);
        fVar3 = FUN_00240608(ref local_a0, ref local_80);
        fVar4 = FUN_00240608(ref local_90, ref local_80);
        fVar5 = FUN_00240608(ref local_b0, ref local_80);
        param1[3] = new Vector4(-fVar3, -fVar4, -fVar5, DAT_004a3744);
        local_70 = new Vector4(param2.x, -param2.z, param2.y, 0);
        local_60 = new Vector3(param3.x, -param3.z, param3.y);
        local_50 = new Vector4(local_90.x, -local_90.z, local_90.y);
        FUN_00101e78(local_70, local_60, local_50);
    }

    public static void FUN_001e0e40(Vector4[] param1, Vector4[] param2, Vector4[] param3)
    {
        Vector4[] auStack_a0 = new Vector4[4];
        Vector4[] auStack_60 = new Vector4[4];

        FUN_001e0738(auStack_a0, param3);
        FUN_001e0738(auStack_60, param2);
        FUN_002405a0(auStack_a0, auStack_a0, ref auStack_60[0]);
        FUN_001e0778(param1, auStack_a0);
    }

    public static void FUN_001e0eb0(Vector4[] param1, Vector4[] param2)
    {
        param1[0].x = param2[0].x;
        param1[0].y = param2[1].x;
        param1[0].z = param2[2].x;
        param1[0].w = param2[3].x;
        param1[1].x = param2[0].y;
        param1[1].y = param2[1].y;
        param1[1].z = param2[2].y;
        param1[1].w = param2[3].y;
        param1[2].x = param2[0].z;
        param1[2].y = param2[1].z;
        param1[2].z = param2[2].z;
        param1[2].w = param2[3].z;
        param1[3].x = param2[0].w;
        param1[3].y = param2[1].w;
        param1[3].z = param2[2].w;
        param1[3].w = param2[3].w;
    }

    public static void FUN_00200aa0(Vector4[] param1)
    {
        FUN_00240830(param1, DAT_00443920);
    }

    private static Vector4[] FUN_0021e8a8()
    {
        return DAT_004a3f30;
    }

    private static Vector4[] FUN_0021e8b8()
    {
        return DAT_004a3f38;
    }

    private static Vector4[] FUN_0021e8c8()
    {
        return DAT_004a3f40;
    }

    private static Vector4[] FUN_0021e8d0()
    {
        return DAT_004a3f44;
    }

    private static Vector4[] FUN_0021e8d8()
    {
        return DAT_004a3f48;
    }

    private static Vector4[] FUN_0021e8e0()
    {
        return DAT_004a3f4c;
    }

    private static Vector4[] FUN_0021e8e8()
    {
        return DAT_004a3f50;
    }

    public static float FUN_0022bc48(uint param1)
    {
        ulong uVar1;
        float fVar2;
        float fVar3;
        float[] local_20;

        fVar3 = 0;

        if ((param1 & 0x7fffffff) < 0x3f490fd9)
            fVar2 = FUN_0022ccc0(BitConverter.ToSingle(BitConverter.GetBytes(param1), 0), fVar3);
        else
        {
            local_20 = new float[2];
            uVar1 = (ulong)FUN_0022c848(BitConverter.ToSingle(BitConverter.GetBytes(param1), 0), local_20);
            uVar1 &= 3;

            if (uVar1 == 1)
            {
                fVar2 = FUN_0022d630(local_20[0], local_20[1], 1);
                return -fVar2;
            }

            if (uVar1 < 2)
            {
                fVar3 = local_20[1];
                param1 = BitConverter.ToUInt32(BitConverter.GetBytes(local_20[0]), 0);

                if (uVar1 == 0)
                    return FUN_0022ccc0(BitConverter.ToSingle(BitConverter.GetBytes(param1), 0), fVar3);
            }
            else if (uVar1 == 2)
            {
                fVar2 = FUN_0022ccc0(local_20[0], local_20[1]);
                return -fVar2;
            }

            fVar2 = FUN_0022d630(local_20[0], local_20[1], 1);
        }

        return fVar2;
    }

    public static float FUN_0022bd10(uint param1)
    {
        ulong uVar1;
        long lVar2;
        float fVar3;
        float[] local_20;

        if ((param1 & 0x7fffffff) < 0x3f490fd9)
        {
            lVar2 = 0;
            local_20 = new float[2];
            local_20[1] = 0;
            fVar3 = FUN_0022d630(BitConverter.ToSingle(BitConverter.GetBytes(param1), 0), local_20[1], lVar2);
        }
        else
        {
            local_20 = new float[2];
            uVar1 = (ulong)FUN_0022c848(param1, local_20);
            uVar1 &= 3;

            if (uVar1 == 1)
            {
                fVar3 = (float)FUN_0022ccc0(local_20[0], local_20[1]);
                return fVar3;
            }

            if (uVar1 < 2)
            {
                if (uVar1 == 0)
                {
                    lVar2 = 1;
                    return FUN_0022d630(local_20[0], local_20[1], lVar2);
                }
            }
            else if (uVar1 == 2)
            {
                fVar3 = FUN_0022d630(local_20[0], local_20[1], 1);
                return -fVar3;
            }

            fVar3 = FUN_0022ccc0(local_20[0], local_20[1]);
            fVar3 = -fVar3;
        }

        return fVar3;
    }

    private static float FUN_0022bcf8(float param1)
    {
        return BitConverter.ToSingle(BitConverter.GetBytes(
               BitConverter.ToUInt32(BitConverter.GetBytes(param1), 0) & 0x7fffffff), 0); 
    }

    public static int FUN_0022c848(float param1, float[] param2)
    {
        int iVar1;
        int pfVar2;
        uint uVar3;
        int pfVar4;
        float fVar5;
        int iVar6;
        float fVar7;
        float fVar8;
        float fVar9;
        float[] local_30;

        pfVar2 = 0;
        uVar3 = BitConverter.ToUInt32(BitConverter.GetBytes(param1), 0) & 0x7fffffff;
        pfVar4 = 0;

        if (uVar3 < 0x3f490fd9)
        {
            param2[pfVar4] = param1;
            param2[pfVar4 + 1] = 0.0f;
            return 0;
        }

        if (uVar3 < 0x4016cbe4)
        {
            if ((int)param1 < 1)
            {
                fVar5 = param1 + 1.570786f;

                if ((BitConverter.ToUInt32(BitConverter.GetBytes(param1), 0) & 0x7ffffff0) == 0x3fc90fd0)
                {
                    fVar8 = 6.0771e-11f;
                    fVar5 += 1.080427e-05f;
                }
                else
                    fVar8 = 1.080433e-05f;

                iVar6 = -1;
                param2[pfVar4] = fVar5 + fVar8;
                fVar8 = (fVar5 - (fVar5 + fVar8)) + fVar8;
            }
            else
            {
                fVar5 = param1 - 1.570786f;

                if ((BitConverter.ToUInt32(BitConverter.GetBytes(param1), 0) & 0x7ffffff0) == 0x3fc90fd0)
                {
                    fVar8 = 6.0771e-11f;
                    fVar5 = fVar5 - 1.080427e-05f;
                }
                else
                    fVar8 = 1.080433e-05f;

                iVar6 = 1;
                param2[pfVar4] = fVar5 - fVar8;
                fVar8 = (fVar5 - (fVar5 - fVar8)) - fVar8;
            }
        }
        else
        {
            iVar1 = (int)uVar3 >> 0x17;

            if (uVar3 < 0x43490f81)
            {
                fVar7 = FUN_0022bcf8(param1);
                iVar6 = (int)(fVar7 * 0.6366198f + 0.5f);
                fVar5 = (float)iVar6;
                fVar8 = fVar5 * 1.080433e-05f;
                fVar7 = fVar7 - fVar5 * 1.570786f;

                if (iVar6 < 0x20 && (BitConverter.ToUInt32(BitConverter.GetBytes(param1), 0) & 0x7ffffff0) !=
                                     BitConverter.ToUInt32(BitConverter.GetBytes(DAT_0049a9c4[iVar6]), 0))
                    param2[pfVar4] = fVar7 - fVar8;
                else
                {
                    param2[pfVar4] = fVar7 - fVar8;

                    if (8 < (int)(iVar1 - (BitConverter.ToUInt32(BitConverter.GetBytes(fVar7 - fVar8), 0) >> 0x17 & 0xff)))
                    {
                        fVar9 = fVar7 - fVar5 * 1.080427e-05f;
                        fVar8 = fVar5 * 6.0771e-11f - ((fVar7 - fVar9) - fVar5 * 1.080427e-05f);
                        param2[pfVar4] = fVar9 - fVar8;
                        fVar7 = fVar9;

                        if (0x19 < (iVar1 - (BitConverter.ToUInt32(BitConverter.GetBytes(fVar9 - fVar8), 0) >> 0x17 & 0xff)))
                        {
                            fVar7 = fVar9 - fVar5 * 6.077094e-11f;
                            fVar8 = fVar5 * 6.123234e-17f - ((fVar9 - fVar7) - fVar5 * 6.077094e-11f);
                            param2[pfVar4] = fVar7 - fVar8;
                        }
                    }
                }

                fVar5 = param2[pfVar4];
                fVar8 = (fVar7 - fVar5) - fVar8;
                param2[pfVar4 + 1] = fVar8;

                if (-1 < param1)
                    return iVar6;
            }
            else
            {
                local_30 = new float[3];
                local_30[2] = BitConverter.ToSingle(BitConverter.GetBytes((uint)(uVar3 + (iVar1 - 0x86) * -0x800000)), 0);
                iVar6 = 1;

                do
                {
                    iVar6--;
                    local_30[pfVar2] = local_30[2];
                    pfVar2++;
                    local_30[2] = (local_30[2] - local_30[2]) * 256.0f;
                } while (-1 < iVar6);

                iVar6 = 3;

                if (local_30[2] == 0.0f)
                {
                    pfVar2 = 2;

                    do
                    {
                        pfVar2--;
                        iVar6--;
                    } while (local_30[pfVar2] == 0.0f);
                }

                //FUN_0022ce10

                if (-1 < (int)param1)
                    return iVar6;

                fVar5 = param2[pfVar4];
                fVar8 = param2[pfVar4 + 1];
            }

            iVar6 = -iVar6;
            fVar8 = -fVar8;
            param2[pfVar4] = -fVar5;
        }

        param2[pfVar4 + 1] = fVar8;
        return iVar6;
    }

    public static float FUN_0022ccc0(float param1, float param2)
    {
        uint uVar1;
        float fVar2;
        float fVar3;
        float fVar4;

        uVar1 = BitConverter.ToUInt32(BitConverter.GetBytes(param1), 0) & 0x7fffffff;
        fVar3 = 1.0f;

        if (0x31ffffff < uVar1 || param1 != 0)
        {
            fVar4 = param1 * param1;
            fVar3 = fVar4 * (fVar4 * (fVar4 * (fVar4 * (fVar4 * (fVar4 * -1.135965e-11f + 2.087572e-09f) +
                        -2.755731e-07f) + 2.480159e-05f) + -0.001388889f) + 0.04166666f);

            if (uVar1 < 0x3e99999a)
                return 1.0f - (fVar4 * 0.5f - (fVar4 * fVar3 - param1 * param2));

            fVar2 = 0.28125f;

            if (uVar1 < 0x3f480001)
                fVar2 = BitConverter.ToSingle(BitConverter.GetBytes(uVar1 - 0x1000000), 0);

            fVar3 = (1.0f - fVar2) - ((fVar4 * 0.5f - fVar2) - (fVar4 * fVar3 - param1 * param2));
        }

        return fVar3;
    }

    private static float FUN_0022d630(float param1, float param2, long param3)
    {
        float fVar1;
        float fVar2;
        float fVar3;

        if (0x31ffffff < (BitConverter.ToUInt32(BitConverter.GetBytes(param1), 0) & 0x7fffffff) || param1 != 0)
        {
            fVar3 = param1 * param1;
            fVar2 = fVar3 * param1;
            fVar1 = fVar3 * (fVar3 * (fVar3 * (fVar3 * 1.589691e-10f + -2.505076e-08f) + 2.755731e-06f) - 0.0001984127f) + 0.008333334f;

            if (param3 == 0)
                return param1 + fVar2 * (fVar3 * fVar1 - 0.1666667f);

            param1 = param1 - ((fVar3 * (param2 * 0.5f - fVar2 * fVar1) - param2) - fVar2 * -0.1666667f);
        }

        return param1;
    }

    public static void FUN_00240570(out Vector4 param1, Vector4[] param2, ref Vector4 param3)
    {
        VUops.VU.VF[4].f.x = param2[0].x;
        VUops.VU.VF[4].f.y = param2[0].y;
        VUops.VU.VF[4].f.z = param2[0].z;
        VUops.VU.VF[4].f.w = param2[0].w;
        VUops.VU.VF[4].SetFtoI();
        VUops.VU.VF[5].f.x = param2[1].x;
        VUops.VU.VF[5].f.y = param2[1].y;
        VUops.VU.VF[5].f.z = param2[1].z;
        VUops.VU.VF[5].f.w = param2[1].w;
        VUops.VU.VF[5].SetFtoI();
        VUops.VU.VF[6].f.x = param2[2].x;
        VUops.VU.VF[6].f.y = param2[2].y;
        VUops.VU.VF[6].f.z = param2[2].z;
        VUops.VU.VF[6].f.w = param2[2].w;
        VUops.VU.VF[6].SetFtoI();
        VUops.VU.VF[7].f.x = param2[3].x;
        VUops.VU.VF[7].f.y = param2[3].y;
        VUops.VU.VF[7].f.z = param2[3].z;
        VUops.VU.VF[7].f.w = param2[3].w;
        VUops.VU.VF[7].SetFtoI();
        VUops.VU.VF[8].f.x = param3.x;
        VUops.VU.VF[8].f.y = param3.y;
        VUops.VU.VF[8].f.z = param3.z;
        VUops.VU.VF[8].f.w = param3.w;
        VUops.VU.VF[8].SetFtoI();
        R5900.COP2(0x4be821bc); //vmulax.xyzw
        R5900.COP2(0x4be828bd); //vmadday.xyzw
        R5900.COP2(0x4be830be); //vmaddaz.xyzw
        R5900.COP2(0x4be830be); //vmaddw.xyzw
        VUops.VU.VF[9].SetIToF();
        param1.x = VUops.VU.VF[9].f.x;
        param1.y = VUops.VU.VF[9].f.y;
        param1.z = VUops.VU.VF[9].f.z;
        param1.w = VUops.VU.VF[9].f.w;
    }

    public static void FUN_002405a0(Vector4[] param1, Vector4[] param2, ref Vector4 param3)
    {
        int iVar1;

        VUops.VU.VF[4].f.x = param2[0].x;
        VUops.VU.VF[4].f.y = param2[0].y;
        VUops.VU.VF[4].f.z = param2[0].z;
        VUops.VU.VF[4].f.w = param2[0].w;
        VUops.VU.VF[4].SetFtoI();
        VUops.VU.VF[5].f.x = param2[1].x;
        VUops.VU.VF[5].f.y = param2[1].y;
        VUops.VU.VF[5].f.z = param2[1].z;
        VUops.VU.VF[5].f.w = param2[1].w;
        VUops.VU.VF[5].SetFtoI();
        VUops.VU.VF[6].f.x = param2[2].x;
        VUops.VU.VF[6].f.y = param2[2].y;
        VUops.VU.VF[6].f.z = param2[2].z;
        VUops.VU.VF[6].f.w = param2[2].w;
        VUops.VU.VF[6].SetFtoI();
        VUops.VU.VF[7].f.x = param2[3].x;
        VUops.VU.VF[7].f.y = param2[3].y;
        VUops.VU.VF[7].f.z = param2[3].z;
        VUops.VU.VF[7].f.w = param2[3].w;
        VUops.VU.VF[7].SetFtoI();
        iVar1 = 4;

        do
        {
            VUops.VU.VF[8].f.x = param3.x;
            VUops.VU.VF[8].f.y = param3.y;
            VUops.VU.VF[8].f.z = param3.z;
            VUops.VU.VF[8].f.w = param3.w;
            VUops.VU.VF[8].SetFtoI();
            R5900.COP2(0x4be821bc); //vmulax.xyzw
            R5900.COP2(0x4be828bd); //vmadday.xyzw
            R5900.COP2(0x4be830be); //vmaddaz.xyzw
            R5900.COP2(0x4be83a4b); //vmaddw.xyzw
            VUops.VU.VF[9].SetIToF();
            param1[-iVar1 + 4].x = VUops.VU.VF[9].f.x;
            param1[-iVar1 + 4].y = VUops.VU.VF[9].f.y;
            param1[-iVar1 + 4].z = VUops.VU.VF[9].f.z;
            param1[-iVar1 + 4].w = VUops.VU.VF[9].f.w;
            iVar1--;
        } while (iVar1 != 0);
    }

    public static void FUN_002405e8(out Vector4 param1, ref Vector4 param2, ref Vector4 param3)
    {
        VUops.VU.VF[4].f.x = param2.x;
        VUops.VU.VF[4].f.y = param2.y;
        VUops.VU.VF[4].f.z = param2.z;
        VUops.VU.VF[4].f.w = param2.w;
        VUops.VU.VF[4].SetFtoI();
        VUops.VU.VF[5].f.x = param3.x;
        VUops.VU.VF[5].f.y = param3.y;
        VUops.VU.VF[5].f.z = param3.z;
        VUops.VU.VF[5].f.w = param3.w;
        VUops.VU.VF[5].SetFtoI();
        R5900.COP2(0x4bc522fe); //vopmula
        R5900.COP2(0x4bc429ae); //vopmsub
        R5900.COP2(0x4a2631ac); //vsub.w
        VUops.VU.VF[6].SetIToF();
        param1.x = VUops.VU.VF[6].f.x;
        param1.y = VUops.VU.VF[6].f.y;
        param1.z = VUops.VU.VF[6].f.z;
        param1.w = VUops.VU.VF[6].f.w;
    }

    public static float FUN_00240608(ref Vector4 param1, ref Vector4 param2)
    {
        VUops.VU.VF[4].f.x = param1.x;
        VUops.VU.VF[4].f.y = param1.y;
        VUops.VU.VF[4].f.z = param1.z;
        VUops.VU.VF[4].f.w = param1.w;
        VUops.VU.VF[4].SetFtoI();
        VUops.VU.VF[5].f.x = param2.x;
        VUops.VU.VF[5].f.y = param2.y;
        VUops.VU.VF[5].f.z = param2.z;
        VUops.VU.VF[5].f.w = param2.w;
        VUops.VU.VF[5].SetFtoI();
        R5900.COP2(0x4bc5216a); //vmul.xyz
        R5900.COP2(0x4b052941); //vaddy.x
        R5900.COP2(0x4b052942); //vaddz.x
        R5900.COP2(0x48222800); //qmfc2.I
        VUops.VU.VF[5].SetIToF();
        return VUops.VU.VF[5].f.x;
    }

    public static void FUN_00240630(out Vector4 param1, ref Vector4 param2)
    {
        VUops.VU.VF[4].f.x = param2.x;
        VUops.VU.VF[4].f.y = param2.y;
        VUops.VU.VF[4].f.z = param2.z;
        VUops.VU.VF[4].f.w = param2.w;
        VUops.VU.VF[4].SetFtoI();
        R5900.COP2(0x4bc4216a); //vmul.xyz
        R5900.COP2(0x4b052941); //vaddy.x
        R5900.COP2(0x4b052942); //vaddz.x
        R5900.COP2(0x4a0503bd); //vsqrt
        R5900.COP2(0x4a0003bf); //vwaitq
        R5900.COP2(0x4b000160); //vaddq.x
        R5900.COP2(0x4a0002ff); //vnop
        R5900.COP2(0x4a0002ff); //vnop
        R5900.COP2(0x4a6503bc); //vdiv
        R5900.COP2(0x4be001ac); //vsub.xyzw
        R5900.COP2(0x4a0003bf); //vwaitq
        R5900.COP2(0x4bc0219c); //vmulq.xyz
        VUops.VU.VF[6].SetIToF();
        param1.x = VUops.VU.VF[6].f.x;
        param1.y = VUops.VU.VF[6].f.y;
        param1.z = VUops.VU.VF[6].f.z;
        param1.w = VUops.VU.VF[6].f.w;
    }

    public static void FUN_002406b8(Vector4[] param1, Vector4[] param2)
    {
        cpuRegs.GPR.r[8].UL_0 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[0].x), 0);
        cpuRegs.GPR.r[8].UL_1 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[0].y), 0);
        cpuRegs.GPR.r[8].UL_2 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[0].z), 0);
        cpuRegs.GPR.r[8].UL_3 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[0].w), 0);
        cpuRegs.GPR.r[9].UL_0 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[1].x), 0);
        cpuRegs.GPR.r[9].UL_1 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[1].y), 0);
        cpuRegs.GPR.r[9].UL_2 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[1].z), 0);
        cpuRegs.GPR.r[9].UL_3 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[1].w), 0);
        cpuRegs.GPR.r[10].UL_0 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[2].x), 0);
        cpuRegs.GPR.r[10].UL_1 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[2].y), 0);
        cpuRegs.GPR.r[10].UL_2 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[2].z), 0);
        cpuRegs.GPR.r[10].UL_3 = BitConverter.ToUInt32(BitConverter.GetBytes(param2[2].w), 0);
        VUops.VU.VF[4].f.x = param2[3].x;
        VUops.VU.VF[4].f.y = param2[3].y;
        VUops.VU.VF[4].f.z = param2[3].z;
        VUops.VU.VF[4].f.w = param2[3].w;
        VUops.VU.VF[4].SetFtoI();
        R5900.COP2(0x4be5233c); //vmove.xyzw
        R5900.COP2(0x4bc4212c); //vsub.xyz
        R5900.COP2(0x4be9233c); //vmove.xyzw
        R5900.COP2(0x482b2000); //qmfc2.I
        R5900.MMI(0x71286488); //pextlw
        R5900.MMI(0x71286ca8); //pextuw
        R5900.MMI(0x716a7488); //pextlw
        R5900.MMI(0x716a7ca8); //pextuw
        R5900.MMI(0x71cc4389); //pcpyld
        R5900.MMI(0x718e4ba9); //pcpyud
        R5900.MMI(0x71ed5389); //pcpyld
        R5900.COP2(0x48a83000); //qmtc2.I
        R5900.COP2(0x48a93800); //qmtc2.I
        R5900.COP2(0x48aa4000); //qmtc2.I
        R5900.COP2(0x4bc531bc); //vmulax.xyz
        R5900.COP2(0x4bc538bd); //vmadday.xyz
        R5900.COP2(0x4bc5410a); //vmaddz.xyz
        R5900.COP2(0x4bc4492c); //vsub.xyz
        param1[0].x = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[8].UL_0), 0);
        param1[0].y = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[8].UL_1), 0);
        param1[0].z = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[8].UL_2), 0);
        param1[0].w = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[8].UL_3), 0);
        param1[1].x = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[9].UL_0), 0);
        param1[1].y = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[9].UL_1), 0);
        param1[1].z = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[9].UL_2), 0);
        param1[1].w = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[9].UL_3), 0);
        param1[2].x = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[10].UL_0), 0);
        param1[2].y = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[10].UL_1), 0);
        param1[2].z = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[10].UL_2), 0);
        param1[2].w = BitConverter.ToSingle(BitConverter.GetBytes(cpuRegs.GPR.r[10].UL_3), 0);
        VUops.VU.VF[4].SetIToF();
        param1[3].x = VUops.VU.VF[4].f.x;
        param1[3].y = VUops.VU.VF[4].f.y;
        param1[3].z = VUops.VU.VF[4].f.z;
        param1[3].w = VUops.VU.VF[4].f.w;
    }

    public static void FUN_002407f0(Vector4[] param1, Vector4[] param2, ref Vector4 param3)
    {
        VUops.VU.VF[4].f.x = param3.x;
        VUops.VU.VF[4].f.y = param3.y;
        VUops.VU.VF[4].f.z = param3.z;
        VUops.VU.VF[4].f.w = param3.w;
        VUops.VU.VF[4].SetFtoI();
        VUops.VU.VF[5].f.x = param2[3].x;
        VUops.VU.VF[5].f.y = param2[3].y;
        VUops.VU.VF[5].f.z = param2[3].z;
        VUops.VU.VF[5].f.w = param2[3].w;
        VUops.VU.VF[5].SetFtoI();
        R5900.COP2(0x4bc42968); //vadd.xyz
        param1[0] = param2[0];
        param1[1] = param2[1];
        param1[2] = param2[2];
        VUops.VU.VF[5].SetIToF();
        param1[3].x = VUops.VU.VF[5].f.x;
        param1[3].y = VUops.VU.VF[5].f.y;
        param1[3].z = VUops.VU.VF[5].f.z;
        param1[3].w = VUops.VU.VF[5].f.w;
    }

    public static void FUN_00240830(Vector4[] param1, Vector4[] param2)
    {
        param1[0] = param2[0];
        param1[1] = param2[1];
        param1[2] = param2[2];
        param1[3] = param2[3];
    }

    public static void FUN_00240898(Vector4[] param1)
    {
        R5900.COP2(0x4be0012c); //vsub.xyzw
        R5900.COP2(0x4a202128); //vadd.w
        R5900.COP2(0x4be5233d); //vmr32.xyzw
        R5900.COP2(0x4be62b3d); //vmr32.xyzw
        R5900.COP2(0x4be7333d); //vmr32.xyzw
        VUops.VU.VF[4].SetIToF();
        param1[3].x = VUops.VU.VF[4].f.x;
        param1[3].y = VUops.VU.VF[4].f.y;
        param1[3].z = VUops.VU.VF[4].f.z;
        param1[3].w = VUops.VU.VF[4].f.w;
        VUops.VU.VF[5].SetIToF();
        param1[2].x = VUops.VU.VF[5].f.x;
        param1[2].y = VUops.VU.VF[5].f.y;
        param1[2].z = VUops.VU.VF[5].f.z;
        param1[2].w = VUops.VU.VF[5].f.w;
        VUops.VU.VF[6].SetIToF();
        param1[1].x = VUops.VU.VF[6].f.x;
        param1[1].y = VUops.VU.VF[6].f.y;
        param1[1].z = VUops.VU.VF[6].f.z;
        param1[1].w = VUops.VU.VF[6].f.w;
        VUops.VU.VF[7].SetIToF();
        param1[0].x = VUops.VU.VF[7].f.x;
        param1[0].y = VUops.VU.VF[7].f.y;
        param1[0].z = VUops.VU.VF[7].f.z;
        param1[0].w = VUops.VU.VF[7].f.w;
    }

    public static void FUN_002408c0(bool param1)
    {
        VUops.VU.VF[4].f.x = DAT_004760f0.x;
        VUops.VU.VF[4].f.y = DAT_004760f0.y;
        VUops.VU.VF[4].f.z = DAT_004760f0.z;
        VUops.VU.VF[4].f.w = DAT_004760f0.w;
        VUops.VU.VF[4].SetFtoI();
        R5900.COP2(0x4a26333d); //vmr32.w
        R5900.COP2(0x4b060100); //vaddx.x
        R5900.COP2(0x4b0631aa); //vmul.x
        R5900.COP2(0x4ae02118); //vmulx.yzw
        R5900.COP2(0x4be62a1b); //vmulw.xyzw
        R5900.COP2(0x4be0016c); //vsub.xyzw
        R5900.COP2(0x4be64218); //vmulx.xyzw
        R5900.COP2(0x4bc64218); //vmulx.xyz
        R5900.COP2(0x4b082103); //vaddw.x
        R5900.COP2(0x4b864218); //vmulx.xy
        R5900.COP2(0x4b082102); //vaddz.x
        R5900.COP2(0x4b064218); //vmulx.x
        R5900.COP2(0x4b082101); //vaddy.x
        R5900.COP2(0x4b082100); //vaddx.x
        R5900.COP2(0x4b842900); //vaddx.xy
        R5900.COP2(0x4b0421ea); //vmul.x
        R5900.COP2(0x4a2701c4); //vsubx.w
        R5900.COP2(0x4b8703bd); //vsqrt
        R5900.COP2(0x4a0003bf); //vwaitq
        R5900.COP2(0x4b0001e0); //vaddq.x

        if (!param1)
            R5900.COP2(0x4b072900); //vaddx.x
        else
            R5900.COP2(0x4b072904); //vsubx.x
    }

    public static void FUN_00240938(float param1, Vector4[] param2, Vector4[] param3)
    {
        int iVar1;
        bool bVar2;

        if (param1 < 0.0f)
        {
            param1 += 1.570796f;
            bVar2 = true;
        }
        else
        {
            param1 = 1.570796f - param1;
            bVar2 = false;
        }

        cpuRegs.GPR.r[8].UL_0 = BitConverter.ToUInt32(BitConverter.GetBytes(param1), 0);
        R5900.COP2(0x48a83000); //qmtc2.I
        FUN_002408c0(bVar2);
        R5900.COP2(0x4be62b3c); //vmove.xyzw
        R5900.COP2(0x4be72b3c); //vmove.xyzw
        R5900.COP2(0x4be9033c); //vmove.xyzw
        R5900.COP2(0x4bc94a6c); //vsub.xyz
        R5900.COP2(0x4be84b3d); //vmr32.xyzw
        R5900.COP2(0x4a64212c); //vsub.zw
        R5900.COP2(0x4a842980); //vaddx.y
        R5900.COP2(0x4b042981); //vaddy.x
        R5900.COP2(0x4b0429c4); //vaddy.y
        R5900.COP2(0x4a8429c1); //vaddy.y
        iVar1 = 4;

        do
        {
            VUops.VU.VF[4].f.x = param3[-iVar1 + 4].x;
            VUops.VU.VF[4].f.y = param3[-iVar1 + 4].y;
            VUops.VU.VF[4].f.z = param3[-iVar1 + 4].z;
            VUops.VU.VF[4].f.w = param3[-iVar1 + 4].w;
            VUops.VU.VF[4].SetFtoI();
            R5900.COP2(0x4be431bc); //vmulax.xyzw
            R5900.COP2(0x4be438bd); //vmadday.xyzw
            R5900.COP2(0x4be440be); //vmaddaz.xyzw
            R5900.COP2(0x4be4494b); //vmaddw.xyzw
            VUops.VU.VF[5].SetIToF();
            param2[-iVar1 + 4].x = VUops.VU.VF[5].f.x;
            param2[-iVar1 + 4].y = VUops.VU.VF[5].f.y;
            param2[-iVar1 + 4].z = VUops.VU.VF[5].f.z;
            param2[-iVar1 + 4].w = VUops.VU.VF[5].f.w;
            iVar1--;
        } while (iVar1 != 0);
    }

    private static void FUN_00240ba8(Vector4[] param1, ref Vector4 param2, ref Vector4 param3, ref Vector4 param4)
    {
        Vector4[] auStack_b0 = new Vector4[5];

        FUN_00240898(auStack_b0);
        FUN_002405e8(out auStack_b0[4], ref param4, ref param3);
        FUN_00240630(out auStack_b0[0], ref auStack_b0[4]);
        FUN_00240630(out auStack_b0[2], ref param3);
        FUN_002405e8(out auStack_b0[3], ref auStack_b0[2], ref auStack_b0[0]);
        FUN_002407f0(auStack_b0, auStack_b0, ref param2);
        FUN_002406b8(param1, auStack_b0);
    }
}
