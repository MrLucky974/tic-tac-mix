using LuckiusDev.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Voleur
{
    public class GameManager : MonoBehaviour
    {
        [Header("End1_HasbeenWatched")]
        private bool playerOIsAlive = true;
        private bool playerXIsAlive = true;

        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private TMP_Text _text;

        [Header("End2_EndTimeBetterScore")]
        //CHRONO OF THE GAME
        private float _time;
        [SerializeField] private float _endTime;

        private int _chrono;
        [SerializeField] private TMP_Text _chronoText;

        private PlayerController[] _playerController;
        [SerializeField] private TMP_Text[] _scoreText;

        private void Start()
        {
            _playerController = FindObjectsOfType<PlayerController>();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            _chrono = (int)_time;
            _chronoText.text = _chrono.ToString();

            if (_time >= _endTime)
            {
                _victoryPanel.SetActive(true);
                Score();

                if (_time <= 0)
                {
                    _time = 0;
                }
            }
        }

        public void PlayerFinished(bool isPlayerO)
        {
            if (isPlayerO)
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


        private void DetermineWinner()
        {

            _victoryPanel.SetActive(true);

            if (!playerOIsAlive && playerXIsAlive)
            {
                _text.GetComponent<TMP_Text>().color = Color.blue;
                _text.text = "Victory: Player X";
            }

            else if (!playerXIsAlive && playerOIsAlive)
            {
                _text.GetComponent<TMP_Text>().color = Color.red;
                _text.text = "Victory: Player O";
            }

            else if (!playerOIsAlive && !playerXIsAlive)
            {
                _text.text = "Victory: Tie";
            }

            ShowScoreText();
            Time.timeScale = 0f;
        }

        void Score()
        {
            if (_playerController[0]._obtainedCake > _playerController[1]._obtainedCake)
            {
                if (_playerController[0]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = Color.red;
                    _text.text = "Victory: Player O";                   
                }             
            }
            else if (_playerController[0]._obtainedCake < _playerController[1]._obtainedCake)
            {
                if (!_playerController[1]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = Color.blue;
                    _text.text = "Victory: Player X";
                }
            }

            else if (_playerController[0]._obtainedCake == _playerController[1]._obtainedCake)
            {
                _text.text = "Victory: Tie";
            }

            ShowScoreText();
            Time.timeScale = 0f;
        }

        void ShowScoreText()
        {
            _scoreText[0].text = _playerController[0]._obtainedCake.ToString();
            _scoreText[1].text = _playerController[1]._obtainedCake.ToString();
        }
    }
}

