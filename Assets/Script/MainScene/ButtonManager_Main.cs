using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Main : UIScript
{

    public GameObject EditorPlayPopup;
    public GameObject EditorSettingPopup;

	public GameObject[] islandList;
	public int maxLevel;
	XMLManager xMLManager;

	private void Start()
	{
        /*if(xMLManager == null)
        {
			xMLManager = XMLManager.ins;
        }*/

		int highLevel = GameManager.instance.user.stage;

		if (Island_Name(highLevel) == "Tutorial")
		{
			islandList[0].SetActive(true);
		}
		else if (Island_Name(highLevel) == "Icecream")
		{
			islandList[1].SetActive(true);
		}
		else if (Island_Name(highLevel) == "Beach")
		{
			islandList[2].SetActive(true);
		}
		else if (Island_Name(highLevel) == "Cracker")
		{
			islandList[3].SetActive(true);
		}
		else if (Island_Name(highLevel) == "Cottoncandy")
		{
			islandList[4].SetActive(true);
		}
		else
		{
			// default : last island
			int index = islandList.Length - 1;
			islandList[index].SetActive(true);
		}
	}

	public void PressIslandBtn()
	{
		SceneManager.LoadScene("LevelScene");
	}

	public void PressPlayBtn()
	{
		/*if (xMLManager == null)
		{
			xMLManager = XMLManager.ins;
		}*/

		int high_level = GameManager.instance.user.stage;

		if(maxLevel < high_level)
		{
			GameManager.instance.nowLevel = high_level - 1;

			Load_Island(GameManager.instance.nowLevel);
		}
		else
		{
			GameManager.instance.nowLevel = high_level;

			Load_Island(GameManager.instance.nowLevel);
		}
	}

	public void PressEglooBtn()
	{
		SceneManager.LoadScene("MyInfoScene");
	}

	public void PressStoreBtn()
	{
		SceneManager.LoadScene("StoreScene");
	}

    public void EditorPlayBtn()
    {
		//추후에 난이도 설정
		//EditorPlayPopup.SetActive(true);
		StartCoroutine(GameManager.instance.LoadCustomMapList(success =>
		{
			if (success)
			{
				EditorPlayPopup.SetActive(true);
				
			}
			else
			{
				Debug.Log("cant load custom map list...");
			}
		}));

		
		//SceneManager.LoadScene("CustomMapPlayScene");
	}

    public void EditorBtn()
    {
        //맵크기설정
        EditorSettingPopup.SetActive(true);
        //SceneManager.LoadScene("MapEditor");
    }
}
