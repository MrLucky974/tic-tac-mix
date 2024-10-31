using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace RapidPrototyping.TicTacMix
{
    public class Spawner : MonoBehaviour
    {
        [Header("Items")]
        [SerializeField] private GameObject[] _star;
        [SerializeField] private Transform _posy;
        private float _limit = 8f;

        private float _time;
        private float _targetTime; //Random

        [Header("Ground")]
        [SerializeField] private GameObject _spikes;
        private float _timeSpike;
        private float _targetTimeSpike; //Random


        private void Update()
        {
            Timer();
            TimerSpike();
        }
        void Spawn()
        {
            float RandomposX = Random.Range(-_limit, _limit);
            float RandomposZ = Random.Range(-_limit / 2, _limit / 2);

            Vector3 pos = new Vector3(RandomposX, _posy.transform.position.y, RandomposZ);

            int dice = Random.Range(0, 100);

            if (dice <= 70)
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

                int randomAppear = Random.Range(1,3);
                for (int i = 0; i < randomAppear; i++)
                {

                Spawn();
                }
                _targetTime = Random.Range(2, 3);
                _time = 0;
            }
        }

        void TimerSpike()
        {
            _timeSpike += Time.deltaTime;

            if (_timeSpike >= _targetTimeSpike)
            {
                int randomAppear = Random.Range(1, 3);
                for (int i = 0; i < randomAppear; i++)
                {
                    SpawnSpike();
                }
                _targetTimeSpike = Random.Range(1, 5);
                _timeSpike = 0;
            }
        }

        void SpawnSpike()
        {
            float RandomposX = Random.Range(-_limit, _limit);
            float RandomposZ = Random.Range(-_limit / 2, _limit / 2);

            Vector3 pos = new Vector3(RandomposX, 0, RandomposZ);

            GameObject randomSpike = Instantiate(_spikes, pos, Quaternion.identity);

            StartCoroutine(Twitching(randomSpike));
        }

        IEnumerator Twitching(GameObject randomspike)
        {
            randomspike.GetComponentInChildren<BoxCollider>().enabled = false;

            for (int i = 0; i < 2; i++)
            {
            yield return new WaitForSeconds(0.2f);
            randomspike.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            randomspike.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;

            }
            randomspike.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
            randomspike.GetComponentInChildren<BoxCollider>().enabled = true;
            Destroy(randomspike, 2);
        }
        
    }
}
