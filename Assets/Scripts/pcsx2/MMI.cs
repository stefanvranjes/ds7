using static PCSX2.R5900;

namespace PCSX2
{
    public class MMI
    {
        public static void PEXTLW()
        {
            if (_Rd_ == 0) return;

            cpuRegs.GPR.r[_Rd_].UL_0 = cpuRegs.GPR.r[_Rt_].UL_0;
            cpuRegs.GPR.r[_Rd_].UL_1 = cpuRegs.GPR.r[_Rs_].UL_0;
            cpuRegs.GPR.r[_Rd_].UL_2 = cpuRegs.GPR.r[_Rt_].UL_1;
            cpuRegs.GPR.r[_Rd_].UL_3 = cpuRegs.GPR.r[_Rs_].UL_1;
        }

        public static void PEXTUW()
        {
            if (_Rd_ == 0) return;

            cpuRegs.GPR.r[_Rd_].UL_0 = cpuRegs.GPR.r[_Rt_].UL_2;
            cpuRegs.GPR.r[_Rd_].UL_1 = cpuRegs.GPR.r[_Rs_].UL_2;
            cpuRegs.GPR.r[_Rd_].UL_2 = cpuRegs.GPR.r[_Rt_].UL_3;
            cpuRegs.GPR.r[_Rd_].UL_3 = cpuRegs.GPR.r[_Rs_].UL_3;
        }

        public static void PCPYLD()
        {
            if (_Rd_ == 0) return;

            cpuRegs.GPR.r[_Rd_].UL_0 = cpuRegs.GPR.r[_Rs_].UL_0;
            cpuRegs.GPR.r[_Rd_].UL_1 = cpuRegs.GPR.r[_Rs_].UL_1;
            cpuRegs.GPR.r[_Rd_].UL_2 = cpuRegs.GPR.r[_Rt_].UL_0;
            cpuRegs.GPR.r[_Rd_].UL_3 = cpuRegs.GPR.r[_Rt_].UL_1;
        }

        public static void PCPYUD()
        {
            if (_Rd_ == 0) return;

            cpuRegs.GPR.r[_Rd_].UL_0 = cpuRegs.GPR.r[_Rs_].UL_2;
            cpuRegs.GPR.r[_Rd_].UL_1 = cpuRegs.GPR.r[_Rs_].UL_3;
            cpuRegs.GPR.r[_Rd_].UL_2 = cpuRegs.GPR.r[_Rt_].UL_2;
            cpuRegs.GPR.r[_Rd_].UL_3 = cpuRegs.GPR.r[_Rt_].UL_3;
        }
    }
}
