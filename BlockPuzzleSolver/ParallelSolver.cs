using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BlockPuzzleSolver
{
    public class ParallelSolver : PuzzleSolver
    {
        public ParallelSolver(Piece[] pieces, Vector3 bounding)
            : base(pieces, bounding)
        {
        }

        public override List<int> Solve()
        {
            var hash = new HashSet<Vector3>();
            DateTime start = DateTime.Now;

            var first = PieceVariants[0];

            List<int> result = null;

            Parallel.For(0, first.Count, (i, loopState) =>
                {
                    var newHash = new HashSet<Vector3>(hash);
                    newHash.UnionWith(first[i].Layout);

                    var newList = new List<int>();
                    newList.Add(i);
                    List<int> subResult = SolveHelper(newHash, 1, newList);
                    if (subResult != null)
                    {
                        result = subResult;
                        loopState.Stop();
                    }
                });

            if (result != null)
            {
                Log.Add("Solution:");
                for (int i = 0; i < result.Count; i++)
                {
                    int idx = result[i];
                    Piece piece = PieceVariants[i][idx];
                    Log.Add("Piece " + (char) (i + 65) + ":");
                    foreach (Vector3 v in piece.Layout)
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

        private List<int> SolveHelper(HashSet<Vector3> hash, int num, List<int> history)
        {
            List<Piece> pieceGroup = PieceVariants[num];

            for (int i = 0; i < pieceGroup.Count; i++)
            {

                Piece pieceVariant = pieceGroup[i];

                if (hash.Overlaps(pieceVariant.Layout))
                {
                    continue;
                }

                var newList = new List<int>(history);
                newList.Add(i);


                if (num + 1 >= PieceVariants.Count)
                {
                    return newList;
                }

                var newHash = new HashSet<Vector3>(hash);
                newHash.UnionWith(pieceVariant.Layout);

                List<int> possibleSolution = SolveHelper(newHash, num + 1, newList);

                if (possibleSolution != null)
                {
                    return possibleSolution;
                }
            }
            return null;
        }
    }
}