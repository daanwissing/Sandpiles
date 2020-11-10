using System;

namespace Sandpiles.Calc
{
    public class SandPileGrid
    {
        public int[][] Grid { get; }

        public SandPileGrid() : this(3,3)
        {
        }

        public SandPileGrid(int height, int width)
        {
            int[][] newGrid = new int[height][];
            for (int i = 0; i < height; i++)
            {
                var row = new int[width];
                newGrid[i] = row;
            }
            Grid = newGrid;
        }

        public void Collapse()
        {

        }
    }
}
