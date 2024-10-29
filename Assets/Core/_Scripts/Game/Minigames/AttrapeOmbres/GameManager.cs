using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.AttrapeOmbres
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

                _victoryPanel.SetActive(true);


                if (_time <= 0)
                {
                    _time = 0;
                }
            }
        }

        public void Score()
        {
            //Player[0] = X & Player[1] = O
            if (_player[0]._score > _player[1]._score)
            {

                if (!_player[0]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = Color.red;
                    _text.text = "Victory: Player O";

                }
            }
            else if (_player[0]._score < _player[1]._score)
            {

                if (_player[1]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = Color.blue;
                    _text.text = "Victory: Player X";

                }
            }

            else if (_player[0]._score == _player[1]._score)
            {
                _text.text = "Victory: Tie";
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
