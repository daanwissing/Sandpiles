using System;
using Sandpiles.Calc;

namespace Sandpiles.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var pile = new SandPileGrid(100, 100);            
            pile.Grid[50][50] = 1000;
            while(pile.Topple());
            pile.Print();
        }
    }
}
