using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ChangeInt : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM6", 9600);
    

    Light directionalLight;
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;


    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 100;
        directionalLight = GetComponent<Light>();
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
        Debug.Log("geen");
        directionalLight.intensity = minIntensity;
    }

    void SwitchToInfrared()
    {
        Debug.Log("wel");
        directionalLight.intensity = maxIntensity;
        

    }

}
