using RapidPrototyping.Utils.Input;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.AttrapeOmbres
{
    public class PlayerController : MonoBehaviour, IPlayerMovementControls
    {
        [Header("Player")]
        public bool _isPlayerO;

        [Header("Move")]
        private float _horizontal;
        private float _vertical;
        private Rigidbody _rb;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotSpeed = 10f;
        private int _limit = 8;

        [Header("Score")]
        [HideInInspector] public int _score;
        [SerializeField] private TMP_Text _scoreText;

        [SerializeField] private string[] _nameInput;

        [Header("GameManager")]
        private GameManager _gameManager;

        [Header("Audio")]
        [SerializeField] private AudioClip[] _audioClip;

        #region Input Variables

        private Vector2 m_movementInput;

        #endregion

        private void Start()
        {
            GameInputHandler.SetReciever(gameObject, _isPlayerO ? 1 : 0);

            _gameManager = FindObjectOfType<GameManager>();
            _rb = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            //Movements
            _horizontal = m_movementInput.x;
            _vertical = m_movementInput.y;
        
            //Limits de la map
            float limitsx = Mathf.Clamp(transform.position.x, -_limit, _limit);
            float limitsz = Mathf.Clamp(transform.position.z, -_limit/2, _limit/2);

            transform.position = new Vector3(limitsx, transform.position.y, limitsz);

             // Rotation vers la direction du mouvement
            Vector3 moveDirection = new(_horizontal, 0, _vertical);
            if (moveDirection.magnitude > 0.1f) // V�rifier si on se d�place 
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotSpeed); // Ajuste la vitesse de rotation
            }
        }

        private void FixedUpdate()
        {
            // Appliquer le mouvement dans la direction actuelle
            Vector3 direction = new(m_movementInput.x, 0, m_movementInput.y);
            Vector3 moveDirection = _speed * direction.magnitude * transform.forward;
            _rb.linearVelocity = new Vector3(moveDirection.x, _rb.linearVelocity.y, moveDirection.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);

            if (other.CompareTag("Finish"))
            {
                _gameManager.PlayerDead(_isPlayerO);
                SoundManager.Play(_audioClip[1]);
                return;
            }

            SoundManager.Play(_audioClip[0]);

            _score++;
            _rb.linearDamping ++;
            _rb.mass++;

            _scoreText.text = _score.ToString();
        }

        public void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            m_movementInput = ctx.ReadValue<Vector2>();
        }
    }
}
