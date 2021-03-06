﻿using System;
using System.Linq;
using Sandpiles.Calc;
using System.Diagnostics;
using System.Drawing;

namespace Sandpiles.Cmd
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            var settings = ParseArgs(args);
            var pile = new SandPileGrid(settings.Height, settings.Width);
            pile.SetSeed(settings);

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
                        SaveImage(pile, settings.Filename);
                    intervalTime.Restart();
                }
            }
            totalTime.Stop();

            if (settings.PrintConsole)
                pile.Print();

            Console.WriteLine($"Total iterations: {iterations}");
            Console.WriteLine($"Total Time: {totalTime.Elapsed}");
            string speed = (totalTime.ElapsedMilliseconds == 0)
                ? "INFINITE!"
                : (1000 * (decimal)iterations / totalTime.ElapsedMilliseconds).ToString("#:###");

            Console.WriteLine($"Iterations/s: {speed}");

            if (settings.SaveFinalImage)
                SaveImage(pile, settings.Filename);
        }

        private static void SaveImage(SandPileGrid pile, string fileName)
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
                bmp.Save(fileName);
            }
        }

        private static Color GetColor(int grains)
        {
            return grains switch
            {
                0 => Color.Black,
                1 => Color.Red,
                2 => Color.Orange,
                3 => Color.Yellow,
                4 => Color.LightYellow,
                _ => Color.White,
            };
        }

        private static PileSettings ParseArgs(string[] args)
        {
            var settings = new PileSettings();
            if (args.Contains("-h"))
                settings.Height = Convert.ToInt32(GetArgumentValue(args, "-h"));

            if (args.Contains("-w"))
                settings.Width = Convert.ToInt32(GetArgumentValue(args, "-w"));

            if (args.Contains("-s"))
                settings.Seed = Convert.ToInt32(GetArgumentValue(args, "-s"));

            if (args.Contains("-p"))
                settings.PrintConsole = true;

            if (args.Contains("-i"))
                settings.SaveIntermediateImage = true;

            if (args.Contains("-f"))
                settings.Filename = GetArgumentValue(args, "-f");
            else
                settings.Filename = $"{DateTime.Now:yyyyMMdd-HHmmss}_{settings.Height}x{settings.Width}_{settings.Seed}.bmp";

            return settings;
        }

        private static string GetArgumentValue(string[] args, string argument)
        {
            return args.SkipWhile(a => a != argument).Skip(1).First();
        }
    }
}