using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BlockPuzzleSolver;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

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
            Inspector,
            Solve
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

        private List<int> solution;

        private readonly RasterizerState wireFrameState = new RasterizerState
            {
                FillMode = FillMode.WireFrame,
                CullMode = CullMode.None,
            };

        private Camera camera;
        private GeometricPrimitive cube;

        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
        public Vector3 CursorPosition = Vector3.Zero;

        private readonly GraphicsDeviceManager graphics;

        private SpriteFont hudFont;

        private KeyboardState lastKeyboardState;

        private MouseState lastMouseState;
        private GameMode mode = GameMode.Solve;

        private PuzzleSolver[] solvers;
        private int currentSolver;

        private bool solving;
        private SpriteBatch spriteBatch;
        public List<Vector3> TempPiecePoints = new List<Vector3>();
        private Puzzle currentPuzzle;
        private int pieceIdx;
        private int pieceVariant;


        public BlockPuzzleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
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

            solvers = new PuzzleSolver[]
                {
                    new RecursiveSingleThreadedSolver(), 
                    new ParallelSolver()
                };
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

            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            float timeDifference = (float) gameTime.ElapsedGameTime.TotalMilliseconds/1000.0f;

            ProcessInput(timeDifference);

            camera.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(33/255f, 40/255f, 48/255f));

            if ((mode == GameMode.Creator || mode == GameMode.Inspector) && currentPuzzle != null)
            {
                GraphicsDevice.RasterizerState = wireFrameState;
                cube.Draw(Matrix.Identity * Matrix.CreateScale(currentPuzzle.Bounding) * Matrix.CreateTranslation(currentPuzzle.Bounding / 2), camera.View, camera.Projection, Color.Yellow);
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            }
            
            cube.Draw(Matrix.Identity*Matrix.CreateScale(100, .05f, .05f)*Matrix.CreateTranslation(50, 0, 0), camera.View,
                      camera.Projection, Color.Red);
            cube.Draw(Matrix.Identity*Matrix.CreateScale(.05f, 100, .05f)*Matrix.CreateTranslation(0, 50, 0), camera.View,
                      camera.Projection, Color.Green);
            cube.Draw(Matrix.Identity*Matrix.CreateScale(.05f, .05f, 100)*Matrix.CreateTranslation(0, 0, 50), camera.View,
                      camera.Projection, Color.Blue);

//            DrawPiece(currentPiece);

            if (mode == GameMode.Creator)
            {
                DrawPoint(CursorPosition, new Color(1f, 1f, 0, .5f));

                foreach (Vector3 tempPiecePoint in TempPiecePoints)
                {
                    DrawPoint(tempPiecePoint, Color.Yellow);
                }
            } else if (mode == GameMode.Inspector)
            {
                if (currentPuzzle != null && currentPuzzle.Variants.Count > pieceIdx && currentPuzzle.Variants[0].Count > pieceVariant)
                {
                    DrawPiece(currentPuzzle.Variants[pieceIdx][pieceVariant], Color.Yellow);
                }
            }

            if (mode == GameMode.Solve && solution != null && currentPuzzle != null)
            {
                for (int i = 0; i < solution.Count; i++)
                {
                    int variant = solution[i];
                    DrawPiece(currentPuzzle.Variants[i][variant], colors[i%colors.Length]);
                }
            }

            string text = "Mode: " + mode;
            text += "\nPuzzle: " + ((currentPuzzle != null) ? currentPuzzle.ToString() : "None");
            text += "\nKeys: [Switch Mode] M [Load] L [Save] K";
            if (mode == GameMode.Creator)
            {
                text +=
                    "\nCreator Keys: [Cursor] W A S D Q E [Add Cube] Space [Add Piece] P [New Puzzle] N [Reset Piece] R";
            } else if (mode == GameMode.Solve)
            {
                text += "\nSolver: " + solvers[currentSolver].GetType().Name;
                text += "\nSolver Keys: [Change Solver] C [Solve] Enter";
            } else if (mode == GameMode.Inspector)
            {
                text += "\nCurrent Piece: " + (char) (65 + pieceIdx) + " Variant: " + (pieceVariant+1);
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(hudFont, text, new Vector2(10, GraphicsDevice.Viewport.Height - 100), Color.White);
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
            Matrix world = Matrix.Identity * Matrix.CreateScale(.99999999999f);
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

            if (IsPressed(Keys.M))
            {
                mode = (GameMode)((((int)mode) + 1) % (Enum.GetNames(typeof(GameMode)).Length));
            }
            if (IsPressed(Keys.L))
            {
                Load();
            }
            if (IsPressed(Keys.K))
            {
                Save();
            }

            if (mode == GameMode.Creator)
            {
                if (IsPressed(Keys.W))
                    CursorPosition += Vector3.Up;
                if (IsPressed(Keys.S))
                    CursorPosition += Vector3.Down;
                if (IsPressed(Keys.A))
                    CursorPosition += Vector3.Left;
                if (IsPressed(Keys.D))
                    CursorPosition += Vector3.Right;

                if (IsPressed(Keys.Q))
                    CursorPosition += Vector3.Forward;
                if (IsPressed(Keys.E))
                    CursorPosition += Vector3.Backward;

                CursorPosition.X = MathHelper.Max(0, CursorPosition.X);
                CursorPosition.Y = MathHelper.Max(0, CursorPosition.Y);
                CursorPosition.Z = MathHelper.Max(0, CursorPosition.Z);

                if (IsPressed(Keys.Space))
                    TempPiecePoints.Add(CursorPosition);
                if (IsPressed(Keys.R))
                    TempPiecePoints.Clear();

                if (IsPressed(Keys.N))
                {
                    currentPuzzle = new Puzzle();
                    Log.Add("Created new puzzle");
                    var prompt = new DimensionDialog();
                    prompt.ShowDialog(Control.FromHandle(Window.Handle));
                    if (prompt.DialogResult == DialogResult.OK)
                    {
                        Log.Add("Changed bounding to " + prompt.Result);
                        currentPuzzle.Bounding = prompt.Result;
                    }
                    
                }
                if (IsPressed(Keys.P))
                {
                    Vector3 size = Vector3.Zero;

                    foreach (Vector3 tempPiecePoint in TempPiecePoints)
                    {
                        size.X = MathHelper.Max(tempPiecePoint.X + 1, size.X);
                        size.Y = MathHelper.Max(tempPiecePoint.Y + 1, size.Y);
                        size.Z = MathHelper.Max(tempPiecePoint.Z + 1, size.Z);
                    }

                    var newPiece = new Piece(TempPiecePoints.ToArray(), size, Vector3.Zero);
                    currentPuzzle.Add(newPiece);
                }
            }
            else if (mode == GameMode.Solve)
            {
                if (IsPressed(Keys.C))
                {
                    currentSolver = (currentSolver + 1) % solvers.Length;
                }

                if (IsPressed(Keys.Enter) && !solving)
                {
                    solving = true;
                    solution = solvers[currentSolver].Solve(currentPuzzle);
                    solving = false;
                }
            } else if (mode == GameMode.Inspector)
            {
                if (IsPressed(Keys.D))
                {
                    pieceIdx++;
                    pieceVariant = 0;
                }
                if (IsPressed(Keys.A))
                {
                    pieceIdx--;
                    pieceVariant = 0;
                }

                if (IsPressed(Keys.S))
                {
                    pieceVariant--;
                }
                if (IsPressed(Keys.W))
                {
                    pieceVariant++;
                }
            }

            if (currentPuzzle != null && currentPuzzle.Variants != null)
            {
                if (pieceIdx < 0)
                {
                    pieceIdx += currentPuzzle.Variants.Count;
                }
                if (pieceVariant < 0)
                {
                    pieceVariant += currentPuzzle.Variants[pieceIdx].Count;
                }
                pieceIdx = pieceIdx % currentPuzzle.Variants.Count;
                pieceVariant = pieceVariant % currentPuzzle.Variants[pieceIdx].Count;
            }
            
        }

        private void Save()
        {
            bool ret = false;
            string filename = "";
            using (var saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "XML|*.xml";
                saveFileDialog1.Title = "Save Puzzle";
                saveFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != "")
                {
                    filename = saveFileDialog1.FileName;
                    ret = currentPuzzle.Save(filename);
                }
            }

            if (ret)
                Log.Add("Successfully saved " + filename);
            else
                Log.Add("Saving failed.");
        }

        private void Load()
        {
            using (var openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "XML|*.xml";
                openFileDialog1.Title = "Open Puzzle";
                openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (openFileDialog1.FileName != "")
                {
                    Log.Add("Loading puzzle...");
                    var puzzle = Puzzle.Load(openFileDialog1.FileName);
                    if (puzzle != null)
                    {
                        Log.Add("Success!");
                        currentPuzzle = puzzle;
                    }
                }
            }
        }
    }
}