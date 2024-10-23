using LuckiusDev.Utils;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private CameraController m_camera;

        [Space]

        [SerializeField] private float m_floorHeight = 3f;

        [SerializeField] private Transform m_tower;
        [SerializeField] private Floor[] m_floorPrefabs;
        [SerializeField] private Ground m_groundFloor;
        [SerializeField] private Player m_playerPrefab;

        private Floor m_topFloor;

        private void Start()
        {
            GenerateTower();
            SetupPlayers();
        }

        private void GenerateTower()
        {
            if (m_floorPrefabs == null)
                return;

            if (m_floorPrefabs.Length == 0)
                return;

            Floor previousFloor = null;
            Vector3 position = m_groundFloor.transform.position;
            for (int i = 0; i < 5; i++)
            {
                position += Vector3.up * m_floorHeight;
                var prefab = m_floorPrefabs[0];
                var floor = Instantiate(prefab, position, Quaternion.identity);
                floor.transform.parent = m_tower;
                floor.name = $"Floor_{i + 1}";

                if (previousFloor != null)
                {
                    floor.previousStage = previousFloor;
                }
                else
                {
                    floor.previousStage = m_groundFloor;
                }

                previousFloor = floor;
            }

            m_topFloor = previousFloor;
        }

        private void SetupPlayers()
        {
            var playerOne = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
            playerOne.Initialize(0, m_topFloor);
            m_camera.AddTarget(playerOne.transform);

            var playerTwo = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
            playerTwo.Initialize(1, m_topFloor);
            m_camera.AddTarget(playerTwo.transform);
        }

        public static Floor TopFloor => Instance.m_topFloor;
    }
}
