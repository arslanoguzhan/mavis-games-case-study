using MavisCase.Common.Pooling;
using UnityEngine;

namespace MavisCase.Games.TicTacToeGame
{
    [AddComponentMenu("TicTacToeGame.BoardView")]
    public class BoardView : MonoBehaviour, IPoolItem
    {
        public void Recycle()
        {

        }
    }
}