using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace RapidPrototyping.TicTacMix.AvionEnPapier
{
    public class Spawner : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private GameObject _hand;
        [SerializeField] private Transform[] _posR;
        [SerializeField] private Transform[] _posL;
   
        private List<GameObject> _handsR = new List<GameObject>();
        private List<GameObject> _handsL = new List<GameObject>();

        [SerializeField] private int _nbHands;

        [SerializeField] private Sprite[] _handSprite;

        [Header("Time")]

        [SerializeField] private float _time;
        [SerializeField] private float _timeMoreHands;

        [SerializeField] private GameObject _platform;


        private void Start()
        {
            Destroy(_platform, 2f);
        }
        private void Update()
        {

            if (_nbHands <= 2)
            {
                _time += Time.deltaTime;

                if (_time >= _timeMoreHands)
                {

                    _nbHands++;

                    _time = 0;
                }
            }
        }

        /*
        public void Spawn(List<GameObject> hands, Transform[] pos)
        {
            foreach (GameObject hand in hands)
            {
                Destroy(hand);
            }
            hands.Clear();

            for (int i = 0; i < 3; i++)
            {
                int randomPos = Random.Range(0, pos.Length);
                GameObject instanciatedhand = Instantiate(_hand, pos[randomPos]);
                hands.Add(instanciatedhand);
            }

        }
        */

        public void SpawnR()
        {
            foreach (GameObject hand in _handsR)
            {
                Destroy(hand);
            }
            _handsR.Clear();

            for (int i = 0; i < _nbHands; i++)
            {
                int randomPos = Random.Range(0, _posR.Length);

                GameObject instanciatedhand = Instantiate(_hand, _posR[randomPos]);
                _handsR.Add(instanciatedhand);
            }

        }

        public void SpawnL()
        {

            foreach (GameObject hand in _handsL)
            {
                Destroy(hand);
            }
            _handsL.Clear();


            for (int i = 0; i < _nbHands; i++)
            {
                int randomPos = Random.Range(0, _posL.Length);
                GameObject instanciatedhand = Instantiate(_hand, _posL[randomPos].position, Quaternion.Euler(0, 180, 0));
                RandomSprite(instanciatedhand);
                _handsL.Add(instanciatedhand);
            }

        }

        void RandomSprite(GameObject spawnedHandSprite)
        {
            int RandomHandSprite = Random.Range(0,_handSprite.Length);
            spawnedHandSprite.GetComponentInChildren<SpriteRenderer>().sprite = _handSprite[RandomHandSprite];

            int RandomHandFlip = Random.Range(0, 2);
            print("randomFlip"+RandomHandFlip);
            if (RandomHandFlip == 0)
            {
                spawnedHandSprite.GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            else if (RandomHandFlip == 1) 
            {
                spawnedHandSprite.GetComponentInChildren<SpriteRenderer>().flipX = true;
            }

        }

    }
}
