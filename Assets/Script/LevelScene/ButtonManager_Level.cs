using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Level : MonoBehaviour
{
	public GameObject stage1Level;

	bool isLevelOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void PressBackBtn()
	{
		SceneManager.LoadScene("MainScene");
	}

	public void PressStageBtn(int stageNum)
	{
		switch(stageNum)
		{
			case 1:
				if (!isLevelOn)
				{
					stage1Level.SetActive(true);
					isLevelOn = true;
				}
				else
				{
					stage1Level.SetActive(false);
					isLevelOn = false;
				}
				break;
			default:
				break;
		}
	}

	public void PressLevelBtn(int levelNum)
	{
		switch (levelNum)
		{
			case 11:
				SceneManager.LoadScene("SampleScene");
				break;
			default:
				break;
		}
	}
}
