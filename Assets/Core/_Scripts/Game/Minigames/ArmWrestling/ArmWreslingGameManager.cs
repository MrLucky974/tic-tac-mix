using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.ArmWresling
{
    public class ArmWreslingGameManager : MonoBehaviour
    {
        private int m_score;
        private int m_minScore = -100;
        private int m_maxScore = 100;

        public int GetScore()
        {
            return m_score;
        }
        public void IncreaseScore()
        {
            m_score++;
            m_score = Mathf.Clamp(m_score, m_minScore, m_maxScore);
        }

        public void DecreaseScore()
        {
            m_score--;
            m_score = Mathf.Clamp(m_score, m_minScore, m_maxScore);
        }
    }
}
