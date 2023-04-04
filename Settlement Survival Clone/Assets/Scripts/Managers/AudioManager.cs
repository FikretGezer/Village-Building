using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField] AudioClip clip_OnPlaced;
    AudioSource source;

    private void OnEnable()
    {
        BuildingManager.OnObjectPlaced += PlayOnObjectPlaced;
    }
    private void OnDisable()
    {
        BuildingManager.OnObjectPlaced -= PlayOnObjectPlaced;
    }
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void PlayOnObjectPlaced()
    {
        if(source != null)
            source.PlayOneShot(clip_OnPlaced);
    }
}
