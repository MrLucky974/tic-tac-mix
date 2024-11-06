using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapidPrototyping.TicTacMix.Main
{
    public class GameCanvas : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Vector2 m_worldCellSize;

        [SerializeField] private Color m_crossColor;
        [SerializeField] private Color m_circleColor;

        #region Cursor Variables

        [Header("Cursor")]
        [SerializeField] private float m_animationDuration = 0.4f;
        [SerializeField] private Image m_cursor;

        [Space]

        #endregion

        #region Grid Variables

        [Header("Grid")]
        [SerializeField] private List<Image> m_cellImages = new List<Image>();

        [SerializeField] private Sprite m_emptySprite;
        [SerializeField] private Sprite m_crossSprite;
        [SerializeField] private Sprite m_circleSprite;

        #endregion

        private Vector2Int m_gridPosition = Vector2Int.zero;
        private Coroutine m_movementCoroutine;
        private bool m_canSelectMinigame = true;

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

        private void HandlePlayerTurn(GameDataHandler.Turn turn)
        {
            switch (turn)
            {
                case GameDataHandler.Turn.PLAYER_1:
                    m_cursor.color = m_crossColor;
                    break;
                case GameDataHandler.Turn.PLAYER_2:
                    m_cursor.color = m_circleColor;
                    break;
            }
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

        private void Update()
        {
            MoveCursor();
        }

        private void MoveCursor()
        {
            if (GameDataHandler.CurrentTurn == GameDataHandler.Turn.PLAYER_1)
            {
                var input = InputManager.InputActions.P1Gameplay;
                var movementInput = input.Movement;
                if (movementInput.WasPressedThisFrame())
                {
                    var movement = movementInput.ReadValue<Vector2>();
                    if (movement.x != 0f)
                    {
                        m_gridPosition.x += Mathf.CeilToInt(movement.x);
                    }
                    else if (movement.y != 0f)
                    {
                        m_gridPosition.y -= Mathf.CeilToInt(movement.y);
                    }
                    m_gridPosition.x = Mathf.Clamp(m_gridPosition.x, 0, 2);
                    m_gridPosition.y = Mathf.Clamp(m_gridPosition.y, 0, 2);
                    GameDataHandler.DataHolder.GridPosition = m_gridPosition;
                    AnimateCursor();
                }

                if (input.Primary.WasPressedThisFrame() && m_canSelectMinigame)
                {
                    bool cellEmpty = GridManager.IsCellEmpty(m_gridPosition.x,
                        m_gridPosition.y);
                    if (cellEmpty)
                    {
                        GameDataHandler.SelectRandomMinigame();
                    }
                }
            }
            else
            {
                var input = InputManager.InputActions.P2Gameplay;
                var movementInput = input.Movement;
                if (movementInput.WasPressedThisFrame())
                {
                    var movement = movementInput.ReadValue<Vector2>();
                    if (movement.x != 0f)
                    {
                        m_gridPosition.x += Mathf.CeilToInt(movement.x);
                    }
                    else if (movement.y != 0f)
                    {
                        m_gridPosition.y -= Mathf.CeilToInt(movement.y);
                    }
                    m_gridPosition.x = Mathf.Clamp(m_gridPosition.x, 0, 2);
                    m_gridPosition.y = Mathf.Clamp(m_gridPosition.y, 0, 2);
                    GameDataHandler.DataHolder.GridPosition = m_gridPosition;
                    AnimateCursor();
                }

                if (input.Primary.WasPressedThisFrame() && m_canSelectMinigame)
                {
                    bool cellEmpty = GridManager.IsCellEmpty(m_gridPosition.x,
                        m_gridPosition.y);
                    if (cellEmpty)
                    {
                        GameDataHandler.SelectRandomMinigame();
                    }
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
        }

        private void AnimateCursor()
        {
            if (m_movementCoroutine != null)
            {
                StopCoroutine(m_movementCoroutine);
            }

            m_movementCoroutine = StartCoroutine(nameof(UpdateCursorPosition));
        }
    }
}
