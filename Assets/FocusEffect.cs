using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FocusEffect : MonoBehaviour
{


    public UnityEvent enhance;
    public UnityEvent normal;


    public void startEnhance() {
        enhance.Invoke();
    }
    public void startNormal() {
        normal.Invoke();
    }

}
