using System;
using System.Linq;
using Sandpiles.Calc;
using System.Diagnostics;

namespace Sandpiles.Cmd
{
    class Program
    {

        static void Main(string[] args)
        {
            var settings = ParseArgs(args);
            var pile = new SandPileGrid(settings.Height, settings.Width);
            pile.Grid[settings.Height / 2][settings.Width / 2] = settings.Seed;

            var iterations = 0;
            Console.Clear();
            var sw = Stopwatch.StartNew();
            while (pile.Topple()){
                iterations++;
                Console.WriteLine($"Iteration {iterations}");
                Console.WriteLine($"Time: {sw.Elapsed}");
                Console.WriteLine($"Iterations/ms: {(decimal)iterations/sw.ElapsedMilliseconds}");
                Console.SetCursorPosition(0,0);
            } 
            sw.Stop();
            pile.Print();
            Console.WriteLine($"Total iterations: {iterations}");
            Console.WriteLine($"Total Time: {sw.Elapsed}");
            Console.WriteLine($"Iterations/ms: {(decimal)iterations/sw.ElapsedMilliseconds}");
        }

        static PileSettings ParseArgs(string[] args)
        {
            var settings = new PileSettings();
            if (args.Contains("-h"))
            {
                settings.Height = Convert.ToInt32(args.SkipWhile(a => a != "-h").Skip(1).First());
            }
            if (args.Contains("-w"))
            {
                settings.Width = Convert.ToInt32(args.SkipWhile(a => a != "-w").Skip(1).First());
            }

            if (args.Contains("-s"))
            {
                settings.Seed = Convert.ToInt32(args.SkipWhile(a => a != "-s").Skip(1).First());
            }


            return settings;
        }

        internal class PileSettings
        {
            public int Height { get; set; } = 100;
            public int Width { get; set; } = 100;
            public int Seed { get; set; } = 1000;
        }
    }
}
