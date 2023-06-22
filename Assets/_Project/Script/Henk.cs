using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Henk : MonoBehaviour
{

    SerialPort sp = new SerialPort("COM7", 9600);
    Light myLight;
    private string receivedString;

    public GameObject gameobject;
    public Light directionalLight;
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;

    private string[] datas;
    private int recv_angl = 0;
    private int rcv_int = 0;

    LayerMask layer;



    // Start is called before the first frame update
    void Start()
    {
        // Application.targetFrameRate = 300;

        sp.Open();
        sp.RtsEnable = true;
        sp.DtrEnable = true; 
        
        directionalLight.intensity = maxIntensity;
       // InvokeRepeating("SerialDataRead", 0f, 0.1f);
    }

    void SetIntensity(int Status)
    {
        if (Status == 1)
        {
            NoInfrared();
            
        } else {
            SwitchToInfrared();
            
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (sp.IsOpen)
        {
            try
            {
                receivedString = sp.ReadLine();
                datas = receivedString.Split(',');


                var temp_angl = Mathf.RoundToInt(float.Parse(datas[0]));
                var temp_int = Mathf.RoundToInt(float.Parse(datas[1]));


                if (recv_angl == temp_angl && rcv_int == temp_int) return;
                //if (recv_angl != temp_angl) recv_angl = temp_angl;
                //if (rcv_int != temp_int) rcv_int = temp_int;

                recv_angl = temp_angl;
                rcv_int = temp_int;

                SetIntensity(rcv_int);
                //print(sp.ReadByte());
               // Debug.Log(rcv_int.ToString());

                gameobject.transform.eulerAngles = new Vector3(0, recv_angl, 0);
                sp.ReadTimeout = 25;
                //sp.DiscardInBuffer();
                //GC.Collect();
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

