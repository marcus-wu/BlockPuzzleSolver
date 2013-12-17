using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlockPuzzleSolver
{

    public class Piece
    {
        public Vector3[] Layout;
        public Vector3 Size;
        public Vector3 Offset;

        private HashSet<Vector3> hash;

        public Piece(Vector3[] layout, Vector3 size, Vector3 offset)
        {
            Layout = layout;
            Size = size;
            Offset = offset;

            hash = new HashSet<Vector3>(Layout);
        }

        public Piece(bool[,,] layout)
        {
            // set size
            Size.Z = layout.GetLength(0);
            Size.Y = layout.GetLength(1);
            Size.X = layout.GetLength(2);

            // offset defaults to 0,0,0
            Offset = Vector3.Zero;

            // create cube positions in piece
            int size = layout.GetLength(0)*layout.GetLength(1)*layout.GetLength(2);
            var points = new List<Vector3>(size);
            for (int z = 0; z < layout.GetLength(0); z++)
            {
                for (int y = 0; y < layout.GetLength(1); y++)
                {
                    
                    for (int x = 0; x < layout.GetLength(2); x++)
                    {
                        if (layout[z, y, x])
                        {
                            Vector3 v = Vector3.Zero;
                            v.X = x;
                            v.Y = y;
                            v.Z = z;
                            points.Add(v);
                        }
                            
                    }
                }
            }
            Layout = points.ToArray();

            hash = new HashSet<Vector3>(Layout);
        }

        public string ToArrayStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            for (int z = 0; z < Size.Z; z++)
            {
                sb.AppendLine("    {");
                for (int y = 0; y < Size.Y; y++)
                {
                    sb.Append("        {");
                    for (int x = 0; x < Size.X; x++)
                    {
                        if (x != 0)
                        {
                            sb.Append(", ");
                        }

                        sb.Append(hash.Contains(new Vector3(x, y, z)) ? "true" : "false");
                    }
                    sb.AppendLine("}" + ((y < Size.Y - 1) ? "," : ""));
                }
                sb.AppendLine("    }" + ((z < Size.Z - 1) ? "," : ""));
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        public Piece GenerateRotate(float x, float y, float z)
        {
            // Create rotation matrix
            var rotMatrix = Matrix.CreateFromYawPitchRoll(y, x, z);

            // generate new sizes
            Vector3 size;
            Vector3.Transform(ref Size, ref rotMatrix, out size);
            size.X = (int)Math.Round(size.X);
            size.Y = (int)Math.Round(size.Y);
            size.Z = (int)Math.Round(size.Z);

            // align to center
            Vector3 offset = Vector3.Zero;
            if (size.X < 0)
            {
                size.X = -size.X;
                offset.X = size.X - 1;
            }
            if (size.Y < 0)
            {
                size.Y = -size.Y;
                offset.Y = size.Y - 1;
            }
            if (size.Z < 0)
            {
                size.Z = -size.Z;
                offset.Z = size.Z - 1;
            }
            rotMatrix.Translation = offset;

            // generate new points
            var points = new List<Vector3>(Layout.Length);
            for (int i = 0; i < Layout.Length; i++)
            {
                Vector3 result;
                Vector3.Transform(ref Layout[i], ref rotMatrix, out result);
                result.X = (int)Math.Round(result.X);
                result.Y = (int)Math.Round(result.Y);
                result.Z = (int)Math.Round(result.Z);
                points.Add(result);
            }

            return new Piece(points.ToArray(), size, Vector3.Zero);
        }

        public Piece GenerateMove(float x, float y, float z)
        {
            Vector3 offset = Vector3.Zero;
            offset.X = x;
            offset.Y = y;
            offset.Z = z;

            // generate new points
            var points = new List<Vector3>(Layout.Length);
            for (int i = 0; i < Layout.Length; i++)
            {
                Vector3 result = Layout[i];
                Vector3.Add(ref result, ref offset, out result);
                result.X = (int)Math.Round(result.X);
                result.Y = (int)Math.Round(result.Y);
                result.Z = (int)Math.Round(result.Z);
                points.Add(result);
            }

            return new Piece(points.ToArray(), Size, offset);
        }

        public Piece[] GenerateAllOrientations()
        {
            var orientations = new List<Piece>(6*4);

            for (float x = 0; x < MathHelper.TwoPi - .1f; x += MathHelper.PiOver2)
                for (float z = 0; z < MathHelper.TwoPi - .1f; z += MathHelper.PiOver2)
                {
                    orientations.Add(GenerateRotate(x, 0, z));
                }

            var ys = new[] {MathHelper.PiOver2, -MathHelper.PiOver2};

            foreach (float y in ys)
            {
                for (float z = 0; z < MathHelper.TwoPi - .1f; z += MathHelper.PiOver2)
                {
                    orientations.Add(GenerateRotate(0, y, z));
                }
            }

            return orientations.ToArray();
        }

        public Piece[] GenerateAllWithinBounding(Vector3 bounding)
        {
            Vector3 delta;
            Vector3.Subtract(ref bounding, ref Size, out delta);

            var pieces = new List<Piece>();

            for (int z = 0; z <= (int) Math.Round(delta.Z); z++)
            {
                for (int y = 0; y <= (int)Math.Round(delta.Y); y++)
                {
                    for (int x = 0; x <= (int)Math.Round(delta.X); x++)
                    {
                        pieces.Add(GenerateMove(x,y,z));
                    }
                }
            }

            return pieces.ToArray();
        }

        public bool Contains(ref Vector3 vector)
        {
            return hash.Contains(vector);
        }

        public bool Collides(Piece other)
        {
            return other.Layout.Any(otherPoint => Contains(ref otherPoint));
        }
    }
}
