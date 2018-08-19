using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace remu
{
    class Preprocessor
    {
        public const int constMemSize = 16;
        public const int ramSize = 8;
        const int dataBusWidth = 12;
        const UInt32 constMemAddrMask = 0xF;
        const UInt32 ramAddrMask = 0x7;
        const UInt32 dataBusMask = ~(UInt32.MaxValue << dataBusWidth);
        const UInt32 romAddrMask = 0x3F;
        
        MemManager constMem = new MemManager(constMemSize);
        MemManager ramMem = new MemManager(ramSize);
        public uint[] cmem = new uint[constMemSize];

        public bool debug = false;

        public class ProcProg
        {
            public string[] prog;
            public uint[] cmem;
            public int constUsed;
            public int ramUsed;
            public ProcProg(string[] prog, uint[] cmem, int constUsed, int ramUsed)
            {
                this.prog = prog;
                this.cmem = cmem;
                this.constUsed = constUsed;
                this.ramUsed = ramUsed;
            }
        }

        private void printLines(string[] lines)
        {
            if(!debug) return;
            foreach (string l in lines)
                Console.WriteLine(l);
            Console.WriteLine("");
        }

        public ProcProg go(string prog)
        {
            string[] lines = prog.Split(new char[] { '\n' });
            printLines(lines);

            for (int i = 0; i < lines.Length; ++i)
                lines[i] = strip(lines[i]);
            printLines(lines);

            //register consts
            for (int i = 0; i < lines.Length; ++i)
                lines[i] = registerConsts(lines[i]);
            lines = dropNulls(lines);
            printLines(lines);

            //register labels
            uint nextPc = 0;
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = registerLabels(lines[i], nextPc);
                if (lines[i] != null) ++nextPc;
            }
            lines = dropNulls(lines);
            printLines(lines);

            //resolve consts
            for (int i = 0; i < lines.Length; ++i)
                lines[i] = resolveConsts(lines[i]);
            printLines(lines);

            //resolve labels
            for (int i = 0; i < lines.Length; ++i)
                lines[i] = resolveLabels(lines[i]);
            printLines(lines);

            return new ProcProg(lines, cmem, constMem.size, ramMem.size);
        }

        public string[] dropNulls(string[] strs)
        {
            List<string> l = new List<string>();
            foreach (string s in strs)
                if(s != null) l.Add(s);
            return l.ToArray();
        }

        string strip(string line)
        {
            if (line == null) return null;
            line = line.Trim();
            if (line == "" || line.StartsWith(";")) return null; //drop empty and comment lines
            line = line.Split(new char[] { ';' }, 2)[0]; //remove comment
            line = line.Trim();
            return line;
        }

        string registerLabels(string line, uint nextPC)
        {
            if (line == null) return null;
            string[] label = line.Split(new char[] { ':' }, 2);
            if(label.Length > 1)    //curent line is a label
            {
                int cAddr = constMem.registerVar(label[0]);
                cmem[cAddr] = nextPC;
                if(label[1] == "") return null;
                line = label[1];
            }
            return line;
        }

        string registerConsts(string line)
        {
            if (line == null) return null;
            string[] tokens = line.Split(new char[] { ' ', '\t' }, 3);
            if (tokens.Length == 3 && tokens[0] == "const")    //curent line is a const
            {
                int cAddr = constMem.registerVar(tokens[1]);
                cmem[cAddr] = uint.Parse(tokens[2]);
                return null;
            } else if (tokens.Length == 2 && tokens[0] == "var")    //curent line is a var
            {
                ramMem.registerVar(tokens[1]);
                return null;
            }
            return line;
        }

        string resolveLabels(string line)
        {
            if (line == null) return null;
            if (line.StartsWith("j"))
            {
                string[] cmd = line.Split(new char[] { ' ', '\t' }, 2);
                int n;
                if (!int.TryParse(cmd[1], out n))
                    line = cmd[0] + " " + constMem.getVarAddr(cmd[1]);
            }
            return line;
        }

        string resolveConsts(string line)
        {
            if (line == null) return null;
            string[] tokens = line.Split(new char[] { ' ', '\t' }, 2);
            if(tokens.Length > 1)
            {
                string cmd = tokens[0];
                string[] ops = tokens[1].Split(new char[] { ',' });
                for(int i = 0; i < ops.Length; ++i)
                {
                    ops[i] = ops[i].Trim();
                    if(constMem.isRegistered(ops[i]))
                        ops[i] = constMem.getVarAddr(ops[i]).ToString();
                    else if (ramMem.isRegistered(ops[i]))
                        ops[i] = ramMem.getVarAddr(ops[i]).ToString();
                    cmd += (i==0? " " : "") + ops[i] + (i < ops.Length - 1 ? "," : "");
                }
                return cmd;
            }

            return line;
        }

        class MemManager
        {
            public readonly int maxSize;
            public int size = 0;
            int nextFree;
            bool inverse;
            Dictionary<string, int> db;
            public MemManager(int size, bool inverse = false)
            {
                this.maxSize = size;
                this.inverse = inverse;
                nextFree = inverse ? size-1 : 0;
                db = new Dictionary<string, int>();
            }
            public int registerVar(string name)
            {
                if (nextFree >= maxSize || nextFree < 0)
                    throw new Exception("Out of available memory.");
                if(isRegistered(name))
                    throw new Exception("Name \"" + name + "\" is already registered.");
                int cur = nextFree;
                db.Add(name, cur);
                ++size;
                nextFree += inverse ? -1 : 1;
                return cur;
            }
            public int getVarAddr(string name)
            {
                return db[name];
            }
            public bool isRegistered(string name)
            {
                return db.ContainsKey(name);
            }
        }
    }
}
