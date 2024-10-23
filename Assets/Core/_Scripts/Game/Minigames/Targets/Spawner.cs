using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class Spawner : MonoBehaviour
    {
        [Serializable]
        public struct TargetObject
        {
            public Target Target;
            public int Weight;
        }

        [SerializeField] private TargetObject[] m_targets;
        [SerializeField] private Transform m_spawnParent;
        [SerializeField] private Transform[] m_spawnPoints;

        private void Start()
        {
            SpawnTarget();
        }

        public void SpawnTarget()
        {
            var targetPrefab = SelectTarget();
            var spawnPosition = SelectSpawnPosition();

            Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        }

        public Vector3 SelectSpawnPosition()
        {
            var spawnPoint = m_spawnPoints.PickRandom();
            return spawnPoint.position;
        }

        public Target SelectTarget()
        {
            int weightedSum = 0;
            foreach (var targetObject in m_targets)
            {
                weightedSum += targetObject.Weight;
            }

            int r = Random.Range(0, weightedSum);
            foreach (var targetObject in m_targets)
            {
                if (r < targetObject.Weight && r > 0)
                {
                    return targetObject.Target;
                }

                r -= targetObject.Weight;
            }

            return null;
        }
    }
}
