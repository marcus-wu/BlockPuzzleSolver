using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BlockPuzzleSolver
{
    public abstract class PuzzleSolver
    {
        public List<List<Piece>> PieceVariants;
        public BoundingBox BoundingBox;

        protected PuzzleSolver(Piece[] pieces, Vector3 bounding)
        {
            BoundingBox = new BoundingBox(Vector3.Zero, bounding);
            PieceVariants = GenerateAllVariants(pieces, bounding);
        }

        public List<List<Piece>> GenerateAllVariants(Piece[] pieces, Vector3 bounding)
        {
            var pieceGroup = new List<List<Piece>>();
            foreach (var piece in pieces)
            {

                var orientations = piece.GenerateAllOrientations();
                var variants = new List<Piece>();

                foreach (var oPiece in orientations)
                {
                    variants.AddRange(oPiece.GenerateAllWithinBounding(bounding));
                }
                pieceGroup.Add(variants);
            }

            return pieceGroup;
        }

        public abstract List<int> Solve();

    }
}
