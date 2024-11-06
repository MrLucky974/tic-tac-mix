using LuckiusDev.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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

        [Serializable]
        public struct WeightData
        {
            public MinigameData Minigame { get; private set; }

            [SerializeField] private float baseWeight;
            [SerializeField] private float minWeight;
            [SerializeField] private float weightRecoveryRate;
            [SerializeField] private float currentWeight;

            public float CurrentWeight => currentWeight;

            public WeightData(MinigameData minigame, float baseWeight = 1f, float minWeight = 0.1f, float recoveryRate = 0.2f)
            {
                Minigame = minigame;
                this.baseWeight = baseWeight;
                this.minWeight = minWeight;
                this.weightRecoveryRate = recoveryRate;
                this.currentWeight = baseWeight;
            }

            public void Reset()
            {
                currentWeight = baseWeight;
            }

            public void Reduce()
            {
                currentWeight = Mathf.Max(minWeight, currentWeight * 0.5f);
            }

            public void Recover()
            {
                currentWeight = Mathf.Min(baseWeight, currentWeight + weightRecoveryRate);
            }
        }

        [SerializeField] private MinigameData[] m_minigames;
        private List<WeightData> m_weightedMinigames;

        public static void SelectRandomMinigame()
        {
            // Guard clause in case there are no minigames
            if (Instance.m_weightedMinigames == null || Instance.m_weightedMinigames.Count == 0)
            {
                Debug.LogError("No minigames available!");
                return;
            }

            // Calculate total weight
            float totalWeight = 0f;
            foreach (var weightData in Instance.m_weightedMinigames)
            {
                totalWeight += weightData.CurrentWeight;
            }

            // Select a random point in the total weight range
            float randomPoint = Random.Range(0f, totalWeight);

            // Find the selected minigame
            WeightData selectedWeight = default;
            float currentWeight = 0f;

            foreach (var weightData in Instance.m_weightedMinigames)
            {
                currentWeight += weightData.CurrentWeight;
                if (randomPoint <= currentWeight)
                {
                    selectedWeight = weightData;
                    break;
                }
            }

            // This shouldn't happen, but just in case
            if (selectedWeight.Minigame == null)
            {
                selectedWeight = Instance.m_weightedMinigames[Instance.m_weightedMinigames.Count - 1];
            }

            // Reduce the weight of the selected minigame
            int selectedIndex = Instance.m_weightedMinigames.FindIndex(w => w.Minigame == selectedWeight.Minigame);
            if (selectedIndex != -1)
            {
                var updatedWeight = Instance.m_weightedMinigames[selectedIndex];
                updatedWeight.Reduce();
                Instance.m_weightedMinigames[selectedIndex] = updatedWeight;
            }

            // Recover weights of non-selected minigames
            for (int i = 0; i < Instance.m_weightedMinigames.Count; i++)
            {
                if (i != selectedIndex)
                {
                    var weightData = Instance.m_weightedMinigames[i];
                    weightData.Recover();
                    Instance.m_weightedMinigames[i] = weightData;
                }
            }

            // Get the random scene name from the selected minigame
            string sceneName = selectedWeight.Minigame.GetRandomSceneName();

            // Load the scene
            SceneManager.LoadScene(sceneName);
        }

        private void InitializeMinigames()
        {
            m_weightedMinigames = new List<WeightData>();
            foreach (var minigame in m_minigames)
            {
                m_weightedMinigames.Add(new WeightData(minigame));
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
            Instance.m_currentTurn = Turn.PLAYER_1;
            GridManager.Clear();
        }
    }
}
