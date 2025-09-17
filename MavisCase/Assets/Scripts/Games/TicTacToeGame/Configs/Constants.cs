namespace MavisCase.Games.TicTacToeGame
{
    public class Constants
    {
        public static readonly int[][] WinConditions = new int[][]
        {
            // Rows
            new [] {0, 1, 2},
            new [] {3, 4, 5},
            new [] {6, 7, 8},

            // Columns
            new [] {0, 3, 6},
            new [] {1, 4, 7},
            new [] {2, 5, 8},

            // Diagonals
            new [] {0, 4, 8},
            new [] {2, 4, 6},
        };
    }
}