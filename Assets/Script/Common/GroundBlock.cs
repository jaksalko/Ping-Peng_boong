using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
	public Material red;
	public Renderer _renderer;

	// Start is called before the first frame update
	void Start()
    {
		_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			_renderer.material = red;
		}
	}
}
