using MavisCase.Common.GridSystem;

namespace MavisCase.Games.MemoryGame
{
    public class GridConfig : IGridConfig
    {
        public float CellSize { get; } = 1f;
        public int RowCount { get; } = 6;
        public int ColCount { get; } = 4;
    }
}