using System;

namespace CrazyShooter
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameRoot game = new CrazyShooter.GameRoot())
            {
                game.Run();
            }
        }
    }
#endif
}

