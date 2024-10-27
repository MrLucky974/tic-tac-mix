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

        public void PlayerFinished(bool isPlayerO) 
        {
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
            _text.text = "Victory: Player O";
        }
        else if (_isFinishedX)
        {
            _text.text = "Victory: Player X";
        }

            Time.timeScale = 0;
        
    }
    }
}
