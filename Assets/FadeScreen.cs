using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //ToggleFade();
        }
    }


    public void ToggleFade() {
        anim.SetTrigger("ToggleFade");
    }
    public void BlackScreenOfDeath() {
        anim.SetTrigger("Blackscreen");
    }

}
