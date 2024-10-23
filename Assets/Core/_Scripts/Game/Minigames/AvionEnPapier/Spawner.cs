using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.AvionEnPapier
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] GameObject _hand;
        [SerializeField] Transform[] _posR;
        [SerializeField] Transform[] _posL;

        [SerializeField] List<GameObject> _hands;

        public void SpawnR()
        {
            ResetSpawn();

            int randomPos = Random.Range(0, _posR.Length);

            for (int i = 0; i < 3; i++)
            {
                GameObject instanciatedhand = Instantiate(_hand, _posR[randomPos]);
                _hands.Add(instanciatedhand);
            }

        }

        public void SpawnL()
        {

            ResetSpawn();

            int randomPos = Random.Range(0, _posL.Length);

            for (int i = 0; i < 3; i++)
            {
                Instantiate(_hand, _posL[randomPos]);
            }

        }

        private void ResetSpawn()
        {
            foreach (GameObject hand in _hands)
            {
                Destroy(hand);
            }
            _hands.Clear();
        }
    }
}
