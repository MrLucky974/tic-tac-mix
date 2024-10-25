using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Voleur
{
    public class Cake : MonoBehaviour
    {
        private int _pieceOfCake; //Random

        [SerializeField] private TMP_Text _text;


        void Start()
        {
        RandomPieceOfCake();
        }

        void RandomPieceOfCake()
        {
            _pieceOfCake = Random.Range(1,10);

            _text.text = _pieceOfCake.ToString();
        }

        public void RemovePieceOfCake()
        {
            _pieceOfCake--;

            if (_pieceOfCake <= 0 )
            {
                Destroy(gameObject);
            }

            _text.text = _pieceOfCake.ToString();
        }

    }
}
