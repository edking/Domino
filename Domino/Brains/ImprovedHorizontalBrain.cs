using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Lib.Brains
{
    public class ImprovedHorizontalBrain : BrainBase
    {
        public ImprovedHorizontalBrain(List<List<int>> input) : base(input) { }
        public override void Parse()
        {
            Board.Clear();
            PlaceFirstDomino();

            var candidates = CandidateCells();
            while (candidates.Any())
            {
                var placed = (TryCandidates(candidates, Constants.HorizontalDirs) ||
                    TryCandidates(candidates, Constants.VerticalDirs));

                if (!placed) break;
                candidates = CandidateCells();
            }
        }

        protected override void PlaceFirstDomino()
        {
            for (var y = 0; y < MaxHeight; y++)
                for (var x = 0; x < MaxWidth - 1; x++)
                {
                    if (Input[y][x] == 0) continue;
                    if (Input[y][x + 1] == 0) continue;
                    var d = new Domino(Input[y][x], Input[y][x + 1]);
                    var c1 = Board.CellAt(x, y);
                    var c2 = Board.CellAt(x + 1, y);
                    Board.PlaceDomino(c1, c2, d);
                    return;
                }
        }
    }
}
