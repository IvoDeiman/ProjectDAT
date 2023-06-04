using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMessageListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMessageArrived(string msg)
    {
        Debug.Log("Arrived:"+msg);
    }

    void OnConnectionEvent(bool success)
    {
        Debug.Log(success?"Device connected":"Device disconnected");
    }
}
