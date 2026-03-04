using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static int FUN_00145150(int param1)
    {
        return DAT_00374718[param1];
    }

    public static int FUN_001451b0(int param1)
    {
        return DAT_00374808[param1];
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
}
