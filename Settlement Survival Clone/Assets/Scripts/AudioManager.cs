using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioClip[] audioClips;
    AudioSource camAudio;

    public static AudioManager Instance { get; private set; }

    private void Start()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;

        camAudio = GetComponent<AudioSource>();
    }
    public void AudioPlay(AudioClip audioClip)
    {
        camAudio.PlayOneShot(audioClip);
    }
}
