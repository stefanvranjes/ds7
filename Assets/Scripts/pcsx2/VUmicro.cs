using System;
using static PCSX2.VU;
using static PCSX2.Config;
using static PCSX2.MTVU;

namespace PCSX2
{
    static class VUmicro
    {
        static uint CalculateMinRunCycles(uint cycles, bool requiresAccurateCycles)
        {
            if (requiresAccurateCycles)
                return cycles;

            return Math.Max(16U, cycles);
        }
    }

    class BaseVUmicroCPU
    {
        public int m_Idx = 0;
        public bool IsInterpreter;

        void ExecuteBlock(bool startUp)
        {
            uint stat = VU0.VI[(int)VURegFlags.REG_VPU_STAT].UL;
            uint test = m_Idx != 0 ? 0x100U : 1U;

            //if (m_Idx && THREAD_VU1)
            //{
                //vu1Thread.
            //}
        }
    }
}
