using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BlockPuzzleSolver
{
    public class RecursiveSingleThreadedSolver : PuzzleSolver
    {
        public RecursiveSingleThreadedSolver(Piece[] pieces, Vector3 bounding) : base(pieces, bounding)
        {
        }

        public override List<int> Solve()
        {
            var hash = new HashSet<Vector3>();
            var start = DateTime.Now;

            var result = SolveHelper(hash, 0, new List<int>(), "");
            

            if (result != null)
            {
                
                Log.Add("Solution:");
                for (int i = 0; i < result.Count; i++)
                {
                    var idx = result[i];
                    var piece = PieceVariants[i][idx];
                    Log.Add("Piece " + (char)(i + 65) + ":");
                    foreach (var v in piece.Layout)
                    {
                        Log.Add("\t" + v);
                    }
                }
                Log.Add("Elapsed: " + (DateTime.Now - start).ToPrettyFormat());
                
                return result;
            }

            Log.Add("Elapsed: " + (DateTime.Now - start).ToPrettyFormat());
            Log.Add("No solutions found.");
            return null;
        }

        private List<int> SolveHelper(HashSet<Vector3> hash, int num, List<int> history, string tabs)
        {
            var pieceGroup = PieceVariants[num];
            var newTabs = tabs + "\t";

            for (int i = 0; i < pieceGroup.Count; i++)
            {
                var newMsg = newTabs + (char)(num + 65) + i;
                var msg = newMsg + " [" + pieceGroup.Count + "]";

                var pieceVariant = pieceGroup[i];


                if (hash.Overlaps(pieceVariant.Layout))
                {
                    Log.Add(msg + " doesn't fit");
                    continue;
                }

                var newList = new List<int>(history);
                newList.Add(i);

                Log.Add(msg + " fits");

                if (num + 1 >= PieceVariants.Count)
                {
                    Log.Add(newTabs + "All pieces analyzed");
                    return newList;
                }

                var newHash = new HashSet<Vector3>(hash);
                newHash.UnionWith(pieceVariant.Layout);

                var possibleSolution = SolveHelper(newHash, num + 1, newList, newMsg);

                if (possibleSolution != null)
                {
                    Log.Add(newTabs + "Found solution");
                    return possibleSolution;
                }
            }
            Log.Add(newTabs + "giving up");
            return null;
        }
    }
}
