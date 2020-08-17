using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    public ParticleSystem particle;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Leg"))
        {
            //particle.Play();
            Destroy(gameObject);
        }
    }
	
}
