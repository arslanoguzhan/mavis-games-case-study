using MavisCase.Common.Persistence;

namespace MavisCase.Games.MemoryGame
{
    public class Progress : IProgress
    {
        public int CurrentLevel;

        public static Progress CreateInitialProgress()
        {
            return new Progress()
            {
                CurrentLevel = 1,
            };
        }
    }
}
