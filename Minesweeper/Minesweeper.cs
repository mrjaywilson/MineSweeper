namespace Minesweeper
{
    /// <summary>
    /// Driver Class
    /// </summary>
    /// 
    /// <remarks>
    /// Desc.:      Driver class that tests the code.
    /// 
    /// Author:     Jay Wilson
    /// Date:       12/17/18
    /// Version:    2.0
    /// </remarks>
    class Minesweeper
    {
        static void Main(string[] args)
        {
            // Declare and initialize game object
            Game game = new Game(10);

            // Start the game
            game.PlayGame();
        }
    }
}
