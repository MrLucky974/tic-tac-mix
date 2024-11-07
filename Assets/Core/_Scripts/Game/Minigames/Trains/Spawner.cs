using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Trains
{
    public class Spawner : MonoBehaviour
    {

        [SerializeField] private float _limit;
        [SerializeField] private GameObject _item;

        private float _time;
        private float _targetTime; //Random

        void Update()
        {
            TimerItem();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spawn();
            }
        }
        void TimerItem()
        {
            _time += Time.deltaTime;

            if (_time >= _targetTime)
            {
                for (int i = 0; i < 2; i++)
                {
                    Spawn();
                }
                _targetTime = Random.Range(5, 10);
                _time = 0;
            }
        }

        void Spawn()
        {
            float RandomposX = Random.Range(-_limit, _limit);
            float RandomposZ = Random.Range(-_limit/2, _limit/2);

            Vector3 pos = new Vector3(RandomposX, transform.position.y, RandomposZ);

            Instantiate(_item, pos, Quaternion.identity);

        }
    }
}
