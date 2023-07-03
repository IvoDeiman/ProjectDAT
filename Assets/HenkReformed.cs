using System;
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

    [SerializeField] private ThreatDetector dangerAwareness;
    [SerializeField] private AudioChecker focusedHearing;

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

    private void InterpretInput(int input)
    {
        switch (input)
        {
            case 0:
                dangerAwareness.SetActive(false);
                DisableInfrared();
                focusedHearing.SetActive(false);
                break;
            case 1:
                focusedHearing.SetActive(false);
                DisableInfrared();
                dangerAwareness.SetActive(true);
                break;
            case 2:
                focusedHearing.SetActive(false);
                dangerAwareness.SetActive(false);
                EnableInfrared();
                break;
            case 3:
                dangerAwareness.SetActive(false);
                DisableInfrared();
                focusedHearing.SetActive(true);
                break;
            default:
                throw new Exception("Input outside of expected range (0-3).");
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

                InterpretInput(rcv_int);

                //print(sp.ReadByte());
                //Debug.Log(rcv_int.ToString());

                // NOTE: NOT gameObject, but variable.
                gameobject.transform.eulerAngles = new Vector3(0, recv_angl, 0);
            }
            catch (Exception)
            {
            }
        }
    }

    void DisableInfrared()
    {
        directionalLight.intensity = minIntensity;

        int LayerNum = 0;
        layer = LayerNum;

        if (transform.childCount > 0)
            SetLayerAllChildren(transform, LayerNum);
    }

    void EnableInfrared()
    {
        directionalLight.intensity = maxIntensity;

        int LayerNum = 6;
        layer = LayerNum;

        if (transform.childCount > 0)
            SetLayerAllChildren(transform, LayerNum);
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        Transform[] children = root.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (Transform child in children)
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

