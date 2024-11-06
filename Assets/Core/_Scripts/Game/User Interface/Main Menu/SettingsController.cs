using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace LBB.BRA1NFvCK
{
    public class SettingsController : MonoBehaviour
    {
        [Serializable]
        public class AudioSetting
        {
            [SerializeField] private AudioMixerGroup m_group;

            [SerializeField] private Slider m_volumeSlider;
            [SerializeField] private TextMeshProUGUI m_volumeLabel;

            private AudioMixer m_mixer;

            public void Initialize()
            {
                if (m_group == null)
                    return;

                m_mixer = m_group.audioMixer;

                if (m_volumeSlider == null)
                    return;

                m_mixer.GetFloat(m_group.name, out float volume);
                m_volumeSlider.onValueChanged.AddListener(SetVolume);
                m_volumeSlider.value = JUtils.DbToNormalized(volume);
            }

            public void SetVolume(float value)
            {
                m_mixer.SetFloat(m_group.name, JUtils.NormalizedToDb(value));
                UpdateLabel(value);
            }

            private void UpdateLabel(float value)
            {
                m_volumeLabel.SetText((value * 100).ToString("0.0") + "%");
            }
        }

        [SerializeField] private AudioSetting m_masterAudioSetting;
        [SerializeField] private AudioSetting m_sfxAudioSetting;
        [SerializeField] private AudioSetting m_musicAudioSetting;

        private void Start()
        {
            m_masterAudioSetting?.Initialize();
            m_sfxAudioSetting?.Initialize();
            m_musicAudioSetting?.Initialize();
        }

        public void PlayTestSound()
        {

        }
    }
}
