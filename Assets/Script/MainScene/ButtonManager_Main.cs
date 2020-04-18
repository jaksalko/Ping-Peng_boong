using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PressIslandBtn()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void PressPlayBtn()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void PressEglooBtn()
	{
		SceneManager.LoadScene("MyInfoScene");
	}
}
