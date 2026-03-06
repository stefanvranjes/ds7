using static PCSX2.R5900;
using static PCSX2.VU;
using static PCSX2.VUops;

namespace PCSX2
{
    static class VU0
    {
        public static void QMFC2()
        {
            if (_Rt_ == 0) return;

            cpuRegs.GPR.r[_Rt_].UL_0 = VU.VU0.VF[_Fs_].GetUL(0);
            cpuRegs.GPR.r[_Rt_].UL_1 = VU.VU0.VF[_Fs_].GetUL(1);
            cpuRegs.GPR.r[_Rt_].UL_2 = VU.VU0.VF[_Fs_].GetUL(2);
            cpuRegs.GPR.r[_Rt_].UL_3 = VU.VU0.VF[_Fs_].GetUL(3);
        }

        public static void QMTC2()
        {
            if (_Fs_ == 0) return;

            VU.VU0.VF[_Fs_].i.x = cpuRegs.GPR.r[_Rt_].UL_0;
            VU.VU0.VF[_Fs_].i.y = cpuRegs.GPR.r[_Rt_].UL_1;
            VU.VU0.VF[_Fs_].i.z = cpuRegs.GPR.r[_Rt_].UL_2;
            VU.VU0.VF[_Fs_].i.w = cpuRegs.GPR.r[_Rt_].UL_3;
        }
    }
}