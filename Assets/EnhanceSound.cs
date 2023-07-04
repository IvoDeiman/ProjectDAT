using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhanceSound : MonoBehaviour
{
    [Header("Volume settings are between values 0 and 1")]
    [SerializeField] private float loud, normal;

    private AudioSource _audio;
    private float threshold = 0.01f;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void EnhanceAudioVolume()
    {
        if (_audio.volume >= loud - threshold) return;

        _audio.volume = Mathf.Lerp(_audio.volume, loud, 1f * Time.deltaTime);
    }

    public void NormalizeAudioVolume()
    {
        if (_audio.volume == normal) return;

        if (_audio.volume <= normal + threshold)
            _audio.volume = normal;

        _audio.volume = Mathf.Lerp(_audio.volume, normal, 1f * Time.deltaTime);
    }

}
