using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class DataHolder
    {
        public Vector2Int GridPosition;

        public void Reset()
        {
            GridPosition = Vector2Int.zero;
        }
    }
}
