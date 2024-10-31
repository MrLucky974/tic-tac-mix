using System;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Maze
{
    public class MazeManager : MonoBehaviour
    {
        [Serializable]
        public struct GridData
        {
            public int Width;
            public int Height;
        }

        [SerializeField] private GridData m_gridData;

        [Space]

        [SerializeField] private Tile m_tilePrefab;
        [SerializeField] private Transform m_camera;

        private Dictionary<Vector2Int, Tile> m_tiles;

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            m_tiles = new();

            for (int x = 0; x < m_gridData.Width; x++)
            {
                for (int y = 0; y < m_gridData.Height; y++)
                {
                    var tile = Instantiate(m_tilePrefab,
                        new Vector3(x, y),
                        Quaternion.identity);
                    tile.name = $"Tile[{x}, {y}]";
                    tile.transform.parent = transform;
                    tile.gameObject.layer = gameObject.layer;

                    var vector = new Vector2Int(x, y);
                    m_tiles[vector] = tile;
                }
            }

            m_camera.position = new Vector3((float)m_gridData.Width / 2 - 0.5f, (float)m_gridData.Height / 2 - 0.5f, -10f);
        }

        public Tile GetTileAtPosition(Vector2Int position)
        {
            if (m_tiles.TryGetValue(position, out Tile tile))
            {
                return tile;
            }

            return null;
        }
    }
}
