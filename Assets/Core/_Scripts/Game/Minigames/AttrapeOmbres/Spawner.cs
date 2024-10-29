using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _star;
        [SerializeField] private Transform _posy;
        private float _limit = 8f;

        private float _time;
        private float _targetTime; //Random


        private void Update()
        {
            Timer();
        }
        void Spawn()
        {
            float RandomposX = Random.Range(-_limit, _limit);
            float RandomposZ = Random.Range(-_limit/2, _limit/2);

            Vector3 pos = new Vector3(RandomposX, _posy.transform.position.y , RandomposZ);

            int RandomItem = Random.Range(0, 2);

            if (RandomItem == 0)
            {
                Instantiate(_star[0], pos, Quaternion.identity);
            }
            else
            {
                Instantiate(_star[1], pos, Quaternion.identity);
            }

            }

        void Timer()
        {
            _time += Time.deltaTime;

            if (_time >= _targetTime)
            {             
                Spawn();
                _targetTime = Random.Range(3, 10);
                _time = 0;
            }
        }
    }
}
