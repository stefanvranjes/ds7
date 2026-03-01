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
}
