using System;
using System.Collections;
using UnityEngine;

public class ThreatDetector : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Should exactly match the name of the the tag.")]
    private string threatTag = "Threat";

    private GameObject threatObject;
    private Target threatTarget;
    private bool isActive;

    public void SetActive(bool value)
    {
        isActive = value;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(threatTag)) return;

        threatObject = other.gameObject;
        threatTarget = threatObject.GetComponent<Target>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject != threatObject || threatObject == null) return;

        if (Input.GetKey(KeyCode.Q) || isActive)
        {
            if (!threatTarget.IsInvoking("ScannerEnter"))
                threatTarget.ScannerEnter.Invoke();
        }
        else
            threatTarget.ScannerExit.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject != threatObject || threatObject == null) return;

        threatObject = null;
        threatTarget = null;
    }
}

