using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class DecorHands : MonoBehaviour
    {

        [SerializeField] private Sprite[] _handSprite;
        [SerializeField] private GameObject[] _hands;

        private void Start()
        {
            foreach (GameObject hand in _hands)
            {
                int RandomHandSprite = Random.Range(0, _handSprite.Length);
                hand.GetComponent<SpriteRenderer>().sprite = _handSprite[RandomHandSprite];

                int RandomHandFlip = Random.Range(0, 2);
                if(RandomHandFlip == 0)
                {
                hand.GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                hand.GetComponent<SpriteRenderer>().flipX = true;
                }


            }
        
        }
       
    }
}
