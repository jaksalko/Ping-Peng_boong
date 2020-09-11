using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseCanvas : MonoBehaviour
{
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

	private void Update()
	{
		if (SceneManager.GetActiveScene().name == "GameScene")
		{
			changePlayerBtn = GameObject.FindGameObjectWithTag("ChangePlayer");
		}
	}

	public void InitLocBtnClick()
	{
		PlayerPrefs.SetFloat("ChangePlayerBtnX", btnlocX);
		PlayerPrefs.SetFloat("ChangePlayerBtnY", btnlocY);

		if(changePlayerBtn != null)
		{
			changePlayerBtn.GetComponent<RectTransform>().position = new Vector2(btnlocX, btnlocY);
		}
	}

}
