using System;

namespace BlockPuzzle
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BlockPuzzleGame game = new BlockPuzzleGame())
            {
                game.Run();
            }
        }
    }
#endif
}

