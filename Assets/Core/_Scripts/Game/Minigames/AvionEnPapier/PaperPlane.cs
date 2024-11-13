using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        [SerializeField] private GameObject _blow;

        [Space]
        [SerializeField] private GameObject _fx;

        [Header("Spawner")]
        [SerializeField] private Spawner _spawner;


        [Header("GameManager")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private bool _isPlayerO;

        [Header("Audio")]
        [SerializeField] private AudioClip[] _audioClip;


        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _spawner = FindObjectOfType<Spawner>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {     

            if(_gameManager._canMove) 
            {
                _rb.isKinematic = false;
                Move();
            }
            else
            {
                _rb.isKinematic = true;
            }

            if (Input.GetKeyDown(_keyToJump))
            {
                Fly();
            }

        }
        void Fly()
        {
            _rb.velocity = Vector3.up * _jumpForce;
            SoundManager.Play(_audioClip[0]);
            _blow.GetComponent<Animator>().SetTrigger("Blow");

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
 //FIN//
            if(collision.gameObject.CompareTag("Finish"))
            {
                print("end");
                _gameManager.PlayerFinished(_isPlayerO);
            }
        }

        IEnumerator GetFx()
        {
            _fx.SetActive(true);
            yield return new WaitForSeconds(1);
            _fx.SetActive(false);
        }
        void Flip()
        {
            Vector3 flip = new Vector3(0, 180, 0);

            transform.Rotate(flip);
            _rb.velocity = Vector3.up * 3;

            StartCoroutine(GetFx());
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
