using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace RapidPrototyping.TicTacMix.Voleur
{
    public class PlayerController : MonoBehaviour, IPlayerControls
    {
        [Header("Input")]
        [SerializeField] private PlayerInput m_playerInput;

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
            m_playerInput.SwitchCurrentControlScheme(m_playerInput.defaultControlScheme);
            InputUser.PerformPairingWithDevice(Keyboard.current, m_playerInput.user, InputUserPairingOptions.None);
            InputUser.PerformPairingWithDevice(Mouse.current, m_playerInput.user, InputUserPairingOptions.None);
            if (_isPlayerO)
            {
                if (Gamepad.all.Count >= 1)
                {
                    var gamepad = Gamepad.all[0];
                    InputUser.PerformPairingWithDevice(gamepad, m_playerInput.user, InputUserPairingOptions.None);
                }
            }

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

        public void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<Vector2>();
            m_movementPressedThisFrame = (input.y > 0f && ctx.action.WasPressedThisFrame()) | m_movementPressedThisFrame;
        }

        public void OnPrimary(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            // noop
        }
    }
}
