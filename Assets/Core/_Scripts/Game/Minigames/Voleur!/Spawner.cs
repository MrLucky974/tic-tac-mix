using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Voleur
{
    public class Spawner : MonoBehaviour
    {
        [Header("SpawnCake")]
        [SerializeField] private GameObject _cake;
        [SerializeField] private Transform _pos;

        [SerializeField] private float _time;
        [SerializeField] private float _targetTime;

        [Header("GivePlayerCakeRef")]
        private Cake _spawnedCake;
        [SerializeField] private PlayerController[] _playerController;

        private void Start()
        {
            _playerController = FindObjectsOfType<PlayerController>();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            _targetTime = Random.Range(4, 7);

            if(_time >= _targetTime)
            {
                if (_spawnedCake != null)
                {
                    return;
                }

                Spawn();
                _time = 0;
            }
        }
        void Spawn()
        {

            _spawnedCake = Instantiate(_cake.GetComponent<Cake>(), _pos);

            foreach (PlayerController player in _playerController)
            {
                player._appearingCake = _spawnedCake;
            }
        }
    }
}
