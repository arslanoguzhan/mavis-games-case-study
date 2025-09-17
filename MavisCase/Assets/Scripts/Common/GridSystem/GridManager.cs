using System.Collections.Generic;
using UnityEngine;

namespace MavisCase.Common.GridSystem
{
    public interface IGridManagerInitializer : IGridManager
    {
        void Initialize();
    }

    public interface IGridManager
    {
        Cell GetCell(int index);
        Cell[] GetCells();
    }
    
    public class GridManager : IGridManagerInitializer,  IGridManager
    {
        private Cell[] Cells;
        
        private readonly IGridConfig _gridConfig;

        public GridManager(IGridConfig gridConfig)
        {
            _gridConfig = gridConfig;
        }

        public void Initialize()
        {
            int rowCount = _gridConfig.RowCount;
            int colCount =  _gridConfig.ColCount;
            
            Cells = new Cell[rowCount * colCount];
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    var index = y * colCount + x;
                    Cells[index] = new Cell()
                    {
                        Index = index,
                        Position = new Vector2Int(x, y),
                        WorldPosition = GridToWorld(_gridConfig.CellSize, _gridConfig.ColCount, _gridConfig.RowCount, new Vector2Int(x, y)),
                        Items = new List<Item>(),
                    };
                }
            }
        }

        public Cell GetCell(int index)
        {
            return Cells[index];
        }

        public Cell[] GetCells()
        {
            return Cells;
        }
        
        public static Vector3 GridToWorld(float cellSize, int gridWidth, int gridHeight, Vector2Int gridPos)
        {
            var halfWidth = (gridWidth - 1) / 2f;
            var halfHeight = (gridHeight - 1) / 2f;
            float worldX = (gridPos.x - halfWidth) * cellSize;
            float worldY = (gridPos.y - halfHeight) * cellSize;
            return new Vector3(worldX, worldY, 0f);
        }
    }

}