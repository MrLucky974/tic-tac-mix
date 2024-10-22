using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.ArmWresling
{
    public class ArmWrestlingbehavior : MonoBehaviour
    {
        private int m_playerIndex;
        private enum Inputs
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        Dictionary<Inputs, Vector2> m_dictionary = new();

        private Inputs m_p1Inputs;
        private Inputs m_p2Inputs;

        private int m_Score = 0;

        void Start()
        {
            InitDictionary();
            
            m_p1Inputs = Inputs.LEFT;
            m_p2Inputs = Inputs.RIGHT;
        }

        void Update()
        {
            if (m_playerIndex == 0)
            {
                var input = InputManager.InputActions.P1Gameplay;
            }
            else
            {
                var input = InputManager.InputActions.P2Gameplay;
            }
        }
        void InitDictionary()
        {
            m_dictionary.Add(Inputs.UP, new Vector2(0, 1));
            m_dictionary.Add(Inputs.DOWN, new Vector2(0, -1));
            m_dictionary.Add(Inputs.LEFT, new Vector2(-1, 0));
            m_dictionary.Add(Inputs.RIGHT, new Vector2(1, 0));
        }
    }
}
