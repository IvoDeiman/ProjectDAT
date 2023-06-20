using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using System.IO.Ports;
using System;

public class Infrared2 : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM5", 9600);
    //public LayerMask defaultlayer;
    //public LayerMask infraredlayer;
    LayerMask layer;
    public Light directionalLight; 
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;
    private Transform superSenseScanner;

    //head
    public int angle = 0;
    public string receivedstring = "9.00";

    void Start()
    {
        superSenseScanner = GameObject.FindGameObjectWithTag("Scanner").GetComponent<Transform>();
        sp.Open();
        sp.ReadTimeout = 100;

        //layer = gameObject.layer;
        //layer = GetComponent<LayerMask>();
        directionalLight.intensity = maxIntensity;

        SwitchToInfrared();
        NoInfrared();

        //head
         try
         {
           sp.Open();
          sp.DtrEnable = true;
           sp.RtsEnable = true;
         }
         catch (Exception e)
         {
             Debug.Log(e.ToString());
            Debug.Log("So no head?");
         }
    }

    void Update()
    {

        if (sp.IsOpen || Input.GetKeyDown(KeyCode.Tab)) 
        {

            try
            {
                if (sp.ReadByte() == 1)
                {
                    SwitchToInfrared();
                }

                else if (sp.ReadByte() == 2)
                {
                    NoInfrared();

                }
                //               |
                // put head here V


                //head
                try {
                    if (sp.ReadByte() != 1 || sp.ReadByte() != 2 || sp.ReadByte() != 3)
                    {
                        //if (recv_angl == 3) return;
                        // Ignore wakup message "a"
                        receivedstring = sp.ReadLine();
                        print("head angle:" + receivedstring);
                        if (receivedstring == "3") return;
                        int recv_angl = Mathf.RoundToInt(float.Parse(receivedstring));

                        // please dont rotate all objects in space
                        //transform.eulerAngles = new Vector3(0, recv_angl, 0);
                        //superSenseScanner.eulerAngles = new Vector3(0, recv_angl, 0);
                        //superSenseScanner.localRotation = new Quaternion(0, recv_angl, 0);

                        //works V
                        superSenseScanner.eulerAngles = Vector3.up * recv_angl;

                    }
                }
                catch (Exception e) { }


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
        //Debug.Log(LayerNum);

        if (transform.childCount > 0)
        
            SetLayerAllChildren(transform, LayerNum);
        

    }

    void SwitchToInfrared()
    {
        Debug.Log("aan");
        directionalLight.intensity = maxIntensity;

        int LayerNum = 6;
        layer = LayerNum;
        //Debug.Log(LayerNum);

        if (transform.childCount > 0)
        
            SetLayerAllChildren(transform, LayerNum);
            //Debug.Log(LayerNum);
        
        

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
