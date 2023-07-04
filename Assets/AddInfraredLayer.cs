using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInfraredLayer : MonoBehaviour
{


    public void SetInfraredLayer() {
        // Set layer to RenderAbove (6), which is the infrared layer
        // To do: change light? and reset in ResetLayer()
        
        if (transform.root.GetComponent<HenkReformed>().isInfraredOn == true) {
            //gameObject.layer = 6;

        }
    }

    public void ResetLayer() {
        // Reset layer back to default
        //gameObject.layer = 0;
    }

}
