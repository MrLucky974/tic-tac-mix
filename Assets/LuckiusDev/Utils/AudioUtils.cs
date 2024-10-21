using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioUtils
{
    public static AudioSource Play( AudioClip clip, AudioMixerGroup group, Transform parent = null, float pitch = 1.0f, float spatialBlend = 0.0f ) {
        GameObject go = new GameObject($"AudioPlayer[{clip.name}]")
        {
            transform =
            {
                parent = parent
            }
        };

        var source = go.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = group;
        source.spatialBlend = spatialBlend;
        source.pitch = pitch;
        source.clip = clip;
        source.Play();
        
        Object.Destroy(go, clip.length);

        return source;
    }
}
