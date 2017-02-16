using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Domino.Lib
{
    public class Board
    {
        private readonly List<List<Cell>> _cells;

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new List<List<Cell>>();

            for (var y = 0; y < height; y++)
            {
                var l = new List<Cell>();
                for (var x = 0; x < width; x++)
                    l.Add(new Cell(x,y));

                _cells.Add(l);
            }
        }

        public void Clear()
        {
            foreach (var row in _cells)
                foreach (var cell in row)
                    cell.Clear();
        }

        public int CoveredCells
        {
            get { return _cells.Sum(l => l.Count(c => c.IsOccupied)); }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Cell CellAt(int x, int y)
        {
            return _cells[y][x];
        }

        public int PipsAt(int x, int y)
        {
            return _cells[y][x].Pips;
        }

        public List<Cell> OccupiedCells()
        {
            return (from y in _cells
                from x in y
                where x.IsOccupied
                select x).ToList();
        }

        public List<Cell> UnoccupiedCells()
        {
            return (from y in _cells
                    from x in y
                    where !x.IsOccupied
                    select x).ToList();
        }

        public void PlaceDomino(Cell cell1, Cell cell2, Domino d)
        {
            cell1.Domino = d;
            cell1.IsOccupied = true;
            cell1.Pips = d.Pips[0];

            cell2.Domino = d;
            cell2.IsOccupied = true;
            cell2.Pips = d.Pips[1];
        }

        public List<Constants.Direction> NeighborDirections(Cell c)
        {
            var dirs = new List<Constants.Direction>();
            if (c.Coords.X > 0) dirs.Add(Constants.Direction.Left);
            if (c.Coords.X < Width - 1) dirs.Add(Constants.Direction.Right);
            if (c.Coords.Y > 0) dirs.Add(Constants.Direction.Above);
            if (c.Coords.Y < Height - 1) dirs.Add(Constants.Direction.Below);
            return dirs;
        }


    }
}