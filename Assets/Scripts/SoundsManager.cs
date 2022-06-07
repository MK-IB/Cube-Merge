using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;

    public AudioSource audioSource;
    public List<AudioClip> popSounds;
    public AudioClip win;

    private void Awake()
    {
        instance = this;
    }

    public void PlayPop()
    {
        audioSource.PlayOneShot(popSounds[Random.Range(0, popSounds.Count)]);
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
