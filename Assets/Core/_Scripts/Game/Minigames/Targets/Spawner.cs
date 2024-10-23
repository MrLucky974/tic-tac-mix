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
            if (!GameManager.GameRunning)
                return;

            var targetPrefab = SelectTarget();
            var spawnPosition = SelectSpawnPosition();

            var target = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
            target.OnTargetDestroyed += HandleTargetDestroyed;
        }

        private void HandleTargetDestroyed()
        {
            SpawnTarget();
        }

        public Vector3 SelectSpawnPosition()
        {
            var spawnPoint = m_spawnPoints.PickRandomUnity();
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

            return m_targets.PickRandomUnity().Target;
        }
    }
}
