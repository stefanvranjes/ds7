using static PCSX2.VU;

namespace PCSX2
{
    static class VU0micro
    {
        static void vu0ResetRegs()
        {
            VU.VU0.VI[(int)VURegFlags.REG_VPU_STAT].UL &= ~0xffU;
            VU.VU0.VI[(int)VURegFlags.REG_FBRST].UL &= ~0xffU;
            //vif
        }

        static uint vu0DenormalizeMicroStatus(uint nstatus)
        {
            return ((nstatus >> 3) & 0x18U) | ((nstatus >> 11) & 0x1800U) | ((nstatus >> 14) & 0x3cf0000U);
        }

        static void vu0SetMicroFlags(uint[] flags, uint value)
        {
            flags[0] = flags[1] = flags[2] = flags[3] = value;
        }

        static void vu0ExecMicro(uint addr)
        {
            //log

            if ((VU.VU0.VI[(int)VURegFlags.REG_VPU_STAT].UL & 0x1) != 0)
            {
                //warning
                //finish vu0
            }

            uint CLIP = VU.VU0.VI[(int)VURegFlags.REG_CLIP_FLAG].UL;
            uint MAC = VU.VU0.VI[(int)VURegFlags.REG_MAC_FLAG].UL;
            uint STATUS = VU.VU0.VI[(int)VURegFlags.REG_STATUS_FLAG].UL;
            VU.VU0.clipflag = CLIP;
            VU.VU0.macflag = MAC;
            VU.VU0.statusflag = STATUS;

            vu0SetMicroFlags(VU.VU0.micro_clipflags, CLIP);
            vu0SetMicroFlags(VU.VU0.micro_macflags, MAC);
            vu0SetMicroFlags(VU.VU0.micro_statusflags, vu0DenormalizeMicroStatus(STATUS));

            VU.VU0.VI[(int)VURegFlags.REG_VPU_STAT].UL &= ~0xFFU;
            VU.VU0.VI[(int)VURegFlags.REG_VPU_STAT].UL |= 0x01U;
            //cycle
            if ((int)addr != -1) VU.VU0.VI[(int)VURegFlags.REG_TPC].UL = addr & 0x1FF;

            //cpuvu0
        }
    }
}
