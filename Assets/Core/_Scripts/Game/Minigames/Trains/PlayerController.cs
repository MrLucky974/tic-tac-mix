using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Trains
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player")]
        public bool _isPlayerO;

        [Header("Move")]
        [SerializeField] private float _speed = 5f;
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

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            Grow();
        }
        private void Update()
        {
            //Move
            transform.position += transform.forward * _speed * Time.deltaTime;

            //Rotate
            float rotDirection = Input.GetAxis(_horizontal);
            transform.Rotate(Vector3.up * rotDirection * _rotSpeed * Time.deltaTime);

            //StorePosBody
            _pos.Insert(0, transform.position);

            int index = 0;
            foreach (var body in _bodyParts)
            {
                Vector3 point = _pos[Mathf.Min(index * _gap, _pos.Count - 1)];
                Vector3 moveDir = point - body.transform.position;
                body.transform.position += moveDir * _bodySpeed * Time.deltaTime;
                body.transform.LookAt(point);
                index++;
                body.gameObject.SetActive(true);
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

                Destroy(collision.gameObject);

            }

            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                DestroyAll();
                collision.gameObject.GetComponent<PlayerController>().DestroyAll();
                _gameManager.PlayerDead(_isPlayerO);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(_collide))
            {
                _gameManager.PlayerDead(_isPlayerO);
                print("destroy");
                DestroyAll();
            }
        }

        public void DestroyAll()
        {
            Destroy(gameObject);
            Destroy(_parent.gameObject);
        }
    }
}
