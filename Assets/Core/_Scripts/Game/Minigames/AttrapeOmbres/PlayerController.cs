using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.AttrapeOmbres
{
    public class PlayerController : MonoBehaviour
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


        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {

            //Movements
            _horizontal = Input.GetAxis(_nameInput[0]);
            _vertical = Input.GetAxis(_nameInput[1]);
        
            //Limits de la map
            float limitsx = Mathf.Clamp(transform.position.x, -_limit, _limit);
            float limitsz = Mathf.Clamp(transform.position.z, -_limit/2, _limit/2);

            transform.position = new Vector3(limitsx, transform.position.y, limitsz);

             // Rotation vers la direction du mouvement
            Vector3 moveDirection = new Vector3(_horizontal, 0, _vertical);
            if (moveDirection.magnitude > 0.1f) // Vérifier si on se déplace 
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotSpeed); // Ajuste la vitesse de rotation
            }
        }

        private void FixedUpdate()
        {
            // _rb.velocity = new Vector3(_horizontal, transform.position.y, _vertical) * _speed;

            // Appliquer le mouvement dans la direction actuelle
            Vector3 moveDirection = transform.forward * _speed * new Vector3(_horizontal, 0, _vertical).magnitude;
            _rb.velocity = new Vector3(moveDirection.x, _rb.velocity.y, moveDirection.z);
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
            _rb.drag ++;
            _rb.mass++;


            _scoreText.text = _score.ToString();

            
        }
    }
}
