using System;

namespace Minesweeper
{
    /// <summary>
    /// MineSweeper class.
    /// </summary>
    /// 
    /// <remarks>
    /// Descr.:     The main MineSweeper class that inherits from Grid and IPlayable.
    /// 
    /// Author:     Jay Wilson
    /// Date:       12/17/18
    /// Version:    2.0
    /// </remarks>
    class Game : Grid, IPlayable
    {
        // The number of cells already visited.
        private int visitedCount = 0;
        private int liveCount = 0;

        /// <summary>
        /// Constructor that accepts one argument to set size of game board.
        /// </summary>
        /// <param name="size">Size of game board.</param>
        public Game(int size)
            : base(size) { }

        /// <summary>
        /// Method to play the game.
        /// </summary>
        public void PlayGame()
        {
            // Activate random cells as "live" with given percentage perameter.
            liveCount = ActivateRandomCells(20);

            // Status of the input to warn user that given input is incorrect.
            string inputStatus = "";

            // Game Loop
            while (true)
            {
                // Show Grid
                RevealGrid();

                // Show status. Will show blank of nothing to report.
                Console.WriteLine(inputStatus + Environment.NewLine);

                // Get input for X from user...
                Console.Write("Enter X: ");
                int x = -1;
                int.TryParse(Console.ReadLine(), out x);

                // Get input for Y from user...
                Console.Write("Enter Y: ");
                int y = -1;
                int.TryParse(Console.ReadLine(), out y);

                // Additional input handling to avoid errors and
                // other gameplay issues.
                if (x > Size - 1 || y > Size - 1)
                {
                    inputStatus = "Invalid value(s). Try again.";
                    continue;
                }
                else if (x == -1 || y == -1)
                {
                    inputStatus = "You must use numbers only within the range of 0 and " + Size;
                }
                else
                {
                    inputStatus = "";
                }

                var location = Map[y, x];

                // Check if the location has been visited
                if (!location.Visited)
                {
                    // Reveal area that is safe to connecting blocks
                    RevealSafeConnectingBlocks(location);

                    // Check if the number of visited locations equals the number
                    // of available cells. If it does, then player won!
                    if (visitedCount == (Math.Pow(Size, 2) - liveCount))
                    {
                        // Call RevealGrid() from super class
                        base.RevealGrid();

                        // Let player know the end has come...
                        Console.WriteLine(Environment.NewLine + "Congratulations! You Won!");
                        break;
                    }

                    // Check if the player stepped on a mine.
                    if (!location.Live)
                    {
                        // If not, keep going.
                        continue;
                    }
                    else
                    {
                        // Call RevealGrid() from super class
                        base.RevealGrid();

                        // Let player know the end has come...
                        Console.WriteLine(Environment.NewLine + "Excellent try, but you lost!");
                        break;
                    }
                }
            }

            // Check if player wants to try again.
            Console.WriteLine(Environment.NewLine + "Play Again? (Y/N): ");

            // Get input.
            string answer = Console.ReadLine();

            // If user selects 'y' or 'Y', then play again.
            // Anything else will end the application.
            if (answer.ToLower().Equals("y"))
            {
                visitedCount = 0;   // Reset visited cells.
                ResetCells();       // Reset all cells to origin.
                PlayGame();         // Play the game again.
            }
        }

        /// <summary>
        /// Override of RevealGrid() from super class.
        /// </summary>
        public override void RevealGrid()
        {
            // Clear the console before printing grid.
            Console.Clear();

            // Print overall grid in white
            Console.ForegroundColor = ConsoleColor.White;

            // Game title.
            var title = "M I N E S W E E P E R".PadLeft(Size * 4 - 7, ' ');

            Console.WriteLine(title + Environment.NewLine);

            // Print Grid with color exceptions.
            Console.Write("    |");

            for (int i = 0; i < Size; i++)
            {
                Console.Write(" " + i + " |");
            }

            Console.WriteLine();

            for (int i = 0; i < Size; i++)
            {
                Console.Write("| " + i + " |");
                for (int j = 0; j < Size; j++)
                {
                    var location = Map[i, j];

                    if (location.Visited == false)
                    {
                        Console.Write(" ?" + " |");
                    }
                    else
                    {
                        if (Map[i, j].Live == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" *");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" |");
                        }
                        else
                        {
                            if (Map[i, j].LiveNeighbors == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" ~");
                            }
                            else if (Map[i, j].LiveNeighbors >= 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write(" " + Map[i, j].LiveNeighbors);
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" |");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Method to recursively check and visit adjoining cells
        /// that have no "live" neighbors, and to reveal
        /// any adjoining walls that are "live", but to go no
        /// further.
        /// </summary>
        /// <param name="cell">
        /// The cell chosen by the player from which to start the
        /// reveal.
        /// </param>
        public bool RevealSafeConnectingBlocks(Cell cell)
        {
            // Set the current visit to true
            cell.Visited = true;

            // Increase count by 1
            visitedCount += 1;

            // Return if the location is not zero / blank
            if (cell.LiveNeighbors > 0)
            {
                return false;
            }

            // Check to see if we are at outter bounds of entire map
            if (cell.Column - 1 >= 0)
            {
                // Check the west location
                var westLocation = Map[(int)cell.Row, (int)cell.Column - 1];

                // If the location is not live or visited
                if (!westLocation.Live && !westLocation.Visited)
                {
                    // if the location has no live neighbors, keep going
                    if (westLocation.LiveNeighbors == 0)
                    {
                        // Recursively call method
                        RevealSafeConnectingBlocks(westLocation);
                    }
                    else
                    {
                        // Otherwise, mark as visited and
                        // increase visit count
                        westLocation.Visited = true;
                        visitedCount += 1;
                    }
                }
            }

            // Check to see if we are at outter bounds of entire map
            if (cell.Row - 1 >= 0)
            {
                // Check the north location
                var northLocation = Map[(int)cell.Row - 1, (int)cell.Column];

                // If the location is not live or visited
                if (!northLocation.Live && !northLocation.Visited)
                {
                    // if the location has no live neighbors, keep going
                    if (northLocation.LiveNeighbors == 0)
                    {
                        // Recursively call method
                        RevealSafeConnectingBlocks(northLocation);
                    }
                    else
                    {
                        // Otherwise, mark as visited and
                        // increase visit count
                        northLocation.Visited = true;
                        visitedCount += 1;
                    }
                }
            }

            // Check to see if we are at outter bounds of entire map
            if (cell.Column + 1 < Size)
            {
                // Check the south location
                var southLocation = Map[(int)cell.Row, (int)cell.Column + 1];

                // If the location is not live or visited
                if (!southLocation.Live && !southLocation.Visited)
                {
                    // if the location has no live neighbors, keep going
                    if (southLocation.LiveNeighbors == 0)
                    {
                        // Recursively call method
                        RevealSafeConnectingBlocks(southLocation);
                    }
                    else
                    {
                        // Otherwise, mark as visited and
                        // increase visit count
                        southLocation.Visited = true;
                        visitedCount += 1;
                    }
                }
            }

            // Check to see if we are at outter bounds of entire map
            if (cell.Row + 1 < Size)
            {
                // Check the east location
                var eastLocation = Map[(int)cell.Row + 1, (int)cell.Column];

                // If the location is not live or visited
                if (!eastLocation.Live && !eastLocation.Visited)
                {
                    // if the location has no live neighbors, keep going
                    if (eastLocation.LiveNeighbors == 0)
                    {
                        // Recursively call method
                        RevealSafeConnectingBlocks(eastLocation);
                    }
                    else
                    {
                        // Otherwise, mark as visited and
                        // increase visit count
                        eastLocation.Visited = true;
                        visitedCount += 1;
                    }
                }
            }

            // return false
            return false;
        }
    }
}
