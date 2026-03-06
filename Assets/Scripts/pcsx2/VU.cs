using System;
using Dirichlet.Numerics;

namespace PCSX2
{
    enum VURegFlags
    {
        REG_STATUS_FLAG = 16, 
        REG_MAC_FLAG = 17, 
        REG_CLIP_FLAG = 18, 
        REF_ACC_FLAG = 19, 
        REG_R = 20, 
        REG_I = 21, 
        REG_Q = 22, 
        REG_P = 23, 
        REG_VF0_FLAG = 24, 
        REG_TPC = 26, 
        REG_CMSAR0 = 27, 
        REG_FBRST = 28, 
        REG_VPU_STAT = 29, 
        REG_CMSAR1 = 31
    }

    enum VUStatus
    {
        VU_Ready = 0, 
        VU_Run = 1, 
        VU_Stop = 2
    }

    public struct f
    {
        public float x, y, z, w;
    }

    public struct i
    {
        public uint x, y, z, w;
    }

    public struct VECTOR
    {
        public f f;
        public i i;
        float[] F; //4

        UInt128 UQ;
        Int128 SQ;
        ulong[] UD; //2
        long[] SD; //2
        uint[] UL; //4
        int[] SL; //4
        ushort[] US; //8
        short[] SS; //8
        byte UC; //16
        sbyte SC; //16

        public uint GetUL(int index)
        {
            switch (index)
            {
                case 0:
                    return i.x;
                case 1:
                    return i.y;
                case 2:
                    return i.z;
                case 3:
                    return i.w;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public void SetFtoI()
        {
            i.x = BitConverter.ToUInt32(BitConverter.GetBytes(f.x), 0);
            i.y = BitConverter.ToUInt32(BitConverter.GetBytes(f.y), 0);
            i.z = BitConverter.ToUInt32(BitConverter.GetBytes(f.z), 0);
            i.w = BitConverter.ToUInt32(BitConverter.GetBytes(f.w), 0);
        }

        public void SetIToF()
        {
            f.x = BitConverter.ToSingle(BitConverter.GetBytes(i.x), 0);
            f.y = BitConverter.ToSingle(BitConverter.GetBytes(i.y), 0);
            f.z = BitConverter.ToSingle(BitConverter.GetBytes(i.z), 0);
            f.w = BitConverter.ToSingle(BitConverter.GetBytes(i.w), 0);
        }
    }

    public struct REG_VI
    {
        public float F;
        int SL;
        public uint UL;
        short[] SS; //2
        ushort[] US; //2
        sbyte[] SC; //4
        byte[] UC; //4
        uint[] padding; //3
    }

    struct fdivPipe
    {
        int enable;
        REG_VI reg;
        uint sCycle;
        uint Cycle;
        uint statusflag;
    }

    struct efuPipe
    {
        int enable;
        REG_VI reg;
        uint sCycle;
        uint Cycle;
    }

    struct fmacPipe
    {
        uint regupper;
        uint reglower;
        int flagreg;
        uint xyzwupper;
        uint xyzwlower;
        uint sCycle;
        uint Cycle;
        uint macflag;
        uint statusflag;
        uint clipflag;
    }

    struct ialuPipe
    {
        int reg;
        uint sCycle;
        uint Cycle;
    }

    public class VURegs
    {
        public VECTOR[] VF; //32
        public REG_VI[] VI; //32

        public VECTOR ACC;
        public REG_VI q;
        REG_VI p;

        uint idx;

        public uint cycle;
        uint flags;

        public uint code;
        uint start_pc;

        uint branch;
        uint branchpc;
        uint delaybranchpc;
        bool takedelaybranch;
        uint ebit;
        uint pending_q;
        uint pending_p;

        public uint[] micro_macflags; //4
        public uint[] micro_clipflags; //4
        public uint[] micro_statusflags; //4

        public uint macflag;
        public uint statusflag;
        public uint clipflag;

        int nextBlockCycles;

        long Mem; //pointer
        long Micro; //pointer

        uint xgkickaddr;
        uint xgkickdiff;
        uint xgkicksizeremaining;
        uint xgkicklastcycle;
        uint xgkickcyclecount;
        uint xgkickenable;
        uint xgkickendpacket;

        byte VIBackupCycles;
        uint VIOldValue;
        uint VIRegNumber;

        fmacPipe[] fmac; //4
        uint fmacreadpos;
        uint fmacwritepos;
        uint fmaccount;
        fdivPipe fdiv;
        efuPipe efu;
        ialuPipe[] ialu; //4
        uint ialureadpos;
        uint ialuwritepos;
        uint ialucount;
    }

    enum VUPipeState
    {
        VUPIPE_NONE = 0, 
        VUPIPE_FMAC, 
        VUPIPE_FDIV, 
        VUPIPE_EFU, 
        VUPIPE_IALU, 
        VUPIPE_BRANCH, 
        VUPIPE_XGKICK
    }

    static class VU
    {
        public static VURegs VU0;
        public static VURegs VU1;
    }
}
