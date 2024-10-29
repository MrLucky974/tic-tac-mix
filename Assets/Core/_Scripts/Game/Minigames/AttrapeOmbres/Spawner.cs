using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject _star;
        [SerializeField] private Transform _posy;


        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.E))
            {
                Spawn();
            }
        }
        void Spawn()
        {
            float RandomposX = Random.Range(-8f, 8f);
            float RandomposZ = Random.Range(-4f, 4f);

            Vector3 pos = new Vector3(RandomposX, _posy.transform.position.y , RandomposZ);

            GameObject star = Instantiate(_star, pos, Quaternion.identity);
            Destroy(star, 3f);
        }
    }
}
