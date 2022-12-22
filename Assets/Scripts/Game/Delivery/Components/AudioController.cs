using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSourcesPool;
    private static AudioController _instance;

    private void Awake()
    {
        _instance = this;
    }
    
    public static void PlaySound(AudioClip clip, float pitch = 1, float delay = 0f)
    {
        var audioSource = _instance.GetOrCreateAudioSource();
        audioSource.pitch = pitch;
        audioSource.clip = clip;
        audioSource.Play();
    }
    
    private AudioSource GetOrCreateAudioSource()
    {
        if (audioSourcesPool.FirstOrDefault(x => x.isPlaying == false) != null)
            return audioSourcesPool.First(x => x.isPlaying == false);
        else
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSourcesPool.Add(audioSource);
            return audioSource;
        }
    }
}