using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AudioScriptPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private float delayedPlaybackTime;
    [SerializeField] private float sentenceEndDelay = .15f;
    [SerializeField] private VoiceOver[] voiceoverScenes;
    [Header("Debug")]
    [SerializeField] private bool logging = true;

    private int progressCounter = -1;

    private void Start()
    {
        StartCoroutine(ContinueVoiceOver(0, 1));
    }

    /// <summary>
    /// Starts voiceover scene immediately.
    /// </summary>
    /// <param name="sceneNumber">Number of scene in the Audio Script Player.</param>
    public void StartVoiceOverImmediate(int sceneNumber)
    {
        if (logging) print($"Voiceover | Starting Scene: {sceneNumber}");
        StartCoroutine(ContinueVoiceOver(sceneNumber, 0));
    }

    public void StartVoiceOverDelayed(int sceneNumber)
    {
        if (logging) print($"Voiceover | Queue Delayed Scene: {sceneNumber}");
        StartCoroutine(ContinueVoiceOver(sceneNumber, delayedPlaybackTime));
    }

    private IEnumerator ContinueVoiceOver(int sceneNumber, float delayTime)
    {
        VoiceOver voiceOver = voiceoverScenes[sceneNumber];
        yield return new WaitForSeconds(delayTime);

        if (voiceOver.voiceLines.Length - 1 > progressCounter)
        {
            progressCounter++;
            AudioClip nextClip = voiceOver.voiceLines[progressCounter].voiceLine;
            UnityEvent eventBefore = voiceOver.voiceLines[progressCounter].invokeBeforePlaying;

            // If the voiceline has an event bound to it. Execute it now.
            if (eventBefore != null)
                eventBefore.Invoke();

            if (logging) print($"Voiceover | Playing | Scene: {sceneNumber}, Line: {progressCounter}");

            // Play the new audioclip and start recursion.
            _audio.clip = nextClip;
            _audio.Play();
            yield return new WaitForSeconds(_audio.clip.length + sentenceEndDelay);
            StartCoroutine(ContinueVoiceOver(sceneNumber, 0));
        }
        else
        {
            // Reset if there are no more audioclip in scene.
            if (logging) print($"Voiceover | Finished Scene");
            progressCounter = -1;
        }
    }
}

[Serializable]
public struct VoiceOver
{
    public VoiceOverClip[] voiceLines;
}

[Serializable]
public struct VoiceOverClip
{
    public AudioClip voiceLine;
    public UnityEvent invokeBeforePlaying;
}