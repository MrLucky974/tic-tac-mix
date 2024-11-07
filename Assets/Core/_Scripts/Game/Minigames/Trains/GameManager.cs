using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RapidPrototyping.TicTacMix.Trains
{
    public class GameManager : MonoBehaviour
    {
        [Header("CHRONO")]
        //CHRONO OF THE GAME
        private float _time;
        [SerializeField] private float _endTime;

        private int _chrono;
        [SerializeField] private TMP_Text _chronoText;


        [Header("Score")]

        [SerializeField] private GameObject _victoryPanel;
        private PlayerController[] _player;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text[] _scoreText;

        [SerializeField] private Color[] _colors;

        private void Start()
        {
            _player = FindObjectsOfType<PlayerController>();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            _chrono = (int)_time;
            _chronoText.text = _chrono.ToString();

            if (_time >= _endTime)
            {
                Score();

                if (_time <= 0)
                {
                    _time = 0;
                }
            }
        }

        public void Score()
        {
            _victoryPanel.SetActive(true);

            //Player[0] = X & Player[1] = O
            if (_player[0]._score > _player[1]._score)
            {

                if (_player[0]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = _colors[1];
                    _text.text = "Victory: Player O";

                }
                else
                {
                    _text.GetComponent<TMP_Text>().color = _colors[0];
                    _text.text = "Victory: Player X";
                }
            }
            else if (_player[0]._score < _player[1]._score)
            {

                if (!_player[1]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = _colors[0];
                    _text.text = "Victory: Player X";

                }
                else
                {
                    _text.GetComponent<TMP_Text>().color = _colors[1];
                    _text.text = "Victory: Player O";

                }
            }

            else if (_player[0]._score == _player[1]._score)
            {
                _text.text = "Victory: Tie";
            }

            ShowScoreText();
            Time.timeScale = 0f;
        }

        public void PlayerDead(bool isPlayerO)
        {
            _victoryPanel.SetActive(true);

            if (isPlayerO)
            {
                _text.GetComponent<TMP_Text>().color = _colors[0];
                _text.text = "Victory: Player X";
            }
            else
            {
                _text.GetComponent<TMP_Text>().color = _colors[1];
                _text.text = "Victory: Player O";
            }

            ShowScoreText();
            Time.timeScale = 0f;
        }

        void ShowScoreText()
        {
            _scoreText[0].text = _player[0]._score.ToString();
            _scoreText[1].text = _player[1]._score.ToString();
        }
    }
}
