using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhanceSound : MonoBehaviour
{

    private AudioSource _audio;
    [Header("Volume settings are between values 0 and 1")]
    [SerializeField] private float loud, normal;


    private void Start() {
        _audio = GetComponent<AudioSource>();
    }

    public void EnhanceAudioVolume() {
        _audio.volume = loud;
    }

    public void NormalizeAudioVolume() {
        _audio.volume = normal;
    }

}
