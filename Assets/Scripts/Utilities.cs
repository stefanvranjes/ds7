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
        float fVar2;

        if ((param1 & 0x7fffffff) < 0x3f490fd9)
        {
            fVar2 = FUN_0022ccc0(param1, fVar3);
        }
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
}
