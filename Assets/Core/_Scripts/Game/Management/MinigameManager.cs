using LuckiusDev.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix
{
    public class MinigameManager<TData> : Singleton<MinigameManager<TData>> where TData : struct
    {
        public const int TIE_INDEX = -1;
        public const int PLAYER_ONE_INDEX = 0;
        public const int PLAYER_TWO_INDEX = 1;

        [Header("References")]
        [SerializeField] protected GameIntroduction m_gameIntroduction;
        [SerializeField] protected GameTimer m_gameTimer;

        [Header("Audio")]
        [SerializeField] protected AudioClip m_countdownSound;
        [SerializeField] protected AudioClip m_startSound;

        protected bool m_gameRunning = true;
        public static bool GameRunning => Instance.m_gameRunning;

        public event Action<TData> OnGameEnded;

        public void Start()
        {
            StartCoroutine(nameof(StartCountdown));
            OnGameEnded += HandleGameEnd;
            Initialize();
        }

        public virtual void Initialize() { }

        public virtual void OnGameStart() { }

        protected virtual void HandleGameEnd(TData data)
        {
            m_gameTimer.Stop();
        }

        protected IEnumerator StartCountdown()
        {
            m_gameRunning = false;
            Time.timeScale = 0f;

            const int time = 3;
            for (int i = time - 1; i >= 0; i--)
            {
                m_gameIntroduction.UpdateCountdown(i + 1);
                SoundManager.Play(m_countdownSound);
                yield return new WaitForSecondsRealtime(1f);
            }

            Time.timeScale = 1f;
            m_gameRunning = true;

            m_gameTimer.Resume();
            m_gameIntroduction.ShowUserInterface();
            SoundManager.Play(m_startSound);
            OnGameStart();
        }

        public static void EndGame(TData data)
        {
            if (Instance.m_gameRunning is false)
                return;

            Instance.OnGameEnded?.Invoke(data);
            Instance.m_gameRunning = false;
        }

        /// <summary>
        /// Marks the winning symbol on the grid at the specified position when a game ends.
        /// </summary>
        /// <param name="winIndex">
        /// The index indicating the winning player:
        /// - 0 for player using "Cross"
        /// - 1 for player using "Circle"
        /// - -1 if there is no winner (in which case, no symbol is placed).
        /// </param>
        public static void MarkWinningSymbol(int winIndex)
        {
            // Retrieve the current grid position from GameDataHandler.
            var gridPosition = GameDataHandler.DataHolder.GridPosition;

            // Check if there is a winning player.
            if (winIndex != TIE_INDEX)
            {
                // Determine the symbol to place based on the winning player's index.
                var symbol = winIndex == PLAYER_ONE_INDEX ? GridManager.Symbol.Cross : GridManager.Symbol.Circle;

                // Place the winning symbol on the grid at the designated position.
                GridManager.PlaceSymbol(symbol, gridPosition.x, gridPosition.y);
            }
        }

        /// <summary>
        /// Advances the game by changing the player's turn and loading the main gameplay scene.
        /// </summary>
        public static void LoadGameplaySceneForNextTurn()
        {
            // Change the active player turn in GameDataHandler.
            GameDataHandler.ChangeTurn();

            // Set the time scale back to 1
            Time.timeScale = 1f;

            // Load the main gameplay scene based on the configured scene reference.
            SceneManager.LoadScene(GameDataHandler.MainGameplaySceneReference);
        }
    }
}
