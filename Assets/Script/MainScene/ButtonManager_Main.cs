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

	private void Start()
	{
		int highLevel = PlayerPrefs.GetInt("level", 0);
		if (Island_Name(highLevel) == "Tutorial")
		{
			islandList[0].SetActive(true);
		}
		else if (Island_Name(highLevel) == "Icecream")
		{
			islandList[1].SetActive(true);
		}
		else
		{
			// default
			islandList[0].SetActive(true);
		}
	}

	public void PressIslandBtn()
	{
		SceneManager.LoadScene("LevelScene");
	}

	public void PressPlayBtn()
	{
        GoogleInstance.instance.nowLevel = PlayerPrefs.GetInt("level", 0);
		Load_Island(GoogleInstance.instance.nowLevel);
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
        EditorPlayPopup.SetActive(true);

        //SceneManager.LoadScene("CustomMapPlayScene");
    }

    public void EditorBtn()
    {
        //맵크기설정
        EditorSettingPopup.SetActive(true);
        //SceneManager.LoadScene("MapEditor");
    }
}
