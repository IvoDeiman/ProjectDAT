using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public FadeScreen fade;
    private Transform player;
    [SerializeField] private Transform[] hotspots;
    [SerializeField] private Transform[] decoyWalkOut;
    [SerializeField] private Transform[] decoyWalkIn;

    [SerializeField] private float timer = 5f;
    private int counter = 0;
    private float t = 0f;
    private Transform target;
    private float walkProgress;
    private bool isMoving = false;
    [SerializeField] private float speed = 10f;

    private void Start() {
        fade = GameObject.FindGameObjectWithTag("FadeCool").GetComponent<FadeScreen>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SwitchLocation();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(StartTransition());
        }
        if (!isMoving) return;
        if (Vector3.Distance(transform.position, target.position) < .1f) {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, walkProgress);
        walkProgress = Mathf.Lerp(0, Time.deltaTime * speed, t);
        t += Time.deltaTime;
    }

    private IEnumerator StartTransition() {
        isMoving = true;
        target = decoyWalkOut[counter];
        fade.ToggleFade();
        yield return new WaitForSeconds(1f);
        SwitchLocation();
        fade.ToggleFade();
    }


    private void SwitchLocation() {
        player.position = decoyWalkIn[counter].position;
        target = hotspots[counter];
        counter++;
        t = 0;
    }




}
