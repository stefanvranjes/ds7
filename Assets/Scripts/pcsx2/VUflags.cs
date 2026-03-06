using System;

namespace PCSX2
{
    static class VUflags
    {
        private static uint VU_MAC_UPDATE(int shift, VURegs VU, float f)
        {
            uint v = BitConverter.ToUInt32(BitConverter.GetBytes(f), 0);
            int exp = (int)(v >> 23) & 0xff;
            uint s = v & 0x80000000;

            if (s != 0)
                VU.macflag |= 0x0010u << shift;
            else
                VU.macflag &= ~(0x0010u << shift);

            if (f == 0)
            {
                VU.macflag = (VU.macflag & ~(0x1100u << shift)) | (0x0001u << shift);
                return v;
            }

            switch (exp)
            {
                case 0:
                    VU.macflag = (VU.macflag & ~(0x1000u << shift)) | (0x0101u << shift);
                    return s;
                case 255:
                    VU.macflag = (VU.macflag & ~(0x0101u << shift)) | (0x1000u << shift);
                    return v;
                default:
                    VU.macflag = (VU.macflag & ~(0x1101u << shift));
                    return v;
            }
        }

        public static uint VU_MACx_UPDATE(VURegs VU, float x)
        {
            return VU_MAC_UPDATE(3, VU, x);
        }

        public static uint VU_MACy_UPDATE(VURegs VU, float y)
        {
            return VU_MAC_UPDATE(2, VU, y);
        }

        public static uint VU_MACz_UPDATE(VURegs VU, float z)
        {
            return VU_MAC_UPDATE(1, VU, z);
        }

        public static uint VU_MACw_UPDATE(VURegs VU, float w)
        {
            return VU_MAC_UPDATE(0, VU, w);
        }

        public static void VU_MACx_CLEAR(VURegs VU)
        {
            VU.macflag &= ~(0x1111u << 3);
        }

        public static void VU_MACy_CLEAR(VURegs VU)
        {
            VU.macflag &= ~(0x1111u << 2);
        }

        public static void VU_MACz_CLEAR(VURegs VU)
        {
            VU.macflag &= ~(0x1111u << 1);
        }

        public static void VU_MACw_CLEAR(VURegs VU)
        {
            VU.macflag &= ~(0x1111u << 0);
        }

        public static void VU_STAT_UPDATE(VURegs VU)
        {
            int newflag = 0;

            if ((VU.macflag & 0x000F) != 0) newflag = 0x1;
            if ((VU.macflag & 0x00F0) != 0) newflag |= 0x2;
            if ((VU.macflag & 0x0F00) != 0) newflag |= 0x4;
            if ((VU.macflag & 0xF000) != 0) newflag |= 0x8;

            VU.statusflag = (uint)newflag;
        }
    }
}
