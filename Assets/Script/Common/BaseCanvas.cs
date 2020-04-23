using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCanvas : MonoBehaviour
{
	public static BaseCanvas Instance;

    void Awake()
    {
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

	}

}
