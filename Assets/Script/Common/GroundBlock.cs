using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
	
    
	
	private void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.CompareTag("Leg"))
		{
			
		}
	}

   
}
