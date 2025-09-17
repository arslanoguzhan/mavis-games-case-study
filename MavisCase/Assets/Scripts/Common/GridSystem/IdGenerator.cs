namespace MavisCase.Common.GridSystem
{
    public static class IdGenerator
    {
        private static int _id = 0;

        public static int GenerateId()
        {
            return _id++;
        }
    }
}
