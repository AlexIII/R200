using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace remu
{
    public class Remulator
    {
        public const int constMemSize = 16;
        public const int ramSize = 8;
        public const int dataBusWidth = 12;
        private const UInt32 constMemAddrMask = 0xF;
        private const UInt32 ramAddrMask = 0x7;
        private const UInt32 dataBusMask = ~(UInt32.MaxValue << dataBusWidth);
        private const UInt32 romAddrMask = 0x3F;

        public State state = new State();
        private UInt32[] cmem;
        private string[] rom;
        public int cycle;
        public bool halt;
        public string prvCmd = "";

        public EventHandler StepCompleted;
        public UInt32[] ConstMemory => cmem.ToList().ToArray(); // poor man's shallow copy

        public Remulator(UInt32[] cmem, string[] rom = null)
        {
            this.cmem = cmem;
            this.rom = rom;
            reset();
        }

        public void step()
        {
            exec(prvCmd = rom[state.PC]);

            // Callback for subscribers
            StepCompleted?.Invoke(this, EventArgs.Empty);
        }

        public bool run(int maxCycles = 5000)
        {
            while (!halt)
            {
                step();
                if (cycle >= maxCycles) return true;
            }
            return false;
        }

        public class State
        {
            private UInt32 _RA;  //GPR
            private UInt32 _RB;  //GPR
            private UInt32 _LEAF;
            private UInt32 _PC;

            public class Memory
            {
                private UInt32[] membuff = new UInt32[ramSize];
                public UInt32 this[int index]
                {
                    get
                    {
                        UInt32 tmp = membuff[index & ramAddrMask];
                        return tmp;
                    }
                    set { membuff[index & ramAddrMask] = value&dataBusMask; }
                }
            }

            public Memory RAM = new Memory();
            public bool c;
            public bool z;
            public bool skip;
            bool cbuff;

            public UInt32 RA
            {
                get { return _RA; }
                set { _RA = value & dataBusMask; }
            }
            public UInt32 RB
            {
                get { return _RB; }
                set { _RB = value & dataBusMask; }
            }
            public UInt32 LEAF
            {
                get { return _LEAF; }
                set { _LEAF = value & romAddrMask; }
            }
            public UInt32 PC
            {
                get { return _PC; }
                set { _PC = value & romAddrMask; }
            }

            public override string ToString()
            {
                string ram = "";
                for(int i = 0; i < ramSize; ++i)
                    ram += RAM[i].ToString() + " ";

                return "RA: " + RA.ToString() + "[0b" + Convert.ToString(RA, 2) + ", 0x" + Convert.ToString(RA, 16) + "]" 
                    + ", RB: " + RB.ToString() + "[0b" + Convert.ToString(RB, 2) + ", 0x" + Convert.ToString(RB, 16) + "]"
                    + ", LEAF: " + LEAF.ToString() + ", PC: " + PC.ToString()
                    + "\nRAM: " + ram
                    + "\nc:" + (c ? "1" : "0") + ", z:" + (z ? "1" : "0") + ", skip:" + (skip ? "1" : "0") + ", bc:" + (cbuff ? "1" : "0");
            }

            public enum Zmode {
                SKIP,
                UPD,
                AND
            }

            public void reset()
            {
                PC = 0;
                skip = false;
            }
            public void setGPRupdFlags(string name, UInt32 val, Zmode zmode)
            {
                c = (val & ~dataBusMask & (dataBusMask << 1)) != 0;
                val &= dataBusMask;
                if (zmode == Zmode.UPD) z = val == 0;
                else if (zmode == Zmode.AND) z = z && val == 0;
                setGPR(name, val);
            }
            public void setGPR(string name, UInt32 val)
            {
                setGetGPR(name, val, true);
            }
            public UInt32 getGPR(string name)
            {
                return setGetGPR(name, 0, false);
            }
            UInt32 setGetGPR(string name, UInt32 val, bool isSet)
            {
                switch(name)
                {
                    case "ra": if(isSet) RA = val; return RA;
                    case "rb": if (isSet) RB = val; return RB;
                    default: throw new Exception("Unknown GPR: " + name);
                }
            }
            public void bufc() { cbuff = c; }
            public void recc() { c = cbuff; }
        }
        
        public void reset()
        {
            state.reset();
            cycle = 0;
        }
        public void exec(string cmd)
        {
            halt = false;
            transfer(new Cmd(cmd.ToLower()));
            ++cycle;
        }

        void transfer(Cmd cmd)
        {
            state.PC = (state.PC + 1) & romAddrMask;

            if (state.skip)
            {
                state.skip = false;
                return;
            }

            switch(cmd.op)
            {
                case "movc": state.setGPR(cmd.args[0], cmem[int.Parse(cmd.args[1])]); break;
                case "jmp": state.PC = cmem[int.Parse(cmd.args[0])]; break;
                case "mov":
                    if (cmd.args[0] == "pc") state.PC = state.getGPR(cmd.args[1]);
                    else if (cmd.args[0] == "rb") state.setGPR(cmd.args[0], UInt32.Parse(cmd.args[1]) & 0xF);
                    else throw new Exception("Incorect operand in " + cmd.cmd);
                    break;
                case "jc": if (state.c) state.PC = cmem[int.Parse(cmd.args[0])] ; break;
                case "jz": if (state.z) state.PC = cmem[int.Parse(cmd.args[0])]; break;
                case "jnc": if (!state.c) state.PC = cmem[int.Parse(cmd.args[0])]; break;
                case "jnz": if (!state.z) state.PC = cmem[int.Parse(cmd.args[0])]; break;
                case "movm":
                    int addr;
                    if (int.TryParse(cmd.args[0], out addr)) //write to RAM
                        state.RAM[addr] = state.getGPR(cmd.args[1]);
                    else //read from RAM
                    {
                        int ind = int.Parse(cmd.args[1]);
                        state.setGPR(cmd.args[0], state.RAM[ind]);
                        state.RAM[ind] = 0;
                    }
                    break;
                case "lim":
                    state.RA = state.RAM[(int)state.RB];
                    state.RAM[(int)state.RB] = 0;
                    break;
                case "sim": state.RAM[(int)state.RB] = state.RA; break;
                case "lic": state.RA = cmem[state.RB & constMemAddrMask]; break;
                case "clrz": state.z = false; break;
                case "buc": state.bufc(); break;
                case "rec": state.recc(); break;
                case "leaf": state.LEAF = state.PC; break;
                case "ret": state.PC = state.LEAF; state.skip = true; break;
                case "sc": if (state.c) state.skip = true;  break;
                case "sz": if (state.z) state.skip = true; break;
                case "snc": if (!state.c) state.skip = true; break;
                case "snz": if (!state.z) state.skip = true; break;
                case "clrc": state.c = false; break;
                case "setc": state.c = true; break;
                case "halt": halt = true; break;
                case "nop": break;
                default: aluOp(cmd); break;
            }
        }

        void aluOp(Cmd cmd)
        {
            string dest = cmd.args[0];
            string arg = dest == "ra" ? "rb" : "ra";
            UInt32 r1 = state.getGPR(dest);
            UInt32 r2 = state.getGPR(arg);
            switch (cmd.op)
            {
                case "add": state.setGPRupdFlags(dest, r1 + r2, State.Zmode.UPD); break;
                case "adc": state.setGPRupdFlags(dest, r1 + r2 + Convert.ToUInt32(state.c), State.Zmode.AND); break;
                case "sub": state.setGPRupdFlags(dest, r1 - r2, State.Zmode.UPD); break;
                case "sbc": state.setGPRupdFlags(dest, r1 - r2 - Convert.ToUInt32(state.c), State.Zmode.AND); break;
                case "inc": state.setGPRupdFlags(dest, r1 + 1, State.Zmode.UPD); break;
                case "dec": state.setGPRupdFlags(dest, r1 - 1, State.Zmode.UPD); break;
                case "shcr": state.setGPRupdFlags(dest, ((r1&1) << dataBusWidth) | (Convert.ToUInt32(state.c) << (dataBusWidth-1)) | (r1>>1), State.Zmode.AND); break;
                case "shr": state.setGPRupdFlags(dest, r1>>1, State.Zmode.SKIP); break;
                case "shcl": state.setGPRupdFlags(dest, Convert.ToUInt32(state.c) | (r1 << 1), State.Zmode.AND); break;
                case "shl": state.setGPRupdFlags(dest, (r1 << 1) & dataBusMask | (((1 << dataBusWidth - 1) & r1) >> dataBusWidth - 1), State.Zmode.SKIP); break;
                case "not": state.setGPRupdFlags(dest, ((r1 << 1) & ~dataBusMask) | (~r1 & dataBusMask), State.Zmode.UPD); break;
                case "and": state.setGPRupdFlags(dest, (1 << dataBusWidth) | (r1 & r2), State.Zmode.UPD); break;
                case "or": state.setGPRupdFlags(dest, (r1 | r2), State.Zmode.UPD); break;
                case "xor": state.setGPRupdFlags(dest, (r1 ^ r2), State.Zmode.UPD); break;
                case "ide": state.setGPRupdFlags(dest, (Convert.ToUInt32(state.c) << dataBusWidth) | r1, State.Zmode.AND); break;
                default: throw new Exception("Unknown instruction: " + cmd);
            }
        }

        private class Cmd
        {
            public string cmd;
            public string op;
            public string[] args;
            public Cmd(string cmd)
            {
                this.cmd = cmd;
                string[] s = cmd.Split(new char[] {' ', '\t'}, 2);
                op = s[0];
                if (s.Length > 1)
                {
                    args = s[1].Split(new char[] { ',' });
                    for (int i = 0; i < args.Length; ++i)
                        args[i] = args[i].Trim();
                }
            }
        }
    }
}
