using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Marelle
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool _isFinishedO;
        [SerializeField] private bool _isFinishedX;

        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private TMP_Text _text;

        [Header("CHRONO")]
        //CHRONO OF THE GAME
        private float _time;
        [SerializeField] private float _endTime;
        private int _chrono;
        [SerializeField] private TMP_Text _chronoText;

        [Header("ArrowSequence")]
        private ArrowSequence _arrowSequence;

        private void Start()
        {
            _arrowSequence = GetComponent<ArrowSequence>();
        }

        private void Update()
        {

            _time += Time.deltaTime;
            _chrono = (int)_time;
            _chronoText.text = _chrono.ToString();

            if (_time >= _endTime)
            {
                _victoryPanel.SetActive(true);
                DetermineWinner();
                _arrowSequence._canMove = false;

                if (_time <= 0)
                {
                    _time = 0;
                }
            }
        }

        public void PlayerFinished(bool isPlayerO) 
        {
            _arrowSequence._canMove = false;

            if (isPlayerO)
            {
                _isFinishedO = true;
            }
            else
            {
                _isFinishedX = true;
            }
             DetermineWinner();
        }

    private void DetermineWinner()
    {
        _victoryPanel.SetActive(true);

        if (_isFinishedO)
        {
                _text.GetComponent<TMP_Text>().color = Color.red;
                _text.text = "Victory: Player O";
        }
        else if (_isFinishedX)
        {
                _text.GetComponent<TMP_Text>().color = Color.blue;
                _text.text = "Victory: Player X";
        }
        else
            {
                _text.text = "Victory: Tie";
            }

            Time.timeScale = 0;
        
    }
    }
}
