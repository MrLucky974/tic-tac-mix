using LuckiusDev.Utils;
using System;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameEndReason
        {
            PlayerDeath,
            TimeUp
        }

        [SerializeField] private CameraController m_camera;

        [Space]

        [SerializeField] private float m_floorHeight = 3f;

        [Space]

        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private Transform m_tower;
        [SerializeField] private Ground m_groundFloor;
        [SerializeField] private Floor[] m_floorPrefabs;

        private Floor m_topFloor;

        [Space]

        [SerializeField] private float m_totalDuration;

        public event Action<GameEndReason, int> OnGameEnded;

        private bool m_gameRunning = true;
        private float m_currentTime;

        private void Start()
        {
            m_currentTime = m_totalDuration;

            // Generate the tower and spawn the players in the top floor
            GenerateTower();
            SetupPlayers();
        }

        private void Update()
        {
            if (m_gameRunning is false)
                return;

            m_currentTime -= Time.deltaTime;

            if (m_currentTime <= 0f)
            {
                EndGame(GameEndReason.TimeUp, -1);
            }
        }

        public static void EndGame(GameEndReason reason, int winIndex)
        {
            if (Instance.m_gameRunning is false)
                return;

            Instance.OnGameEnded?.Invoke(reason, winIndex);
            Instance.m_gameRunning = false;
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
            for (int i = 0; i < 5; i++)
            {
                // Get this floor position
                position += Vector3.up * m_floorHeight;

                // Instantiate a new floor
                var prefab = m_floorPrefabs[0];
                var floor = Instantiate(prefab, position, Quaternion.identity);
                floor.transform.parent = m_tower;
                floor.name = $"Floor_{i + 1}";

                floor.Initialize();

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
            var playerOne = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
            playerOne.Initialize(0, m_topFloor);
            m_camera.AddTarget(playerOne.transform);

            // Instantiate the second player on the right side of the top floor
            var playerTwo = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
            playerTwo.Initialize(1, m_topFloor);
            m_camera.AddTarget(playerTwo.transform);
        }

        public static Floor TopFloor => Instance.m_topFloor;
    }
}
