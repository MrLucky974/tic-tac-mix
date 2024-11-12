using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        [Header("Countdown")]
        [SerializeField] private TMP_Text _countdownText;
        private bool _canMove = false;

        [Header("Audio")]
        [SerializeField] private AudioClip[] _audioClip;



        private void Start()
        {
            if (!_canMove)
            {
                StartCoroutine(Countdown());
            }
        }

        private void Update()
        {
          
            if (_canMove)
            {
                TimerItem();
                TimerSpike();
            }
        }

        IEnumerator Countdown()
        {
            _countdownText.gameObject.SetActive(true);

            _countdownText.text = "3";
            SoundManager.Play(_audioClip[0]);
            yield return new WaitForSeconds(1f);
            _countdownText.text = "2";
            SoundManager.Play(_audioClip[0]);
            yield return new WaitForSeconds(1f);
            _countdownText.text = "1";
            SoundManager.Play(_audioClip[0]);
            yield return new WaitForSeconds(1f);
            _countdownText.text = "GO!";
            SoundManager.Play(_audioClip[1]);
            yield return new WaitForSeconds(0.5f);


            _countdownText.gameObject.SetActive(false);

            _canMove = true;
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

        void TimerItem()
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

            for (int i = 0; i < 4; i++)
            {
            yield return new WaitForSeconds(0.2f);
                randomspike.GetComponentInChildren<SpriteRenderer>().material.color = Color.black;
                SoundManager.Play(_audioClip[2]);
                //randomspike.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                yield return new WaitForSeconds(0.2f);
                randomspike.GetComponentInChildren<SpriteRenderer>().material.color = Color.red;
                SoundManager.Play(_audioClip[2]);
                //randomspike.GetComponentInChildren<MeshRenderer>().material.color = Color.black;

            }
            //randomspike.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            randomspike.GetComponentInChildren<SpriteRenderer>().material.color = Color.white;
            randomspike.GetComponentInChildren<BoxCollider>().enabled = true;
            Destroy(randomspike, 2);
        }
        
    }
}
