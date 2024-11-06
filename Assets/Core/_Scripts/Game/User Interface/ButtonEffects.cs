using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RapidPrototyping.TicTacMix
{
    [DisallowMultipleComponent]
    public sealed class ButtonEffects : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [Serializable]
        public struct Audio
        {
            public AudioClip Hover;
            public AudioClip Click;
        }

        [SerializeField] private Audio m_audio;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_audio.Click == null)
                return;

            SoundManager.Play(m_audio.Click);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_audio.Hover == null)
                return;

            SoundManager.Play(m_audio.Hover);
        }
    }
}
