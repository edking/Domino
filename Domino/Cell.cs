using System.Collections.Generic;
using Domino.Lib.Brains;

namespace Domino.Lib
{
    public class Cell
    {
        public Cell(int x, int y)
        {
            Coords = new CellCoords {X = x, Y = y};
            Clear();
        }

        public Domino Domino { get; set; }
        public bool IsOccupied { get; set; }
        public int Pips { get; set; }

        public CellCoords Coords { get; set; }

        public void Clear()
        {
            Domino = null;
            IsOccupied = false;
            Pips = 0;
        }
    }
}