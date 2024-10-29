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
        private int _limit = 8;

        [Header("Score")]
        [HideInInspector] public int _score;
        [SerializeField] private TMP_Text _scoreText;

        [SerializeField] private string[] _nameInput;

        private void Start()
        {
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
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector3(_horizontal, 0, _vertical) * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
            _score++;

            _scoreText.text = _score.ToString();

            if(other.CompareTag("Finish"))
            {
                //InstantDead
            }
        }
    }
}
