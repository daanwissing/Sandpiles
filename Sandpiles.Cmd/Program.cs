using System;
using System.Linq;
using Sandpiles.Calc;
using System.Diagnostics;
using System.Drawing;

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
            var totalTime = Stopwatch.StartNew();
            var intervalTime = Stopwatch.StartNew();
            while (pile.ToppleInPlace())
            {
                iterations++;
                var iterationSkip = 1000;
                if (iterations % iterationSkip == 0)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"Iteration {iterations}");
                    Console.WriteLine($"Time: {totalTime.Elapsed}");
                    Console.WriteLine($"Iterations/s: {1000 * (decimal)iterationSkip / intervalTime.ElapsedMilliseconds:#.###}");
                    if (settings.SaveIntermediateImage)
                        SaveImage(pile);
                    intervalTime.Restart();
                }
            }
            totalTime.Stop();

            if (settings.PrintConsole)
                pile.Print();

            Console.WriteLine($"Total iterations: {iterations}");
            Console.WriteLine($"Total Time: {totalTime.Elapsed}");
            Console.WriteLine($"Iterations/s: {1000 * (decimal)iterations / totalTime.ElapsedMilliseconds:#.###}");

            if (settings.SaveFinalImage)
                SaveImage(pile);
        }

        private static void SaveImage(SandPileGrid pile)
        {
            using (var bmp = new Bitmap(pile.Height, pile.Width))
            {
                for (int i = 0; i < pile.Height; i++)
                {
                    for (int j = 0; j < pile.Width; j++)
                    {
                        bmp.SetPixel(i, j, GetColor(pile.Grid[i][j]));
                    }
                }
                bmp.Save("out.bmp");
            }
        }

        static Color GetColor(int i)
        {
            return i switch
            {
                0 => Color.Black,
                1 => Color.DarkRed,
                2 => Color.Red,
                3 => Color.OrangeRed,
                4 => Color.Orange,
                _ => Color.White,
            };
        }

        static PileSettings ParseArgs(string[] args)
        {
            var settings = new PileSettings();
            if (args.Contains("-h"))
            {
                settings.Height = Convert.ToInt32(GetArgumentValue(args, "-h"));
            }
            if (args.Contains("-w"))
            {
                settings.Width = Convert.ToInt32(GetArgumentValue(args, "-w"));
            }

            if (args.Contains("-s"))
            {
                settings.Seed = Convert.ToInt32(GetArgumentValue(args, "-s"));
            }

            if (args.Contains("-p"))
            {
                settings.PrintConsole = true;
            }

            if (args.Contains("-i"))
            {
                settings.SaveIntermediateImage = true;
            }

            return settings;
        }

        private static string GetArgumentValue(string[] args, string argument)
        {
            return args.SkipWhile(a => a != argument).Skip(1).First();
        }

        internal class PileSettings
        {
            public int Height { get; set; } = 100;
            public int Width { get; set; } = 100;
            public int Seed { get; set; } = 1000;
            public bool PrintConsole { get; set; } = false;
            public bool SaveIntermediateImage { get; set; } = false;
            public bool SaveFinalImage { get; set; } = true;

            public string Filename { get; set; }
        }
    }
}
