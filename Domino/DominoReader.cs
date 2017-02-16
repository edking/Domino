using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Domino.Lib
{
    public static class DominoReader
    {
        public static List<List<int>> Read(string fn)
        {
            var rv = new List<List<int>>();
            using (var rdr = new StreamReader(fn))
            {
                while (!rdr.EndOfStream)
                {
                    var line = rdr.ReadLine();
                    if (line == null) continue;
                    var l = line.Select((t, i) => Convert.ToInt32(line.Substring(i, 1))).ToList();
                    rv.Add(l);
                }
            }
            return rv;
        }
    }
}