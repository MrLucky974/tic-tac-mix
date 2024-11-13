using LuckiusDev.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public enum MatchResult
    {
        EXIT_DOOR_OPENED,
        TIMES_UP
    }

    public struct GameData
    {
        public MatchResult Result;
        public int PlayerIndex;
    }

    public class GameManager : MinigameManager<GameData>
    {
        [SerializeField] private CameraController m_camera;

        [Space]

        [SerializeField] private float m_floorHeight = 3f;
        [SerializeField] private int m_floorCount = 6;

        [Space]

        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private Transform m_tower;
        [SerializeField] private Ground m_groundFloor;
        [SerializeField] private Floor[] m_floorPrefabs;
        [SerializeField] private Color[] m_floorColors;

        private Floor m_topFloor;
        public static Floor TopFloor => ((GameManager)Instance).m_topFloor;

        private Player m_playerOne;
        private Player m_playerTwo;

        private CountdownTimer m_timer;

        public override void Initialize()
        {
            base.Initialize();

            // Generate the tower
            GenerateTower();

            // Spawn the players in the top floor
            SetupPlayers();

            m_timer = new CountdownTimer(1f);
            m_timer.OnTimerStop += () =>
            {
                if (m_timer.IsFinished is false)
                    return;

                LoadGameplaySceneForNextTurn();
            };
        }

        private void Update()
        {
            var unscaledDeltaTime = Time.unscaledDeltaTime;
            m_timer.Tick(unscaledDeltaTime);
        }

        protected override void HandleGameEnd(GameData data)
        {
            base.HandleGameEnd(data);
            MarkWinningSymbol(data.PlayerIndex);
            m_timer.Start();
        }

        public static void ConcludeGameOnTimeout()
        {
            var data = new GameData
            {
                Result = MatchResult.TIMES_UP,
                PlayerIndex = TIE_INDEX,
            };
            EndGame(data);
        }

        private void GenerateTower()
        {
            // This code only executes if at least one floor prefab has been
            // given to the game manager
            if (m_floorPrefabs == null)
                return;

            if (m_floorPrefabs.Length == 0)
                return;

            Floor previousFloor = null;
            Vector3 position = m_groundFloor.transform.position;
            for (int i = 0; i < m_floorCount; i++)
            {
                // Get this floor position
                position += Vector3.up * m_floorHeight;

                // Instantiate a new floor
                var prefab = m_floorPrefabs[Random.Range(0, m_floorPrefabs.Length)];
                var floor = Instantiate(prefab, position, Quaternion.identity);
                floor.transform.parent = m_tower;
                floor.name = $"Floor_{i + 1}";
                floor.SetIndex(i);

                Color color = Color.white;
                if (m_floorColors != null && m_floorColors.Length > 0)
                {
                    int colorIndex = i % m_floorColors.Length;
                    color = m_floorColors[colorIndex];
                }
                floor.Initialize(color);

                // This piece of code will link each stage with each other
                // to let the player navigate in between them with doors.
                if (previousFloor != null) // If a floor was generated before this one...
                {
                    floor.previousStage = previousFloor;
                }
                else // Else, set the previous stage as the ground floor
                {
                    floor.previousStage = m_groundFloor;
                }

                // Mark this generated floor as the predecessor for the next one
                previousFloor = floor;
            }

            // Mark the last generated floor as the top one
            m_topFloor = previousFloor;
        }

        private void SetupPlayers()
        {
            // This code only executes if a player prefab has been given to
            // the game manager
            if (m_playerPrefab == null)
                return;

            // Instantiate the first player on the left side of the top floor
            m_playerOne = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
            m_playerOne.Initialize(0, m_topFloor);
            m_camera.AddTarget(m_playerOne.transform);

            // Instantiate the second player on the right side of the top floor
            m_playerTwo = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
            m_playerTwo.Initialize(1, m_topFloor);
            m_camera.AddTarget(m_playerTwo.transform);
        }
    }
}
