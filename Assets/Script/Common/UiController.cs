using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using CloudOnce;




public class UiController : UIScript
{
    public GameObject inGame;
    public GameObject resultPopup;
	public Button nextLevelBtn;
    public GameObject pausePopup;

    public Text devtext;
    public Text remainText;
    public Text moveText;
    bool mini = false;

    int order = 0;
    public GameObject[] parfaitOrder;
    public GameObject[] parfaitOrder_done;

    public GameObject mission_default;
    public GameObject mission_parfait;

    private void Awake()
    {
        devtext.text = "platform : " + Application.platform + "\n" + "level : " + PlayerPrefs.GetInt("level", 0);
       
    }

    public void ParfaitDone()
    {
        parfaitOrder[order].SetActive(false);
        parfaitOrder_done[order].SetActive(true);
        order++;
    }

    public void SetMoveCountText(int count)
    {
        moveText.text = count.ToString();
    }
    public void SetRemainText(int remain , int total)
    {
        remainText.text = "<size=60>" + remain + "</size>/<size=40>" + total + "</size>";
    }

    public void MiniMapButton()
    {
        mini = GameController.instance.cameraController.MiniMapView(mini);
        GameController.instance.SetPlaying(!mini);
    }

    public void ChangeCharacter()
    {
        Player now = GameController.instance.nowPlayer;

		if (!now.Moving())
		{
//			Debug.Log("change Character");
			GameController.instance.nowPlayer.isActive = false;

			if (now == GameController.instance.player1)
			{
				GameController.instance.nowPlayer = GameController.instance.player2;
			}
			else
			{
				GameController.instance.nowPlayer = GameController.instance.player1;
			}
			GameController.instance.nowPlayer.isActive = true;

			if (!GameController.instance.nowPlayer.GetComponent<AudioSource>().isPlaying)
			{
				GameController.instance.nowPlayer.GetComponent<AudioSource>().loop = false;
				GameController.instance.nowPlayer.GetComponent<AudioSource>().clip = GameController.instance.nowPlayer.GetComponent<Player>().departureSound;

				GameController.instance.nowPlayer.GetComponent<AudioSource>().Play();
			}

			//			Debug.Log("player 1 : " + GameController.instance.player1.isActive);
			//			Debug.Log("player 2 : " + GameController.instance.player2.isActive);
		}
		else
		{
			Debug.Log("Can't change!");
		}
        
    }
    public void MasterFocus(Player master)
    {
        GameController.instance.nowPlayer = master;
        GameController.instance.nowPlayer.isActive = true;
        Debug.Log("master : " + master.name);
    }
    public void NextLevel()
    {
        //GameController googleinstance level++....
        if(GameController.instance.infiniteMode)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            Load_Island(GoogleInstance.instance.nowLevel);
        
    }

    public void Pause()
    {
        GameController.instance.SetPlaying(false);
        pausePopup.SetActive(true);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoLobby()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Resume()
    {
        GameController.instance.SetPlaying(true);
        pausePopup.SetActive(false);
    }

    public void CloudInitializeCompleted()
    {
        Cloud.OnInitializeComplete -= CloudInitializeCompleted;
        Debug.Log("initialize completed");
    }

    public void FirstClear()
    {
        Achievements.FirstPlay.Unlock();
        Debug.Log("first play");
    }

}
