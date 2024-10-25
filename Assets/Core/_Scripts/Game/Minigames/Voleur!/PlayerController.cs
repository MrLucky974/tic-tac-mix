using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Voleur
{
    public class PlayerController : MonoBehaviour
    {
        [Header("PlayerTakeCake")]
        public bool _isPlayerO;

        [SerializeField] private KeyCode _keyToTake;
        public Cake _appearingCake;
        public int _obtainedCake;
        public bool _isTakingCake;

        [Header("UI")]
        [SerializeField] private TMP_Text _text;

        [Header("Animation")]
        private Animator _animator;

        /// Clique = prend un gateau OK
        /// Gateau = parts => Disparait OK
        /// Spawner de gateau OK
        /// Score / Barre de progression? OK
        /// Ennemi regarde = ne pas prendre de gateau = fin
        /// Systeme de fin : vue par le watcher, avoir le plus de part dans un temps OK
        /// Animation, bouge? OK

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
        private void Update()
        {
            if (_appearingCake != null)
            {
                if (Input.GetKeyDown(_keyToTake))
                {
                    _animator.SetTrigger("TakeCake");

                }
            }
        }
        //Animation
        public void TakeCakes()
        {
            if (_appearingCake != null)
            {
                _appearingCake.RemovePieceOfCake();

                _obtainedCake++;

                Score();
            }
        }
        void Score()
        {
            
           _text.text = _obtainedCake.ToString();
         
        }
   
        public void IsTakingCake()
        {
            _isTakingCake = true;
        }

        public void NotTakingCake()
        {
            _isTakingCake = false;
        }
    }
}
