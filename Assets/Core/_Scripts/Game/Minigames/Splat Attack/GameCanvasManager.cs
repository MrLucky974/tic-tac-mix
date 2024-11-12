using LuckiusDev.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RapidPrototyping.TicTacMix.SplatAttack
{
    public class GameCanvasManager : MonoBehaviour
    {
        [SerializeField] private RectTransform m_scoreIcon;
        [SerializeField] private Image m_scoreImage;
        [SerializeField] private TextMeshProUGUI m_scoreText;
        [SerializeField] private Gradient m_gradient;

        private void Update()
        {
            float offset = GameManager.GetPlayerCoverage(PlayerIdentifier.PLAYER_ONE) - GameManager.GetPlayerCoverage(PlayerIdentifier.PLAYER_TWO);

            m_scoreText.SetText(Mathf.RoundToInt(JMath.Remap(-offset, -1f, 1f, -100f, 100f)).ToString());

            var position = m_scoreIcon.anchoredPosition;
            position.x = -offset * 600f;
            m_scoreIcon.anchoredPosition = position;

            m_scoreImage.color = m_gradient.Evaluate(JMath.Remap(-offset, -1f, 1f, 0f, 1f));
        }
    }
}
