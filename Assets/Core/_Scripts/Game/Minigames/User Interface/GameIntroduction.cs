using TMPro;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class GameIntroduction : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_introductionCanvasGroup;
        [SerializeField] private CanvasGroup m_userInterfaceCanvasGroup;

        [Space]

        [SerializeField] private TextMeshProUGUI m_timerLabel;

        private void Start()
        {
            Show(m_introductionCanvasGroup);
            Hide(m_userInterfaceCanvasGroup);
        }

        public void UpdateCountdown(int seconds)
        {
            m_timerLabel.SetText(seconds.ToString());
        }

        public void ShowUserInterface()
        {
            Hide(m_introductionCanvasGroup);
            Show(m_userInterfaceCanvasGroup);
        }

        private void Hide(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.GetComponent<RectTransform>().localScale = Vector3.zero;
        }

        private void Show(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }
}
