using LuckiusDev.Utils;
using System;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Main
{
    public class GameManager : Singleton<GameManager>
    {
        private const int GRID_SIZE = 3;

        [Serializable]
        public enum Symbol
        {
            None = 0,
            Cross = 1,
            Circle = 2,
        }

        private Symbol[] m_grid;
        public event Action<Symbol[]> OnGridChanged;

        protected override void Awake()
        {
            base.Awake(); // The singleton code is fired in Awake() so we need to call it.
            InitializeGrid();
        }

        private void Start()
        {
            OnGridChanged?.Invoke(m_grid);
        }

        private void InitializeGrid()
        {
            // Initialize a 3*3 grid of symbols (none, cross, circle)
            m_grid = new Symbol[GRID_SIZE * GRID_SIZE];
            for (int i = 0; i < m_grid.Length; i++)
            {
                m_grid[i] = Symbol.None;
            }
        }

        public static bool IsCellEmpty(int x, int y)
        {
            int index = x + y * GRID_SIZE;
            if (Instance.m_grid[index] != Symbol.None)
            {
                return false;
            }
            return true;
        }

        public static Symbol GetSymbol(int x, int y)
        {
            int index = x + y * GRID_SIZE;
            return Instance.m_grid[index];
        }

        public static bool PlaceSymbol(Symbol symbol, int x, int y)
        {
            int index = x + y * GRID_SIZE;
            Debug.Log($"{x} + {y} * {GRID_SIZE} = {index}");
            if (Instance.m_grid[index] != Symbol.None)
            {
                Debug.LogWarning("Can't place symbol here!");
                return false;
            }

            Instance.m_grid[index] = symbol;
            Instance.OnGridChanged?.Invoke(Instance.m_grid);
            return true;
        }
    }
}
