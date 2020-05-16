using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSoungManager : MonoBehaviour
{
	public AudioSource sfxSound;

	GameObject baseCanvas;
	GameObject setting;
	Slider sfxVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
		baseCanvas = GameObject.FindWithTag("BaseCanvas");
		setting = baseCanvas.transform.GetChild(1).gameObject;
		sfxVolumeSlider = setting.gameObject.transform.GetChild(5).GetComponent<Slider>();
	}

    // Update is called once per frame
    void Update()
    {
		sfxSound.volume = sfxVolumeSlider.value;

	}
}
