using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_MyInfo : PlayerSkin
{
	public GameObject player1Btn;
	public GameObject player2Btn;
	public GameObject blockSkinBtn;
	public GameObject skinInfoText;
	public GameObject skinContents;
	public List<PlayerSkin> skinList;

	private int selectSkinNum;
	private int usingSkinNum;

	public void Start()
	{
		skinList = base.PlayerSkinList();
	}

	public void Update()
	{
		
	}

	public void PressBackBtn()
	{
		SceneManager.LoadScene("MainScene");
	}

	public void PressPlayer1Btn()
	{

	}

	public void PressSkinBtn(int skinNum)
	{
		if (!skinList[skinNum - 1].isUsing)
		{
			switch (skinNum)
			{
				case 1:
				case 2:
				case 3:
					selectSkinNum = skinNum;
					skinInfoText.GetComponent<Text>().text = skinList[skinNum - 1].skinInfo;
					player1Btn.GetComponent<Image>().sprite = skinList[skinNum - 1].skinImg;
					break;
				default:
					skinInfoText.GetComponent<Text>().text = "";
					player1Btn.transform.GetChild(0).GetComponent<Text>().text = "Player1";
					break;
			}
		}
	}

	public void PressApplyBtn()
	{
		if(!skinList[selectSkinNum - 1].isUsing)
		{
			Debug.Log("Apply skin " + selectSkinNum);
			player1Btn.transform.GetChild(0).GetComponent<Text>().text = skinList[selectSkinNum - 1].skinName;
			skinList[selectSkinNum - 1].isUsing = true;
			skinContents.transform.GetChild(selectSkinNum - 1).GetComponent<Button>().interactable = false;
			if (usingSkinNum != 0)
			{
				skinList[usingSkinNum - 1].isUsing = false;
				skinContents.transform.GetChild(usingSkinNum - 1).GetComponent<Button>().interactable = true;
			}

			usingSkinNum = selectSkinNum;
		}
	}
}
