namespace MavisCase.Common.GridSystem
{
    public class Item
    {
        public int Kind { get; set; }
        public int Id { get; set; }
        public int CellIndex { get; set; }
        public CollisionLayer Layer { get; set; }
    }
}