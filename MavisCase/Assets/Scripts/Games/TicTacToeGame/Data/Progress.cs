using MavisCase.Common.Persistence;

namespace MavisCase.Games.TicTacToeGame
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
