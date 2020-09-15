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
			Debug.Log("tutorial");
			islandList.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
			islandListContents.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
		}
		else if (Island_Name(highLevel) == "Icecream")
		{
			Debug.Log("icecream");
			islandList.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0.5f ;
			islandListContents.transform.GetChild(1).GetComponent<Toggle>().isOn = true;
		}
		else
		{
			Debug.Log("default");
			islandList.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
			islandListContents.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
		}

		Color clearcolor;
		ColorUtility.TryParseHtmlString("#0056A2FF", out clearcolor);
		Color nonclearcolor;
		ColorUtility.TryParseHtmlString("#93CEE2FF", out nonclearcolor);

		int i;
		for(i = 0; i < highLevel; i++)
		{
			levelList[i].transform.GetChild(0).GetComponent<Image>().sprite = clearBtn;
			levelList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = clearSelect;
			levelList[i].transform.GetChild(1).GetComponent<Text>().color = clearcolor;
			levelList[i].GetComponent<Toggle>().interactable = true;
		}
		levelList[i].transform.GetChild(0).GetComponent<Image>().sprite = nonclearBtn;
		levelList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = nonclearSelect;
		levelList[i].transform.GetChild(1).GetComponent<Text>().color = nonclearcolor;
		levelList[i].GetComponent<Toggle>().interactable = true;
		levelList[i].GetComponent<Toggle>().isOn = true;
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
