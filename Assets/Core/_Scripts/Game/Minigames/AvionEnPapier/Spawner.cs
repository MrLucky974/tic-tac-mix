using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [Header("Time")]

        [SerializeField] private float _time;
        [SerializeField] private float _moreHands;

        [SerializeField] private GameObject _platform;


        private void Start()
        {
            Destroy(_platform, 2f);
        }
        private void Update()
        {

            if (_nbHands <= 4)
            {
                _time += Time.deltaTime;

                if (_time >= _moreHands)
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
                _handsL.Add(instanciatedhand);
            }

        }

    }
}
