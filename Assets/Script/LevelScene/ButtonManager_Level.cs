using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Level : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void PressBackBtn()
	{
		SceneManager.LoadScene("MainScene");
	}

	public void PressLevelBtn(int levelNum)
	{
		switch (levelNum)
		{
			// Tutorial Island
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
				googleInstance.nowLevel = levelNum;
				SceneManager.LoadScene("GameScene");
				break;
			// Icecream Island
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
				googleInstance.nowLevel = levelNum;
				SceneManager.LoadScene("GameScene");
				break;
			default:
				break;
		}
	}
}
