using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.Marelle
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private TMP_Text text;
        public bool _victory = false;

        private void Start()
        {
            _victoryPanel.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Victory"))
            {
                _victory = true;

                Time.timeScale = 0;

                _victoryPanel.SetActive(true);

                text.text = "Victory: " + gameObject.name;
            }
        }
    }
}
