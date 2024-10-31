using UnityEngine;

namespace RapidPrototyping.TicTacMix.Maze
{
    public class Tile : MonoBehaviour
    {
        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.gameObject.layer = gameObject.layer;
            }
        }
    }
}
