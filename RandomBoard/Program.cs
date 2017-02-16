using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBoard
{
    class Program
    {
        public static readonly Random RNG = new Random();
        static void Main(string[] args)
        {
            var maxPips = Convert.ToInt32(args[0]);
            var width = Convert.ToInt32(args[1]);
            var height = Convert.ToInt32(args[2]);
            var file = args[3];

            using (var writer = new StreamWriter(file))
            {
                for (var y = 0; y < height; y++)
                {
                    var sb = new StringBuilder();
                    for (var x = 0; x < width; x++)
                    {
                        sb.Append(RNG.Next((maxPips+1)));
                    }
                    writer.WriteLine(sb.ToString());
                }
            }
        }
    }
}
