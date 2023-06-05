using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChecker : MonoBehaviour
{

    public List<GameObject> audios = new List<GameObject>();
    public float rotateAngle = .5f;


    private void Update() {
        transform.Rotate(Vector3.up * (rotateAngle * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other) {
        // fuck off capsule
        if (other.gameObject.name == "Capsule") return;
        // executing unity event on object to start senses
        if (other.gameObject.GetComponent<FocusEffect>() != null) {
            other.gameObject.GetComponent<FocusEffect>().startEnhance();
        }
        audios.Add(other.gameObject);

    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "Capsule") return;

        if (other.gameObject.GetComponent<FocusEffect>() != null) {
            other.gameObject.GetComponent<FocusEffect>().startNormal();
        }

        foreach (GameObject audio in audios) {
            if (audio.gameObject == other.gameObject) {
                audios.Remove(audio);
                return;
            }
        }

    }
}
