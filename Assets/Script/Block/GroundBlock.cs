using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBlock : Block
{

	public GameObject snow;


    public void IsNormal(bool normal)
    {
		if (normal)
			snow.SetActive(true);
		else
			snow.SetActive(false);
    }


	private void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.CompareTag("Leg"))
		{
			
		}
	}

   
}
