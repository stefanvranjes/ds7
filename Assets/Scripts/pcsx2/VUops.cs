using System;
using static PCSX2.VU;
using static PCSX2.VUflags;

namespace PCSX2
{
    static class VUops
    {
        struct _VURegsNum
        {
            byte pipe;
            byte VFwrite;
            byte VFwxyzw;
            byte VFr0xyzw;
            byte VFr1xyzw;
            byte VFread0;
            byte VFread1;
            uint VIwrite;
            uint VIread;
            int cycles;
        }
        delegate float Fn(uint u1, uint u2);
        delegate float Fn2(uint u1, uint u2, uint u3);
        delegate uint Fn3(uint u1, uint u2);
        enum MACOpDst { Fd, Acc };

        public static VURegs VU;
        public static uint _Ft_ => (VU.code >> 16) & 0x1F;
        public static uint _Fs_ => (VU.code >> 11) & 0x1F;
        public static uint _Fd_ => (VU.code >>  6) & 0x1F;
        public static uint _It_ => _Ft_ & 0xF;
        public static uint _Is_ => _Fs_ & 0xF;
        public static uint _Id_ => _Fd_ & 0xF;
        public static uint _X => (VU.code >> 24) & 0x1;
        public static uint _Y => (VU.code >> 23) & 0x1;
        public static uint _Z => (VU.code >> 22) & 0x1;
        public static uint _W => (VU.code >> 21) & 0x1;
        public static uint _XYZW => (VU.code >> 21) & 0xF;
        public static uint _Fsf_ => (VU.code >> 21) & 0x03;
        public static uint _Ftf_ => (VU.code >> 23) & 0x03;
        public static int _Imm11_ => (int)((VU.code & 0x400) != 0 ? 0xfffffc00 | (VU.code & 0x3ff) : VU.code & 0x3ff);
        public static int _UImm11_ => (int)(VU.code & 0x7ff);

        static float vuDouble(uint f)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(f), 0);
        }

        static void applyBinaryMACOp(VURegs VU, MACOpDst Dst, Fn Fn)
        {
            if (Dst == MACOpDst.Acc)
            {
                if (_X != 0) { VU.ACC.i.x = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.x, VU.VF[_Ft_].i.x)); } else VU_MACx_CLEAR(VU);
                if (_Y != 0) { VU.ACC.i.y = VU_MACy_UPDATE(VU, Fn(VU.VF[_Fs_].i.y, VU.VF[_Ft_].i.y)); } else VU_MACy_CLEAR(VU);
                if (_Z != 0) { VU.ACC.i.z = VU_MACz_UPDATE(VU, Fn(VU.VF[_Fs_].i.z, VU.VF[_Ft_].i.z)); } else VU_MACz_CLEAR(VU);
                if (_W != 0) { VU.ACC.i.w = VU_MACw_UPDATE(VU, Fn(VU.VF[_Fs_].i.w, VU.VF[_Ft_].i.w)); } else VU_MACw_CLEAR(VU);
            }
            else
            {
                if (_X != 0) { VU.VF[_Fd_].i.x = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.x, VU.VF[_Ft_].i.x)); } else VU_MACx_CLEAR(VU);
                if (_Y != 0) { VU.VF[_Fd_].i.y = VU_MACy_UPDATE(VU, Fn(VU.VF[_Fs_].i.y, VU.VF[_Ft_].i.y)); } else VU_MACy_CLEAR(VU);
                if (_Z != 0) { VU.VF[_Fd_].i.z = VU_MACz_UPDATE(VU, Fn(VU.VF[_Fs_].i.z, VU.VF[_Ft_].i.z)); } else VU_MACz_CLEAR(VU);
                if (_W != 0) { VU.VF[_Fd_].i.w = VU_MACw_UPDATE(VU, Fn(VU.VF[_Fs_].i.w, VU.VF[_Ft_].i.w)); } else VU_MACw_CLEAR(VU);
            }

            VU_STAT_UPDATE(VU);
        }

        private static void applyBinaryMACOpBroadcast(VURegs VU, MACOpDst Dst, Fn Fn, uint bc)
        {
            if (Dst == MACOpDst.Acc)
            {
                if (_X != 0) { VU.ACC.i.x = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.x, bc)); } else VU_MACx_CLEAR(VU);
                if (_Y != 0) { VU.ACC.i.y = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.y, bc)); } else VU_MACx_CLEAR(VU);
                if (_Z != 0) { VU.ACC.i.z = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.z, bc)); } else VU_MACx_CLEAR(VU);
                if (_W != 0) { VU.ACC.i.w = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.w, bc)); } else VU_MACx_CLEAR(VU);
            }
            else
            {
                if (_X != 0) { VU.VF[_Fd_].i.x = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.x, bc)); } else VU_MACx_CLEAR(VU);
                if (_Y != 0) { VU.VF[_Fd_].i.y = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.y, bc)); } else VU_MACx_CLEAR(VU);
                if (_Z != 0) { VU.VF[_Fd_].i.z = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.z, bc)); } else VU_MACx_CLEAR(VU);
                if (_W != 0) { VU.VF[_Fd_].i.w = VU_MACx_UPDATE(VU, Fn(VU.VF[_Fs_].i.w, bc)); } else VU_MACx_CLEAR(VU);
            }

            VU_STAT_UPDATE(VU);
        }

        private static float _vuOpADD(uint fs, uint ft)
        {
            return vuDouble(fs) + vuDouble(ft);
        }

        public static void _vuADD(VURegs VU)
        {
            applyBinaryMACOp(VU, MACOpDst.Fd, _vuOpADD);
        }

        private static void vuADDbc(VURegs VU, uint bc)
        {
            applyBinaryMACOpBroadcast(VU, MACOpDst.Fd, _vuOpADD, bc);
        }

        public static void _vuADDi(VURegs VU)
        {
            vuADDbc(VU, VU.VI[(int)VURegFlags.REG_I].UL);
        }

        public static void _vuADDq(VURegs VU) { vuADDbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuADDx(VURegs VU) { vuADDbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuADDy(VURegs VU) { vuADDbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuADDz(VURegs VU) { vuADDbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuADDw(VURegs VU) { vuADDbc(VU, VU.VF[_Ft_].i.w); }

        public static void _vuADDA(VURegs VU)
        {
            applyBinaryMACOp(VU, MACOpDst.Acc, _vuOpADD);
        }

        public static void vuADDAbc(VURegs VU, uint bc)
        {
            applyBinaryMACOpBroadcast(VU, MACOpDst.Acc, _vuOpADD, bc);
        }

        public static void _vuADDAi(VURegs VU) { vuADDAbc(VU, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuADDAq(VURegs VU) { vuADDAbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuADDAx(VURegs VU) { vuADDAbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuADDAy(VURegs VU) { vuADDAbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuADDAz(VURegs VU) { vuADDAbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuADDAw(VURegs VU) { vuADDAbc(VU, VU.VF[_Ft_].i.w); }

        private static float _vuOpSUB(uint fs, uint ft)
        {
            return vuDouble(fs) - vuDouble(ft);
        }

        public static void _vuSUB(VURegs VU)
        {
            applyBinaryMACOp(VU, MACOpDst.Fd, _vuOpSUB);
        }

        private static void vuSUBbc(VURegs VU, uint bc)
        {
            applyBinaryMACOpBroadcast(VU, MACOpDst.Fd, _vuOpSUB, bc);
        }

        public static void _vuSUBi(VURegs VU) { vuSUBbc(VU, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuSUBq(VURegs VU) { vuSUBbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuSUBx(VURegs VU) { vuSUBbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuSUBy(VURegs VU) { vuSUBbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuSUBz(VURegs VU) { vuSUBbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuSUBw(VURegs VU) { vuSUBbc(VU, VU.VF[_Ft_].i.w); }

        public static void _vuSUBA(VURegs VU)
        {
            applyBinaryMACOp(VU, MACOpDst.Acc, _vuOpSUB);
        }

        private static void vuSUBAbc(VURegs VU, uint bc)
        {
            applyBinaryMACOpBroadcast(VU, MACOpDst.Acc, _vuOpSUB, bc);
        }

        public static void _vuSUBAi(VURegs VU) { vuSUBAbc(VU, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuSUBAq(VURegs VU) { vuSUBAbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuSUBAx(VURegs VU) { vuSUBAbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuSUBAy(VURegs VU) { vuSUBAbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuSUBAz(VURegs VU) { vuSUBAbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuSUBAw(VURegs VU) { vuSUBAbc(VU, VU.VF[_Ft_].i.w); }

        private static float _vuOpMUL(uint fs, uint ft)
        {
            return vuDouble(fs) * vuDouble(ft);
        }

        public static void _vuMUL(VURegs VU)
        {
            applyBinaryMACOp(VU, MACOpDst.Fd, _vuOpMUL);
        }

        private static void vuMULbc(VURegs VU, uint bc)
        {
            applyBinaryMACOpBroadcast(VU, MACOpDst.Fd, _vuOpMUL, bc);
        }

        public static void _vuMULi(VURegs VU) { vuMULbc(VU, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuMULq(VURegs VU) { vuMULbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuMULx(VURegs VU) { vuMULbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuMULy(VURegs VU) { vuMULbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuMULz(VURegs VU) { vuMULbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuMULw(VURegs VU) { vuMULbc(VU, VU.VF[_Ft_].i.w); }
        
        public static void _vuMULA(VURegs VU)
        {
            applyBinaryMACOp(VU, MACOpDst.Acc, _vuOpMUL);
        }

        private static void vuMULAbc(VURegs VU, uint bc)
        {
            applyBinaryMACOpBroadcast(VU, MACOpDst.Acc, _vuOpMUL, bc);
        }

        public static void _vuMULAi(VURegs VU) { vuMULAbc(VU, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuMULAq(VURegs VU) { vuMULAbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuMULAx(VURegs VU) { vuMULAbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuMULAy(VURegs VU) { vuMULAbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuMULAz(VURegs VU) { vuMULAbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuMULAw(VURegs VU) { vuMULAbc(VU, VU.VF[_Ft_].i.w); }

        private static void applyTernaryMACOp(VURegs VU, MACOpDst Dst, Fn2 Fn)
        {
            if (Dst == MACOpDst.Acc)
            {
                if (_X != 0) { VU.ACC.i.x = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.x, VU.VF[_Fs_].i.x, VU.VF[_Ft_].i.x)); } else VU_MACx_CLEAR(VU);
                if (_Y != 0) { VU.ACC.i.y = VU_MACy_UPDATE(VU, Fn(VU.ACC.i.y, VU.VF[_Fs_].i.y, VU.VF[_Ft_].i.y)); } else VU_MACy_CLEAR(VU);
                if (_Z != 0) { VU.ACC.i.z = VU_MACz_UPDATE(VU, Fn(VU.ACC.i.z, VU.VF[_Fs_].i.z, VU.VF[_Ft_].i.z)); } else VU_MACz_CLEAR(VU);
                if (_W != 0) { VU.ACC.i.w = VU_MACw_UPDATE(VU, Fn(VU.ACC.i.w, VU.VF[_Fs_].i.w, VU.VF[_Ft_].i.w)); } else VU_MACw_CLEAR(VU);
            }
            else
            {
                if (_X != 0) { VU.VF[_Fd_].i.x = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.x, VU.VF[_Fs_].i.x, VU.VF[_Ft_].i.x)); } else VU_MACx_CLEAR(VU);
                if (_Y != 0) { VU.VF[_Fd_].i.y = VU_MACy_UPDATE(VU, Fn(VU.ACC.i.y, VU.VF[_Fs_].i.y, VU.VF[_Ft_].i.y)); } else VU_MACy_CLEAR(VU);
                if (_Z != 0) { VU.VF[_Fd_].i.z = VU_MACz_UPDATE(VU, Fn(VU.ACC.i.z, VU.VF[_Fs_].i.z, VU.VF[_Ft_].i.z)); } else VU_MACz_CLEAR(VU);
                if (_W != 0) { VU.VF[_Fd_].i.w = VU_MACw_UPDATE(VU, Fn(VU.ACC.i.w, VU.VF[_Fs_].i.w, VU.VF[_Ft_].i.w)); } else VU_MACw_CLEAR(VU);
            }

            VU_STAT_UPDATE(VU);
        }

        private static void applyTernaryMACOpBroadcast(VURegs VU, MACOpDst Dst, Fn2 Fn, uint bc)
        {
            if (Dst == MACOpDst.Acc)
            {
                if (_X != 0) { VU.ACC.i.x = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.x, VU.VF[_Fs_].i.x, bc)); } else VU_MACx_CLEAR(VU);
                if (_Y != 0) { VU.ACC.i.y = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.y, VU.VF[_Fs_].i.y, bc)); } else VU_MACx_CLEAR(VU);
                if (_Z != 0) { VU.ACC.i.z = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.z, VU.VF[_Fs_].i.z, bc)); } else VU_MACx_CLEAR(VU);
                if (_W != 0) { VU.ACC.i.w = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.w, VU.VF[_Fs_].i.w, bc)); } else VU_MACx_CLEAR(VU);
            }
            else
            {
                if (_X != 0) { VU.VF[_Fd_].i.x = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.x, VU.VF[_Fs_].i.x, bc)); } else VU_MACx_CLEAR(VU);
                if (_Y != 0) { VU.VF[_Fd_].i.y = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.y, VU.VF[_Fs_].i.y, bc)); } else VU_MACx_CLEAR(VU);
                if (_Z != 0) { VU.VF[_Fd_].i.z = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.z, VU.VF[_Fs_].i.z, bc)); } else VU_MACx_CLEAR(VU);
                if (_W != 0) { VU.VF[_Fd_].i.w = VU_MACx_UPDATE(VU, Fn(VU.ACC.i.w, VU.VF[_Fs_].i.w, bc)); } else VU_MACx_CLEAR(VU);
            }

            VU_STAT_UPDATE(VU);
        }

        private static float _vuOpMADD(uint acc, uint fs, uint ft)
        {
            return vuDouble(acc) + vuDouble(fs) * vuDouble(ft);
        }

        public static void _vuMADD(VURegs VU)
        {
            applyTernaryMACOp(VU, MACOpDst.Fd, _vuOpMADD);
        }

        private static void vuMADDbc(VURegs VU, uint bc)
        {
            applyTernaryMACOpBroadcast(VU, MACOpDst.Fd, _vuOpMADD, bc);
        }

        public static void _vuMADDi(VURegs VU) { vuMADDbc(VU, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuMADDq(VURegs VU) { vuMADDbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuMADDx(VURegs VU) { vuMADDbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuMADDy(VURegs VU) { vuMADDbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuMADDz(VURegs VU) { vuMADDbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuMADDw(VURegs VU) { vuMADDbc(VU, VU.VF[_Ft_].i.w); }

        public static void _vuMADDA(VURegs VU)
        {
            applyTernaryMACOp(VU, MACOpDst.Acc, _vuOpMADD);
        }

        private static void vuMADDAbc(VURegs VU, uint bc)
        {
            applyTernaryMACOpBroadcast(VU, MACOpDst.Acc, _vuOpMADD, bc);
        }

        public static void _vuMADDAi(VURegs VU) { vuMADDAbc(VU, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuMADDAq(VURegs VU) { vuMADDAbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuMADDAx(VURegs VU) { vuMADDAbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuMADDAy(VURegs VU) { vuMADDAbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuMADDAz(VURegs VU) { vuMADDAbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuMADDAw(VURegs VU) { vuMADDAbc(VU, VU.VF[_Ft_].i.w); }

        private static float _vuOpMSUB(uint acc, uint fs, uint ft)
        {
            return vuDouble(acc) - vuDouble(fs) * vuDouble(ft);
        }

        public static void _vuMSUB(VURegs VU)
        {
            applyTernaryMACOp(VU, MACOpDst.Fd, _vuOpMSUB);
        }

        private static void vuMSUBbc(VURegs VU, uint bc)
        {
            applyTernaryMACOpBroadcast(VU, MACOpDst.Fd, _vuOpMSUB, bc);
        }

        public static void _vuMSUBi(VURegs VU) { vuMSUBbc(VU, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuMSUBq(VURegs VU) { vuMSUBbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuMSUBx(VURegs VU) { vuMSUBbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuMSUBy(VURegs VU) { vuMSUBbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuMSUBz(VURegs VU) { vuMSUBbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuMSUBw(VURegs VU) { vuMSUBbc(VU, VU.VF[_Ft_].i.w); }

        public static void _vuMSUBA(VURegs VU)
        {
            applyTernaryMACOp(VU, MACOpDst.Acc, _vuOpMSUB);
        }

        private static void vuMSUBAbc(VURegs VU, uint bc)
        {
            applyTernaryMACOpBroadcast(VU, MACOpDst.Acc, _vuOpMSUB, bc);
        }

        public static void _vuMSUBAi(VURegs VU) { vuMSUBAbc(VU, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuMSUBAq(VURegs VU) { vuMSUBAbc(VU, VU.VI[(int)VURegFlags.REG_Q].UL); }
        public static void _vuMSUBAx(VURegs VU) { vuMSUBAbc(VU, VU.VF[_Ft_].i.x); }
        public static void _vuMSUBAy(VURegs VU) { vuMSUBAbc(VU, VU.VF[_Ft_].i.y); }
        public static void _vuMSUBAz(VURegs VU) { vuMSUBAbc(VU, VU.VF[_Ft_].i.z); }
        public static void _vuMSUBAw(VURegs VU) { vuMSUBAbc(VU, VU.VF[_Ft_].i.w); }

        private static uint fp_max(uint a, uint b)
        {
            return (uint)(((int)a < 0 && (int)b < 0) ? Math.Min((int)a, (int)b) : Math.Max((int)a, (int)b));
        }

        private static uint fp_min(uint a, uint b)
        {
            return (uint)(((int)a < 0 && (int)b < 0) ? Math.Max((int)a, (int)b) : Math.Min((int)a, (int)b));
        }

        private static void applyMinMax(VURegs VU, Fn3 Fn)
        {
            if (_Fd_ == 0)
                return;

            if (_X != 0) VU.VF[_Fd_].i.x = Fn(VU.VF[_Fs_].i.x, VU.VF[_Ft_].i.x);
            if (_Y != 0) VU.VF[_Fd_].i.y = Fn(VU.VF[_Fs_].i.y, VU.VF[_Ft_].i.y);
            if (_Z != 0) VU.VF[_Fd_].i.z = Fn(VU.VF[_Fs_].i.z, VU.VF[_Ft_].i.z);
            if (_W != 0) VU.VF[_Fd_].i.w = Fn(VU.VF[_Fs_].i.w, VU.VF[_Ft_].i.w);
        }

        private static void applyMinMaxBroadcast(VURegs VU, Fn3 Fn, uint bc)
        {
            if (_Fd_ == 0)
                return;

            if (_X != 0) VU.VF[_Fd_].i.x = Fn(VU.VF[_Fs_].i.x, bc);
            if (_Y != 0) VU.VF[_Fd_].i.y = Fn(VU.VF[_Fs_].i.y, bc);
            if (_Z != 0) VU.VF[_Fd_].i.z = Fn(VU.VF[_Fs_].i.z, bc);
            if (_W != 0) VU.VF[_Fd_].i.w = Fn(VU.VF[_Fs_].i.w, bc);
        }

        public static void _vuMAX(VURegs VU)
        {
            applyMinMax(VU, fp_max);
        }

        public static void _vuMAXi(VURegs VU) { applyMinMaxBroadcast(VU, fp_max, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuMAXx(VURegs VU) { applyMinMaxBroadcast(VU, fp_max, VU.VF[_Ft_].i.x); }
        public static void _vuMAXy(VURegs VU) { applyMinMaxBroadcast(VU, fp_max, VU.VF[_Ft_].i.y); }
        public static void _vuMAXz(VURegs VU) { applyMinMaxBroadcast(VU, fp_max, VU.VF[_Ft_].i.z); }
        public static void _vuMAXw(VURegs VU) { applyMinMaxBroadcast(VU, fp_max, VU.VF[_Ft_].i.w); }

        public static void _vuMINI(VURegs VU)
        {
            applyMinMax(VU, fp_min);
        }

        public static void _vuMINIi(VURegs VU) { applyMinMaxBroadcast(VU, fp_min, VU.VI[(int)VURegFlags.REG_I].UL); }
        public static void _vuMINIx(VURegs VU) { applyMinMaxBroadcast(VU, fp_min, VU.VF[_Ft_].i.x); }
        public static void _vuMINIy(VURegs VU) { applyMinMaxBroadcast(VU, fp_min, VU.VF[_Ft_].i.y); }
        public static void _vuMINIz(VURegs VU) { applyMinMaxBroadcast(VU, fp_min, VU.VF[_Ft_].i.z); }
        public static void _vuMINIw(VURegs VU) { applyMinMaxBroadcast(VU, fp_min, VU.VF[_Ft_].i.w); }

        public static void _vuOPMULA(VURegs VU)
        {
            VU.ACC.i.x = VU_MACx_UPDATE(VU, vuDouble(VU.VF[_Fs_].i.y) * vuDouble(VU.VF[_Ft_].i.z));
            VU.ACC.i.y = VU_MACy_UPDATE(VU, vuDouble(VU.VF[_Fs_].i.z) * vuDouble(VU.VF[_Ft_].i.x));
            VU.ACC.i.z = VU_MACz_UPDATE(VU, vuDouble(VU.VF[_Fs_].i.x) * vuDouble(VU.VF[_Ft_].i.y));
            VU_STAT_UPDATE(VU);
        }

        public static void _vuOPMSUB(VURegs VU)
        {
            if (_Fd_ == 0) return;

            float ftx, fty, ftz;
            float fsx, fsy, fsz;

            ftx = vuDouble(VU.VF[_Ft_].i.x);
            fty = vuDouble(VU.VF[_Ft_].i.y);
            ftz = vuDouble(VU.VF[_Ft_].i.z);
            fsx = vuDouble(VU.VF[_Fs_].i.x);
            fsy = vuDouble(VU.VF[_Fs_].i.y);
            fsz = vuDouble(VU.VF[_Fs_].i.z);

            VU.VF[_Fd_].i.x = VU_MACx_UPDATE(VU, vuDouble(VU.ACC.i.x) - fsy * ftz);
            VU.VF[_Fd_].i.y = VU_MACy_UPDATE(VU, vuDouble(VU.ACC.i.y) - fsz * ftx);
            VU.VF[_Fd_].i.z = VU_MACz_UPDATE(VU, vuDouble(VU.ACC.i.z) - fsx * fty);
            VU_STAT_UPDATE(VU);
        }

        public static void _vuNOP(VURegs VU)
        {
            return;
        }

        public static void _vuDIV(VURegs VU)
        {
            float ft = vuDouble(VU.VF[_Ft_].GetUL((int)_Ftf_));
            float fs = vuDouble(VU.VF[_Fs_].GetUL((int)_Fsf_));

            VU.statusflag &= ~0x30u;

            if (ft == 0.0)
            {
                if (fs == 0.0)
                    VU.statusflag |= 0x10;
                else
                    VU.statusflag |= 0x20;

                if (((VU.VF[_Ft_].GetUL((int)_Ftf_) & 0x80000000) ^
                    (VU.VF[_Fs_].GetUL((int)_Fsf_) & 0x80000000)) != 0)
                    VU.q.UL = 0xFF7FFFFF;
                else
                    VU.q.UL = 0x7F7FFFFF;

                VU.q.F = BitConverter.ToSingle(BitConverter.GetBytes(VU.q.UL), 0);
            }
            else
            {
                VU.q.F = fs / ft;
                VU.q.F = vuDouble(VU.q.UL);
                VU.q.UL = BitConverter.ToUInt32(BitConverter.GetBytes(VU.q.F), 0);
            }
        }

        public static void _vuSQRT(VURegs VU)
        {
            float ft = vuDouble(VU.VF[_Ft_].GetUL((int)_Ftf_));

            VU.statusflag &= ~0x30u;

            if (ft < 0.0f)
                VU.statusflag |= 0x10;
            VU.q.F = (float)Math.Sqrt(Math.Abs(ft));
            VU.q.UL = BitConverter.ToUInt32(BitConverter.GetBytes(VU.q.F), 0);
        }

        public static void _vuMOVE(VURegs VU)
        {
            if (_Ft_ == 0) return;

            if (_X != 0) VU.VF[_Ft_].i.x = VU.VF[_Fs_].i.x;
            if (_Y != 0) VU.VF[_Ft_].i.y = VU.VF[_Fs_].i.y;
            if (_Z != 0) VU.VF[_Ft_].i.z = VU.VF[_Fs_].i.z;
            if (_W != 0) VU.VF[_Ft_].i.w = VU.VF[_Fs_].i.w;
        }

        public static void _vuMR32(VURegs VU)
        {
            uint tx;

            if (_Ft_ == 0)
                return;

            tx = VU.VF[_Fs_].i.x;

            if (_X != 0) VU.VF[_Ft_].i.x = VU.VF[_Fs_].i.y;
            if (_Y != 0) VU.VF[_Ft_].i.y = VU.VF[_Fs_].i.z;
            if (_Z != 0) VU.VF[_Ft_].i.z = VU.VF[_Fs_].i.w;
            if (_W != 0) VU.VF[_Ft_].i.w = tx;
        }

        public static void _vuWAITQ(VURegs VU)
        {
            return;
        }
    }
}
