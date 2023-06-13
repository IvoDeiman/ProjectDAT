using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using System.IO.Ports;

public class Infrared : MonoBehaviour
{
    SerialPort sp = new SerialPort("/dev/cu.usbmodem11101", 9600);
    public LayerMask defaultlayer;
    public LayerMask infraredlayer;

    private bool infraredActive;

    public Light directionalLight;
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;

    private bool isLightOn = true;

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 100;
    }

    private void Update()
    {
        if(sp.ReadByte() == 1)
        {
            SwitchToInfrared();
        }

        //if(Input.GetKeyUp(KeyCode.Tab)){
            //SwitchToInfrared();
        //}
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
}
    