namespace PCSX2
{
    static class GS
    {
        public enum CSR_FifoState
        {
            CSR_FIFO_NORMAL = 0,
            CSR_FIFO_EMPTY,
            CSR_FIFO_FULL,
            CSR_FIFO_RESERVED
        }

        struct tGS_CSR
        {
            public ulong SIGNAL;
            ulong FINISH;
            ulong HSINT;
            ulong VSINT;
            ulong EDWINT;
            ulong _zero1;
            ulong _zero2;
            ulong pad1;
            ulong FLUSH;
            ulong RESET;
            ulong _pad2;
            ulong NFIELD;
            ulong FIELD;
            ulong FIFO;
            ulong REV;
            ulong ID;

            ulong _u64;

            uint _u32;
            uint _unused32;

            void SwapField()
            {
                _u32 ^= 0x2000;
            }

            void SetField()
            {
                _u32 |= 0x2000;
            }

            void Reset()
            {
                FIFO = (ulong)CSR_FifoState.CSR_FIFO_EMPTY;
                REV = 0x1B;
                ID = 0x55;
            }

            bool HasAnyInterrupts() { return (SIGNAL != 0 || FINISH != 0 || HSINT != 0 || VSINT != 0 || EDWINT != 0); }

            uint GetInterruptMask()
            {
                return _u32 & 0x1f;
            }

            void SetAllInterrupts(bool value = true)
            {
                SIGNAL = FINISH = HSINT = VSINT = EDWINT = value ? 1 : 0U;
            }
        }

        struct tGS_IMR
        {
            uint _reserved1;
            uint SIGMSK;
            uint FINISHHMSK;
            uint HSMSK;
            uint VSMSK;
            uint EDWMSK;
            uint _undefined;
            uint _reserved2;

            uint _u32;

            void reset()
            {
                _u32 = 0;
                SIGMSK = FINISHHMSK = HSMSK = VSMSK = EDWMSK = 1;
                _undefined = 0x3;
            }

            void set(uint value)
            {
                _u32 = (value & 0x1f00);
                _undefined = 0x3;
            }

            bool masked() { return (SIGMSK != 0 || FINISHHMSK != 0 || HSMSK != 0 || VSMSK != 0 || EDWMSK != 0); }
        }

        struct GSRegSIGBLID
        {
            uint SIGID;
            uint LBLID;
        }

        //public static tGS_CSR CSRreg;
    }
}
