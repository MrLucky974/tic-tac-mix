using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RapidPrototyping.TicTacMix
{
    public class GameCanvas : MonoBehaviour
    {
        #region Cursor

        [Header("Cursor")]
        [SerializeField] private Vector2 m_worldCellSize;
        [SerializeField] private float m_animationDuration = 0.4f;

        [Space]

        [SerializeField] private Image m_cursor;

        #endregion

        private Vector2Int m_gridPosition = Vector2Int.zero;
        private Coroutine m_movementCoroutine;

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
