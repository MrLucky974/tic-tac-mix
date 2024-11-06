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
            var spawnPoint = SelectSpawnPoint();

            var target = Instantiate(targetPrefab, spawnPoint.position, Quaternion.identity);
            target.transform.parent = spawnPoint;
            target.OnTargetDestroyed += HandleTargetDestroyed;
        }

        private void HandleTargetDestroyed()
        {
            SpawnTarget();
        }

        public Transform SelectSpawnPoint()
        {
            return m_spawnPoints.PickRandomUnity();
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
