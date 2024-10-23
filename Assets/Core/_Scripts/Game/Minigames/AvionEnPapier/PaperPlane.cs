using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.AvionEnPapier
{
    public class PaperPlane : MonoBehaviour
    {
        [Header("Move")]
        private Rigidbody _rb;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _moveSpeed;

        [SerializeField] private KeyCode _keyToJump;

        [Header("Spawner")]
        [SerializeField] private Spawner _spawner;

        private void Start()
        {
            _spawner = FindObjectOfType<Spawner>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Move();

            if (Input.GetKeyDown(_keyToJump))
            {
                Fly();
            }

        }
        void Fly()
        {
            _rb.velocity = Vector3.up * _jumpForce;
        }

        void Move()
        {
            transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {

            if(collision.gameObject.CompareTag("Terrain"))
            {
            Vector3 flip = new Vector3(0, 180, 0);

            transform.Rotate(flip);
            _rb.velocity = Vector3.up * 4;

                StartCoroutine(waitSpawn());

            }
        }

        IEnumerator waitSpawn()
        {
            yield return new WaitForSeconds(0.2f);
            _spawner.SpawnR();
            _spawner.SpawnL();
        }
    }
}
