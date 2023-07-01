using System;
using System.Collections;
using UnityEngine;

public class ThreatDetector : MonoBehaviour
{
    [SerializeField][Tooltip("Should exactly match the name of the the tag.")] private string threatTag = "Threat";

    private GameObject threatObject;
    private Target threatTarget;

    public void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(threatTag)) return;

        threatObject = other.gameObject;
        threatTarget = gameObject.GetComponent<Target>();
    }

    public void OnCollisionStay(Collision other)
    {
        if (other.gameObject != threatObject || threatObject == null) return;

        if (Input.GetKey(KeyCode.Q))
        {
            if (!threatTarget.IsInvoking("ScannerEnter"))
                threatTarget.ScannerEnter.Invoke();
        }
        else
            threatTarget.ScannerExit.Invoke();
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject != threatObject || threatObject == null) return;

        threatObject = null;
        threatTarget = null;
    }
}

