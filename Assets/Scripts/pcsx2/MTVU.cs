using static PCSX2.Pcsx2Defs;
using static PCSX2.GS;
using static PCSX2.GifUnit;

namespace PCSX2
{
    static class MTVU
    {
        public static VU_Thread vu1Thread;
    }

    class VU_Thread
    {
        int buffer_size = (int)((_1mb * 16) / sizeof(int));

        uint[] buffer; //buffer_size
        int m_write_pos;

        public enum InterruptFlag
        {
            InterruptFlagFinish = 1 << 0, 
            InterruptFlagSignal = 1 << 1, 
            InterruptFlagLabel  = 1 << 2, 
            InterruptFlagVUEBit = 1 << 3, 
            InterruptFlagVUTBit = 1 << 4
        }

        public uint mtvuInterrupts; // atomic
        public ulong gsLabel; // atomic
        public ulong gsSignal; // atomic

        public void Get_MTVUChanges()
        {
            uint interrupts = mtvuInterrupts; // atomic load

            if (interrupts == 0)
                return;

            if ((interrupts & (int)InterruptFlag.InterruptFlagSignal) != 0)
            {
                // atomic fence
                ulong signal = gsSignal; // atomic load
                mtvuInterrupts &= ~(uint)InterruptFlag.InterruptFlagSignal;
                uint signalMsk = (uint)(signal >> 32);
                uint signalData = (uint)signal;

                //if (CSRreg.SIGNAL != 0)
                //{

                //}
            }
        }
    }
}
