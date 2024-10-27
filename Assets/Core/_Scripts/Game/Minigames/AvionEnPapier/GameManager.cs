using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class GameManager : MonoBehaviour
    {


        [Header("Countdown")]
        [SerializeField] private TMP_Text _countdownText;
        public bool _canMove;


        [Header("End1_CatchedByHand")]
        private bool playerOIsAlive = true;  
        private bool playerXIsAlive = true;

        private bool _isFinished = false;

        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private TMP_Text _text;

        [Header("End2_CHRONO")]
        //CHRONO OF THE GAME
        private float _time;
        [SerializeField] private float _endTime;
        private int _chrono;
        [SerializeField] private TMP_Text _chronoText;

        private void Update()
        {
            if (!_canMove)
            {
                StartCoroutine(Countdown());
            }

            if(_canMove)
            {

            _time += Time.deltaTime;
            _chrono = (int)_time;
            _chronoText.text = _chrono.ToString();

            if (_time >= _endTime)
            {
                _victoryPanel.SetActive(true);
                DetermineWinner();

                if (_time <= 0)
                {
                    _time = 0;
                }
            }
            }
        }

        IEnumerator Countdown()
        {

            _countdownText.gameObject.SetActive(true);

            _countdownText.text = "3";
            yield return new WaitForSeconds(1f);
            _countdownText.text = "2";
            yield return new WaitForSeconds(1f);
            _countdownText.text = "1";
            yield return new WaitForSeconds(1f);
            _countdownText.text = "GO!";
            yield return new WaitForSeconds(0.5f);

            _countdownText.gameObject.SetActive(false);

            _canMove = true;
        }



        public void PlayerFinished(bool isPlayerA)
        {
            if(!_isFinished)
            {

            if (isPlayerA)
            {
                playerOIsAlive = false;
                Debug.Log("PlayerO is dead");
            }
            else
            {
                playerXIsAlive = false;
                Debug.Log("Player X is dead");
            }

            DetermineWinner();
            }
           
        }


        private void DetermineWinner()
        {
            
            _victoryPanel.SetActive(true);

            if (!playerOIsAlive && playerXIsAlive)
            {
                _text.GetComponent<TMP_Text>().color = Color.blue;
                _text.text = "Victory: Player X" ;
            }
          
            else if (!playerXIsAlive && playerOIsAlive)
            {
                _text.GetComponent<TMP_Text>().color = Color.red;
                _text.text = "Victory: Player O";
            }
            
            else if (playerOIsAlive && playerXIsAlive)
            {
                _text.text = "Victory: Tie";
            }

            _isFinished = true;
            Time.timeScale = 0f;
        }
    }
}

