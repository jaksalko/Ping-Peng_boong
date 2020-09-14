using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Level : UIScript
{
	public GameObject[] levelList;
	public Sprite clearBtn;
	private GoogleInstance googleInstance;
	private int highLevel;

	

    // Start is called before the first frame update
    void Start()
    {
		googleInstance = GameObject.FindGameObjectWithTag("GoogleInstance").GetComponent<GoogleInstance>();
		highLevel =  PlayerPrefs.GetInt("level", 0);

		int i;
		for(i = 0; i < highLevel; i++)
		{
			levelList[i].GetComponent<Image>().sprite = clearBtn;
			levelList[i].GetComponent<Button>().interactable = true;
		}
		levelList[i].GetComponent<Button>().interactable = true;
	}

	
	public void PressBackBtn()
	{
		SceneManager.LoadScene("MainScene");
	}

	public void PressLevelBtn(int levelNum)
	{
		googleInstance.nowLevel = levelNum;
		Load_Island(levelNum);

		
	}
}
