using RapidPrototyping.Utils.Input;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Voleur
{
    public class PlayerController : MonoBehaviour, IPlayerPrimaryControls
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

        #region Input Variables

        private bool m_movementPressedThisFrame;

        #endregion

        private void Start()
        {
            GameInputHandler.SetReciever(gameObject, _isPlayerO ? 1 : 0);

            _animator = GetComponent<Animator>();
        }
        
        private void Update()
        {
            if (_appearingCake != null)
            {
                if (m_movementPressedThisFrame)
                {
                    _animator.SetTrigger("TakeCake");
                }
            }
        }

        private void LateUpdate()
        {
            m_movementPressedThisFrame = false;
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
        
        private void Score()
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

        public void OnPrimary(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            m_movementPressedThisFrame |= ctx.action.WasPressedThisFrame();
        }
    }
}
