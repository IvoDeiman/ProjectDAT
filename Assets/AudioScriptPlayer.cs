using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AudioScriptPlayer : MonoBehaviour
{
    [SerializeField] private VoiceOver[] voScenes;
    [SerializeField] private AudioSource _audio;

    public void StartSceneImmediate(int sceneNumber)
    {
        _audio.clip = voScenes[sceneNumber].voiceLines[0];
        _audio.Play();
    }

    public void StartSceneDelayed(int sceneNumber, float delay)
    {
        _audio.clip = voScenes[sceneNumber].voiceLines[0];
        _audio.PlayDelayed(delay);
    }
}

public struct VoiceOver
{
    public AudioClip[] voiceLines;
}