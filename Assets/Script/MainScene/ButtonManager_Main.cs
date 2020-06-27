using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Main : MonoBehaviour
{

    public GameObject EditorPlayPopup;
    public GameObject EditorSettingPopup;

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
		SceneManager.LoadScene("LevelScene");
	}

	public void PressPlayBtn()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void PressEglooBtn()
	{
		SceneManager.LoadScene("MyInfoScene");
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
