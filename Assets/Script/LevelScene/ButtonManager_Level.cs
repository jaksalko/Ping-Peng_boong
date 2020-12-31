using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Level : UIScript
{
	public GameObject islandList;
	public GameObject islandListContents;
	public List<string> isLandNameList;
	public GameObject[] levelList;
	public Sprite clearBtn;
	public Sprite nonclearBtn;
	public Sprite clearSelect;
	public Sprite nonclearSelect;
	private GameManager googleInstance;
	private int highLevel;
	private int selectLevel;
    
	public Text[] levelStep;

	XMLManager xMLManager;

    // Start is called before the first frame update
    void Start()
    {
		//xMLManager = XMLManager.ins;
		googleInstance = GameObject.FindGameObjectWithTag("GoogleInstance").GetComponent<GameManager>();
		highLevel = GameManager.instance.user.stage;
		Debug.Log("high level : " + highLevel);

		for (int l = 0; l < levelList.Length; l++)
        {
			levelStep[l] = levelList[l].transform.GetChild(2).GetComponent<Text>();
        }

		/* need to fix
        for(int j = 0; j < highLevel; j++)
        {
			int step = GoogleInstance.instance.stages[j].stage_step;
			if (step > 99) step = 99;
			levelStep[j].text = string.Format("{0:D2}", step);
        }
		*/

		// show the highest level
		string highest = Island_Name(highLevel);
		Debug.Log("highest island : " + highest);
		int index = isLandNameList.IndexOf(highest);
		if(index == -1)
		{
			// last level cleared
			Debug.Log(isLandNameList.Count);
			index = isLandNameList.Count - 1;
		}
		islandList.GetComponent<ScrollSnapRect>().Set(index);

		Color clearcolor;
		ColorUtility.TryParseHtmlString("#0056A2FF", out clearcolor);
		Color nonclearcolor;
		ColorUtility.TryParseHtmlString("#93CEE2FF", out nonclearcolor);

		/* for test
		for (int i = 0; i < levelList.Length; i++)
		{
			levelList[i].transform.GetChild(0).GetComponent<Image>().sprite = clearBtn;
			levelList[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = clearSelect;
			levelList[i].transform.GetChild(1).GetComponent<Text>().color = clearcolor;
			levelList[i].GetComponent<Toggle>().interactable = true;
		}
		*/

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
