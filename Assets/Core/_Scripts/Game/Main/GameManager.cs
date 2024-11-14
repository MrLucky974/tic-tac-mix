using LuckiusDev.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix.Main
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneReference m_mainMenuScene;

        public enum GameResult
        {
            None,       // Game is ongoing
            PlayerOne,  // Player with Cross wins
            PlayerTwo,  // Player with Circle wins
            Tie         // Grid is full with no winner
        }

        private void Start()
        {
            CheckGrid();
        }

        private void CheckGrid()
        {
            // TODO : Open game over scene
            GameResult result = CheckForWin();
            switch (result)
            {
                case GameResult.PlayerOne:
                    Debug.Log("Player One wins!");
                    GameDataHandler.AddP1Score();
                    ReturnToMainMenu();
                    break;
                case GameResult.PlayerTwo:
                    Debug.Log("Player Two wins!");
                    GameDataHandler.AddP2Score();
                    ReturnToMainMenu();
                    break;
                case GameResult.Tie:
                    Debug.Log("It's a tie!");
                    ReturnToMainMenu();
                    break;
                case GameResult.None:
                    Debug.Log("Game is ongoing.");
                    break;
            }
        }

        public void ReturnToMainMenu()
        {
            GameDataHandler.ResetGame();
            if (m_mainMenuScene != null &&
                string.IsNullOrEmpty(m_mainMenuScene) is false)
            {
                SceneManager.LoadScene(m_mainMenuScene);
            }
        }

        private GameResult CheckForWin()
        {
            GridManager.Symbol[] grid = GridManager.Grid;
            bool isGridFull = true;

            // Check rows
            for (int row = 0; row < 3; row++)
            {
                if (grid[row * 3] != GridManager.Symbol.None &&
                    grid[row * 3] == grid[row * 3 + 1] &&
                    grid[row * 3] == grid[row * 3 + 2])
                {
                    return grid[row * 3] == GridManager.Symbol.Cross ? GameResult.PlayerOne : GameResult.PlayerTwo;
                }
            }

            // Check columns
            for (int col = 0; col < 3; col++)
            {
                if (grid[col] != GridManager.Symbol.None &&
                    grid[col] == grid[col + 3] &&
                    grid[col] == grid[col + 6])
                {
                    return grid[col] == GridManager.Symbol.Cross ? GameResult.PlayerOne : GameResult.PlayerTwo;
                }
            }

            // Check diagonals
            if (grid[0] != GridManager.Symbol.None &&
                grid[0] == grid[4] &&
                grid[0] == grid[8])
            {
                return grid[0] == GridManager.Symbol.Cross ? GameResult.PlayerOne : GameResult.PlayerTwo;
            }

            if (grid[2] != GridManager.Symbol.None &&
                grid[2] == grid[4] &&
                grid[2] == grid[6])
            {
                return grid[2] == GridManager.Symbol.Cross ? GameResult.PlayerOne : GameResult.PlayerTwo;
            }

            // Check if grid is full
            foreach (var cell in grid)
            {
                if (cell == GridManager.Symbol.None)
                {
                    isGridFull = false;
                    break;
                }
            }

            return isGridFull ? GameResult.Tie : GameResult.None;
        }
    }
}
