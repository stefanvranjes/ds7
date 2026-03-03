using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0xe8 - size
public class PTR_DAT_004a3660
{
    public bool DAT_02; //0x02
    public byte DAT_08; //0x08
    public byte DAT_09; //0x09
    public ushort DAT_48; //0x48
    public int DAT_4c; //0x4C
    public int DAT_50; //0x50
    public int DAT_54; //0x54
    public int DAT_58; //0x58
    public int DAT_5c; //0x5C
    public int DAT_60; //0x60
    public ushort DAT_72; //0x72
}

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public PTR_DAT_004a3660[] DAT_004a3660;
    public int DAT_004a366c;
}
