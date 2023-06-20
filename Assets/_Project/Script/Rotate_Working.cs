using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class Rotate_Working : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM5", 9600); // set port of your arduino connected to computer
    public int angle = 0;
    // public Vector3 Vector3 { get; private set; }
    public string receivedstring = "9.00";
    private Transform superSenseScanner;
    // Start is called before the first frame update
    void Start()
    {
        superSenseScanner = GameObject.FindGameObjectWithTag("Scanner").GetComponent<Transform>();
        Debug.Log("komt -ie hier?");
        try
        {
            sp.Open();
            print("Open ");
            sp.DtrEnable = true;
            sp.RtsEnable = true;
        } catch (Exception e)
        {
            Debug.Log(e.ToString());
            Debug.Log("wat is dat voor irritant geluid op de gang?");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("hier?");
        try
        {
            if (sp.IsOpen)
            {
                // Ignore wakup message "a"
                if (sp.ReadLine() == "a") return;

                receivedstring = sp.ReadLine();
                Debug.Log(receivedstring);
                //string[] datas = receivedstring.Split(',');
                //int recv_angl = Mathf.RoundToInt(float.Parse(datas[0]));
                int recv_angl = Mathf.RoundToInt(float.Parse(receivedstring));
                //transform.eulerAngles = new Vector3(0, recv_angl, 0);
                //superSenseScanner.rotation = Quaternion.Euler(Vector3.up * (recv_angl));
                superSenseScanner.eulerAngles = Vector3.up * recv_angl;
            }
         }
        catch (Exception e) { }
        }
}
