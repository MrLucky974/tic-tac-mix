using System.Collections;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class Door : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer m_spriteRenderer;

        [Header("Animations")]
        [SerializeField] private Sprite m_closedSprite;
        [SerializeField] private Sprite m_openedSprite;

        [Header("User Interface")]
        [SerializeField] private CanvasGroup m_p1InputPromptCanvasGroup;
        [SerializeField] private CanvasGroup m_p2InputPromptCanvasGroup;

        protected Player m_playerOne, m_playerTwo;
        private bool m_isTrapped;

        public void Initialize(bool trapped)
        {
            m_isTrapped = trapped;
            m_spriteRenderer.sprite = m_closedSprite;

            m_p1InputPromptCanvasGroup.alpha = 0f;
            m_p1InputPromptCanvasGroup.GetComponent<RectTransform>().localScale = Vector3.zero;
            m_p1InputPromptCanvasGroup.gameObject.SetActive(false);

            m_p2InputPromptCanvasGroup.alpha = 0f;
            m_p2InputPromptCanvasGroup.GetComponent<RectTransform>().localScale = Vector3.zero;
            m_p2InputPromptCanvasGroup.gameObject.SetActive(false);
        }

        protected virtual void Update()
        {
            if (!m_playerOne && !m_playerTwo)
                return;

            if (m_playerOne)
            {
                var input = InputManager.InputActions.P1Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().y;
                if (input.Movement.WasPressedThisFrame() && movement > 0f)
                {
                    //if (m_isTrapped)
                    //{
                    //    var topStage = GameManager.TopFloor;
                    //    m_playerOne.SetNewStage(topStage);
                    //}
                    //else
                    //{
                    //    if (m_playerOne.currentStage is Floor floor)
                    //    {
                    //        var nextStage = floor.previousStage;
                    //        m_playerOne.SetNewStage(nextStage);
                    //    }
                    //}
                    StartCoroutine(OpenDoor(m_playerOne));
                }
            }

            if (m_playerTwo)
            {
                var input = InputManager.InputActions.P2Gameplay;
                var movement = input.Movement.ReadValue<Vector2>().y;
                if (input.Movement.WasPressedThisFrame() && movement > 0f)
                {
                    //if (m_isTrapped)
                    //{
                    //    var topStage = GameManager.TopFloor;
                    //    m_playerTwo.SetNewStage(topStage);
                    //}
                    //else
                    //{
                    //    if (m_playerTwo.currentStage is Floor floor)
                    //    {
                    //        var nextStage = floor.previousStage;
                    //        m_playerTwo.SetNewStage(nextStage);
                    //    }
                    //}
                    StartCoroutine(OpenDoor(m_playerTwo));
                }
            }
        }

        private IEnumerator OpenDoor(Player player)
        {
            m_spriteRenderer.sprite = m_openedSprite;
            player.SetAnimationState(Player.AnimationState.OPEN_DOOR);

            yield return new WaitForSecondsRealtime(0.1f);

            if (m_isTrapped)
            {
                var topStage = GameManager.TopFloor;
                player.SetNewStage(topStage);
            }
            else
            {
                if (player.currentStage is Floor floor)
                {
                    var nextStage = floor.previousStage;
                    player.SetNewStage(nextStage);
                }
            }

            m_spriteRenderer.sprite = m_closedSprite;
            player.SetAnimationState(Player.AnimationState.IDLE);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                if (player.PlayerIndex == 0)
                {
                    m_p1InputPromptCanvasGroup.gameObject.SetActive(true);
                    m_p1InputPromptCanvasGroup.alpha = 1f;
                    m_p1InputPromptCanvasGroup.GetComponent<RectTransform>().localScale = Vector3.one;

                    m_playerOne = player;
                }

                if (player.PlayerIndex == 1)
                {
                    m_p2InputPromptCanvasGroup.gameObject.SetActive(true);
                    m_p2InputPromptCanvasGroup.alpha = 1f;
                    m_p2InputPromptCanvasGroup.GetComponent<RectTransform>().localScale = Vector3.one;

                    m_playerTwo = player;
                }
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                if (player.PlayerIndex == 0)
                {
                    m_p1InputPromptCanvasGroup.alpha = 0f;
                    m_p1InputPromptCanvasGroup.GetComponent<RectTransform>().localScale = Vector3.zero;
                    m_p1InputPromptCanvasGroup.gameObject.SetActive(false);

                    m_playerOne = null;
                }

                if (player.PlayerIndex == 1)
                {
                    m_p2InputPromptCanvasGroup.alpha = 0f;
                    m_p2InputPromptCanvasGroup.GetComponent<RectTransform>().localScale = Vector3.zero;
                    m_p2InputPromptCanvasGroup.gameObject.SetActive(false);

                    m_playerTwo = null;
                }
            }
        }
    }
}
