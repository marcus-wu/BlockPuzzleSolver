using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace BlockPuzzleSolver.Tests
{
    [TestClass]
    public class PieceSolverTest
    {
        readonly Piece[] piecesSimple = new[]
                {
                    new Piece(new[,,]
                        {
                            {
                                {true, false},
                                {true, true}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {true}
                            }
                        })
                };

        readonly Piece[] pieces = new[]
                {
                    new Piece(new[,,]
                        {
                            {
                                {false, true},
                                {true, true}
                            },
                            {
                                {false, true},
                                {false, true}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {false},
                                {true}
                            },
                            {
                                {true},
                                {true}
                            }
                        })
                };

        readonly Piece[] piecesThree = new[]
                {
                    new Piece(new[,,]
                        {
                            {
                                {false, true},
                                {true, true}
                            },
                            {
                                {false, true},
                                {false, false}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {false},
                                {true}
                            },
                            {
                                {false},
                                {true}
                            }
                        }),

                    new Piece(new[,,]
                        {
                            {
                                {true, true}
                            }
                        })
                };

        readonly Piece[] somaCube = new[]
                {
                    new Piece(new[,,]
                        {
                            {
                                {false, true},
                                {true, true}
                            },
                            {
                                {false, false},
                                {true, false}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {true, false},
                                {true, true}
                            }
                        }),
                        new Piece(new[,,]
                        {
                            {
                                {false, true, false},
                                {true, true, true}
                            }
                        }),
                        new Piece(new[,,]
                        {
                            {
                                {false, true, true},
                                {true, true, false}
                            }
                        }),
                        new Piece(new[,,]
                        {
                            {
                                {true, true, true},
                                {false, false, true}
                            }
                        }),
                        new Piece(new[,,]
                        {
                            {
                                {false, true},
                                {true, true}
                            },
                            {
                                {false, true},
                                {false, false}
                            }
                        }),
                        new Piece(new[,,]
                        {
                            {
                                {false, true},
                                {true, true}
                            },
                            {
                                {false, false},
                                {false, true}
                            }
                        })
                };

        [TestMethod]
        public void TestConstructor()
        {
            var solver = new PuzzleSolver(pieces, new Vector3(2,2,2));
            Assert.AreEqual(2, solver.PieceVariants.Count);
            Assert.AreEqual(24, solver.PieceVariants[0].Count);
            Assert.AreEqual(24*2, solver.PieceVariants[1].Count);
        }

        [TestMethod]
        public void TestSolveSimple()
        {
            var solver = new PuzzleSolver(piecesSimple, new Vector3(2, 2, 1));

            solver.Solve();
        }

        [TestMethod]
        public void TestSolveSimpleRotation()
        {
            var solver = new PuzzleSolver(pieces, new Vector3(2, 2, 2));

            solver.Solve();
        }

        [TestMethod]
        public void TestSolveSimpleThree()
        {
            var solver = new PuzzleSolver(piecesThree, new Vector3(2, 2, 2));

            solver.Solve();
        }

        [TestMethod]
        public void TestSolveSomaCube()
        {
            var solver = new PuzzleSolver(somaCube, new Vector3(3, 3, 3));

            solver.Solve();
        }

        
    }
}
