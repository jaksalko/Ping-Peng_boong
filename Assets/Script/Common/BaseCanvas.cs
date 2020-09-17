using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


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


	void Awake()
    {
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);


		StartCoroutine(UpdateInfo());

	}

    IEnumerator UpdateInfo()
    {
        while(true)
        {
			UnityWebRequest www = UnityWebRequest.Get("http://ec2-15-164-219-253.ap-northeast-2.compute.amazonaws.com:3000/account/info?id=" + GoogleInstance.instance.id);
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				// Show results as text
				Debug.Log(www.downloadHandler.text);

				// Or retrieve results as binary data
				byte[] results = www.downloadHandler.data;

				//Get data and convert to samplemap list..


				string fixdata = JsonHelper.fixJson(www.downloadHandler.text);
				Debug.Log(fixdata);

				UserData[] datas = JsonHelper.FromJson<UserData>(fixdata);
				Debug.Log(datas.Length);

				UserData selectedData = datas[0];
				cash.text = selectedData.cash.ToString();

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
		if (SceneManager.GetActiveScene().name == "Beach_Island" || SceneManager.GetActiveScene().name == "Tutorial_Island")
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

}
