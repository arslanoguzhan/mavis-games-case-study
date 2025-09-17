using UnityEngine;

namespace MavisCase.Common.Helpers
{
    public static class ArrayHelper
    {
        public static void Shuffle<T>(T[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var rnd = Random.Range(i, array.Length);
                (array[i], array[rnd]) = (array[rnd], array[i]);
            }
        }
    }
}