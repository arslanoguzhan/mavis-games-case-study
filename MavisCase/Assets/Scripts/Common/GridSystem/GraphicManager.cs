using System.Collections.Generic;
using MavisCase.Common.Pooling;

namespace MavisCase.Common.GridSystem
{
    public class GraphicManager
    {
        private Dictionary<int, Graphic> _graphics = new();
    
        public Graphic GetGraphic(int id)
        {
            return _graphics[id];
        }

        public void SetGraphic(Graphic graphic)
        {
            _graphics[graphic.ItemId] = graphic;
        }

        public void Clear()
        {
            _graphics.Clear();
        }
    }
}