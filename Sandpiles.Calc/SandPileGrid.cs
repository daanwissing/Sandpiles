﻿using System;

namespace Sandpiles.Calc
{
    public class SandPileGrid
    {
        private int[][] _grid;
        public readonly int Height;
        public readonly int Width;


        public int[][] Grid
        {
            get
            {
                return _grid; // TODO: copy
            }
        }

        public SandPileGrid() : this(3, 3)
        {
        }

        public SandPileGrid(int height, int width)
        {
            int[][] newGrid = InitializeGrid(height, width);
            _grid = newGrid;
            Height = height;
            Width = width;
        }

        private static int[][] InitializeGrid(int height, int width)
        {
            int[][] newGrid = new int[height][];
            for (int i = 0; i < height; i++)
            {
                var row = new int[width];
                newGrid[i] = row;
            }

            return newGrid;
        }

        public bool ToppleInPlace()
        {
            bool changed = false;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    var oldValue = _grid[i][j];
                    if (_grid[i][j] > 3)
                    {
                        int addValue = oldValue / 4;
                        if (i > 0)
                            _grid[i - 1][j] += addValue;
                        if (i < Height - 1)
                            _grid[i + 1][j] += addValue;
                        if (j > 0)
                            _grid[i][j - 1] += addValue;
                        if (j < Width - 1)
                            _grid[i][j + 1] += addValue;

                        _grid[i][j] = oldValue % 4;
                        changed = true;

                    }
                }
            }

            return changed;
        }

        public bool Topple()
        {
            var newGrid =  InitializeGrid(Height, Width);
            var changed = false;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    var oldValue = Grid[i][j];
                    if (oldValue > 3)
                    {
                        int addValue = oldValue / 4;
                        if (i > 0)
                            newGrid[i - 1][j] += addValue;
                        if (i < Height - 1)
                            newGrid[i + 1][j] += addValue;
                        if (j > 0)
                            newGrid[i][j - 1] += addValue;
                        if (j < Width - 1)
                            newGrid[i][j + 1] += addValue;

                        newGrid[i][j] += oldValue % 4;
                        changed = true;
                    }
                    else
                    {
                        newGrid[i][j] += oldValue;
                    }
                }
            }

            _grid = newGrid;
            return changed;
        }

        public void Print()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(_grid[i][j]);
                }
                Console.WriteLine();
            }
        }
    }
}
