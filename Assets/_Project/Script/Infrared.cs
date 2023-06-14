using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using System.IO.Ports;

public class Infrared : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM5", 9600);
    public LayerMask defaultlayer;
    public LayerMask infraredlayer;

    private bool infraredActive;

    public Light directionalLight;
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;

    private bool isLightOn = true;

    private bool isButtonPressed = false;

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 500;
    }

    private void Update()
    {
        
        if (sp.IsOpen && sp.ReadByte() == 49) // ASCII code for '1'
        {
            if (!isButtonPressed)
            {
            isButtonPressed = true;
            SwitchToInfrared();
            }
         else
        {
           if (isButtonPressed)
           {
            isButtonPressed = false;
            SwitchToInfrared();
           }
        }
            //if (Input.GetKeyDown(KeyCode.Tab))
            //{
               // SwitchToInfrared(); // Perform the action you want when the Arduino button is pressed
            //}
        }


        // if(sp.ReadByte() == 1)
        // {
        // SwitchToInfrared();
        // }



       // if (Input.GetKeyDown(KeyCode.Tab))
       // {
       // SwitchToInfrared();
       // }
       // if (Input.GetKeyUp(KeyCode.Tab)){
       // SwitchToInfrared();
       // }
        
    }

    private void SwitchToInfrared(){
        if(infraredActive)
            {
                infraredActive = !infraredActive;
                int LayerNum = (int)Mathf.Log(defaultlayer.value, 2);
                gameObject.layer = LayerNum;

                if (transform.childCount > 0)
                    SetLayerAllChildren(transform, LayerNum);
            }
            else
            {
                infraredActive = !infraredActive;
                int LayerNum = (int)Mathf.Log(infraredlayer.value, 2);
                gameObject.layer = LayerNum;

                if (transform.childCount > 0)
                    SetLayerAllChildren(transform, LayerNum);
            }          
            isLightOn = !isLightOn;
            float intensity = isLightOn ? maxIntensity : minIntensity;
            if(directionalLight != null) {
                directionalLight.intensity = intensity;
            }
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
    private void OnApplicationQuit()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
        }
    }
}
