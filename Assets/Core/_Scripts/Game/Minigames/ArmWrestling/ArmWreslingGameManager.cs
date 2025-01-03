using LuckiusDev.Utils;
using RapidPrototyping.Utils.Input;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix.ArmWresling
{
    public class ArmWreslingGameManager : MonoBehaviour
    {
        public const int TIE_INDEX = -1;
        public const int PLAYER_ONE_INDEX = 0;
        public const int PLAYER_TWO_INDEX = 1;

        public const int MIN_SCORE = -100;
        public const int MAX_SCORE = 100;

        public event Action<int> OnGameEnded;

        [SerializeField] private GameIntroduction m_gameIntroduction;
        [SerializeField] private GameTimer m_gameTimer;

        [Space]

        [SerializeField] private ArmWrestlingBehavior m_playerOne;
        [SerializeField] private ArmWrestlingBehavior m_playerTwo;

        [Space]

        [SerializeField] private Transform m_armPivot;

        [Header("Audio")]
        [SerializeField] protected AudioClip m_countdownSound;
        [SerializeField] protected AudioClip m_startSound;

        private int m_score;
        private CountdownTimer m_timer;

        protected IEnumerator StartCountdown()
        {
            m_playerOne.enabled = false;
            m_playerTwo.enabled = false;
            Time.timeScale = 0f;

            const int time = 3;
            for (int i = time - 1; i >= 0; i--)
            {
                m_gameIntroduction.UpdateCountdown(i + 1);
                SoundManager.Play(m_countdownSound);
                yield return new WaitForSecondsRealtime(1f);
            }

            Time.timeScale = 1f;
            m_playerOne.enabled = true;
            m_playerTwo.enabled = true;

            m_gameTimer.Resume();
            m_gameIntroduction.ShowUserInterface();
            SoundManager.Play(m_startSound);
        }

        private void Start()
        {
            GameInputHandler.SetActionMap(GameInputHandler.ActionMapIndex.Doors);

            m_timer = new CountdownTimer(3f);
            m_timer.OnTimerStop += () =>
            {
                LoadGameplaySceneForNextTurn();
            };
            StartCoroutine(nameof(StartCountdown));
        }

        private void Update()
        {
            m_timer.Tick(Time.unscaledDeltaTime);

            var currentRotation = m_armPivot.localEulerAngles;
            currentRotation.x = Mathf.LerpAngle(-90f, 90f, JMath.Remap(m_score, MIN_SCORE, MAX_SCORE, 1f, 0f));
            m_armPivot.localRotation = Quaternion.Euler(currentRotation);
        }

        public void StopGame()
        {
            m_playerOne.enabled = false;
            m_playerTwo.enabled = false;

            int winIndex = TIE_INDEX;
            if (m_score < 0)
            {
                winIndex = PLAYER_TWO_INDEX;
            }
            else if (m_score > 0)
            {
                winIndex = PLAYER_ONE_INDEX;
            }

            OnGameEnded?.Invoke(winIndex);

            MarkWinningSymbol(winIndex);
            m_timer.Start();
        }

        public int GetScore()
        {
            return m_score;
        }

        public void IncreaseScore()
        {
            m_score++;
            m_score = Mathf.Clamp(m_score, MIN_SCORE, MAX_SCORE);
        }

        public void DecreaseScore()
        {
            m_score--;
            m_score = Mathf.Clamp(m_score, MIN_SCORE, MAX_SCORE);
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
    }
}
