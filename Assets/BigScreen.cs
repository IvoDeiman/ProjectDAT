using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigScreen : MonoBehaviour
{
    private int width;
    private int height;


    private float t = 0f;
    public float refreshtime = 3f;

    private void Start() {
        width = Screen.width * 2;
        height = Screen.height * 2;
    }

    // Update is called once per frame
    void Update()
    {

        if(t > refreshtime) { 
        Screen.SetResolution(width, height, false);
            t = 0f;
        }
        t += Time.deltaTime;

    }


    
}
