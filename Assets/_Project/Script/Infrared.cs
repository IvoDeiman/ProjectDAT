using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Infrared : MonoBehaviour
{
    public LayerMask defaultlayer;
    public LayerMask infraredlayer;

    private bool infraredActive;

    public Light directionalLight;
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;

    private bool isLightOn = true;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(infraredActive)
            {
                infraredActive = !infraredActive;
                int LayerNum = (int)Mathf.Log(defaultlayer.value, 2);
                gameObject.layer = LayerNum;

                if (transform.childCount > 0)
                    SetLayerAllChildren(transform, LayerNum);

            }
            else
            {
                infraredActive = !infraredActive;
                int LayerNum = (int)Mathf.Log(infraredlayer.value, 2);
                gameObject.layer = LayerNum;

                if (transform.childCount > 0)
                    SetLayerAllChildren(transform, LayerNum);

            }
            
                isLightOn = !isLightOn;
                float intensity = isLightOn ? maxIntensity : minIntensity;
                directionalLight.intensity = intensity;
            
        }
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
    