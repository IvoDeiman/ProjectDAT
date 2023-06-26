using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{

    [Header("Use this to assign the reaction the object should do, \nwhen the player is focused on it")]
    public UnityEvent ScannerExit;
    [Header("Use this to assign what happens, \nwhen the Target is no longer in focus of the player")]
    public UnityEvent ScannerEnter;

}
