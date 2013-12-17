using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BlockPuzzleSolver
{
    public static class Extensions
    {
        public const float EqualityVariance = 0.0000001f;

        public static bool Intersects(this Vector3 a, Vector3 b)
        {
            return Math.Abs(a.X - b.X) < EqualityVariance &&
                   Math.Abs(a.Y - b.Y) < EqualityVariance &&
                   Math.Abs(a.Z - b.Z) < EqualityVariance;
        }

        public static string ToPrettyFormat(this TimeSpan span)
        {

            if (span == TimeSpan.Zero) return "0 minutes";

            var sb = new StringBuilder();
            if (span.Days > 0)
                sb.AppendFormat("{0} day{1} ", span.Days, span.Days > 1 ? "s" : String.Empty);
            if (span.Hours > 0)
                sb.AppendFormat("{0} hr{1} ", span.Hours, span.Hours > 1 ? "s" : String.Empty);
            if (span.Minutes > 0)
                sb.AppendFormat("{0} min{1} ", span.Minutes, span.Minutes > 1 ? "s" : String.Empty);
            if (span.Seconds > 0)
                sb.AppendFormat("{0} sec{1} ", span.Seconds, span.Seconds > 1 ? "s" : String.Empty);
            if (span.Milliseconds > 0)
                sb.AppendFormat("{0} ms", span.Milliseconds);
            return sb.ToString();

        }
    }
}
