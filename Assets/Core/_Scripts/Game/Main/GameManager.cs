using LuckiusDev.Utils;
using System;

namespace RapidPrototyping.TicTacMix.Main
{
    public class GameManager : Singleton<GameManager>
    {
        private const int GRID_SIZE = 3;

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

        private void InitializeGrid()
        {
            // Initialize a 3*3 grid of symbols (none, cross, circle)
            m_grid = new Symbol[GRID_SIZE * GRID_SIZE];
            for (int i = 0; i < m_grid.Length; i++)
            {
                m_grid[i] = Symbol.None;
            }
            OnGridChanged?.Invoke(m_grid);
        }

        public bool IsCellEmpty(int x, int y)
        {
            int index = y + x * GRID_SIZE;
            if (m_grid[index] != Symbol.None)
            {
                return false;
            }
            return true;
        }

        public bool SetSymbol(Symbol symbol, int x, int y)
        {
            int index = y + x * GRID_SIZE;
            if (m_grid[index] != Symbol.None)
            {
                return false;
            }

            m_grid[index] = symbol;
            OnGridChanged?.Invoke(m_grid);
            return true;
        }
    }
}
