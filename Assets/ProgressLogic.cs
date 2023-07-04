using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLogic : MonoBehaviour
{
    [SerializeField] private float timeToStartProgression = 5f;
    [SerializeField] private GameObject followUpProgressionCylinder;
    private bool isProgressing = false;
    private bool willMove = false;
    public CameraMovement player;

    private void Start() {
        //player = GetComponent<CameraMovement>();
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
        willMove = player.StartMoving();
        // using willmove to make sure player will actually start moving. to ensure progression cylinder doesnt disable before route is finished
        if (followUpProgressionCylinder != null && willMove == true) {
            followUpProgressionCylinder.SetActive(true);
        } else {
            print("Reached last section of the game. Play fall down animation here");
        }
        if (willMove) gameObject.SetActive(false);
    }


}
