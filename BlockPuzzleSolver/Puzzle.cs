using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace BlockPuzzleSolver
{
    public class Puzzle
    {
        [XmlElement("Bounding")]
        public Vector3 Bounding { get; set; }

        [XmlElement("Pieces", typeof(List<bool[][][]>))]
        public List<bool[][][]> Pieces { get; set; }

        [XmlIgnore]
        public List<List<Piece>> Variants { get; set; }

        public Puzzle()
        {
            Pieces = new List<bool[][][]>();
        }

        public Puzzle(List<bool[][][]> pieces, Vector3 bounding)
        {
            Pieces = pieces;
            Bounding = bounding;
            Variants = GenerateAllVariants();
        }

        public List<List<Piece>> GenerateAllVariants()
        {
            var pieceGroup = new List<List<Piece>>();
            foreach (var pieceArray in Pieces)
            {
                var multiDimensionArrayPiece = new bool[pieceArray.Length, pieceArray[0].Length, pieceArray[0][0].Length];

                for (var z = 0; z < pieceArray.Length; z++)
                {
                    for (var y = 0; y < pieceArray[z].Length; y++)
                    {
                        for (var x = 0; x < pieceArray[z][y].Length; x++)
                        {
                            multiDimensionArrayPiece[z, y, x] = pieceArray[z][y][x];
                        }
                    }
                }

                var piece = new Piece(multiDimensionArrayPiece);

                var orientations = piece.GenerateAllOrientations();
                var variants = new List<Piece>();

                foreach (var oPiece in orientations)
                {
                    variants.AddRange(oPiece.GenerateAllWithinBounding(Bounding));
                }
                pieceGroup.Add(variants);
            }

            return pieceGroup;
        }

        public bool Save(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof (Puzzle));
                using (TextWriter textWriter = new StreamWriter(filePath))
                {
                    serializer.Serialize(textWriter, this);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static Puzzle Load(string filePath)
        {
            var deserializer = new XmlSerializer(typeof(Puzzle));
            Puzzle puzzle;
            using (TextReader textReader = new StreamReader(filePath))
            {
                puzzle = (Puzzle)deserializer.Deserialize(textReader);
            }

            if (puzzle != null)
            {
                puzzle.Variants = puzzle.GenerateAllVariants();
            }

            return puzzle;
        }

        public void Add(Piece piece)
        {
            Pieces.Add(piece.ToArray());
            Variants = GenerateAllVariants();
        }

        public override string ToString()
        {
            return "[Bounding] " + Bounding + " [Pieces] " + ((Variants != null) ? Variants.Count.ToString() : "Empty");
        }
    }
}
