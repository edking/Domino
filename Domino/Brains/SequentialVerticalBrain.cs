using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Lib.Brains
{
    public class SequentialVerticalBrain : BrainBase
    {
        public SequentialVerticalBrain(List<List<int>> input) : base(input) { }

        public override void Parse()
        {
            Board.Clear();
            PlaceFirstDomino();

            for (var p = 0; p < Constants.cMaxPasses; p++)
                for (var x = 0; x < MaxWidth; x++)
                    for (var y = 0; y < MaxHeight; y++)
                    {
                        var placed = false;

                        if (Input[y][x] == 0) continue;

                        if (y < (MaxHeight - 1))
                        {
                            if (Input[y + 1][x] != 0)
                            {
                                var dm = new Domino(Input[y][x], Input[y + 1][x]);
                                placed = TryVerticalDomino(x, y, dm);
                            }
                        }

                        if ((placed) || (x >= MaxWidth - 1)) continue;
                        if (Input[y][x + 1] == 0) continue;
                        var d = new Domino(Input[y][x], Input[y][x + 1]);
                        TryHorizontalDomino(x, y, d);
                    }
        }

        protected override void PlaceFirstDomino()
        {
            for (var y = 0; y < MaxHeight-1; y++)
                for (var x = 0; x < MaxWidth; x++)
                {
                    if (Input[y][x] == 0) continue;
                    if (Input[y+1][x] == 0) continue;
                    var d = new Domino(Input[y][x], Input[y+1][x]);
                    var c1 = Board.CellAt(x, y);
                    var c2 = Board.CellAt(x, y+1);
                    Board.PlaceDomino(c1, c2, d);
                    return;
                }

        }
        private bool TryVerticalDomino(int x, int y, Domino d)
        {
            var c1 = Board.CellAt(x, y);
            var c2 = Board.CellAt(x, y + 1);

            if (c1.IsOccupied || c2.IsOccupied) return false;

            var canPlacePip1 = CanPlacePip(c1, d.Pips[0]);
            var neighbor1 = AtLeastOneNeighbor(c1);

            var canPlacePip2 = CanPlacePip(c2, d.Pips[1]);
            var neighbor2 = AtLeastOneNeighbor(c2);

            var canPlace = (canPlacePip1 && canPlacePip2) && (neighbor1 || neighbor2);

            if (canPlace)  Board.PlaceDomino(c1, c2, d);

            return (canPlace);
        }

        private bool TryHorizontalDomino(int x, int y, Domino d)
        {
            var c1 = Board.CellAt(x, y);
            var c2 = Board.CellAt(x + 1, y);

            if (c1.IsOccupied || c2.IsOccupied) return false;

            var canPlacePip1 = CanPlacePip(c1, d.Pips[0]);
            var neighbor1 = AtLeastOneNeighbor(c1);

            var canPlacePip2 = CanPlacePip(c2, d.Pips[1]);
            var neighbor2 = AtLeastOneNeighbor(c2);

            var canPlace = (canPlacePip1 && canPlacePip2) && (neighbor1 || neighbor2);

            if (canPlace) Board.PlaceDomino(c1, c2, d);

            return (canPlace);
        }
    }
}
