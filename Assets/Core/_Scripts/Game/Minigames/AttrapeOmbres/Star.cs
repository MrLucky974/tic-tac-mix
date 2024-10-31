using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace RapidPrototyping.TicTacMix
{
    public class Star : MonoBehaviour
    {

        private RaycastHit _hitData;
        [SerializeField] private GameObject _shadow;

        private void Update()
        {
            Raycast();
        }
        void Raycast()
        {
            Ray ray = new Ray(transform.position, -transform.up);
            Debug.DrawRay(ray.origin, ray.direction * 10);

            float hitDistance = _hitData.distance;

            //Shawdow
            if (Physics.Raycast(ray, out _hitData))
            {

                _shadow.transform.position = _hitData.point;

                _shadow.transform.localScale = Vector3.one * hitDistance;



                Color shadColor = _shadow.GetComponent<SpriteRenderer>().color;
                //Rendre plus foncé
                _shadow.GetComponent<SpriteRenderer>().color = new Color(shadColor.r, shadColor.g, shadColor.b, 1/hitDistance); ;
                

            }

            if(transform.position.y <= _hitData.point.y)
            {
                Destroy(gameObject);
            }

        }
    }
}
