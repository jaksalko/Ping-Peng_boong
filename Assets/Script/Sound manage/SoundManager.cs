using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
	public AudioClip mainBGSound;
	public AudioClip gameBGSound;

	public int[] mainSoundScene;
	public int[] gameSoundScene;

	public Slider bgmVolumnSlider;

	AudioSource audioSource;
	int swtichScene = -1;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = gameObject.GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
		int index = SceneManager.GetActiveScene().buildIndex;
		if (mainSoundScene.Contains(index) && swtichScene == -1)
		{
			audioSource.clip = mainBGSound;
			audioSource.Play();
			swtichScene = 0;
		}
		else if(gameSoundScene.Contains(index) && swtichScene == 0)
		{
			audioSource.clip = gameBGSound;
			audioSource.Play();
			swtichScene = -1;
		}

		audioSource.volume = bgmVolumnSlider.value;
	}
}
