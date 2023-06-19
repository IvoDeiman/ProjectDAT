using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using System.IO.Ports;

public class Infrared2 : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM6", 9600);
    //public LayerMask defaultlayer;
    //public LayerMask infraredlayer;
    LayerMask layer;
    public Light directionalLight; 
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 100;
        layer = GetComponent<LayerMask>();
        directionalLight.intensity = maxIntensity;
    }

    void Update()
    {

        if (sp.IsOpen)
        {

            try
            {
                if (sp.ReadByte() == 1)
                {
                    SwitchToInfrared();
                }

                else
                {
                    NoInfrared();

                }

            }
            catch (System.Exception)
            {

            }
        }



    }

    void NoInfrared()
    {
        Debug.Log("uit");
        directionalLight.intensity = minIntensity;


        int LayerNum = 0;
        layer = LayerNum;
        Debug.Log(LayerNum);

        if (transform.childCount > 0)
        
            SetLayerAllChildren(transform, LayerNum);
        

    }

    void SwitchToInfrared()
    {
        Debug.Log("aan");
        directionalLight.intensity = maxIntensity;

        int LayerNum = 6;
        layer = LayerNum;
        Debug.Log(LayerNum);

        if (transform.childCount > 0)
        
            SetLayerAllChildren(transform, LayerNum);
            Debug.Log(LayerNum);
        
        

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
