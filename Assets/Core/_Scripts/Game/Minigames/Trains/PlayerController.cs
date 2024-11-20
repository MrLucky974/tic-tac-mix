using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace RapidPrototyping.TicTacMix.Trains
{
    public class PlayerController : MonoBehaviour, IPlayerControls
    {
        [Header("Input")]
        [SerializeField] private PlayerInput m_playerInput;

        [Header("Player")]
        public bool _isPlayerO;

        [Header("Move")]
        public float _speed = 5f;
        [SerializeField] private float _rotSpeed = 180f;

        [SerializeField] private string _horizontal;
        [SerializeField] private float _bodySpeed = 10f;

        [Header("Instantiate")]
        [SerializeField] private GameObject _bodyPrefab;
        private List<GameObject> _bodyParts = new List<GameObject>();
        [SerializeField] private Transform _bodypos;

        [SerializeField] private int _gap = 10;

        private List<Vector3> _pos = new List<Vector3>();

        [Header("Score")]
        public int _score;

        [SerializeField] private TMP_Text _scoreText;

        [SerializeField] private string _collide;

        [SerializeField] private Transform _parent;
        [Header("GameManager")]
        public GameManager _gameManager;

        [Header("Audio")]
        [SerializeField] private AudioClip[] _audioClip;

        #region Input Variables

        private Vector2 m_movementInput;

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

            _gameManager = FindObjectOfType<GameManager>();
           // Grow();
        }
        private void Update()
        {
            //Move
            transform.position += _speed * Time.deltaTime * transform.forward;

            //Rotate
            float rotDirection = m_movementInput.x;
            transform.Rotate(_rotSpeed * rotDirection * Time.deltaTime * Vector3.up);

            //StorePosBody
            _pos.Insert(0, transform.position);

            int index = 0;
            foreach (var body in _bodyParts)
            {
                Vector3 point = _pos[Mathf.Min(index * _gap, _pos.Count - 1)];
                Vector3 moveDir = point - body.transform.position;
                body.transform.position += _bodySpeed * Time.deltaTime * moveDir;
                body.transform.LookAt(point);
                index++;
                body.SetActive(true);
            }

        }

        void Grow()
        {
            GameObject body = Instantiate(_bodyPrefab, _bodypos.position, Quaternion.identity, _parent);
            _bodyParts.Add(body);
            body.gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Item>() != null)
            {
                Grow();
                _score++;
                _scoreText.text = _score.ToString();
                SoundManager.Play(_audioClip[0]);

                Destroy(collision.gameObject);

            }

            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                print("destroyBOTH");
                DestroyAll();
                collision.gameObject.GetComponent<PlayerController>().DestroyAll();
                _gameManager.Tie() ;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(_collide))
            {
                _gameManager.PlayerDead(_isPlayerO);
                print("destroyONE");
                DestroyAll();
            }
        }

        public void DestroyAll()
        {
            Destroy(gameObject);
            Destroy(_parent.gameObject);

            SoundManager.Play(_audioClip[1]);
        }

        public void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            m_movementInput = ctx.ReadValue<Vector2>();
        }

        public void OnPrimary(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            // noop
        }
    }
}
