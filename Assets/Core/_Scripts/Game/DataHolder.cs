using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    [CreateAssetMenu(menuName = "Tic Tac Mix/New Data Holder")]
    public class DataHolder : ScriptableObject
    {
        public Vector2Int GridPosition;

        private void Reset()
        {
            GridPosition = Vector2Int.zero;
        }
    }
}
