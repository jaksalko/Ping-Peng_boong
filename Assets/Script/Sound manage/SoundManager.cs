using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
	public AudioClip mainBGSound;
	public AudioClip tutorialBGSound;
	public AudioClip parfaitBGSound;
	public AudioClip popupSound;

	public Slider bgmVolumnSlider;

	AudioSource audioSource;
	bool gameSceneOn = false;

	// Start is called before the first frame update
	void Start()
    {
		audioSource = gameObject.GetComponent<AudioSource>();

		audioSource.clip = mainBGSound;
		audioSource.Play();
	}

    // Update is called once per frame
    void Update()
    {
		audioSource.volume = bgmVolumnSlider.value;
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Tutorial_Island")
		{
			gameSceneOn = true;

			audioSource.loop = true;
			audioSource.clip = tutorialBGSound;

			audioSource.Play();			
		}

		if (scene.name == "Beach_Island")
		{
			gameSceneOn = true;

			audioSource.loop = true;
			audioSource.clip = parfaitBGSound;

			audioSource.Play();
		}

		if (scene.name == "MainScene" && gameSceneOn)
		{
			gameSceneOn = false;

			audioSource.loop = true;
			audioSource.clip = mainBGSound;

			audioSource.Play();			
		}
	}

	public void GameResultPopup()
	{
		audioSource.Stop();

		audioSource.loop = false;
		audioSource.clip = popupSound;

		audioSource.Play();
	}
}
