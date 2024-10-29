using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.AttrapeOmbres
{
    public class PlayerController : MonoBehaviour
    {

        private float _horizontal;
        private float _vertical;
        private Rigidbody _rb;
        [SerializeField] private int _limitR;
        [SerializeField] private int _limitL;

        private int _score;
        [SerializeField] private TMP_Text _scoreText;

        //[SerializeField] private string[] _nameInput;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            //Movements
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
        
            //Limits de la map
            float limitsx = Mathf.Clamp(transform.position.x, _limitL, _limitR);
            float limitsz = Mathf.Clamp(transform.position.z, _limitL/2, _limitR/2);

            transform.position = new Vector3(limitsx, transform.position.y, limitsz);
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector3(_horizontal, 0, _vertical) * 10;
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
            _score++;

            _scoreText.text = _score.ToString();
        }
    }
}
