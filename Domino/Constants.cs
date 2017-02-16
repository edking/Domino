using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Lib
{
    public static class Constants
    {
        public enum Direction
        {
            Above,
            Below,
            Left,
            Right
        };

        public readonly static List<Direction> AllDirs = new List<Direction>
        {
            Direction.Above,
            Direction.Right,
            Direction.Below,
            Direction.Left
        };

        public readonly static List<Direction> VerticalDirs = new List<Direction>
        {
            Direction.Below,
            Direction.Above
        };

        public readonly static List<Direction> HorizontalDirs = new List<Direction>
        {
            Direction.Right,
            Direction.Left
        };

        public const int cMaxPasses = 5;

        public const int cMaxDepth = 100;

        public static readonly Random RNG = new Random();
    }
}
