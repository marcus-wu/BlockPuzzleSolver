using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace BlockPuzzleSolver.Tests
{
    [TestClass]
    public class PieceTest
    {
        [TestMethod]
        public void TestLayoutConstruction()
        {
            var testLayout = new[,,]
                {
                    {
                        { true, false, false},
                        { true, true, false},
                        { false, false, false}
                    },
                    {
                        { true, true, true},
                        { false, true, false},
                        { false, true, true}
                    },
                    {
                        { false, true, true},
                        { false, false, false},
                        { false, false, true}
                    }
                };
            var expected = new[]
                {
                    new Vector3(0,0,0), new Vector3(0,1,0), new Vector3(1,1,0), 
                    
                    new Vector3(0,0,1), new Vector3(1,0,1), new Vector3(2,0,1), 
                    new Vector3(1,1,1), new Vector3(1,2,1), new Vector3(2,2,1), 

                    new Vector3(1,0,2), new Vector3(2,0,2), new Vector3(2,2,2)
                };
            Piece piece = new Piece(testLayout);

            for (int i = 0; i < piece.Layout.Length; i++)
            {
                var a = expected[i];
                var b = piece.Layout[i];
                Assert.IsTrue(a.Intersects(b), "Failed comparing " + a + " and " + b + " at index " + i);
            }

            Assert.IsTrue(piece.Size.Intersects(new Vector3(3,3,3)), "Size should be 3x3x3");
        }

        [TestMethod]
        public void TestRotation()
        {
            var testLayout = new[,,]
                {
                    {
                        {true, true}
                    },
                    {
                        {true, true}
                    },
                    {
                        {true, true}
                    }
                };

            Piece piece = new Piece(testLayout);
            Assert.IsTrue(piece.Size.Intersects(new Vector3(2, 1, 3)), "Size should be 2x1x3");

            Piece result = piece.GenerateRotate(MathHelper.PiOver2, 0, 0);
            Assert.IsTrue(result.Size.Intersects(new Vector3(2, 3, 1)), "Size should be 2x3x1");

            result = result.GenerateRotate(MathHelper.PiOver2, 0, 0);
            Assert.IsTrue(result.Size.Intersects(new Vector3(2, 1, 3)), "Size should be 2x1x3");

            result = result.GenerateRotate(MathHelper.PiOver2, 0, 0);
            Assert.IsTrue(result.Size.Intersects(new Vector3(2, 3, 1)), "Size should be 2x3x1");

            result = result.GenerateRotate(MathHelper.PiOver2, 0, 0);
            Assert.IsTrue(result.Size.Intersects(new Vector3(2, 1, 3)), "Size should be 2x1x3");

            ////

            result = piece.GenerateRotate(0, MathHelper.PiOver2, 0);
            Assert.IsTrue(result.Size.Intersects(new Vector3(3, 1, 2)), "Size should be 3x1x2");

            //

            result = piece.GenerateRotate(0, 0, MathHelper.PiOver2);
            Assert.IsTrue(result.Size.Intersects(new Vector3(1, 2, 3)), "Size should be 2x3x2");

            //
            result = piece.GenerateRotate(0, 0, MathHelper.PiOver2);
            result = result.GenerateRotate(0, MathHelper.PiOver2, 0);


            var result2 = piece.GenerateRotate(0, MathHelper.PiOver2, MathHelper.PiOver2);
            Assert.IsTrue(result.Size.Intersects(result2.Size));
        }

        [TestMethod]
        public void TestOrientations()
        {
            var testLayout = new[, ,]
                {
                    {
                        {true, true}
                    },
                    {
                        {true, true}
                    },
                    {
                        {true, true}
                    }
                };

            Piece piece = new Piece(testLayout);

            Piece[] generateAllOrientations = piece.GenerateAllOrientations();

            Assert.AreEqual(generateAllOrientations.Length, 24);
        }

        [TestMethod]
        public void TestGenerateAllWithinBounding()
        {
            var testLayout = new[, ,]
                {
                    {
                        {true, true}
                    },
                    {
                        {true, true}
                    },
                    {
                        {true, true}
                    }
                };

            Piece piece = new Piece(testLayout);

            Piece[] pieces = piece.GenerateAllWithinBounding(new Vector3(2,1,3));
            Assert.AreEqual(pieces.Length, 1);

            pieces = piece.GenerateAllWithinBounding(new Vector3(2, 2, 3));
            Assert.AreEqual(pieces.Length, 2);

            pieces = piece.GenerateAllWithinBounding(new Vector3(2, 2, 4));
            Assert.AreEqual(pieces.Length, 4);

            pieces = piece.GenerateAllWithinBounding(new Vector3(3, 2, 4));
            Assert.AreEqual(pieces.Length, 8);
        }

        [TestMethod]
        public void TestHashSetTest()
        {
            var testLayout = new[,,]
                {
                    {
                        {true, true}
                    },
                    {
                        {true, true}
                    },
                    {
                        {true, true}
                    }
                };

            Piece piece = new Piece(testLayout);

            var v = Vector3.Zero;
            Assert.IsTrue(piece.Contains(ref v));

            v = new Vector3(1, 0, 0);
            Assert.IsTrue(piece.Contains(ref v));

            v = new Vector3(0, 0, 1);
            Assert.IsTrue(piece.Contains(ref v));

            v = new Vector3(1, 0, 1);
            Assert.IsTrue(piece.Contains(ref v));

            v = new Vector3(0, 0, 2);
            Assert.IsTrue(piece.Contains(ref v));

            v = new Vector3(1, 0, 2);
            Assert.IsTrue(piece.Contains(ref v));
        }
    }
}
