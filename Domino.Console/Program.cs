using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Domino.Lib;
using Domino.Lib.Brains;

namespace Domino.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fn = args[0];
            var input = DominoReader.Read(fn);
            if (input.Any())
            {
                var brain = new CompositeBrain(input);
                brain.Parse();
                var board = brain.Board;
                PrintBoard(board);
            }
        }

        private static void PrintBoard(Board b)
        {
            using (var writer = new StreamWriter("d:\\tmp\\output.txt"))
            {
                for (var y = 0; y < b.Height; y++)
                {
                    var sb = new StringBuilder();
                    for (var x = 0; x < b.Width; x++)
                    {
                        var c1 = b.CellAt(x, y);

                        sb.Append((c1.IsOccupied ? b.PipsAt(x, y).ToString() : " "));
                    }
                    writer.WriteLine(sb.ToString());
                }
            }
        }
    }
}