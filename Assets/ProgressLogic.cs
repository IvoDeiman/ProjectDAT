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
        if (isProgressing || gameObject.activeSelf == false) return;
        print("Start progression");
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
        if (followUpProgressionCylinder != null) {
            followUpProgressionCylinder.SetActive(true);
        } else {
            print("Reached last section of the game. Play fall down animation here");
        }
        gameObject.SetActive(false);
    }


}
