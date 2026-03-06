namespace PCSX2
{
    static class GSRegs
    {
        public const uint VM_SIZE = 4194304;
        public const uint HALF_VM_SIZE = VM_SIZE / 2;
        public const uint GS_PAGE_SIZE = 8192;
        public const uint GS_BLOCK_SIZE = 256;
        public const uint GS_COLUMN_SIZE = 64;
        public const uint GS_BLOCKS_PER_PAGE = GS_PAGE_SIZE / GS_BLOCK_SIZE;

        public const uint GS_MAX_PAGES = VM_SIZE / GS_PAGE_SIZE;
        public const uint GS_MAX_BLOCKS = VM_SIZE / GS_BLOCK_SIZE;
        public const uint GS_MAX_COLUMNS = VM_SIZE / GS_COLUMN_SIZE;

        public enum GS_PRIM
        {
            GS_POINTLIST     = 0, 
            GS_LINELIST      = 1, 
            GS_LINESTRIP     = 2, 
            GS_TRIANGLELIST  = 3, 
            GS_TRIANGLESTRIP = 4,
            GS_TRIANGLEFAN   = 5, 
            GS_SPRITE        = 6, 
            GS_INVALID       = 7
        }

        public enum GS_PRIM_CLASS
        {
            GS_POINT_CLASS    = 0, 
            GS_LINE_CLASS     = 1, 
            GS_TRIANGLE_CLASS = 2,
            GS_SPRITE_CLASS   = 3, 
            GS_INVALID_CLASS  = 7
        }

        public enum GIF_REG
        {
            GIF_REG_PRIM      = 0x00, 
            GIF_REG_RGBA      = 0x01, 
            GIF_REG_STQ       = 0x02, 
            GIF_REG_UV        = 0x03, 
            GIF_REG_XYZF2     = 0x04, 
            GIF_REG_XYZ2      = 0x05, 
            GIF_REG_TEX0_1    = 0x06, 
            GIF_REG_TEX0_2    = 0x07, 
            GIF_REG_CLAMP_1   = 0x08, 
            GIF_REG_CLAMP_2   = 0x09, 
            GIF_REG_FOG       = 0x0a, 
            GIF_REG_INVALID   = 0x0b, 
            GIF_REG_XYZF3     = 0x0c, 
            GIF_REG_XYZ3      = 0x0d, 
            GIF_REG_A_D       = 0x0e, 
            GIF_REG_NOP       = 0x0f
        }

        public enum GIF_REG_COMPLEX
        {
            GIF_REG_STQRGBAXYZF2 = 0x00, 
            GIF_REG_STQRGBAXYZ2  = 0x01
        }

        public enum GIF_A_D_REG
        {
            GIF_A_D_REG_PRIM       = 0x00, 
            GIF_A_D_REG_RGBAQ      = 0x01, 
            GIF_A_D_REG_ST         = 0x02, 
            GIF_A_D_REG_UV         = 0x03, 
            GIF_A_D_REG_XYZF2      = 0x04, 
            GIF_A_D_REG_XYZ2       = 0x05, 
            GIF_A_D_REG_TEX0_1     = 0x06, 
            GIF_A_D_REG_TEX0_2     = 0x07, 
            GIF_A_D_REG_CLAMP_1    = 0x08, 
            GIF_A_D_REG_CLAMP_2    = 0x09, 
            GIF_A_D_REG_FOG        = 0x0a, 
            GIF_A_D_REG_XYZF3      = 0x0c, 
            GIF_A_D_REG_XYZ3       = 0x0d, 
            GIF_A_D_REG_NOP        = 0x0f, 
            GIF_A_D_REG_TEX1_1     = 0x14, 
            GIF_A_D_REG_TEX1_2     = 0x15, 
            GIF_A_D_REG_TEX2_1     = 0x16, 
            GIF_A_D_REG_TEX2_2     = 0x17, 
            GIF_A_D_REG_XYOFFSET_1 = 0x18, 
            GIF_A_D_REG_XYOFFSET_2 = 0x19, 
            GIF_A_D_REG_PRMODECONT = 0x1a, 
            GIF_A_D_REG_PRMODE     = 0x1b, 
            GIF_A_D_REG_TEXCLUT    = 0x1c, 
            GIF_A_D_REG_SCANMSK    = 0x22, 
            GIF_A_D_REG_MIPTBP1_1  = 0x34, 
            GIF_A_D_REG_MIPTBP1_2  = 0x35, 
            GIF_A_D_REG_MIPTBP2_1  = 0x36, 
            GIF_A_D_REG_MIPTBP2_2  = 0x37, 
            GIF_A_D_REG_TEXA       = 0x3b, 
            GIF_A_D_REG_FOGCOL     = 0x3d, 
            GIF_A_D_REG_TEXFLUSH   = 0x3f,
            GIF_A_D_REG_SCISSOR_1  = 0x40, 
            GIF_A_D_REG_SCISSOR_2  = 0x41, 
            GIF_A_D_REG_ALPHA_1    = 0x42, 
            GIF_A_D_REG_ALPHA_2    = 0x43, 
            GIF_A_D_REG_DIMX       = 0x44, 
            GIF_A_D_REG_DTHE       = 0x45, 
            GIF_A_D_REG_COLCLAMP   = 0x46, 
            GIF_A_D_REG_TEST_1     = 0x47, 
            GIF_A_D_REG_TEXT_2     = 0x48, 
            GIF_A_D_REG_PABE       = 0x49, 
            GIF_A_D_REG_FBA_1      = 0x4a, 
            GIF_A_D_REG_FBA_2      = 0x4b, 
            GIF_A_D_REG_FRAME_1    = 0x4c, 
            GIF_A_D_REG_ZBUF_1     = 0x4e, 
            GIF_A_D_REG_ZBUG_2     = 0x4f,
            GIF_A_D_REG_BITBLTBUF  = 0x50, 
            GIF_A_D_REG_TRXPOS     = 0x51, 
            GIF_A_D_REG_TRXREG     = 0x52, 
            GIF_A_D_REG_TRXDIR     = 0x53, 
            GIF_A_D_REG_HWREG      = 0x54, 
            GIF_A_D_REG_SIGNAL     = 0x60, 
            GIF_A_D_REG_FINISH     = 0x61, 
            GIF_A_D_REG_LABEL      = 0x62
        }

        public enum GIF_FLG
        {
            GIF_FLG_PACKED  = 0, 
            GIF_FLG_REGLIST = 1, 
            GIF_FLG_IMAGE   = 2, 
            GIF_FLG_IMAGE2  = 3
        }

        public enum GS_PSM
        {
            PSMCT32  =  0, 
            PSMCT24  =  1, 
            PSMCT16  =  2, 
            PSMCT16S = 10, 
            PSGPU24  = 18, 
            PSMT8    = 19, 
            PSMT4    = 20, 
            PSMT8H   = 27, 
            PSMT4HL  = 36, 
            PSMT4HH  = 44, 
            PSMZ32   = 48, 
            PSMZ24   = 49, 
            PSMZ16   = 50, 
            PSMZ16S  = 58
        }

        public enum GS_TFX
        {
            TFX_MODULATE   = 0, 
            TFX_DECAL      = 1, 
            TFX_HIGHLIGHT  = 2, 
            TFX_HIGHLIGHT2 = 3, 
            TFX_NONE       = 4
        }

        public enum GS_CLAMP
        {
            CLAMP_REPEAT        = 0, 
            CLAMP_CLAMP         = 1, 
            CLAMP_REGION_CLAMP  = 2, 
            CLAMP_REGION_REPEAT = 3
        }

        public enum GS_ZTST
        {
            ZTST_NEVER   = 0, 
            ZTST_ALWAYS  = 1,
            ZTST_GEQUAL  = 2, 
            TTST_GREATER = 3
        }

        public enum GS_ATST
        {
            ATST_NEVER    = 0, 
            ATST_ALWAYS   = 1, 
            ATST_LESS     = 2, 
            ATST_LEQUAL   = 3, 
            ATST_EQUAL    = 4, 
            ATST_GEQUAL   = 5, 
            ATST_GREATER  = 6,
            ATST_NOTEQUAL = 7
        }

        public enum GS_AFAIL
        {
            AFAIL_KEEP     = 0, 
            AFAIL_FB_ONLY  = 1, 
            AFAIL_ZB_ONLY  = 2, 
            AFAIL_RGB_ONLY = 3
        }

        public enum GS_MIN_FILTER
        {
            Nearest                = 0, 
            Linear                 = 1, 
            Nearest_Mipmap_Nearest = 2, 
            Nearest_Mipmap_Linear  = 3, 
            Linear_Mipmap_Nearest  = 4, 
            Linear_Mipmap_Linear   = 5
        }

        struct GSReg_BGCOLOR
        {
            public byte R;
            byte G;
            byte B;
            byte[] _PAD1; //5
        }

        struct GSReg_BUSDIR
        {
            public uint DIR;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GSReg_CSR
        {
            public uint rSIGNAL;
            public uint rFINISH;
            public uint rHSINT;
            public uint rVSINT;
            public uint rEDWINT;
            public uint rZERO1;
            public uint rZERO2;
            public uint r_PAD1;
            public uint rFLUSH;
            public uint rRESET;
            public uint r_PAD2;
            public uint rNFIELD;
            public uint rFIELD;
            public uint rFIFO;
            public uint rREV;
            public uint rID;
            public uint wSIGNAL;
            public uint wFINISH;
            public uint wHSINT;
            public uint wVSINT;
            public uint wEDWINT;
            public uint wZERO1;
            public uint wZERO2;
            public uint w_PAD1;
            public uint wFLUSH;
            public uint wRESET;
            public uint w_PAD2;
            public uint wNFIELD;
            public uint wFIELD;
            public uint wFIFO;
            public uint wREV;
            public uint wID;
        }

        struct GSReg_DISPFB
        {
            public uint FBP;
            public uint FBW;
            public uint PSM;
            public uint _PAD;
            public uint DBX;
            public uint DBY;
            public uint _PAD2;
            
            uint Block() { return FBP << 5; }
        }

        struct GSReg_DISPLAY
        {
            public uint DX;
            public uint DY;
            public uint MAGH;
            public uint MAGV;
            public uint _PAD;
            public uint DW;
            public uint DH;
            public uint _PAD2;
        }

        struct GSReg_EXTBUF
        {
            public uint EXBP;
            public uint EXBW;
            public uint FBIN;
            public uint WFFMD;
            public uint EMODA;
            public uint EMODC;
            public uint _PAD1;
            public uint WDX;
            public uint WDY;
            public uint _PAD2;
        }

        struct GSReg_EXTDATA
        {
            public uint SX;
            public uint SY;
            public uint SMPH;
            public uint SMPV;
            public uint _PAD1;
            public uint WW;
            public uint WH;
            public uint _PAD2;
        }

        struct GSReg_EXTWRITE
        {
            public uint WRITE;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GSReg_IMR
        {
            public uint _PAD1;
            public uint SIGMSK;
            public uint FINISHMSK;
            public uint HSMSK;
            public uint VSMSK;
            public uint EDWMSK;
            public uint _PAD2;
            public uint _PAD3;
        }

        struct GSReg_PMODE
        {
            public uint EN1;
            public uint EN2;
            public uint CRTMD;
            public uint MMOD;
            public uint AMOD;
            public uint SLBG;
            public uint ALP;
            public uint _PAD;
            public uint _PAD1;
            public uint EN;
            public uint _PAD2;
            public uint _PAD3;
        }

        struct GSReg_SIGLBLID
        {
            public uint SIGID;
            public uint LBLID;
        }

        struct GSReg_SMODE1
        {
            public uint RC;
            public uint LC;
            public uint T1248;
            public uint SLCK;
            public uint CMOD;
            public uint EX;
            public uint PRST;
            public uint SINT;
            public uint XPCK;
            public uint PCK2;
            public uint SPML;
            public uint GCONT;
            public uint PHS;
            public uint PVS;
            public uint PEHS;
            public uint PEVS;
            public uint CLKSEL;
            public uint NVCK;
            public uint SLCK2;
            public uint VCKSEL;
            public uint VHP;
            public uint _PAD1;
        }
        
        struct GSReg_SMODE2
        {
            public uint INT;
            public uint FFMD;
            public uint DPMS;
            public uint _PAD2;
            public uint _PAD3;
        }

        struct GSReg_SRFSH
        {
            public uint _DUMMY;
        }

        struct GSReg_SYNCH1
        {
            public uint _DUMMY;
        }

        struct GSReg_SYNCH2
        {
            public uint _DUMMY;
        }

        struct GSReg_SYNCV
        {
            public uint VFP;
            public uint VFPE;
            public uint VBP;
            public uint VBPE;
            public uint VDP;
            public uint VS;
        }

        struct GIFTag
        {
            public uint NLOOP;
            public uint EOP;
            public uint _PAD1;
            public uint _PAD2;
            public uint PRE;
            public uint PRIM;
            public uint FLG;
            public uint NREG;
            public ulong REGS;
        }

        struct GIFReg_ALPHA
        {
            public uint A;
            public uint B;
            public uint C;
            public uint D;
            public uint _PAD1;
            public byte FIX;
            public byte[] _PAD2;

            bool IsOpaque() { return ((A == B || (C == 2 && FIX == 0)) && D == 0) || (A == 0 && B == D && C == 2 && FIX == 0x80); }
            bool IsOpaque(int amin, int amax) { return ((A == B || amax == 0) && D == 0) || (A == 0 && B == D && amin == 0x80 && amax == 0x80); }
            bool IsCd() { return (A == B) && (D == 1); }

            bool IsCdOutput() { return (C == 2 && D != 1 && FIX == 0x00); }
            bool IsCdInBlend() { return (A == 1 || B == 1 || D == 1); }
            bool IsUsingCs() { return (A == 0 || B == 0 || D == 0); }
            bool IsUsingAs() { return (A != B && C == 0); }

            bool IsBlack() { return ((C == 2 && FIX == 0) || (A == 2 && A == B)) && D == 2; }
        }

        struct GIFReg_BITBLTBUF
        {
            public uint SBP;
            public uint _PAD1;
            public uint SBW;
            public uint _PAD2;
            public uint SPSM;
            public uint _PAD3;
            public uint DBP;
            public uint _PAD4;
            public uint DBW;
            public uint _PAD5;
            public uint DPSM;
            public uint _PAD6;
        }

        struct GIFReg_CLAMP
        {
            public uint WMS;
            public uint WMT;
            public uint MINU;
            public uint MAXU;
            public uint _PAD1;
            public uint _PAD2;
            public uint MAXV;
            public uint _PAD3;
            public ulong _PAD4;
            public ulong MINV;
            public ulong _PAD5;
        }

        struct GIFReg_COLCLAMP
        {
            public uint CLAMP;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_DIMX
        {
            public int DM00;
            public int _PAD00;
            public int DM01;
            public int _PAD01;
            public int DM02;
            public int _PAD02;
            public int DM03;
            public int _PAD03;
            public int DM10;
            public int _PAD10;
            public int DM11;
            public int _PAD11;
            public int DM12;
            public int _PAD12;
            public int DM13;
            public int _PAD13;
            public int DM20;
            public int _PAD20;
            public int DM21;
            public int _PAD21;
            public int DM22;
            public int _PAD22;
            public int DM23;
            public int _PAD23;
            public int DM30;
            public int _PAD30;
            public int DM31;
            public int _PAD31;
            public int DM32;
            public int _PAD32;
            public int DM33;
            public int _PAD33;
        }

        struct GIFReg_DTHE
        {
            public uint DTHE;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_FBA
        {
            public uint FBA;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_FINISH
        {
            public uint[] _PAD1;
        }

        struct GIFReg_FOG
        {
            public byte[] _PAD1;
            public byte F;
        }

        struct GIFReg_FOGCOL
        {
            public byte FCR;
            public byte FCG;
            public byte FCB;
            public byte[] _PAD1;
        }

        struct GIFReg_FRAME
        {
            public uint FBP;
            public uint _PAD1;
            public uint FBW;
            public uint _PAD2;
            public uint PSM;
            public uint _PAD3;
            public uint FBMSK;

            uint Block() { return FBP << 5; }
        }

        struct GIFReg_HWREG
        {
            public uint DATA_LOWER;
            public uint DATA_UPPER;
        }

        struct GIFReg_LABEL
        {
            public uint ID;
            public uint IDMSK;
        }

        struct GIFReg_MIPTBP1
        {
            public ulong TBP1;
            public ulong TBW1;
            public ulong TBP2;
            public ulong TBW2;
            public ulong TBP3;
            public ulong TBW3;
            public ulong _PAD;
        }

        struct GIFReg_MIPTBP2
        {
            public ulong TBP4;
            public ulong TBW4;
            public ulong TBP5;
            public ulong TBW5;
            public ulong TBP6;
            public ulong TBW6;
            public ulong _PAD;
        }

        struct GIFReg_NOP
        {
            public uint[] _PAD;
        }

        struct GIFReg_PABE
        {
            public uint PABE;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_PRIM
        {
            public uint PRIM;
            public uint IIP;
            public uint TME;
            public uint FGE;
            public uint ABE;
            public uint AA1;
            public uint FST;
            public uint CTXT;
            public uint FIX;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_PRMODE
        {
            public uint _PRIM;
            public uint IIP;
            public uint TME;
            public uint FGE;
            public uint ABE;
            public uint AA1;
            public uint FST;
            public uint CTXT;
            public uint FIX;
            public uint _PAD2;
            public uint _PAD3;
        }

        struct GIFReg_PRMODECONT
        {
            public uint AC;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_RGBAQ
        {
            public byte R;
            public byte G;
            public byte B;
            public byte A;
            public float Q;
        }

        struct GIFReg_SCANMSK
        {
            public uint MSK;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_SCISSOR
        {
            public uint SCAX0;
            public uint _PAD1;
            public uint SCAX1;
            public uint _PAD2;
            public uint SCAY0;
            public uint _PAD3;
            public uint SCAY1;
            public uint _PAD4;
        }

        struct GIFReg_SIGNAL
        {
            public uint ID;
            public uint IDMSK;
        }

        struct GIFReg_ST
        {
            public float S;
            public float T;
        }

        struct GIFReg_TEST
        {
            public uint ATE;
            public uint ATST;
            public uint AREF;
            public uint AFAIL;
            public uint DATE;
            public uint DATM;
            public uint ZTE;
            public uint ZTST;
            public uint _PAD1;
            public uint _PAD2;

            bool DoFirstPass() { return ATE == 0 || ATST != (uint)GS_ATST.ATST_NEVER; }
            bool DoSecondPass() { return ATE != 0 && ATST != (uint)GS_ATST.ATST_ALWAYS && AFAIL != (uint)GS_AFAIL.AFAIL_KEEP; }
            bool NoSecondPass() { return ATE != 0 && ATST != (uint)GS_ATST.ATST_ALWAYS && AFAIL == (uint)GS_AFAIL.AFAIL_KEEP; }
            uint GetAFAIL(uint fpsm) { return (AFAIL == (uint)GS_AFAIL.AFAIL_RGB_ONLY && (fpsm & 0xF) != 0) ? (uint)GS_AFAIL.AFAIL_FB_ONLY : AFAIL; }
        }

        struct GIFReg_TEX0
        {
            public uint TBP0;
            public uint TBW;
            public uint PSM;
            public uint TW;
            public uint _PAD1;
            public uint _PAD2;
            public uint TCC;
            public uint TFX;
            public uint CBP;
            public uint CPSM;
            public uint CSM;
            public uint CSA;
            public uint CLD;
            public ulong _PAD3;
            public ulong TH;
            public ulong _PAD4;

            bool IsRepeating()
            {
                if (TBW < 2)
                {
                    if (PSM == (uint)GS_PSM.PSMT8)
                        return TW > 7 || TH > 6;
                    if (PSM == (uint)GS_PSM.PSMT4)
                        return TW > 7 || TH > 7;
                }

                return (TBW << 6) < (1U << (int)TW);
            }

            static GIFReg_TEX0 Create(uint bp, uint bw, uint psm)
            {
                GIFReg_TEX0 ret = new GIFReg_TEX0
                {
                    TBP0  =  bp,
                    TBW   =  bw,
                    PSM   = psm,
                    TW    =   4,
                    _PAD1 =   2,
                    _PAD2 =   2,
                    TCC   =   1,
                    TFX   =   2,
                    CBP   =  14,
                    CPSM  =   4,
                    CSM   =   1,
                    CSA   =   5,
                    CLD   =   3,
                    _PAD3 =  30,
                    TH    =   4,
                    _PAD4 =  30
                };
                return ret;
            }
        }

        struct GIFReg_TEX1
        {
            public uint LCM;
            public uint _PAD1;
            public uint MXL;
            public uint MMAG;
            public uint MMIN;
            public uint MTBA;
            public uint _PAD2;
            public uint L;
            public uint _PAD3;
            public int K;
            public uint _PAD4;

            bool IsMinLinear() { return (MMIN == 1) || ((MMIN & 4) != 0); }
            bool IsMagLinear() { return MMAG != 0; }
        }

        struct GIFReg_TEX2
        {
            public uint _PAD1;
            public uint PSM;
            public uint _PAD2;
            public uint _PAD3;
            public uint CBP;
            public uint CPSM;
            public uint CSM;
            public uint CSA;
            public uint CLD;
        }

        struct GIFReg_TEXA
        {
            public byte TA0;
            public byte _PAD1;
            public byte AEM;
            public ushort _PAD2;
            public byte TA1;
            public byte[] _PAD3;
        }

        struct GIFReg_TEXCLUT
        {
            public uint CBW;
            public uint COU;
            public uint COV;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_TEXFLUSH
        {
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_TRXDIR
        {
            public uint XDIR;
            public uint _PAD1;
            public uint _PAD2;
        }

        struct GIFReg_TRXPOS
        {
            public uint SSAX;
            public uint _PAD1;
            public uint SSAY;
            public uint _PAD2;
            public uint DSAX;
            public uint _PAD3;
            public uint DSAY;
            public uint DIRY;
            public uint DIRX;
            public uint _PAD4;
        }

        struct GIFReg_TRXREG
        {
            public uint RRW;
            public uint _PAD1;
            public uint RRH;
            public uint _PAD2;
        }

        struct GIFReg_UV
        {
            public ushort U;
            public ushort V;
            public uint _PAD3;
        }

        struct GIFReg_XYOFFSET
        {
            public uint OFX;
            public uint OFY;
        }

        struct GIFReg_XYZ
        {
            public ushort X;
            public ushort Y;
            public uint Z;
        }

        struct GIFReg_XYZF
        {
            public ushort X;
            public ushort Y;
            public uint Z;
            public uint F;
        }

        struct GIFReg_ZBUF
        {
            public uint ZBP;
            public uint _PAD1;
            public uint PSM;
            public uint _PAD2;
            public uint ZMSK;
            public uint _PAD3;

            uint Block() { return ZBP << 5; }
        }



        static GSReg_BUSDIR BUSDIR = new GSReg_BUSDIR
        {
            DIR   =  1,
            _PAD1 = 31,
            _PAD2 = 32
        };

        static GSReg_CSR CSR = new GSReg_CSR
        {
            rSIGNAL = 1,
            rFINISH = 1,
            rHSINT  = 1,
            rVSINT  = 1, 
            rEDWINT = 1, 
            rZERO1  = 1, 
            rZERO2  = 1, 
            r_PAD1  = 1, 
            rFLUSH  = 1, 
            rRESET  = 1, 
            r_PAD2  = 2, 
            rNFIELD = 1, 
            rFIELD  = 1, 
            rFIFO   = 2, 
            rREV    = 8, 
            rID     = 8, 
            wSIGNAL = 1, 
            wFINISH = 1, 
            wHSINT  = 1, 
            wVSINT  = 1, 
            wEDWINT = 1, 
            wZERO1  = 1, 
            wZERO2  = 1, 
            w_PAD1  = 1, 
            wFLUSH  = 1, 
            wRESET  = 1, 
            w_PAD2  = 2, 
            wNFIELD = 1, 
            wFIELD  = 1, 
            wFIFO   = 2, 
            wREV    = 8, 
            wID     = 8
        };

        static GSReg_DISPFB DISPFB = new GSReg_DISPFB
        {
            FBP   =  9,
            FBW   =  6,
            PSM   =  5,
            _PAD  = 12,
            DBX   = 11,
            DBY   = 11,
            _PAD2 = 10
        };

        static GSReg_DISPLAY DISPLAY = new GSReg_DISPLAY
        {
            DX    = 12,
            DY    = 11,
            MAGH  =  4,
            MAGV  =  2,
            _PAD  =  3,
            DW    = 12,
            DH    = 11,
            _PAD2 =  9
        };

        static GSReg_EXTBUF EXTBUF = new GSReg_EXTBUF
        {
            EXBP  = 14,
            EXBW  =  6,
            FBIN  =  2,
            WFFMD =  1,
            EMODA =  2,
            EMODC =  2,
            _PAD1 =  5,
            WDX   = 11,
            WDY   = 11,
            _PAD2 = 10
        };

        static GSReg_EXTDATA EXTDATA = new GSReg_EXTDATA
        {
            SX    = 12,
            SY    = 11,
            SMPH  =  4,
            SMPV  =  2,
            _PAD1 =  3,
            WW    = 12,
            WH    = 11,
            _PAD2 =  9
        };

        static GSReg_EXTWRITE EXTWRITE = new GSReg_EXTWRITE
        {
            WRITE =  1,
            _PAD1 = 31,
            _PAD2 = 32
        };

        static GSReg_IMR IMP = new GSReg_IMR
        {
            _PAD1     =  8, 
            SIGMSK    =  1, 
            FINISHMSK =  1, 
            HSMSK     =  1, 
            VSMSK     =  1, 
            EDWMSK    =  1, 
            _PAD2     = 19, 
            _PAD3     = 32
        };

        static GSReg_PMODE PMODE = new GSReg_PMODE
        {
            EN1   =  1, 
            EN2   =  1, 
            CRTMD =  3, 
            MMOD  =  1, 
            AMOD  =  1, 
            SLBG  =  1, 
            ALP   =  8, 
            _PAD  = 16, 
            _PAD1 = 32, 
            EN    =  2, 
            _PAD2 = 30, 
            _PAD3 = 32
        };

        static GSReg_SIGLBLID SIGLBLID;

        static GSReg_SMODE1 SMODE1 = new GSReg_SMODE1
        {
            RC     =  3, 
            LC     =  7, 
            T1248  =  2, 
            SLCK   =  1, 
            CMOD   =  2, 
            EX     =  1, 
            PRST   =  1, 
            SINT   =  1, 
            XPCK   =  1, 
            PCK2   =  2, 
            SPML   =  4, 
            GCONT  =  1, 
            PHS    =  1, 
            PVS    =  1, 
            PEHS   =  1, 
            PEVS   =  1, 
            CLKSEL =  2, 
            NVCK   =  1, 
            SLCK2  =  1, 
            VCKSEL =  2, 
            VHP    =  1, 
            _PAD1  = 27
        };

        static GSReg_SMODE2 SMODE2 = new GSReg_SMODE2
        {
            INT   =  1, 
            FFMD  =  1, 
            DPMS  =  2, 
            _PAD2 = 28, 
            _PAD3 = 32
        };

        static GSReg_SRFSH SRFSH;

        static GSReg_SYNCH1 SYNCH1;

        static GSReg_SYNCH2 SYNCH2;

        static GSReg_SYNCV SYNCV = new GSReg_SYNCV
        {
            VFP  = 10, 
            VFPE = 10, 
            VBP  = 12, 
            VBPE = 10, 
            VDP  = 11, 
            VS   = 11
        };

        static GIFTag GIFTAG = new GIFTag
        {
            NLOOP = 15,
            EOP   =  1,
            _PAD1 = 16,
            _PAD2 = 14,
            PRE   =  1,
            PRIM  = 11,
            FLG   =  2,
            NREG  =  4,
            REGS  =  0
        };

        static GIFReg_ALPHA ALPHA = new GIFReg_ALPHA
        {
            A     =  2,
            B     =  2,
            C     =  2,
            D     =  2,
            _PAD1 = 24,
            FIX   =  0,
            _PAD2 = new byte[3]
        };

        static GIFReg_BITBLTBUF BITBLTBUF = new GIFReg_BITBLTBUF
        {
            SBP   = 14,
            _PAD1 =  2,
            SBW   =  6,
            _PAD2 =  2,
            SPSM  =  6,
            _PAD3 =  2,
            DBP   = 14,
            _PAD4 =  2,
            DBW   =  6,
            _PAD5 =  2,
            DPSM  =  6,
            _PAD6 =  2
        };

        static GIFReg_CLAMP CLAMP = new GIFReg_CLAMP
        {
            WMS   =  2,
            WMT   =  2,
            MINU  = 10,
            MAXU  = 10,
            _PAD1 =  8,
            _PAD2 =  2,
            MAXV  = 10,
            _PAD3 = 20,
            _PAD4 = 24,
            MINV  = 10,
            _PAD5 = 30
        };

        static GIFReg_COLCLAMP COLCLAMP = new GIFReg_COLCLAMP
        {
            CLAMP =  1,
            _PAD1 = 31,
            _PAD2 = 32
        };

        static GIFReg_DIMX DIMX = new GIFReg_DIMX
        {
            DM00   = 3,
            _PAD00 = 1,
            DM01   = 3,
            _PAD01 = 1,
            DM02   = 3,
            _PAD02 = 1,
            DM03   = 3,
            _PAD03 = 1,
            DM10   = 3,
            _PAD10 = 1,
            DM11   = 3,
            _PAD11 = 1,
            DM12   = 3,
            _PAD12 = 1,
            DM13   = 3,
            _PAD13 = 1,
            DM20   = 3,
            _PAD20 = 1,
            DM21   = 3,
            _PAD21 = 1,
            DM22   = 3,
            _PAD22 = 1,
            DM23   = 3,
            _PAD23 = 1,
            DM30   = 3,
            _PAD30 = 1,
            DM31   = 3,
            _PAD31 = 1,
            DM32   = 3,
            _PAD32 = 1,
            DM33   = 3,
            _PAD33 = 1
        };

        static GIFReg_DTHE DTHE = new GIFReg_DTHE
        {
            DTHE  =  1, 
            _PAD1 = 31, 
            _PAD2 = 32
        };

        static GIFReg_FBA FBA = new GIFReg_FBA
        {
            FBA   =  1,
            _PAD1 = 31,
            _PAD2 = 32
        };

        static GIFReg_FINISH FINISH = new GIFReg_FINISH
        {
            _PAD1 = new uint[2]
        };

        static GIFReg_FOG FOG = new GIFReg_FOG
        {
            _PAD1 = new byte[7],
            F     = 0
        };

        static GIFReg_FOGCOL FOGCOL = new GIFReg_FOGCOL
        {
            FCR   = 0,
            FCG   = 0,
            FCB   = 0,
            _PAD1 = new byte[5]
        };

        static GIFReg_FRAME FRAME = new GIFReg_FRAME
        {
            FBP   = 9,
            _PAD1 = 7,
            FBW   = 6,
            _PAD2 = 2,
            PSM   = 6,
            _PAD3 = 2,
            FBMSK = 0
        };

        static GIFReg_HWREG HWREG;

        static GIFReg_LABEL LABEL;

        static GIFReg_MIPTBP1 MIPTBP1 = new GIFReg_MIPTBP1
        {
            TBP1 = 14,
            TBW1 =  6,
            TBP2 = 14,
            TBW2 =  6,
            TBP3 = 14,
            TBW3 =  6,
            _PAD =  4
        };

        static GIFReg_MIPTBP2 MIPTBP2 = new GIFReg_MIPTBP2
        {
            TBP4 = 14,
            TBW4 =  6,
            TBP5 = 14,
            TBW5 =  6,
            TBP6 = 14,
            TBW6 =  6,
            _PAD =  4
        };

        static GIFReg_NOP NOP = new GIFReg_NOP
        {
            _PAD = new uint[2]
        };

        static GIFReg_PABE PABE = new GIFReg_PABE
        {
            PABE  =  1,
            _PAD1 = 31,
            _PAD2 = 32
        };

        static GIFReg_PRIM PRIM = new GIFReg_PRIM
        {
            PRIM  =  3,
            IIP   =  1,
            TME   =  1,
            FGE   =  1,
            ABE   =  1,
            AA1   =  1,
            FST   =  1,
            CTXT  =  1,
            FIX   =  1,
            _PAD1 = 21,
            _PAD2 = 32
        };

        static GIFReg_PRMODE PRMODE = new GIFReg_PRMODE
        {
            _PRIM =  3,
            IIP   =  1,
            TME   =  1,
            FGE   =  1,
            ABE   =  1,
            AA1   =  1,
            FST   =  1,
            CTXT  =  1,
            FIX   =  1,
            _PAD2 = 21,
            _PAD3 = 32
        };

        static GIFReg_PRMODECONT PRMODECONT = new GIFReg_PRMODECONT
        {
            AC    =  1, 
            _PAD1 = 31, 
            _PAD2 = 32
        };

        static GIFReg_RGBAQ RGBAQ;

        static GIFReg_SCANMSK SCANMSK = new GIFReg_SCANMSK
        {
            MSK   =  2,
            _PAD1 = 30,
            _PAD2 = 32
        };

        static GIFReg_SCISSOR SCISSOR = new GIFReg_SCISSOR
        {
            SCAX0 = 11, 
            _PAD1 =  5, 
            SCAX1 = 11, 
            _PAD2 =  5, 
            SCAY0 = 11, 
            _PAD3 =  5, 
            SCAY1 = 11, 
            _PAD4 =  5
        };

        static GIFReg_SIGNAL SIGNAL;

        static GIFReg_ST ST;

        static GIFReg_TEST TEST = new GIFReg_TEST
        {
            ATE   =  1,
            ATST  =  3,
            AREF  =  8,
            AFAIL =  2,
            DATE  =  1,
            DATM  =  1,
            ZTE   =  1,
            ZTST  =  2,
            _PAD1 = 13,
            _PAD2 = 32
        };

        static GIFReg_TEX0 TEX0 = new GIFReg_TEX0
        {
            TBP0  = 14,
            TBW   =  6,
            PSM   =  6,
            TW    =  4,
            _PAD1 =  2,
            _PAD2 =  2,
            TCC   =  1,
            TFX   =  2,
            CBP   = 14,
            CPSM  =  4,
            CSM   =  1,
            CSA   =  5,
            CLD   =  3,
            _PAD3 = 30,
            TH    =  4,
            _PAD4 = 30
        };

        static GIFReg_TEX1 TEX1 = new GIFReg_TEX1
        {
            LCM   =  1,
            _PAD1 =  1,
            MXL   =  3,
            MMAG  =  1,
            MMIN  =  3,
            MTBA  =  1,
            _PAD2 =  9,
            L     =  2,
            _PAD3 = 11,
            K     = 12,
            _PAD4 = 20
        };

        static GIFReg_TEX2 TEX2 = new GIFReg_TEX2
        {
            _PAD1 = 20,
            PSM   =  6,
            _PAD2 =  6,
            _PAD3 =  5,
            CBP   = 14,
            CPSM  =  4,
            CSM   =  1,
            CSA   =  5,
            CLD   =  3
        };

        static GIFReg_TEXA TEXA = new GIFReg_TEXA
        {
            TA0   = 0,
            _PAD1 = 7,
            AEM   = 1,
            _PAD2 = 0,
            TA1   = 8,
            _PAD3 = new byte[3]
        };

        static GIFReg_TEXCLUT TEXCLUT = new GIFReg_TEXCLUT
        {
            CBW   =  6,
            COU   =  6,
            COV   = 10,
            _PAD1 = 10,
            _PAD2 = 32
        };

        static GIFReg_TEXFLUSH TEXFLUSH = new GIFReg_TEXFLUSH
        {
            _PAD1 = 32,
            _PAD2 = 32
        };

        static GIFReg_TRXDIR TRXDIR = new GIFReg_TRXDIR
        {
            XDIR  =  2,
            _PAD1 = 30,
            _PAD2 = 32
        };

        static GIFReg_TRXPOS TRXPOS = new GIFReg_TRXPOS
        {
            SSAX  = 11,
            _PAD1 =  5,
            SSAY  = 11,
            _PAD2 =  5,
            DSAX  = 11,
            _PAD3 =  5,
            DSAY  = 11,
            DIRX  =  1,
            DIRY  =  1,
            _PAD4 =  3
        };

        static GIFReg_TRXREG TRXREG = new GIFReg_TRXREG
        {
            RRW   = 12,
            _PAD1 = 20,
            RRH   = 12,
            _PAD2 = 20
        };

        static GIFReg_UV UV;

        static GIFReg_XYOFFSET XYOFFSET;

        static GIFReg_XYZ XYZ;

        static GIFReg_XYZF XYZF = new GIFReg_XYZF
        {
            X =  0,
            Y =  0,
            Z = 24,
            F =  8
        };

        static GIFReg_ZBUF ZBUF = new GIFReg_ZBUF
        {
            ZBP   =  9,
            _PAD1 = 15,
            PSM   =  6,
            _PAD2 =  2,
            ZMSK  =  1,
            _PAD3 = 31
        };
    }
}
