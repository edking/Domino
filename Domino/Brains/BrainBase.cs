using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Domino.Lib.Brains
{
    public abstract class BrainBase : IBrain
    {
        public Board Board { get; protected set; }

        protected int MaxHeight;
        protected int MaxWidth;

        protected List<List<int>> Input { get; set; }

        public abstract void Parse();

        protected abstract void PlaceFirstDomino();

        public BrainBase(List<List<int>> input)
        {
            Input = input;
            MaxWidth = input[0].Count;
            MaxHeight = input.Count;

            Board = new Board(MaxWidth, MaxHeight);
        }

        protected bool AtLeastOneNeighbor(Cell c)
        {
            return Neighbors(c).Any(x => x.IsOccupied);
        }

        protected bool CanPlacePip(Cell c, int pips)
        {
            var byPips = Neighbors(c).Any(x => x.IsOccupied && x.Pips == pips);
            var empty = Neighbors(c).All(y => !y.IsOccupied);
            return (byPips || empty);
        }

        protected List<Cell> UnoccupiedNeighbors(Cell c)
        {
            return Neighbors(c).Where(x => !x.IsOccupied && Input[c.Coords.Y][c.Coords.X] != 0).ToList();
        }

        protected List<Cell> Neighbors(Cell c)
        {
            var n = new List<Cell>();
            var dirs = Board.NeighborDirections(c);

            foreach (var dir in dirs)
            {
                Cell c1 = null;
                switch (dir)
                {
                    case Constants.Direction.Above:
                        c1 = Board.CellAt(c.Coords.X, c.Coords.Y - 1);
                        break;
                    case Constants.Direction.Below:
                        c1 = Board.CellAt(c.Coords.X, c.Coords.Y + 1);
                        break;
                    case Constants.Direction.Left:
                        c1 = Board.CellAt(c.Coords.X - 1, c.Coords.Y);
                        break;
                    case Constants.Direction.Right:
                        c1 = Board.CellAt(c.Coords.X + 1, c.Coords.Y);
                        break;
                }
                n.Add(c1);
            }
            return n;
        }

        protected List<Cell> CandidateCells()
        {
            var rv = (from c in Board.UnoccupiedCells()
                      where (AtLeastOneNeighbor(c) && Input[c.Coords.Y][c.Coords.X] != 0)
                      select c).ToList();
            return rv;
        }

        protected bool TryPlaceDomino(int x1, int y1, Constants.Direction d)
        {
            if ((x1 == 0) && (d == Constants.Direction.Left)) return false;
            if ((x1 == MaxWidth - 1) && (d == Constants.Direction.Right)) return false;
            if ((y1 == 0) && (d == Constants.Direction.Above)) return false;
            if ((y1 == MaxHeight - 1) && (d == Constants.Direction.Below)) return false;

            var x2 = x1;
            var y2 = y1;

            Domino domino = null;

            switch (d)
            {
                case Constants.Direction.Left:
                    x2--;
                    domino = new Domino(Input[y2][x2], Input[y1][x1]);
                    break;

                case Constants.Direction.Right:
                    x2++;
                    domino = new Domino(Input[y1][x1], Input[y2][x2]);
                    break;

                case Constants.Direction.Above:
                    y2--;
                    domino = new Domino(Input[y2][x2], Input[y1][x1]);
                    break;

                case Constants.Direction.Below:
                    y2++;
                    domino = new Domino(Input[y1][x1], Input[y2][x2]);
                    break;
            }

            if (Input[y1][x1] == 0) return false;
            if (Input[y2][x2] == 0) return false;

            var c1 = Board.CellAt(x1, y1);
            var c2 = Board.CellAt(x2, y2);

            if (c1.IsOccupied || c2.IsOccupied) return false;

            var canPlacePip1 = CanPlacePip(c1, domino.Pips[0]);
            var canPlacePip2 = CanPlacePip(c2, domino.Pips[1]);

            var canPlace = (canPlacePip1 && canPlacePip2);

            if (canPlace) Board.PlaceDomino(c1, c2, domino);

            return (canPlace);
        }

        protected bool TryCandidates(List<Cell> candidates, List<Constants.Direction> dirs)
        {
            var placed = false;
            foreach (var t in candidates)
            {
                var x1 = t.Coords.X;
                var y1 = t.Coords.Y;
                foreach (var d in dirs)
                {
                    placed |= TryPlaceDomino(x1, y1, d);
                    if (placed) break;
                }
            }
            return placed;
        }

    }
}