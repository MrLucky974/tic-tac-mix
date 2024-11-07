using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.ArmWresling
{
    public class ArmWreslingUIManager : MonoBehaviour
    {

        [SerializeField] ArmWreslingGameManager m_gameManager;
        [SerializeField] TextMeshProUGUI m_scoreDisplay;
        [SerializeField] TextMeshProUGUI m_iconP1;
        [SerializeField] TextMeshProUGUI m_iconP2;

        Dictionary<ArmWrestlingbehavior.Inputs, string> m_listIconsP1 = new Dictionary<ArmWrestlingbehavior.Inputs, string>() ;
        Dictionary<ArmWrestlingbehavior.Inputs, string> m_listIconsP2 = new Dictionary<ArmWrestlingbehavior.Inputs, string>();

        private void Awake()
        {
            InitDictionaryP1();
            InitDictionaryP2();
            Debug.Log(m_listIconsP1);
        }

        void InitDictionaryP1()
        {
            m_listIconsP1.Add(ArmWrestlingbehavior.Inputs.UP, "Z");
            m_listIconsP1.Add(ArmWrestlingbehavior.Inputs.DOWN, "S");
            m_listIconsP1.Add(ArmWrestlingbehavior.Inputs.LEFT, "Q");
            m_listIconsP1.Add(ArmWrestlingbehavior.Inputs.RIGHT, "D");

        }

        void InitDictionaryP2()
        {
            m_listIconsP2.Add(ArmWrestlingbehavior.Inputs.UP, "UP");
            m_listIconsP2.Add(ArmWrestlingbehavior.Inputs.DOWN, "DOWN");
            m_listIconsP2.Add(ArmWrestlingbehavior.Inputs.LEFT, "LEFT");
            m_listIconsP2.Add(ArmWrestlingbehavior.Inputs.RIGHT, "RIGHT");
        }

        public void ShowRightIcon(ArmWrestlingbehavior.Inputs currentInput, ArmWrestlingbehavior behavior)
        {
            if (behavior.GetPlayerIndex() == true)
            {
                m_iconP1.text = m_listIconsP1[currentInput];
            }
            else
            {
                m_iconP2.text = m_listIconsP2[currentInput];
                Debug.Log("joueur 2 icon changed");
            }
        }




        private void Update()
        {
            m_scoreDisplay.text =$"Score : { m_gameManager.GetScore().ToString()}";
        }
    }
}
