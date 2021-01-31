using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<AudioSource> sources;

    private void Awake()
    {
        Instance = this;
    }


    public void PlayAudioClip(AudioClip clip)
    {
        var source = GetNextFreeAudioSource();
        source.volume = 1;
        source.clip = clip;
        source.Play();
    }

    public void PlayAudioClip(AudioClip clip, float volume)
    {
        var source = GetNextFreeAudioSource();
        source.volume = volume;
        source.clip = clip;
        source.Play();
    }

    private AudioSource GetNextFreeAudioSource()
    {
        foreach (var source in sources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        return null;
    }
}