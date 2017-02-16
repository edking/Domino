using System.Collections.Generic;
using System.Linq;

namespace Domino.Lib.Brains
{
    public class SpiralBrain : BrainBase
    {
        public SpiralBrain(List<List<int>> input) : base(input) { }

        public override void Parse()
        {
            Board.Clear();
            PlaceFirstDomino();

            var candidates = CandidateCells();
            while (candidates.Any())
            {
                var placed = TryCandidates(candidates, Constants.AllDirs);

                if (!placed) break;
                candidates = CandidateCells();
            }
        }

        protected override void PlaceFirstDomino()
        {
            var x1 = Constants.RNG.Next(MaxWidth);
            var y1 = Constants.RNG.Next(MaxHeight);

            while (Input[y1][x1] == 0)
            {
                x1 = Constants.RNG.Next(MaxWidth);
                y1 = Constants.RNG.Next(MaxHeight);
            }

            var c1 = Board.CellAt(x1, y1);

            var u = UnoccupiedNeighbors(c1);
            var c2 = (u.Count == 1) ? u[0] : u[Constants.RNG.Next(u.Count)];
            var x2 = c2.Coords.X;
            var y2 = c2.Coords.Y;

            foreach (var un in u)
            {
                c2 = un;
                x2 = c2.Coords.X;
                y2 = c2.Coords.Y;
            }

            var d = new Domino(Input[y1][x1], Input[y2][x2]);
            Board.PlaceDomino(c1, c2, d);
        }
    }
}