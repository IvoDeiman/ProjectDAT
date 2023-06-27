using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChecker : MonoBehaviour
{

    public List<GameObject> scannedObjects = new List<GameObject>();
    public float rotateAngle = .5f;


    private void Update() {
        transform.Rotate(Vector3.up * (rotateAngle * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other) {
        // fuck off capsule
        if (other.gameObject.name == "Capsule") return;
        if (other.gameObject.GetComponent<Target>() != null) {
            other.gameObject.GetComponent<Target>().ScannerEnter.Invoke();
        }
        // executing unity event on object to start senses
        /*
        if (other.gameObject.GetComponent<FocusEffect>() != null) {
            other.gameObject.GetComponent<FocusEffect>().startEnhance();
        }*/
        scannedObjects.Add(other.gameObject);

    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "Capsule") return;
        /*
        if (other.gameObject.GetComponent<FocusEffect>() != null) {
            other.gameObject.GetComponent<FocusEffect>().startNormal();
        }*/

        if (other.gameObject.GetComponent<Target>() != null) {
            other.gameObject.GetComponent<Target>().ScannerExit.Invoke();
        }

        foreach (GameObject gameObject in scannedObjects) {
            if (gameObject.gameObject == other.gameObject) {
                gameObject.gameObject.GetComponent<Target>().ScannerExit.Invoke();
                scannedObjects.Remove(gameObject);
                return;
            }
        }

    }
}
