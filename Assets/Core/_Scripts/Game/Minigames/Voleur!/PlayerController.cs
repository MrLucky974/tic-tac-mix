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


        [Header("Audio")]
        [SerializeField] private AudioClip[] _audioClip;

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

                SoundManager.Play(_audioClip[0]);
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
