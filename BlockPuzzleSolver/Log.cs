using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlockPuzzleSolver
{
    public static class Log
    {
        public static int MaxHeight = 12;
        public static List<string> log = new List<string>();


        public static void Add(string str)
        {
            while (log.Count > 12)
            {
                log.RemoveAt(0);
            }
            log.Add(str);
        }

        public static string GetLog()
        {
            var sb = new StringBuilder();
            foreach (var str in log)
            {
                sb.AppendLine(str);
            }
            return sb.ToString();
        }
    }
}
