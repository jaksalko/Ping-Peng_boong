using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	void Awake()
    {
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

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
