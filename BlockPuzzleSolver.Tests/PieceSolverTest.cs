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
            var solver = new ParallelSolver(pieces, new Vector3(2,2,2));
            Assert.AreEqual(2, solver.PieceVariants.Count);
            Assert.AreEqual(24, solver.PieceVariants[0].Count);
            Assert.AreEqual(24*2, solver.PieceVariants[1].Count);
        }

        [TestMethod]
        public void TestSolveSimple()
        {
            var solver = new ParallelSolver(piecesSimple, new Vector3(2, 2, 1));

            var results = solver.Solve();
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestSolveSimpleRotation()
        {
            var solver = new ParallelSolver(pieces, new Vector3(2, 2, 2));

            var results = solver.Solve();
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestSolveSimpleThree()
        {
            var solver = new ParallelSolver(piecesThree, new Vector3(2, 2, 2));

            var results = solver.Solve();
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestSolveSomaCube()
        {
            var solver = new ParallelSolver(somaCube, new Vector3(3, 3, 3));

            var results = solver.Solve();
            Assert.IsNotNull(results);
        }

        
    }
}
