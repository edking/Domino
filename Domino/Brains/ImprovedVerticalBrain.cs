using System.Collections.Generic;
using System.Linq;

namespace Domino.Lib.Brains
{
    public class ImprovedVerticalBrain : BrainBase
    {
        public ImprovedVerticalBrain(List<List<int>> input) : base(input) { }

        public override void Parse()
        {
            Board.Clear();
            PlaceFirstDomino();

            var candidates = CandidateCells();
            while (candidates.Any())
            {
                var placed = TryCandidates(candidates, Constants.VerticalDirs) ||
                    TryCandidates(candidates, Constants.HorizontalDirs);

                if (!placed) break;
                candidates = CandidateCells();
            }
        }

        protected override void PlaceFirstDomino()
        {
            for (var y = 0; y < MaxHeight - 1; y++)
                for (var x = 0; x < MaxWidth; x++)
                {
                    if (Input[y][x] == 0) continue;
                    if (Input[y + 1][x] == 0) continue;
                    var d = new Domino(Input[y][x], Input[y + 1][x]);
                    var c1 = Board.CellAt(x, y);
                    var c2 = Board.CellAt(x, y + 1);
                    Board.PlaceDomino(c1, c2, d);
                    return;
                }
        }

        
    }
}
