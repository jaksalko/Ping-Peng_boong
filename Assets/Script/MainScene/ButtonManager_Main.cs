using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Main : UIScript
{

    public GameObject EditorPlayPopup;
    public GameObject EditorSettingPopup;

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
