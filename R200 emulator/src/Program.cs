using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace remu
{
    class Program
    {
        const double secPerTick = 0.45;
        static bool EnableGUI = false;
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("Usage: ./remu [/gui] *.R200 [run]");
                return;
            }
            bool runAll = args.Length >= 2 && args.Last().ToLower() == "run";
            var fileNamePos = runAll ? args.Length - 2 : args.Length - 1;
            string progLine = File.ReadAllText(args[fileNamePos]);

            // Parse additional options
            foreach (var arg in args.Take(fileNamePos))
            {
                switch (arg)
                {
                    case "/gui": EnableGUI = true; break;
                }
            }


            Preprocessor pr = new Preprocessor();
            //pr.debug = true;
            var res = pr.go(progLine);
            Console.WriteLine("Compilation: ok!");
            Console.WriteLine("Used ROM: \t" + res.prog.Length + " / 64");
            Console.WriteLine("Used CONST: \t" + res.constUsed + " / " + Preprocessor.constMemSize);
            Console.WriteLine("Used RAM: \t" + res.ramUsed + " / " + Preprocessor.ramSize);

            Remulator remulator = new Remulator(res.cmem, res.prog);
            MainForm guiForm = null;
            if (EnableGUI)
            {
                guiForm = new MainForm(remulator, res, runAll);
                Application.EnableVisualStyles();
                Application.Run(guiForm);
                return;
            }

            Console.WriteLine("\nPress any key to start emulation.");
            if (!runAll)
            {
                Console.WriteLine("Press 'q' to break or any other key to advance one step.");
            }
            Console.WriteLine("");
            if (Console.ReadKey().KeyChar == 'q')
                return;

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
            Console.WriteLine("HALT.");
            Console.WriteLine("\nYour programm would have been running on the real machine around " +
                remulator.cycle*secPerTick + " sec.");
        }
    }
}
