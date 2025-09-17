using MavisCase.Common.GridSystem;

namespace MavisCase.Games.TicTacToeGame
{
    public class GridConfig : IGridConfig
    {
        public float CellSize { get; } = 1.3f;
        public int RowCount { get; } = 3;
        public int ColCount { get; } = 3;
    }
}