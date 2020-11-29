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
    
	public Text[] levelStep;

	XMLManager xMLManager;

    // Start is called before the first frame update
    void Start()
    {
		//xMLManager = XMLManager.ins;
		googleInstance = GameObject.FindGameObjectWithTag("GoogleInstance").GetComponent<GoogleInstance>();
		highLevel = GoogleInstance.instance.user.stage;

		for (int l = 0; l < levelList.Length; l++)
        {
			levelStep[l] = levelList[l].transform.GetChild(2).GetComponent<Text>();
        }

        for(int j = 0; j < highLevel; j++)
        {
			int step = GoogleInstance.instance.stages[j].stage_step;
			if (step > 99) step = 99;
			levelStep[j].text = string.Format("{0:D2}", step);
        }



		if (Island_Name(highLevel) == "Tutorial")
		{
			Debug.Log("tutorial");
			islandList.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
		}
		else if (Island_Name(highLevel) == "Icecream")
		{
			Debug.Log("icecream");
			islandList.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0.5f ;
		}
		else if (Island_Name(highLevel) == "Beach")
		{
			Debug.Log("beach");
			islandList.GetComponent<ScrollRect>().horizontalNormalizedPosition = 1f;
		}
		else if (Island_Name(highLevel) == "Cracker")
		{
			Debug.Log("Cracker");
			islandList.GetComponent<ScrollRect>().horizontalNormalizedPosition = 1.5f;
		}
		else if (Island_Name(highLevel) == "Cottoncandy")
		{
			Debug.Log("cottoncandy");
			islandList.GetComponent<ScrollRect>().horizontalNormalizedPosition = 2f;
		}
		else
		{
			Debug.Log("default : last island");
			islandList.GetComponent<ScrollRect>().horizontalNormalizedPosition = 2f;
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

		if (i < levelList.Length)
		{
			levelList[i].transform.GetChild(0).GetComponent<Image>().sprite = nonclearBtn;
			levelList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = nonclearSelect;
			levelList[i].transform.GetChild(1).GetComponent<Text>().color = nonclearcolor;
			levelList[i].GetComponent<Toggle>().interactable = true;
			levelList[i].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			levelList[i - 1].GetComponent<Toggle>().isOn = true;
		}

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
