using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace BlockPuzzleSolver.Tests
{
    [TestClass]
    public class PieceSolverTest
    {
        private readonly bool[][][][] pieces = new[]
            {
                new[]
                    {
                        new[]
                            {
                                new[] {false, true},
                                new[] {true, true}
                            },
                        new[]
                            {
                                new[] {false, true},
                                new[] {false, true}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {false},
                                new[] {true}
                            },
                        new[]
                            {
                                new[] {true},
                                new[] {true}
                            }
                    }
            };

        private readonly bool[][][][] piecesSimple = new[]
            {
                new[]
                    {
                        new[]
                            {
                                new[] {true, false}, 
                                new[] {true, true}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {true}
                            }
                    }
            };

        private readonly bool[][][][] piecesThree = new[]
            {
                new[]
                    {
                        new[]
                            {
                                new[] {false, true},
                                new[] {true, true}
                            },
                        new[]
                            {
                                new[] {false, true},
                                new[] {false, false}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {false},
                                new[] {true}
                            },
                        new[]
                            {
                                new[] {false},
                                new[] {true}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {true, true}
                            }
                    }
            };

        private readonly bool[][][][] somaCube = new[]
            {
                new[]
                    {
                        new[]
                            {
                                new[] {false, true},
                                new[] {true, true}
                            },
                        new[]
                            {
                                new[] {false, false},
                                new[] {true, false}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {true, false},
                                new[] {true, true}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {false, true, false},
                                new[] {true, true, true}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {false, true, true},
                                new[] {true, true, false}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {true, true, true},
                                new[] {false, false, true}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {false, true},
                                new[] {true, true}
                            },
                        new[]
                            {
                                new[] {false, true},
                                new[] {false, false}
                            }
                    },
                new[]
                    {
                        new[]
                            {
                                new[] {false, true},
                                new[] {true, true}
                            },
                        new[]
                            {
                                new[] {false, false},
                                new[] {false, true}
                            }
                    }
            };

        [TestMethod]
        public void TestConstructor()
        {
            var puzzle = new Puzzle(new List<bool[][][]>(pieces), new Vector3(2, 2, 2));
            Assert.AreEqual(2, puzzle.Variants.Count);
            Assert.AreEqual(24, puzzle.Variants[0].Count);
            Assert.AreEqual(24 * 2, puzzle.Variants[1].Count);
        }

        [TestMethod]
        public void TestSolveSimple()
        {
            var puzzle = new Puzzle(new List<bool[][][]>(piecesSimple), new Vector3(2, 2, 1));

            var solver = new RecursiveSingleThreadedSolver();
            List<int> results = solver.Solve(puzzle);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestSolveSimpleRotation()
        {
            var puzzle = new Puzzle(new List<bool[][][]>(pieces), new Vector3(2, 2, 2));
            var solver = new RecursiveSingleThreadedSolver();

            List<int> results = solver.Solve(puzzle);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestSolveSimpleThree()
        {
            var puzzle = new Puzzle(new List<bool[][][]>(piecesThree), new Vector3(2, 2, 2));
            var solver = new RecursiveSingleThreadedSolver();

            List<int> results = solver.Solve(puzzle);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestSolveSomaCube()
        {
            var puzzle = new Puzzle(new List<bool[][][]>(somaCube), new Vector3(3, 3, 3));
            var solver = new RecursiveSingleThreadedSolver();

            List<int> results = solver.Solve(puzzle);
            Assert.IsNotNull(results);
        }
    }
}