using static PCSX2.GSRegs;

namespace PCSX2
{
    struct Gif_Tag
    {
        struct HW_Gif_Tag
        {
            public ushort NLOOP; //15
            ushort EOP; //1
            ushort _dummy0; //16
            uint _dummy1; //14
            uint PRE; //1
            uint PRIM; //11
            public uint FLG; //2
            uint NREG; //4
            uint[] REGS; //2
        }

        HW_Gif_Tag tag;

        uint nLoop;
        uint nRegs;
        uint nRegIdx;
        uint len;
        uint cycles;
        byte[] regs; //16
        bool hadAD;
        bool isValid;

        void Reset()
        {
            //...
        }

        byte curReg() { return regs[nRegIdx & 0xf]; }

        void packetStep()
        {
            if (nLoop > 0)
            {
                nRegIdx++;

                if (nRegIdx >= nRegs)
                {
                    nRegIdx = 0;
                    nLoop--;
                }
            }
        }

        void setTag(ref HW_Gif_Tag pMem, bool analyze = false)
        {
            tag = pMem;
            nLoop = tag.NLOOP;
            hadAD = false;
            nRegIdx = 0;
            isValid = true;
            len = 0;

            switch (tag.FLG)
            {
                //case GIF_
            }
        }
    }

    struct Gif_Path
    {
        int readAmount; //atomic
        byte[] buffer;
        uint buffSize;
        uint buffLimit;
        uint curSize;
        uint curOffset;
        uint dmaRewind;
        Gif_Tag gifTag;
    }

    struct Gif_Unit
    {
        //Gif_Path
    }

    static class GifUnit
    {

    }
}