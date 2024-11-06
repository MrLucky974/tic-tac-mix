using LuckiusDev.Utils;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class SoundManager : PersistentSingleton<SoundManager>
    {
        [SerializeField] private AudioSource m_sfxSource;

        public static void Play(AudioClip clip)
        {
            if (clip == null)
                return;

            Instance.m_sfxSource.PlayOneShot(clip);
        }
    }
}
