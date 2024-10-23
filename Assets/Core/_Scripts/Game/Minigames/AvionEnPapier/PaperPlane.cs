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

            if(collision.gameObject.CompareTag("Right"))
            {
                Flip();

                StartCoroutine(waitSpawnL());

            }
            if (collision.gameObject.CompareTag("Left"))
            {
                Flip();

                StartCoroutine(waitSpawnR());

            }

            if(collision.gameObject.CompareTag("Finish"))
            {
                print("end");
            }
        }

        void Flip()
        {
            Vector3 flip = new Vector3(0, 180, 0);

            transform.Rotate(flip);
            _rb.velocity = Vector3.up * 4;
        }

        IEnumerator waitSpawnL()
        {
            yield return new WaitForSeconds(0.4f);
            _spawner.SpawnL() ;

        }
        IEnumerator waitSpawnR()
        {
            yield return new WaitForSeconds(0.4f);
            _spawner.SpawnR();

        }

    }
}
