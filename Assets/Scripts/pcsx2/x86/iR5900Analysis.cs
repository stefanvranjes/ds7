using PCSX2;
using static PCSX2.VU;

namespace PCSX2
{
    static class R5900
    {
        public struct GPR_reg
        {
            public uint UL_0; //0x00
            public uint UL_1; //0x04
            public uint UL_2; //0x08
            public uint UL_3; //0x0C
        }

        public struct GPRregs
        {
            public GPR_reg[] r; //r0, at, v0, v1, a0, a1, a2, a3,
                                //t0, t1, t2, t3, t4, t5, t6, t7,
                                //s0, s1, s2, s3, s4, s5, s6, s7,
                                //t8, t9, k0, k1, gp, sp, s8, ra;
        }

        public class cpuRegisters
        {
            public GPRregs GPR;
            GPR_reg HI;
            GPR_reg LO;
            uint sa;
            uint isDelaySlot;
            uint pc;
            public uint code;
        }

        public static cpuRegisters cpuRegs;

        public static uint _Rd_ => (cpuRegs.code >> 11) & 0x1F;
        public static uint _Rt_ => (cpuRegs.code >> 16) & 0x1F;
        public static uint _Rs_ => (cpuRegs.code >> 21) & 0x1F;

        public static void MMI(uint code)
        {
            uint funct = code & 0x3F;
            uint rs = ((code >> 21) & 0x1F);
            uint rt = ((code >> 16) & 0x1F);
            uint rd = ((code >> 11) & 0x1F);

            switch (funct)
            {
                case 0: // madd
                case 1: // maddu
                    break;

                case 32: // madd1
                case 33: // maddu1
                    break;

                case 24: // mult1
                case 25: // multu1
                    break;

                case 26: // div1
                case 27: // divu1
                    break;

                case 16: // mfh1
                    break;

                case 17: // mth1
                    break;

                case 18: //mflo1
                    break;

                case 19: //mtlo1
                    break;

                case 4: // plzcw
                    break;

                case 48: // pmfhl
                    break;

                case 49: // pmthl
                    break;

                case 52: // psllh
                case 54: // psrlh
                case 55: // psrah
                case 60: // psllw
                case 62: // psrlw
                case 63: // psraw
                    break;

                case 8: // mmi0
                    uint idx = (code >> 6) & 0x1F;

                    switch (idx)
                    {
                        case 0: // PADDW
                        case 1: // PSUBW
                        case 2: // PCGTW
                        case 3: // PMAXW
                        case 4: // PADDH
                        case 5: // PSUBH
                        case 6: // PCGTH
                        case 7: // PMAXH
                        case 8: // PADDB
                        case 9: // PSUBB
                        case 10: // PCGTB
                        case 16: // PADDSW
                        case 17: // PSUBSW
                        case 18: // PEXTLW
                            PCSX2.MMI.PEXTLW();
                            break;
                        case 19: // PPACW
                        case 20: // PADDSH
                        case 21: // PSUBSH
                        case 22: // PEXTLH
                        case 23: // PPACH
                        case 24: // PADDSB
                        case 25: // PSUBSB
                        case 26: // PEXTLB
                        case 27: // PPACB
                            break;

                        case 30: // PEXT5
                        case 31: // PPAC5
                            break;

                        default:
                            UnityEngine.Debug.LogWarning("Unknown R5900 MMI0: " + code.ToString("X"));
                            break;
                    }
                    break;

                case 40: // mmi1
                    idx = (code >> 6) & 0x1F;

                    switch (idx)
                    {
                        case 2: // PCEQW
                        case 3: // PMINW
                        case 4: // PADSBH
                        case 6: // PCEQH
                        case 7: // PMINH
                        case 10: // PCEQB
                        case 16: // PADDUW
                        case 17: // PSUBUW
                        case 18: // PEXTUW
                            PCSX2.MMI.PEXTUW();
                            break;
                        case 20: // PADDUH
                        case 21: // PSUBUH
                        case 22: // PEXTUH
                        case 24: // PADDUB
                        case 25: // PSUBUB
                        case 26: // PEXTUB
                        case 27: // QFSRV
                            break;

                        case 1: // PABSW
                        case 5: // PABSH
                            break;

                        case 0: // MMI_Unknown
                        default:
                            UnityEngine.Debug.LogWarning("Unknown R5900 MMI1: " + code.ToString("X"));
                            break;
                    }
                    break;

                case 9: // mmi2
                    idx = (code >> 6) & 0x1F;

                    switch (idx)
                    {
                        case 0: // PMADDW
                        case 4: // PMSUBW
                        case 16: // PMADDH
                        case 17: // PHMADH
                        case 20: // PMSUBH
                        case 21: // PHMSBH
                            break;

                        case 12: // PMULTW
                        case 28: // PMULTH
                            break;

                        case 13: // PDIVW
                        case 29: // PDIVBW
                            break;

                        case 2: // PSLLVW
                        case 3: // PSRLVW
                        case 10: // PINTH
                        case 14: // PCPYLD
                            PCSX2.MMI.PCPYLD();
                            break;
                        case 18: // PAND
                        case 19: // PXOR
                            break;

                        case 8: // PMFHI
                            break;

                        case 9: // PMFLO
                            break;

                        case 26: // PEXEH
                        case 27: // PREVH
                        case 30: // PEXEW
                        case 31: // PROT3W
                            break;

                        default:
                            UnityEngine.Debug.LogWarning("Unknown R5900 MMI2: " + code.ToString("X"));
                            break;
                    }
                    break;

                case 41: // mmi3
                    idx = (code >> 6) & 0x1F;

                    switch (idx)
                    {
                        case 0: // PMADDUW
                            break;

                        case 3: // PSRAVW
                        case 10: // PINTEH
                        case 18: // POR
                        case 19: // PNOR
                        case 14: // PCPYUD
                            PCSX2.MMI.PCPYUD();
                            break;

                        case 26: // PEXCH
                        case 27: // PCPYH
                        case 30: // PEXCW
                            break;

                        case 8: // PMTHI
                            break;

                        case 9: // PMTLO
                            break;

                        case 12: // PMULTUW
                            break;

                        case 13: // PDIVUW
                            break;

                        default:
                            UnityEngine.Debug.LogWarning("Unknown R5900 MMI3: " + code.ToString("X"));
                            break;
                    }
                    break;

                default:
                    UnityEngine.Debug.LogWarning("Unknown R5900 MMI: " + code.ToString("X"));
                    break;
            }
        }

        public static void COP2(uint code)
        {
            uint fmt = ((code >> 21) & 0x1F);
            uint rt = ((code >> 16) & 0x1F);
            uint funct = (code & 0x3F);
            VUops.VU.code = code;

            switch (fmt)
            {
                case 1: //qmfc2
                    VU0.QMFC2();
                    break;

                case 2: //cfc1
                    break;

                case 5: //qmtc2
                    VU0.QMTC2();
                    break;

                case 6: //ctc2
                    break;

                case 8: // bc2f/bc2t/bc2fl/bc2tl
                    break;

                case 16: // SPEC1
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31: // SPEC1
                    switch (funct)
                    {
                        case 0: // VADDx
                            VUops._vuADDx(VUops.VU);
                            break;
                        case 1: // VADDy
                            VUops._vuADDy(VUops.VU);
                            break;
                        case 2: // VADDz
                            VUops._vuADDz(VUops.VU);
                            break;
                        case 3: // VADDw
                            VUops._vuADDw(VUops.VU);
                            break;
                        case 4: // VSUBx
                            VUops._vuSUBx(VUops.VU);
                            break;
                        case 5: // VSUBy
                            VUops._vuSUBy(VUops.VU);
                            break;
                        case 6: // VSUBz
                            VUops._vuSUBz(VUops.VU);
                            break;
                        case 7: // VSUBw
                            VUops._vuSUBw(VUops.VU);
                            break;
                        case 16: // VMAXx
                            VUops._vuMAXx(VUops.VU);
                            break;
                        case 17: // VMAXy
                            VUops._vuMAXy(VUops.VU);
                            break;
                        case 18: // VMAXz
                            VUops._vuMAXz(VUops.VU);
                            break;
                        case 19: // VMAXw
                            VUops._vuMAXw(VUops.VU);
                            break;
                        case 20: // VMINIx
                            VUops._vuMINIx(VUops.VU);
                            break;
                        case 21: // VMINIy
                            VUops._vuMINIy(VUops.VU);
                            break;
                        case 22: // VMINIz
                            VUops._vuMINIz(VUops.VU);
                            break;
                        case 23: // VMINIw
                            VUops._vuMINIw(VUops.VU);
                            break;
                        case 24: // VMULx
                            VUops._vuMULx(VUops.VU);
                            break;
                        case 25: // VMULy
                            VUops._vuMULy(VUops.VU);
                            break;
                        case 26: // VMULz
                            VUops._vuMULz(VUops.VU);
                            break;
                        case 27: // VMULw
                            VUops._vuMULw(VUops.VU);
                            break;
                        case 40: // VADD
                            VUops._vuADD(VUops.VU);
                            break;
                        case 42: // VMUL
                            VUops._vuMUL(VUops.VU);
                            break;
                        case 43: // VMAX
                            VUops._vuMAX(VUops.VU);
                            break;
                        case 44: // VSUB
                            VUops._vuSUB(VUops.VU);
                            break;
                        case 47: // VMINI
                            VUops._vuMINI(VUops.VU);
                            break;

                        case 8: // VMADDx
                            VUops._vuMADDx(VUops.VU);
                            break;
                        case 9: // VMADDy
                            VUops._vuMADDy(VUops.VU);
                            break;
                        case 10: // VMADDz
                            VUops._vuMADDz(VUops.VU);
                            break;
                        case 11: // VMADDw
                            VUops._vuMADDw(VUops.VU);
                            break;
                        case 12: // VMSUBx
                            VUops._vuMSUBx(VUops.VU);
                            break;
                        case 13: // VMSUBy
                            VUops._vuMSUBy(VUops.VU);
                            break;
                        case 14: // VMSUBz
                            VUops._vuMSUBz(VUops.VU);
                            break;
                        case 15: // VMSUBw
                            VUops._vuMSUBw(VUops.VU);
                            break;
                        case 41: // VMADD
                            VUops._vuMADD(VUops.VU);
                            break;
                        case 45: // VMSUB
                            VUops._vuMSUB(VUops.VU);
                            break;
                        case 46: // VOPMSUB
                            VUops._vuOPMSUB(VUops.VU);
                            break;

                        case 29: // VMAXi
                            VUops._vuMAXi(VUops.VU);
                            break;
                        case 30: // VMULi
                            VUops._vuMULi(VUops.VU);
                            break;
                        case 31: // VMINIi
                            VUops._vuMINIi(VUops.VU);
                            break;
                        case 34: // VADDi
                            VUops._vuADDi(VUops.VU);
                            break;
                        case 38: // VSUBi
                            VUops._vuSUBi(VUops.VU);
                            break;

                        case 35: // VMADDi
                            VUops._vuMADDi(VUops.VU);
                            break;
                        case 39: // VMSUBi
                            VUops._vuMSUBi(VUops.VU);
                            break;

                        case 28: // VMULq
                            VUops._vuMULq(VUops.VU);
                            break;
                        case 32: // VADDq
                            VUops._vuADDq(VUops.VU);
                            break;
                        case 36: // VSUBq
                            VUops._vuSUBq(VUops.VU);
                            break;

                        case 33: // VMADDq
                            VUops._vuMADDq(VUops.VU);
                            break;
                        case 37: // VMSUBq
                            VUops._vuMSUBq(VUops.VU);
                            break;

                        case 48: // VIADD
                        case 49: // VISUB
                        case 50: // VIADDI
                        case 52: // VIAND
                        case 53: // VIOR
                            break;

                        case 56: // VCALLMS
                        case 57: // VCALLMSR
                            break;

                        case 60: // COP2_SPEC2
                        case 61: // COP2_SPEC2
                        case 62: // COP2_SPEC2
                        case 63: // COP2_SPEC2
                            uint idx = (code & 3u) | ((code >> 4) & 0x7cu);

                            switch (idx)
                            {
                                case 0: // VADDAx
                                    VUops._vuADDAx(VUops.VU);
                                    break;
                                case 1: // VADDAy
                                    VUops._vuADDAy(VUops.VU);
                                    break;
                                case 2: // VADDAz
                                    VUops._vuADDAz(VUops.VU);
                                    break;
                                case 3: // VADDAw
                                    VUops._vuADDAw(VUops.VU);
                                    break;
                                case 4: // VSUBAx
                                    VUops._vuSUBAx(VUops.VU);
                                    break;
                                case 5: // VSUBAy
                                    VUops._vuSUBAy(VUops.VU);
                                    break;
                                case 6: // VSUBAz
                                    VUops._vuSUBAz(VUops.VU);
                                    break;
                                case 7: // VSUBAw
                                    VUops._vuSUBAw(VUops.VU);
                                    break;
                                case 24: // VMULAx
                                    VUops._vuMULAx(VUops.VU);
                                    break;
                                case 25: // VMULAy
                                    VUops._vuMULAy(VUops.VU);
                                    break;
                                case 26: // VMULAz
                                    VUops._vuMULAz(VUops.VU);
                                    break;
                                case 27: // VMULAw
                                    VUops._vuMULAw(VUops.VU);
                                    break;
                                case 40: // VADDA
                                    VUops._vuADDA(VUops.VU);
                                    break;
                                case 42: // VMULA
                                    VUops._vuMULA(VUops.VU);
                                    break;
                                case 44: // VSUBA
                                    VUops._vuSUBA(VUops.VU);
                                    break;

                                case 8: // VMADDAx
                                    VUops._vuMADDAx(VUops.VU);
                                    break;
                                case 9: // VMADDAy
                                    VUops._vuMADDAy(VUops.VU);
                                    break;
                                case 10: // VMADDAz
                                    VUops._vuMADDAz(VUops.VU);
                                    break;
                                case 11: // VMADDAw
                                    VUops._vuMADDAw(VUops.VU);
                                    break;
                                case 12: // VMSUBAx
                                    VUops._vuMSUBAx(VUops.VU);
                                    break;
                                case 13: // VMSUBAy
                                    VUops._vuMSUBAy(VUops.VU);
                                    break;
                                case 14: // VMSUBAz
                                    VUops._vuMSUBAz(VUops.VU);
                                    break;
                                case 15: // VMSUBAw
                                    VUops._vuMSUBAw(VUops.VU);
                                    break;
                                case 41: // VMADDA
                                    VUops._vuMADDA(VUops.VU);
                                    break;
                                case 45: // VMSUBA
                                    VUops._vuMSUBA(VUops.VU);
                                    break;
                                case 46: // VOPMULA
                                    VUops._vuOPMULA(VUops.VU);
                                    break;

                                case 16: // VITOF0
                                case 17: // VITOF4
                                case 18: // VITOF12
                                case 19: // VITOF15
                                case 20: // VFTOI0
                                case 21: // VFTOI4
                                case 22: // VFTOI12
                                case 23: // VFTOI15
                                case 29: // VABS
                                case 48: // VMOVE
                                    VUops._vuMOVE(VUops.VU);
                                    break;
                                case 49: // VMR32
                                    VUops._vuMR32(VUops.VU);
                                    break;

                                case 31: // VCLIP
                                    break;

                                case 30: // VMULAi
                                    VUops._vuMULAi(VUops.VU);
                                    break;
                                case 34: // VADDAi
                                    VUops._vuADDAi(VUops.VU);
                                    break;
                                case 38: // VSUBAi
                                    VUops._vuSUBAi(VUops.VU);
                                    break;

                                case 35: // VMADDAi
                                    VUops._vuMADDAi(VUops.VU);
                                    break;
                                case 39: // VMSUBAi
                                    VUops._vuMSUBAi(VUops.VU);
                                    break;

                                case 32: // VADDAq
                                    VUops._vuADDAq(VUops.VU);
                                    break;
                                case 36: // VSUBAq
                                    VUops._vuSUBAq(VUops.VU);
                                    break;
                                case 28: // VMULAq
                                    VUops._vuMULAq(VUops.VU);
                                    break;

                                case 33: // VMADDAq
                                    VUops._vuMADDAq(VUops.VU);
                                    break;
                                case 37: // VMSUBAq
                                    VUops._vuMSUBAq(VUops.VU);
                                    break;

                                case 52: // VLQI
                                case 54: // VLQD
                                    break;

                                case 53: // VSQI
                                case 55: // VSQD
                                    break;

                                case 56: // VDIV
                                    VUops._vuDIV(VUops.VU);
                                    break;
                                case 58: // VRSQRT
                                    break;

                                case 57: // VSQRT
                                    VUops._vuSQRT(VUops.VU);
                                    break;

                                case 60: //VMTIR
                                    break;

                                case 61: // VMFIR
                                    break;

                                case 62: // VILWR
                                    break;

                                case 63: // VISWR
                                    break;

                                case 64: // VRNEXT
                                case 65: // VRGET
                                    break;

                                case 66: // VRINIT
                                case 67: // VRXOR
                                    break;

                                case 47: // VNOP
                                    VUops._vuNOP(VUops.VU);
                                    break;
                                case 59: // VWAITQ
                                    VUops._vuWAITQ(VUops.VU);
                                    break;

                                default:
                                    UnityEngine.Debug.LogWarning("Unknown R5900 COP2 SPEC2: " + code.ToString("X"));
                                    break;
                            }

                            break;

                        default:
                            UnityEngine.Debug.LogWarning("Unknown R5900 COP2 SPEC1: " + code.ToString("X"));
                            break;
                    }

                    break;

                default:
                    break;
            }
        }
    }
}
