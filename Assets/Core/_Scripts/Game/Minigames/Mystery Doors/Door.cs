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
        [SerializeField] private CanvasGroup[] m_inputPrompts;

        [Header("Audio")]
        [SerializeField] private AudioClip[] m_openSounds;
        [SerializeField] private AudioClip m_correctSound;
        [SerializeField] private AudioClip m_wrongSound;

        protected Player m_playerOne, m_playerTwo;
        private bool m_isTrapped;

        private void Start()
        {
            foreach (var inputPrompt in m_inputPrompts)
            {
                inputPrompt.alpha = 0f;
                inputPrompt.GetComponent<RectTransform>().localScale = Vector3.zero;
                inputPrompt.gameObject.SetActive(false);
            }
        }

        public void Initialize(bool trapped)
        {
            m_isTrapped = trapped;
            m_spriteRenderer.sprite = m_closedSprite;
        }

        public void OpenDoor(Player player)
        {
            StartCoroutine(OpenDoorSequence(player));
        }

        protected virtual void UseDoor(Player player)
        {
            if (m_isTrapped)
            {
                var topStage = GameManager.TopFloor;
                player.SetNewStage(topStage);
                SoundManager.Play(m_wrongSound);
            }
            else
            {
                if (player.currentStage is Floor floor)
                {
                    var nextStage = floor.previousStage;
                    player.SetNewStage(nextStage);
                    SoundManager.Play(m_correctSound);
                }
            }
        }

        private IEnumerator OpenDoorSequence(Player player)
        {
            m_spriteRenderer.sprite = m_openedSprite;
            player.SetAnimationState(Player.AnimationState.OPEN_DOOR);

            if (m_openSounds != null && m_openSounds.Length > 0)
            {
                var sound = m_openSounds.PickRandomUnity();
                SoundManager.Play(sound);
            }

            yield return new WaitForSecondsRealtime(0.1f);

            UseDoor(player);

            m_spriteRenderer.sprite = m_closedSprite;
            player.SetAnimationState(Player.AnimationState.IDLE);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                if (m_inputPrompts.Length > player.PlayerIndex)
                {
                    var inputPrompt = m_inputPrompts[player.PlayerIndex];
                    inputPrompt.gameObject.SetActive(true);
                    inputPrompt.alpha = 1f;
                    inputPrompt.GetComponent<RectTransform>().localScale = Vector3.one;
                }

                player.hoveredDoors.Add(this);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                if (m_inputPrompts.Length > player.PlayerIndex)
                {
                    var inputPrompt = m_inputPrompts[player.PlayerIndex];
                    inputPrompt.alpha = 0f;
                    inputPrompt.GetComponent<RectTransform>().localScale = Vector3.zero;
                    inputPrompt.gameObject.SetActive(false);
                }

                if (player.hoveredDoors.Contains(this))
                {
                    player.hoveredDoors.Remove(this);
                }
            }
        }
    }
}
