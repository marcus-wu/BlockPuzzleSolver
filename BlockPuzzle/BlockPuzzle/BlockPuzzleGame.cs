using System;
using System.Collections.Generic;
using BlockPuzzleSolver;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlockPuzzle
{
    /// <summary>
    ///     This is the main type for your game
    /// </summary>
    public class BlockPuzzleGame : Game
    {
        public enum GameMode
        {
            Creator,
            Solver
        }

        private readonly Color[] colors = new[]
            {
                Color.Red,
                Color.Yellow,
                Color.LightBlue,
                Color.Green,
                Color.DarkGreen,
                Color.Teal,
                Color.Black,
                Color.Violet
            };

        private List<int> solution = null;

        private readonly RasterizerState wireFrameState = new RasterizerState
            {
                FillMode = FillMode.WireFrame,
                CullMode = CullMode.None,
            };

        private Camera camera;
        private GeometricPrimitive cube;
        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
        private Piece currentPiece;
        public Vector3 cursorPosition = Vector3.Zero;

        private GraphicsDeviceManager graphics;

        private SpriteFont hudFont;

        private KeyboardState lastKeyboardState;

        private MouseState lastMouseState;
        private GameMode mode = GameMode.Solver;

        private int pieceIdx = 0;
        private int pieceVariant = 0;
        private Action solvePuzzle;
        private PuzzleSolver solver;

        private bool solving = false;
        private SpriteBatch spriteBatch;
        public List<Vector3> tempPiecePoints = new List<Vector3>();


        public BlockPuzzleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            cube = new CubePrimitive(GraphicsDevice);
            camera = new Camera(MathHelper.Pi, GraphicsDevice.Viewport.AspectRatio, .2f, 10000f);
            hudFont = Content.Load<SpriteFont>("hudfont");


            solver = new ParallelSolver(new[]
                {
                    new Piece(new[,,]
                        {
                            {
                                {true, false, false, true},
                                {true, true, false, true},
                                {false, false, false, false}
                            },
                            {
                                {false, false, false, false},
                                {false, true, true, true},
                                {false, false, true, false}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {false, false, true, true},
                                {true, true, true, false},
                                {true, false, false, false},
                                {false, false, false, false}
                            },
                            {
                                {false, false, false, false},
                                {false, false, false, false},
                                {true, false, false, false},
                                {false, false, false, false}
                            },
                            {
                                {false, false, false, false},
                                {false, false, false, false},
                                {true, false, false, false},
                                {true, false, false, false}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {false, true, false, true},
                                {true, true, false, false},
                                {true, false, false, false}
                            },
                            {
                                {false, true, true, true},
                                {false, false, false, false},
                                {true, false, false, false}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {true, false, false},
                                {false, false, true},
                                {false, false, false},
                                {false, false, false}
                            },
                            {
                                {true, false, false},
                                {true, true, true},
                                {false, false, true},
                                {false, false, true}
                            },
                            {
                                {false, false, false},
                                {false, true, false},
                                {false, false, false},
                                {false, false, false}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {true, true, true},
                                {false, true, false},
                                {false, true, false},
                                {false, false, false}
                            },
                            {
                                {true, false, true},
                                {false, false, false},
                                {true, true, false},
                                {true, false, false}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {true, true, false, false},
                                {true, true, false, false},
                                {false, true, false, false},
                                {false, true, false, false}
                            },
                            {
                                {false, true, true, true},
                                {false, false, false, false},
                                {false, false, false, false},
                                {false, false, false, false}
                            }
                        }),
                    new Piece(new[,,]
                        {
                            {
                                {false, true, false, false},
                                {true, true, false, false},
                                {true, false, false, false},
                                {true, false, false, false}
                            },
                            {
                                {false, true, true, true},
                                {false, false, false, false},
                                {true, false, false, false},
                                {false, false, false, false}
                            }
                        })
                }, new Vector3(4, 4, 4));

            /*solver = new ParallelSolver(new[]
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
                }, new Vector3(3, 3, 3));
            ;*/

//            currentPiece = solver.PieceVariants[0][0];
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     all content.
        /// </summary>
        protected override void UnloadContent()
        {
            cube.Dispose();
            cube = null;
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (IsPressed(Keys.Escape))
                Exit();

            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            float timeDifference = (float) gameTime.ElapsedGameTime.TotalMilliseconds/1000.0f;
            ProcessInput(timeDifference);

            camera.Update(gameTime);
            if (IsPressed(Keys.Tab))
            {

                mode = (GameMode)((((int) mode) + 1)%(Enum.GetNames(typeof (GameMode)).Length));
            }

            if (mode == GameMode.Creator)
            {
                if (IsPressed(Keys.W))
                {
                    cursorPosition += Vector3.Up;
                }
                if (IsPressed(Keys.S))
                {
                    cursorPosition += Vector3.Down;
                }

                if (IsPressed(Keys.A))
                    cursorPosition += Vector3.Left;

                if (IsPressed(Keys.D))
                    cursorPosition += Vector3.Right;

                if (IsPressed(Keys.Q))
                    cursorPosition += Vector3.Forward;
                if (IsPressed(Keys.Z))
                    cursorPosition += Vector3.Backward;

                cursorPosition.X = MathHelper.Max(0, cursorPosition.X);
                cursorPosition.Y = MathHelper.Max(0, cursorPosition.Y);
                cursorPosition.Z = MathHelper.Max(0, cursorPosition.Z);

                if (IsPressed(Keys.Space))
                {
                    tempPiecePoints.Add(cursorPosition);
                }
                if (IsPressed(Keys.R))
                {
                    tempPiecePoints.Clear();
                }
                if (IsPressed(Keys.Enter))
                {
                    Vector3 size = Vector3.Zero;

                    foreach (Vector3 tempPiecePoint in tempPiecePoints)
                    {
                        size.X = MathHelper.Max(tempPiecePoint.X + 1, size.X);
                        size.Y = MathHelper.Max(tempPiecePoint.Y + 1, size.Y);
                        size.Z = MathHelper.Max(tempPiecePoint.Z + 1, size.Z);
                    }

                    var newPiece = new Piece(tempPiecePoints.ToArray(), size, Vector3.Zero);
                    Console.WriteLine(newPiece.ToArrayStr());
                }
            } 
            else if (mode == GameMode.Solver)
            {
                if (IsPressed(Keys.Enter) && !solving)
                {
                    solving = true;
                    solution = solver.Solve();
                    solving = false;
                }
            }

//            if (IsPressed(Keys.A))
//            {
//                pieceIdx = (pieceIdx + 1)%solver.PieceVariants.Count;
//                pieceVariant = 0;
//                currentPiece = solver.PieceVariants[pieceIdx][pieceVariant];
//            }
//
//            if (IsPressed(Keys.S))
//            {
//                pieceVariant = (pieceVariant + 1) % solver.PieceVariants[pieceIdx].Count;
//                currentPiece = solver.PieceVariants[pieceIdx][pieceVariant];
//            }

            

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(33/255f, 40/255f, 48/255f));


            GraphicsDevice.RasterizerState = wireFrameState;
//            cube.Draw(Matrix.Identity * Matrix.CreateScale(solver.BoundingBox.Max) * Matrix.CreateTranslation(solver.BoundingBox.Max/2), camera.View, camera.Projection, Color.Yellow);

            // Reset the fill mode renderstate.
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

//            cube.Draw(Matrix.Identity, camera.View, camera.Projection, Color.Red);

            cube.Draw(Matrix.Identity*Matrix.CreateScale(100, .1f, .1f)*Matrix.CreateTranslation(50, 0, 0), camera.View,
                      camera.Projection, Color.Red);
            cube.Draw(Matrix.Identity*Matrix.CreateScale(.1f, 100, .1f)*Matrix.CreateTranslation(0, 50, 0), camera.View,
                      camera.Projection, Color.Green);
            cube.Draw(Matrix.Identity*Matrix.CreateScale(.1f, .1f, 100)*Matrix.CreateTranslation(0, 0, 50), camera.View,
                      camera.Projection, Color.Blue);

//            DrawPiece(currentPiece);

            if (mode == GameMode.Creator)
            {
                DrawPoint(cursorPosition, new Color(1f, 1f, 0, .5f));

                foreach (Vector3 tempPiecePoint in tempPiecePoints)
                {
                    DrawPoint(tempPiecePoint, Color.Yellow);
                }
            }

            if (solution != null)
            {
                for (int i = 0; i < solution.Count; i++)
                {
                    int variant = solution[i];
                    DrawPiece(solver.PieceVariants[i][variant], colors[i%colors.Length]);
                }
            }

            string text = "Camera: " + camera.HorizontalAngle + " " + camera.VerticalAngle + " " + camera.Zoom;

            text += "\nPiece: " + (char)(65 + pieceIdx) + pieceVariant;
            text += "\nMode: " + mode.ToString();

            spriteBatch.Begin();
            spriteBatch.DrawString(hudFont, text, new Vector2(10, GraphicsDevice.Viewport.Height - 60), Color.White);

            spriteBatch.DrawString(hudFont, Log.GetLog(), new Vector2(10, 10), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawPiece(Piece piece, Color color)
        {
            if (piece == null)
                return;

            foreach (Vector3 point in piece.Layout)
            {
                DrawPoint(point, color);
            }
        }

        private void DrawPoint(Vector3 point, Color color)
        {
            Matrix world = Matrix.Identity;
            world.Translation = point + new Vector3(.5f, .5f, .5f);
            cube.Draw(world, camera.View, camera.Projection, color);
        }

        private bool IsPressed(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key) &&
                    lastKeyboardState.IsKeyUp(key));
        }

        private void ProcessInput(float amount)
        {
            camera.Zoom += 10*(currentMouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue)*amount;
            if (currentMouseState != lastMouseState && IsActive)
            {
                if (currentMouseState.RightButton == ButtonState.Pressed)
                {
                    camera.HorizontalAngle -= (currentMouseState.X - lastMouseState.X)*amount;
                    camera.VerticalAngle -= (currentMouseState.Y - lastMouseState.Y)*amount;
                }
            }
        }
    }
}