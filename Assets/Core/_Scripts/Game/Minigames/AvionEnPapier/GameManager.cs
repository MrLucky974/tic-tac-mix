using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class GameManager : MonoBehaviour
    {
        private bool playerOIsAlive = true;  
        private bool playerXIsAlive = true;

        private bool _isFinished = false;

        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private TMP_Text text;



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

            Time.timeScale = 0f;
        }


        private void DetermineWinner()
        {
            
            _victoryPanel.SetActive(true);

            if (!playerOIsAlive && playerXIsAlive)
            {
                text.text = "Victory: Player X" ;
            }
          
            else if (!playerXIsAlive && playerOIsAlive)
            {
                text.text = "Victory: Player O";
            }
            
            else if (!playerOIsAlive && !playerXIsAlive)
            {
                text.text = "Victory: Tie";
            }

            _isFinished = true;
        }
    }
}

