using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGlowup : MonoBehaviour
{

    [SerializeField] private Material normal, red;
    private ParticleSystem particle;
    private MeshRenderer rend;

    private void Start() {
        rend = GetComponent<MeshRenderer>();
        particle = transform.GetChild(0).GetComponent<ParticleSystem>();
    }


    public void TurnRed() {
        rend.material = red;
        particle.Play();
    }

    public void TurnNormal() {
        rend.material = normal;
        particle.Stop();
    }

}
