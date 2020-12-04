using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class BaseCanvas : MonoBehaviour
{
	public Slider bgmSlider;
	public Slider sfxSlider;
	public GameObject userState;
	public static BaseCanvas Instance;
	public float btnlocX;
	public float btnlocY;

	[SerializeField]
	private GameObject changePlayerBtn;

	[Header("INFO")]
	public Text cash;
	public Text star;

	public GameObject friendManage;
	DateTime d;
	void Awake()
    {
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		d = DateTime.Now;
		Debug.Log(d);
		Instance = this;
		DontDestroyOnLoad(gameObject);


		StartCoroutine(UpdateInfo());

	}

    IEnumerator UpdateInfo()
    {
        while(true)
        {
			UnityWebRequest www = UnityWebRequest.Get(PrivateData.ec2 + "account/info?id=" + GoogleInstance.instance.id);
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				// Show results as text
//				Debug.Log(www.downloadHandler.text);



				string fixdata = JsonHelper.fixJson(www.downloadHandler.text);
				//Debug.Log(fixdata);

				UserData[] datas = JsonHelper.FromJson<UserData>(fixdata);
				//Debug.Log(datas.Length);

				UserData selectedData = datas[0];

				GoogleInstance.instance.user = selectedData;

				cash.text = selectedData.cash.ToString();
				star.text = selectedData.candy + "/5";
			}

            ///stage
			www = UnityWebRequest.Get(PrivateData.ec2 + "stage/info?id=" + GoogleInstance.instance.id);
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				// Show results as text
//				Debug.Log(www.downloadHandler.text);


				string fixdata = JsonHelper.fixJson(www.downloadHandler.text);
//				Debug.Log(fixdata);

				StageData[] datas = JsonHelper.FromJson<StageData>(fixdata);
//				Debug.Log("stage clear : " +datas.Length);

				for(int i = 0; i < datas.Length; i++)
                {
					GoogleInstance.instance.stages[datas[i].stage_num].stage_step = datas[i].stage_step;
                }

			}

			yield return new WaitForSeconds(1f);
		}
		
	}

	private void Start()
	{
		bgmSlider.value = PlayerPrefs.GetFloat("bgmVolumn", 1);
		sfxSlider.value = PlayerPrefs.GetFloat("sfxVolumn", 1);
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().buildIndex >= 4)
		{
			changePlayerBtn = GameObject.FindGameObjectWithTag("ChangePlayer");
			userState.SetActive(false);
		}
		else
		{
			userState.SetActive(true);
		}
	}

	public void InitLocBtnClick()
	{
		PlayerPrefs.SetFloat("ChangePlayerBtnX", btnlocX);
		PlayerPrefs.SetFloat("ChangePlayerBtnY", btnlocY);

		if(changePlayerBtn != null)
		{
			RectTransform rect = changePlayerBtn.GetComponent<RectTransform>();
			rect.position = new Vector2(btnlocX, btnlocY);
		}
	}

	public void SaveBGMVolumn()
	{
		PlayerPrefs.SetFloat("bgmVolumn", bgmSlider.value);
	}

	public void SaveSFXVolumn()
	{
		PlayerPrefs.SetFloat("sfxVolumn", sfxSlider.value);
	}

    public void FriendManageOpen()
    {
		friendManage.SetActive(true);
    }
}
