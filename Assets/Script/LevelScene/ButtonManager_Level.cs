using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Level : UIScript
{
	public GameObject islandList;
	public GameObject islandListContents;
	public GameObject[] levelList;
	public Sprite clearBtn;
	public Sprite nonclearBtn;
	public Sprite clearSelect;
	public Sprite nonclearSelect;
	private GoogleInstance googleInstance;
	private int highLevel;
	private int selectLevel;
	

    // Start is called before the first frame update
    void Start()
    {
		googleInstance = GameObject.FindGameObjectWithTag("GoogleInstance").GetComponent<GoogleInstance>();
		highLevel =  PlayerPrefs.GetInt("level", 0);

		if (Island_Name(highLevel) == "Tutorial")
		{
			islandList.GetComponent<ScrollRect>().content.localPosition = new Vector3(0, 0, 0);
			islandListContents.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
		}
		else if (Island_Name(highLevel) == "Icecream")
		{
			islandList.GetComponent<ScrollRect>().content.localPosition = new Vector3(1, 0, 0);
			islandListContents.transform.GetChild(1).GetComponent<Toggle>().isOn = true;
		}
		else
		{
			// default
			islandList.GetComponent<ScrollRect>().content.localPosition = new Vector3(0, 0, 0);
			islandListContents.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
		}

		int i;
		for(i = 0; i < highLevel; i++)
		{
			levelList[i].transform.GetChild(0).GetComponent<Image>().sprite = clearBtn;
			levelList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = clearSelect;
			levelList[i].GetComponent<Toggle>().interactable = true;
		}
		levelList[i].transform.GetChild(0).GetComponent<Image>().sprite = nonclearBtn;
		levelList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = nonclearSelect;
		levelList[i].GetComponent<Toggle>().interactable = true;
	}

	
	public void PressBackBtn()
	{
		SceneManager.LoadScene("MainScene");
	}

	public void PressLevelBtn(int levelNum)
	{
		selectLevel = levelNum;
	}

	public void PressStartGameBtn()
	{
		googleInstance.nowLevel = selectLevel;
		Load_Island(selectLevel);
	}
}
