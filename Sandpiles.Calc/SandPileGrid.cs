using System;

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

        public void Collapse()
        {
            var newGrid = InitializeGrid(Height, Width);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Grid[i][j] > 3)
                    {
                        newGrid[i-1][j]++;
                        newGrid[i+1][j]++;
                        newGrid[i][j+1]++;
                        newGrid[i][j-1]++;

                        newGrid[i][j] = Grid[i][j] - 4;
                    }
                    else
                    {
                        newGrid[i][j] += Grid[i][j];
                    }
                }
            }
            
            _grid = newGrid;
        }
    }
}
