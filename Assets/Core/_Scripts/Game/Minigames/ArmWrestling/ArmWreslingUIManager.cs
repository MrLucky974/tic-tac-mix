using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.ArmWresling
{
    public class ArmWreslingUIManager : MonoBehaviour
    {
        [SerializeField] ArmWreslingGameManager m_gameManager;
        [SerializeField] TextMeshProUGUI m_scoreDisplay;

        private void Update()
        {
            m_scoreDisplay.text =$"Score : { m_gameManager.GetScore().ToString()}";
        }
    }
}
