using System;
using System.Collections;
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
