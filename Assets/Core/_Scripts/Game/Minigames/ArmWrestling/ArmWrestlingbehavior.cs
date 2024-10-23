using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

namespace RapidPrototyping.TicTacMix.ArmWresling
{
    public class ArmWrestlingbehavior : MonoBehaviour
    {
        [SerializeField] ArmWreslingUIManager m_uiManager;
        [SerializeField] ArmWreslingGameManager m_gameManager;
        [SerializeField] private int m_playerIndex;
        public enum Inputs
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        Dictionary<Inputs, Vector2> m_dictionary = new();

        private Inputs m_p1Inputs;
        private Inputs m_p2Inputs;

        private int m_Score;
        private int m_beforChangeP1 = 5;
        private int m_beforChangeP2 = 5;
        void Start()
        {
            InitDictionary();

            m_p1Inputs = Inputs.UP;
            m_p2Inputs = Inputs.RIGHT;
        }

        void Update()
        {
            if (m_playerIndex == 0)
            {
                var input = InputManager.InputActions.P1Gameplay;
                if (input.Movement.WasPressedThisFrame())
                {
                    var movement = input.Movement.ReadValue<Vector2>();
                    ValidInput(m_p1Inputs, movement, m_playerIndex);
                    m_beforChangeP1--;
                    Debug.Log(m_beforChangeP1);
                    ChangingInput(m_beforChangeP1, m_p1Inputs);
                }
                
            }
            else
            {
                var input = InputManager.InputActions.P2Gameplay;
                if (input.Movement.WasPressedThisFrame())
                {
                    var movement = input.Movement.ReadValue<Vector2>();
                    ValidInput(m_p2Inputs, movement, m_playerIndex);
                    m_beforChangeP2--;
                    Debug.Log(m_beforChangeP2);
                    ChangingInput(m_beforChangeP2, m_p2Inputs);
                }
            }
        }
        void InitDictionary()
        {
            m_dictionary.Add(Inputs.UP, new Vector2(0, 1));
            m_dictionary.Add(Inputs.DOWN, new Vector2(0, -1));
            m_dictionary.Add(Inputs.LEFT, new Vector2(-1, 0));
            m_dictionary.Add(Inputs.RIGHT, new Vector2(1, 0));
        }

        void ChangingInput(int current, Inputs playerinput)
        {
            if (current <= 0)
            {
                if (playerinput == m_p1Inputs)
                {
                    m_p1Inputs = (Inputs)Random.Range(0, 4);
                    m_uiManager.ShowRightIcon(m_p1Inputs, this);
                    Debug.Log(GetPlayerIndex());
                    Debug.Log($"current input for p1 {playerinput.ToString()}, new input for p1 {m_p1Inputs.ToString()}");
                }
                else
                {
                    m_p2Inputs = (Inputs)Random.Range(0, 4);
                    m_uiManager.ShowRightIcon(m_p2Inputs, this);

                    Debug.Log(GetPlayerIndex());
                    Debug.Log($"current input for p2 : {playerinput.ToString()}, new input for p2 {m_p2Inputs.ToString()}");
                }

                if (m_beforChangeP1 == current)
                {
                    m_beforChangeP1 = Random.Range(10, 30);
                }
                else
                {
                    m_beforChangeP2 = Random.Range(10, 30);
                }
            }
        }

        public bool GetPlayerIndex()
        {
            bool IsPlayer1 = false;
            if (m_playerIndex == 0)
            {
                IsPlayer1 = true;
            }
            return IsPlayer1;
        }
        void ValidInput(Inputs currentInput, Vector2 playerInput, int playerindex)
        {
            Vector2 value = new Vector2(0, 0);
            int count = 0;
            foreach (Inputs inputs in m_dictionary.Keys)
            {
                if (inputs == currentInput)
                {
                    value = m_dictionary.ElementAt(count).Value;
                }
                count++;
            }

            if (playerInput == value)
            {
                if (playerindex == 0)
                {
                    Debug.Log("right input for player 1");
                    m_gameManager.IncreaseScore();
                }
                else if (playerindex == 1)
                {
                    Debug.Log("right input for player 2");
                    m_gameManager.DecreaseScore();
                }

            }
        }
    }
}
