using LuckiusDev.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RapidPrototyping.TicTacMix.ArmWresling
{
    public class ArmWreslingUIManager : MonoBehaviour
    {
        [Serializable]
        private struct PlayerIcons
        {
            public Sprite UpIcon;
            public Sprite LeftIcon;
            public Sprite DownIcon;
            public Sprite RightIcon;
        }

        [SerializeField] private ArmWreslingGameManager m_gameManager;

        [Space]

        [SerializeField] private TextMeshProUGUI m_scoreDisplay;
        [SerializeField] private RectTransform m_scoreIcon;
        [SerializeField] private Image m_scoreImage;
        [SerializeField] private Gradient m_scoreGradient;

        [Header("Icons")]

        [SerializeField] private TextMeshProUGUI m_textP1;
        [SerializeField] private Image m_iconP1;
        [SerializeField] private PlayerIcons m_p1Icons;

        [Space]

        [SerializeField] private TextMeshProUGUI m_textP2;
        [SerializeField] private Image m_iconP2;
        [SerializeField] private PlayerIcons m_p2Icons;

        [Header("Game Over Screen")]
        [SerializeField] private CanvasGroup m_gameOverCanvas;
        [SerializeField] private TextMeshProUGUI m_winLabel;
        [SerializeField] private Color m_playerOneColor;
        [SerializeField] private Color m_playerTwoColor;

        private Dictionary<ArmWrestlingBehavior.Inputs, string> m_listIconsP1 = new Dictionary<ArmWrestlingBehavior.Inputs, string>();
        private Dictionary<ArmWrestlingBehavior.Inputs, string> m_listIconsP2 = new Dictionary<ArmWrestlingBehavior.Inputs, string>();

        private void Awake()
        {
            InitDictionaryP1();
            InitDictionaryP2();
        }

        private void Start()
        {
            m_gameOverCanvas.interactable = false;
            m_gameOverCanvas.alpha = 0f;
            m_gameOverCanvas.GetComponent<RectTransform>().localScale = Vector3.zero;

            m_gameManager.OnGameEnded += HandleGameEnd;
        }

        private void OnDestroy()
        {
            m_gameManager.OnGameEnded -= HandleGameEnd;
        }

        private void Update()
        {
            var score = m_gameManager.GetScore();
            m_scoreDisplay.text = score.ToString();

            var offset = JMath.Remap(score, ArmWreslingGameManager.MIN_SCORE, ArmWreslingGameManager.MAX_SCORE, -1f, 1f);
            var position = m_scoreIcon.anchoredPosition;
            position.x = -offset * 600f;
            m_scoreIcon.anchoredPosition = position;

            m_scoreImage.color = m_scoreGradient.Evaluate(JMath.Remap(-offset, -1f, 1f, 0f, 1f));
        }

        private void HandleGameEnd(int winIndex)
        {
            if (winIndex == ArmWreslingGameManager.TIE_INDEX)
            {
                m_winLabel.color = Color.white;
                m_winLabel.SetText("It's a tie :(");
            }
            else
            {
                var symbol = winIndex == ArmWreslingGameManager.PLAYER_ONE_INDEX ? "X" : "O";
                var color = winIndex == ArmWreslingGameManager.PLAYER_ONE_INDEX ? m_playerOneColor : m_playerTwoColor;
                m_winLabel.color = color;
                m_winLabel.SetText($"Player {symbol} won!");
            }

            m_gameOverCanvas.interactable = true;
            m_gameOverCanvas.alpha = 1f;
            m_gameOverCanvas.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        private void InitDictionaryP1()
        {
            m_listIconsP1.Add(ArmWrestlingBehavior.Inputs.UP, "Z");
            m_listIconsP1.Add(ArmWrestlingBehavior.Inputs.DOWN, "S");
            m_listIconsP1.Add(ArmWrestlingBehavior.Inputs.LEFT, "Q");
            m_listIconsP1.Add(ArmWrestlingBehavior.Inputs.RIGHT, "D");
        }

        private void InitDictionaryP2()
        {
            m_listIconsP2.Add(ArmWrestlingBehavior.Inputs.UP, "UP");
            m_listIconsP2.Add(ArmWrestlingBehavior.Inputs.DOWN, "DOWN");
            m_listIconsP2.Add(ArmWrestlingBehavior.Inputs.LEFT, "LEFT");
            m_listIconsP2.Add(ArmWrestlingBehavior.Inputs.RIGHT, "RIGHT");
        }

        public void ShowRightIcon(ArmWrestlingBehavior.Inputs currentInput, ArmWrestlingBehavior behavior)
        {
            if (behavior.GetPlayerIndex() == true)
            {
                m_textP1.text = m_listIconsP1[currentInput];
                switch (currentInput)
                {
                    case ArmWrestlingBehavior.Inputs.UP:
                        m_iconP1.sprite = m_p1Icons.UpIcon;
                        break;
                    case ArmWrestlingBehavior.Inputs.DOWN:
                        m_iconP1.sprite = m_p1Icons.DownIcon;
                        break;
                    case ArmWrestlingBehavior.Inputs.LEFT:
                        m_iconP1.sprite = m_p1Icons.LeftIcon;
                        break;
                    case ArmWrestlingBehavior.Inputs.RIGHT:
                        m_iconP1.sprite = m_p1Icons.RightIcon;
                        break;
                }
            }
            else
            {
                m_textP2.text = m_listIconsP2[currentInput];
                switch (currentInput)
                {
                    case ArmWrestlingBehavior.Inputs.UP:
                        m_iconP2.sprite = m_p2Icons.UpIcon;
                        break;
                    case ArmWrestlingBehavior.Inputs.DOWN:
                        m_iconP2.sprite = m_p2Icons.DownIcon;
                        break;
                    case ArmWrestlingBehavior.Inputs.LEFT:
                        m_iconP2.sprite = m_p2Icons.LeftIcon;
                        break;
                    case ArmWrestlingBehavior.Inputs.RIGHT:
                        m_iconP2.sprite = m_p2Icons.RightIcon;
                        break;
                }
            }
        }
    }
}
