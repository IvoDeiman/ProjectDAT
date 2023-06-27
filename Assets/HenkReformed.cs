using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class HenkReformed : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM9", 115200);

    //SerialPort sp = new SerialPort("/dev/tty.usbmodem143201", 115200);
    Light myLight;
    private string receivedString;

    public GameObject gameobject;
    public Light directionalLight;
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;

    //public GameObject infraObject;
    LayerMask layer;
    



    // Start is called before the first frame update
    void Start()
    {


        Application.targetFrameRate = 300;

        sp.Open();
        sp.DtrEnable = true;
        sp.RtsEnable = true;

        directionalLight.intensity = maxIntensity;

        sp.ReadTimeout = 25;
    }

    void SetIntensity(int Status)
    {
        if (Status == 0)
        {
            SwitchToInfrared();
            
        } else {
            NoInfrared();
            
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
                print(receivedString);
                string[] datas = receivedString.Split(',');

                int recv_angl = Mathf.RoundToInt(float.Parse(datas[0]));
                int rcv_int = Mathf.RoundToInt(float.Parse(datas[1]));

                SetIntensity(rcv_int);
                //print(sp.ReadByte());
                //Debug.Log(rcv_int.ToString());


                gameobject.transform.eulerAngles = new Vector3(0, recv_angl, 0);
            }
            catch (System.Exception)
            {


            }
        }
    }


    void NoInfrared()
    {
        //Debug.Log("uit");
        directionalLight.intensity = minIntensity;


        int LayerNum = 0;
        layer = LayerNum;
        //Debug.Log(LayerNum);

        if (transform.childCount > 0) {
            SetLayerAllChildren(transform, LayerNum);
        }


    }

    void SwitchToInfrared()
    {
        //Debug.Log("aan");
        directionalLight.intensity = maxIntensity;

        int LayerNum = 6;
        layer = LayerNum;
        //Debug.Log(LayerNum);

        if (transform.childCount > 0) {
            SetLayerAllChildren(transform, LayerNum);
        }

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

    //private void SetGameLayerRecursive(GameObject gameObject, int layer)
    //{
    //    gameObject.layer = layer;
    //    foreach (Transform child in gameObject.transform)
    //    {
    //        SetGameLayerRecursive(child.gameObject, layer);
    //    }
    //}


}

