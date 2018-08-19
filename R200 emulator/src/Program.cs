using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace remu
{
    class Program
    {
        const double secPerTick = 0.45;
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("Usage: ./remu *.R200 [run]");
                return;
            }
            string progLine = File.ReadAllText(args[0]);
            bool runAll = args.Length >= 2 && args[1].ToLower() == "run";


            Preprocessor pr = new Preprocessor();
            //pr.debug = true;
            var res = pr.go(progLine);
            Console.WriteLine("Compilation: ok!");
            Console.WriteLine("Used ROM: \t" + res.prog.Length + " / 64");
            Console.WriteLine("Used CONST: \t" + res.constUsed + " / " + Preprocessor.constMemSize);
            Console.WriteLine("Used RAM: \t" + res.ramUsed + " / " + Preprocessor.ramSize);

            Console.WriteLine("\nPress any key to start emulation.");
            if(!runAll)
            {
                Console.WriteLine("Press 'q' to break or any other key to advance one step.");
            }
            Console.WriteLine("");
            if (Console.ReadKey().KeyChar == 'q')
                return;

            Remulator remulator = new Remulator(res.cmem, res.prog);
            while (!remulator.halt)
            {
                remulator.step();
                Console.WriteLine("\nCycle: " + remulator.cycle.ToString());
                Console.WriteLine("CMD: " + remulator.prvCmd);
                Console.WriteLine("State:\n" + remulator.state.ToString());
                Console.Write("CONST: ");
                foreach (int i in res.cmem)
                    Console.Write(i + " ");
                Console.WriteLine("");
                if (!runAll)
                {
                    var k = Console.ReadKey();
                    if (k.KeyChar == 'q')
                        break;
                }
            }
            Console.WriteLine("\nYour programm would have been running on the real machine for about " +
                remulator.cycle*secPerTick + " sec.");
            Console.WriteLine("Done.");
        }
    }
}
