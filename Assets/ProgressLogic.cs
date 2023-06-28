using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLogic : MonoBehaviour
{
    private FadeScreen fade;

    [SerializeField] private float timeToStartProgression = 5f;
    [SerializeField] private GameObject followUpProgressionCylinder;
    private bool isProgressing = false;

    private void Start() {
        fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<FadeScreen>();
    }

    public void StartProgression() {
        if (isProgressing) return;
        StartCoroutine(ChannelingProgression());
    }

    public void StopProgression() {
        StopCoroutine(ChannelingProgression());
        isProgressing = false;
    }


    private IEnumerator ChannelingProgression() {
        isProgressing = true;
        yield return new WaitForSeconds(timeToStartProgression);
        print("Done progressing, should start walking now");
        isProgressing = false;
        //TODO: start walking
        fade.ToggleFade();
        followUpProgressionCylinder.SetActive(true);
        gameObject.SetActive(false);
    }


}
