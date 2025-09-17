namespace MavisCase.Common.GridSystem
{
    public interface IGridConfig
    {
        float CellSize { get; }
        int RowCount { get; }
        int ColCount { get; }
    }
}