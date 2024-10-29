using LuckiusDev.Utils;
using System;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix.Tanks
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameEndReason
        {
            PlayerDeath,
            TimeUp
        }

        public event Action<GameEndReason, int> OnGameEnded;
        private bool m_gameRunning = true;

        public static void TimerGameEnd()
        {
            EndGame(GameEndReason.TimeUp, -1);
        }

        public static void EndGame(GameEndReason reason, int winIndex)
        {
            if (Instance.m_gameRunning is false)
                return;

            Instance.OnGameEnded?.Invoke(reason, winIndex);
            Instance.m_gameRunning = false;

            CompleteGame(winIndex);
        }

        public static void CompleteGame(int winIndex)
        {
            var gridPosition = GameDataHandler.DataHolder.GridPosition;
            if (winIndex != -1)
            {
                var symbol = winIndex == 1 ? GridManager.Symbol.Cross : GridManager.Symbol.Circle;
                GridManager.PlaceSymbol(symbol, gridPosition.x, gridPosition.y);
            }
            GameDataHandler.ChangeTurn();
            SceneManager.LoadScene(GameDataHandler.MainGameplaySceneReference);
        }
    }
}
