using LuckiusDev.Utils;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private CameraController m_camera;

        [Space]

        [SerializeField] private float m_floorHeight = 3f;

        [SerializeField] private Floor[] m_floorPrefabs;
        [SerializeField] private Transform m_groundFloor;
        [SerializeField] private Player m_playerPrefab;

        private void Start()
        {
            GenerateTower();
        }

        private void GenerateTower()
        {
            if (m_floorPrefabs == null)
                return;

            if (m_floorPrefabs.Length == 0)
                return;

            Floor previousFloor = null;
            Vector3 position = m_groundFloor.position;
            for (int i = 0; i < 5; i++)
            {
                position += Vector3.up * m_floorHeight;
                var prefab = m_floorPrefabs[0];
                var floor = Instantiate(prefab, position, Quaternion.identity);
                if (previousFloor != null)
                {
                    previousFloor.nextFloor = floor;
                }
                previousFloor = floor;
            }

            var topFloor = previousFloor;

            var playerOne = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
            playerOne.Initialize(0, topFloor);
            m_camera.AddTarget(playerOne.transform);

            var playerTwo = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
            playerTwo.Initialize(1, topFloor);
            m_camera.AddTarget(playerTwo.transform);
        }
    }
}
