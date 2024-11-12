using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Trains
{
    public class Spawner : MonoBehaviour
    {

        [SerializeField] private float _limit;
        [SerializeField] private GameObject _item;

        private float _time;
        private float _targetTime; //Random

        private GameManager _gameManager;

        [SerializeField] private GameObject[] _trees;

     

        private void Start()
        {
         _gameManager = GetComponent<GameManager>();

        }
        void Update()
        {
            if (_gameManager._canSpawn)
            {
                TimerItem();

                //if (Input.GetKeyDown(KeyCode.Space))
                //{
                //    Spawn();
                //}
            }
        }

        void SpawnTree()
        {
            float RandomposX = Random.Range(-_limit, _limit);
            float RandomposZ = Random.Range(-_limit / 2, _limit / 2);

            Vector3 pos = new Vector3(RandomposX, transform.position.y, RandomposZ);

            int randomTree = Random.Range(0, _trees.Length);
            Instantiate(_trees[randomTree], pos, Quaternion.identity);
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
                _targetTime = Random.Range(3, 7);
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
