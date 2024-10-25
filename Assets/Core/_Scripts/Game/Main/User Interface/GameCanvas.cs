using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapidPrototyping.TicTacMix.Main
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] private Vector2 m_worldCellSize;

        [SerializeField] private Color m_crossColor;
        [SerializeField] private Color m_circleColor;

        #region Cursor Variables

        [Header("Cursor")]
        [SerializeField] private float m_animationDuration = 0.4f;

        [Space]

        [SerializeField] private Image m_cursor;

        #endregion

        #region Grid Variables

        [SerializeField] private List<Image> m_cellImages = new List<Image>();

        [SerializeField] private Sprite m_emptySprite;
        [SerializeField] private Sprite m_crossSprite;
        [SerializeField] private Sprite m_circleSprite;

        #endregion

        private Vector2Int m_gridPosition = Vector2Int.zero;
        private Coroutine m_movementCoroutine;

        private void Start()
        {
            GameManager.Instance.OnGridChanged += UpdateGrid;
            GameDataHandler.Instance.OnTurnChanged += HandlePlayerTurn;
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

        private void UpdateGrid(GameManager.Symbol[] grid)
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

                var symbol = GameManager.GetSymbol(gridPosition.x, gridPosition.y);
                switch (symbol)
                {
                    case GameManager.Symbol.None:
                        cell.sprite = m_emptySprite;
                        break;
                    case GameManager.Symbol.Cross:
                        cell.sprite = m_crossSprite;
                        break;
                    case GameManager.Symbol.Circle:
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
                    AnimateCursor();
                }

                if (input.Primary.WasPressedThisFrame())
                {
                    bool cellEmpty = GameManager.IsCellEmpty(m_gridPosition.x,
                        m_gridPosition.y);
                    if (cellEmpty)
                    {

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
                        m_gridPosition.y += Mathf.CeilToInt(movement.y);
                    }
                    m_gridPosition.x = Mathf.Clamp(m_gridPosition.x, 0, 2);
                    m_gridPosition.y = Mathf.Clamp(m_gridPosition.y, 0, 2);
                    AnimateCursor();
                }
            }
        }

        private IEnumerator UpdateCursorPosition()
        {
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
