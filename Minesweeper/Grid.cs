using System;

namespace Minesweeper
{
    /// <summary>
    /// Simple GameBoard class.
    /// </summary>
    /// 
    /// <remarks>
    /// Descr.:     Handles functionality of the game board class.
    /// 
    /// Author:     Jay Wilson
    /// Date:       12/17/18
    /// Version:    2.0
    /// </remarks>
    abstract class Grid
    {
        // Class members
        public Cell[,] Map { get; set; }
        public int Size { get; set; }

        /// <summary>
        /// Constructor for the game board.
        /// </summary>
        /// <param name="size">Set the size of the game board.</param>
        public Grid(int size)
        {
            // Initialize Members
            Size = size;
            Map = new Cell[Size, Size];

            // Fill the map
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Map[i, j] = new Cell(i, j);
                }
            }
        }

        /// <summary>
        /// Activates random cells within the Map based on the
        /// given number passed to the method.
        /// </summary>
        /// <param name="percentage">Percentage of Random Live Cells.</param>
        public virtual int ActivateRandomCells(double percentage)
        {
            Random random = new Random();
            int totalLiveCount = 0;

            if (percentage > 100 || percentage < 1)
            {
                percentage = 1;
            }
            else
            {
                percentage = (int)Math.Round((Size * Size) * (percentage / 100.00));
                totalLiveCount = (int)percentage;
            }

            while (percentage > 0)
            {
                var cell = Map[
                    random.Next(0, Size),
                    random.Next(0, Size)];

                if (cell.Live == false)
                {
                    cell.Live = true;
                    percentage -= 1;
                }
                else
                {
                    continue;
                }
            }

            SetLiveCount();
            return totalLiveCount;
        }

        /// <summary>
        /// Check neighbors and update cell LiveNeighbor count.
        /// </summary>
        public virtual void SetLiveCount()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {

                    // Check North
                    if (i - 1 >= 0)
                    {
                        if (Map[i - 1, j].Live == true)
                        {
                            Map[i, j].LiveNeighbors += 1;
                        }
                    }

                    // Check South
                    if (i + 1 < Size)
                    {
                        if (Map[i + 1, j].Live == true)
                        {
                            Map[i, j].LiveNeighbors += 1;
                        }
                    }

                    // Check East
                    if (j + 1 < Size)
                    {
                        if (Map[i, j + 1].Live == true)
                        {
                            Map[i, j].LiveNeighbors += 1;
                        }
                    }

                    // Check West
                    if (j - 1 >= 0)
                    {
                        if (Map[i, j - 1].Live == true)
                        {
                            Map[i, j].LiveNeighbors += 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reveals the grid to the user.
        /// </summary>
        public virtual void RevealGrid()
        {
            // Clears the console before printing.
            Console.Clear();

            // Print overall color of the map in white.
            Console.ForegroundColor = ConsoleColor.White;

            var title = "M I N E S W E E P E R".PadLeft(Size * 4 - 7, ' ');

            Console.WriteLine(title + Environment.NewLine);

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
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Resets all cells.
        /// </summary>
        public void ResetCells()
        {
            foreach (Cell cell in Map)
            {
                cell.Reset(cell.Row, cell.Column);
            }
        }
    }
}

