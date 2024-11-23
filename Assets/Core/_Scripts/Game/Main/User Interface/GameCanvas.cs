using LuckiusDev.Utils;
using RapidPrototyping.Utils.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RapidPrototyping.TicTacMix.Main
{
    public class GameCanvas : MonoBehaviour, IPlayerControls
    {
        [Header("Main")]
        [SerializeField] private Vector2 m_worldCellSize;

        [SerializeField] private Color m_crossColor;
        [SerializeField] private Color m_circleColor;

        [Header("Cursor")]
        [SerializeField] private float m_animationDuration = 0.4f;
        [SerializeField] private Image m_cursor;

        [Header("Text")]

        [SerializeField] private TextMeshProUGUI m_playerLabel;
        [SerializeField] private TextMeshProUGUI m_tutorialLabel;

        [Header("Grid")]
        [SerializeField] private List<Image> m_cellImages = new List<Image>();

        [SerializeField] private Sprite m_emptySprite;
        [SerializeField] private Sprite m_crossSprite;
        [SerializeField] private Sprite m_circleSprite;

        private Vector2Int m_gridPosition = Vector2Int.zero;
        private Coroutine m_movementCoroutine;
        private bool m_canSelectMinigame = true;

        [Header("Settings")]
        [SerializeField] private CanvasGroup m_settingsGroup;

        private bool m_lockInput = false;

        #region Input Variables

        private Vector2 m_movementInput;
        private bool m_movementPressedThisFrame;
        private bool m_primaryPressedThisFrame;

        #endregion

        private IEnumerator Start()
        {
            GridManager.Instance.OnGridChanged += UpdateGrid;
            GameDataHandler.Instance.OnTurnChanged += HandlePlayerTurn;

            m_gridPosition = GameDataHandler.DataHolder.GridPosition;
            var targetPosition = m_gridPosition * m_worldCellSize;
            targetPosition.y *= -1f;
            m_cursor.rectTransform.anchoredPosition = targetPosition;

            yield return null;

            UpdateGrid(GridManager.Grid);
            HandlePlayerTurn(GameDataHandler.CurrentTurn);
        }

        private void Update()
        {
            if (m_lockInput)
                return;

            MoveCursor();
            HandlePrimaryPressed();
        }

        private void LateUpdate()
        {
            m_movementPressedThisFrame = false;
            m_primaryPressedThisFrame = false;
        }

        private void HandlePlayerTurn(GameDataHandler.Turn turn)
        {
            switch (turn)
            {
                case GameDataHandler.Turn.PLAYER_1:
                    // Update information labels text
                    m_playerLabel.SetText("Time for X to play!");
                    m_tutorialLabel.SetText("Press \"ZQSD\" to move" + "\n and \"F\" to select");

                    // Update information labels color
                    m_playerLabel.color = m_crossColor;
                    m_tutorialLabel.color = m_crossColor;

                    // Update cursor color
                    m_cursor.color = m_crossColor;
                    break;
                case GameDataHandler.Turn.PLAYER_2:
                    // Update information labels text
                    m_playerLabel.SetText("Time for O to play!");
                    m_tutorialLabel.SetText("Press \"↑←↓→\" to move" + "\n and \"0\" to select");

                    // Update information labels color
                    m_playerLabel.color = m_circleColor;
                    m_tutorialLabel.color = m_circleColor;

                    // Update cursor color
                    m_cursor.color = m_circleColor;
                    break;
            }

            GameInputHandler.SetReciever(gameObject, turn == GameDataHandler.Turn.PLAYER_2 ? 1 : 0);
            GameInputHandler.SetActionMap(GameInputHandler.ActionMapIndex.TicTacToe);
        }

        private void UpdateGrid(GridManager.Symbol[] grid)
        {
            foreach (var cell in m_cellImages)
            {
                if (cell == null) continue;

                var position = cell.rectTransform.anchoredPosition;
                position.y *= -1f;

                var gridPosition = new Vector2Int(
                    Mathf.FloorToInt(position.x / m_worldCellSize.x),
                    Mathf.FloorToInt(position.y / m_worldCellSize.y)
                );

                var symbol = GridManager.GetSymbol(gridPosition.x, gridPosition.y);
                switch (symbol)
                {
                    case GridManager.Symbol.None:
                        cell.sprite = m_emptySprite;
                        break;
                    case GridManager.Symbol.Cross:
                        cell.sprite = m_crossSprite;
                        break;
                    case GridManager.Symbol.Circle:
                        cell.sprite = m_circleSprite;
                        break;
                }
            }
        }

        private void MoveCursor()
        {
            if (m_movementInput.magnitude > 0f && m_movementCoroutine == null)
            {
                if (m_movementInput.x != 0f)
                {
                    m_gridPosition.x += Mathf.CeilToInt(m_movementInput.x);
                }
                else if (m_movementInput.y != 0f)
                {
                    m_gridPosition.y -= Mathf.CeilToInt(m_movementInput.y);
                }

                m_gridPosition.x = Mathf.Clamp(m_gridPosition.x, 0, 2);
                m_gridPosition.y = Mathf.Clamp(m_gridPosition.y, 0, 2);
                GameDataHandler.DataHolder.GridPosition = m_gridPosition;
                AnimateCursor();
            }
        }

        private void HandlePrimaryPressed()
        {
            if (m_primaryPressedThisFrame && m_canSelectMinigame)
            {
                bool cellEmpty = GridManager.IsCellEmpty(m_gridPosition.x,
                    m_gridPosition.y);
                if (cellEmpty)
                {
                    GameDataHandler.SelectRandomMinigame();
                    m_canSelectMinigame = false;
                }
            }
        }

        private IEnumerator UpdateCursorPosition()
        {
            m_canSelectMinigame = false;

            var currentPosition = m_cursor.rectTransform.anchoredPosition;
            var targetPosition = m_gridPosition * m_worldCellSize;
            targetPosition.y *= -1f;

            float time = 0f;
            while (time < m_animationDuration)
            {
                time += Time.deltaTime;
                yield return null;

                m_cursor.rectTransform.anchoredPosition = Vector2.Lerp(
                    a: currentPosition,
                    b: targetPosition,
                    t: Mathf.Clamp01(time / m_animationDuration)
                );
            }

            m_cursor.rectTransform.anchoredPosition = targetPosition;

            m_canSelectMinigame = true;
            m_movementCoroutine = null;
        }

        private void AnimateCursor()
        {
            if (m_movementCoroutine != null)
            {
                StopCoroutine(m_movementCoroutine);
            }

            m_movementCoroutine = StartCoroutine(nameof(UpdateCursorPosition));
        }

        #region Input Event Handlers

        public void OnMovement(InputAction.CallbackContext ctx)
        {
            m_movementInput = ctx.ReadValue<Vector2>();
            m_movementPressedThisFrame |= ctx.action.WasPressedThisFrame();
        }

        public void OnPrimary(InputAction.CallbackContext ctx)
        {
            m_primaryPressedThisFrame |= ctx.action.WasPressedThisFrame();
        }

        #endregion

        public void OpenSettingsMenu()
        {
            m_settingsGroup.Open();
            m_lockInput = true;
        }

        public void CloseSettingsMenu()
        {
            m_settingsGroup.Close();
            m_lockInput = false;
        }
    }
}
