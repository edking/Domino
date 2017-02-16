using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Lib.Brains
{
    public class CompositeBrain : IBrain
    {
        private Task<Board>[] _tasks;
        public Board Board { get; protected set; }

        protected int MaxHeight;
        protected int MaxWidth;

        protected List<List<int>> Input { get; set; }

        public CompositeBrain(List<List<int>> input)
        {
            Input = input;
            MaxWidth = input[0].Count;
            MaxHeight = input.Count;

            Board = new Board(MaxWidth, MaxHeight);
        }

        public void Parse()
        {
            var mx = Input[0].Count;
            var my = Input.Count;

            Board = new Board(mx, my);

            _tasks = new Task<Board>[Constants.cMaxDepth];

            for (var p = 0; p < Constants.cMaxDepth; p++)
            {
                var p1 = p;
                _tasks[p] = Task.Run(() =>
                {
                    var brain = PickABrain(p1);
                    brain.Parse();
                    return brain.Board;
                });
            }

            Task.WaitAll(_tasks);

            var best = _tasks.Aggregate((agg, next) =>
                (next.Result.CoveredCells > agg.Result.CoveredCells) ? next : agg);

            Board = best.Result;

            for (var p = 0; p < Constants.cMaxDepth; p++)
            {
                _tasks[p].Dispose();
            }
            _tasks = null;
        }

        private IBrain PickABrain(int n)
        {
            IBrain rv;

            switch (n)
            {
                case 0:
                    rv = new SequentialHorizontalBrain(Input);
                    break;
                case 1:
                    rv = new SequentialVerticalBrain(Input);
                    break;
                case 2:
                    rv = new ImprovedVerticalBrain(Input);
                    break;
                case 3:
                    rv = new ImprovedHorizontalBrain(Input);
                    break;
                default:
                    rv = new SpiralBrain(Input);
                    break;
            }

            return rv;
        }
    }
}
