using LuckiusDev.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix
{
    public class GameDataHandler : PersistentSingleton<GameDataHandler>
    {
        [SerializeField] private SceneReference m_mainMenuSceneReference;
        public static SceneReference MainMenuSceneReference => Instance.m_mainMenuSceneReference;
        [SerializeField] private SceneReference m_mainGameplaySceneReference;
        public static SceneReference MainGameplaySceneReference => Instance.m_mainGameplaySceneReference;

        [SerializeField] private DataHolder m_dataHolder;
        public static DataHolder DataHolder => Instance.m_dataHolder;

        // Indicates who's going to try to place their symbol on the grid.
        #region Turn

        public enum Turn
        {
            PLAYER_1,
            PLAYER_2,
        }

        private Turn m_currentTurn = Turn.PLAYER_1;
        public static Turn CurrentTurn => Instance.m_currentTurn;
        public event Action<Turn> OnTurnChanged;

        public static void ChangeTurn()
        {
            Instance.m_currentTurn = Instance.m_currentTurn == Turn.PLAYER_1 ?
                Turn.PLAYER_2 : Turn.PLAYER_1;
        }

        #endregion

        // The score in this script indicates how much games were won
        // by player one and two.
        #region Score

        private int m_playerOneScore;
        public static int PlayerOneScore => Instance.m_playerOneScore;
        private int m_playerTwoScore;
        public static int PlayerTwoScore => Instance.m_playerTwoScore;

        public event Action<int, int> OnScoreChanged;

        /// <summary>
        /// Add one point to player one score.
        /// </summary>
        public static void AddP1Score()
        {
            Instance.m_playerOneScore++;
        }

        /// <summary>
        /// Add one point to player two score.
        /// </summary>
        public static void AddP2Score()
        {
            Instance.m_playerTwoScore++;
        }

        /// <summary>
        /// Set both player scores by zero.
        /// </summary>
        public static void ResetScores()
        {
            Instance.m_playerOneScore = 0;
            Instance.m_playerTwoScore = 0;
        }

        #endregion

        [SerializeField] private MinigameData[] m_minigames;
        private List<MinigameData> m_loadedMinigames;

        public static void SelectRandomMinigame()
        {
            var loadedMinigames = Instance.m_loadedMinigames;
            if (loadedMinigames.Count == 0)
            {
                Instance.InitializeMinigames();
            }

            var randomMinigame = loadedMinigames.PickRandomUnity();
            loadedMinigames.Remove(randomMinigame);

            // Get the random scene name from the selected minigame
            string sceneName = randomMinigame.GetRandomSceneName();

            // Load the scene
            SceneManager.LoadScene(sceneName);
        }

        private void InitializeMinigames()
        {
            m_loadedMinigames = new List<MinigameData>();
            foreach (var game in m_minigames)
            {
                m_loadedMinigames.Add(game);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (m_dataHolder == null)
            {
                m_dataHolder = new DataHolder();
                m_dataHolder.Reset();
            }

            InitializeMinigames();
        }

        private void Start()
        {
            OnScoreChanged?.Invoke(PlayerOneScore, PlayerTwoScore);
        }

        public static void ResetGame()
        {
            DataHolder.Reset();
            //foreach (var game in Instance.m_weightedMinigames)
            //{
            //    game.Reset();
            //}
            Instance.InitializeMinigames();
            Instance.m_currentTurn = Turn.PLAYER_1;
            GridManager.Clear();
        }
    }
}
