using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Level : UIScript
{
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

		SpriteState clear_st = new SpriteState();
		clear_st.selectedSprite = clearSelect;
		SpriteState nonclear_st = new SpriteState();
		nonclear_st.selectedSprite = nonclearSelect;

		int i;
		for(i = 0; i < highLevel; i++)
		{
			levelList[i].GetComponent<Image>().sprite = clearBtn;
			levelList[i].GetComponent<Toggle>().interactable = true;
			levelList[i].GetComponent<Toggle>().spriteState = clear_st;
		}
		levelList[i].GetComponent<Image>().sprite = nonclearBtn;
		levelList[i].GetComponent<Toggle>().interactable = true;
		levelList[i].GetComponent<Toggle>().spriteState = nonclear_st;
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
