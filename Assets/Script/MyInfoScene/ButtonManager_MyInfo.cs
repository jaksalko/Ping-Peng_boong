using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonManager_MyInfo : MonoBehaviour
{
	public GameObject skinInfo;
	public GameObject skinContents;
	public GameObject skinPreview;

	public GameObject skinBtnObj;

	private int selectSkinType;
	private int selectSkinNum;

	public void Start()
	{
		GetContents("playerskin");
		selectSkinType = 1;
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
		ResetContents(skinContents);
		GetContents("playerskin");
		selectSkinType = 1;
	}

	public void PressPlayer2Btn()
	{
		ResetContents(skinContents);
		GetContents("playerskin");
		selectSkinType = 2;
	}

	public void PressBlockBtn()
	{
		ResetContents(skinContents);
		GetContents("blockskin");
		selectSkinType = 3;
	}

	public void PressSkinBtn()
	{
		int index = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
		selectSkinNum = index;

		switch(selectSkinType)
		{
			case 1:
			case 2:
				List<Dictionary<string, object>> playerskin = CSVReader.Read("playerskin");
				skinInfo.transform.GetChild(0).GetComponent<Text>().text = playerskin[index]["name"].ToString();
				skinInfo.transform.GetChild(1).GetComponent<Text>().text = playerskin[index]["information"].ToString();
				string location = playerskin[index]["location"].ToString();
				Debug.Log(location);
				Material skinmat = Resources.Load<Material>(location);
				skinPreview.GetComponent<SkinnedMeshRenderer>().material = skinmat;
				break;
			case 3:
				List<Dictionary<string, object>> blockskin = CSVReader.Read("blockskin");
				skinInfo.transform.GetChild(0).GetComponent<Text>().text = blockskin[index]["name"].ToString();
				skinInfo.transform.GetChild(1).GetComponent<Text>().text = blockskin[index]["information"].ToString();
				skinInfo.transform.GetChild(2).GetComponent<Text>().text = blockskin[index]["cost"].ToString();
				break;
			default:
				Debug.Log("Non-set Skin Type Error");
				break;
		}
	}

	public void PressApplyBtn()
	{
		switch (selectSkinType)
		{
			case 1:

				break;
			case 2:

				break;
			case 3:

				break;
			default:
				Debug.Log("Non-set Skin Type Error");
				break;
		}

		/*
		List<Dictionary<string, object>> skin = CSVReader.Read("playerskin");

		for (var i = 0; i < skin.Count; i++)
		{
			Debug.Log("index " + (i).ToString() + " : " + skin[i]["id"] + " " + skin[i]["name"] + " " + skin[i]["grade"]);
		}
		*/
	}

	private void GetContents(string filename)
	{
		List<Dictionary<string, object>> skin = CSVReader.Read(filename);

		for (var i = 0; i < skin.Count; i++)
		{
			GameObject skinBtn = Instantiate(skinBtnObj, new Vector3(0, 0, 0), Quaternion.identity);
			skinBtn.transform.SetParent(skinContents.transform);
			skinBtn.transform.GetComponentInChildren<Text>().text = skin[i]["name"].ToString();
			skinBtn.GetComponent<Button>().onClick.AddListener(() => PressSkinBtn());
		}
	}

	private void ResetContents(GameObject obj)
	{
		Transform[] childList = obj.GetComponentsInChildren<Transform>(true);
		if (childList != null)
		{
			for (int i = 1; i < childList.Length; i++)
			{
				if (childList[i] != transform)
					Destroy(childList[i].gameObject);
			}
		}
	}
}
