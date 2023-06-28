using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChecker : MonoBehaviour
{

    public float rotateAngle = .5f;

    public List<GameObject> scannedObjects = new List<GameObject>();

    // Should be a copy of the scannedObjects list but instead of GameObject data, 
    // It should have Target data, allowing is to invoke without constant GetComponent Usage.
    private List<Target> scannedTargets = new List<Target>(); 


    private void Update()
    {
        transform.Rotate(Vector3.up * (rotateAngle * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        // Pretty sure this check is not needed but I'm still putting it in here for safety.
        // Also fuck off capsule.
        if (scannedObjects.Contains(other.gameObject) || other.gameObject.name == "Capsule") return;

        // Removed redundant calls of GetComponent to improve performance.
        Target otherTarget = other.gameObject.GetComponent<Target>();

        // Stop any object not having the Target Component.
        if (otherTarget == null) return;

        // Scanned Objects now should have only Objects with the Target Class.
        scannedObjects.Add(other.gameObject);
        scannedTargets.Add(otherTarget);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!scannedObjects.Contains(other.gameObject)) return;

        //When button is pressed, Invoke ScannerEnter.
        //Must be GetKey to account for any objects entering the field.
        if (Input.GetKey(KeyCode.E))
            foreach (Target target in scannedTargets)
            {
                //Only invoke ScannerEnter if target has not invoked ScannerEnter yet.
                if (!target.IsInvoking("ScannerEnter"))
                    target.ScannerEnter.Invoke();
            }
        //When button is let go, Invoke ScannerExit.
        else if (Input.GetKeyUp(KeyCode.E))
            foreach (Target target in scannedTargets)
            {
                // Only invoke ScannerExit if target is invoking ScannerEnter.
                    target.ScannerExit.Invoke();
            }
    }

    private void OnTriggerExit(Collider other)
    {
        Target otherTarget = other.gameObject.GetComponent<Target>();

        if (!scannedObjects.Contains(other.gameObject) || otherTarget == null) return;

        // In case the target is leaving the trigger while still invoking ScannerEnter,
        // Switch it back to ScannerExit.
        otherTarget.ScannerExit.Invoke();

        // Remove other gameObject from the scannedObjects List.
        if (scannedObjects.Contains(other.gameObject))
            scannedObjects.Remove(other.gameObject);

        // Remove other Target from the scannedTargets List.
        if (scannedTargets.Contains(otherTarget))
            scannedTargets.Remove(otherTarget);
    }
}
