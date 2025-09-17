using System.Collections.Generic;
using UnityEngine;

namespace MavisCase.Common.GridSystem
{
    public class Cell
    {
        public int Index;
        public Vector2Int Position;
        public Vector3 WorldPosition;
        public List<Item> Items = new();

        public void AddItemOnTop(Item item)
        {
            Items.Add(item);
        }
        public void RemoveItemFromTop()
        {
            Items.RemoveAt(Items.Count - 1);
        }
    }
}