﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    XMLManager xMLManager;
    public CameraController cameraController;
    
    public UiController ui;
    public Player player1;
    public Player player2;
    public Map map;

    public Player nowPlayer;
    public bool infiniteMode;
	//public int infiniteLevel;

	public int maxLevel;

	private bool isRunning;
    public static bool Running
    {
        get => instance.isRunning;

    }

    private bool isPlaying;
    public static bool Playing
    {
        get => instance.isPlaying;
    }

    public float startTime, endTime;

	[SerializeField]
	GameObject backgroundSound;
	SoundManager soundManagerScript;

    JsonAdapter jsonAdapter = new JsonAdapter();
    UserData user = new UserData();

	private void Awake()
    {
        //PlayerPrefs.DeleteAll();


        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
        if (!infiniteMode)
        {
            map.GenerateMap(GoogleInstance.instance.nowLevel);
            player1.SetMap();
            player2.SetMap();
           
            player1.SetPosition(map.sampleMap.startPositionA + new Vector3(0, -0.5f, 0), map.sampleMap.startUpstairA); //position correction fix .. 5/13
            player2.SetPosition(map.sampleMap.startPositionB + new Vector3(0, -0.5f, 0), map.sampleMap.startUpstairB);
        }
            
        else
        {
            Debug.Log("infinite mode");
            StartCoroutine(InfiniteModeSetting());
        }

        



    }
    IEnumerator InfiniteModeSetting()
    {
        yield return StartCoroutine(map.InfiniteMAP(GoogleInstance.instance.infiniteLevel));


        //Debug.Log("set position" + map.sampleMap.startPositionA);
        player1.SetMap();
        player2.SetMap();
        player1.SetPosition(map.sampleMap.startPositionA + new Vector3(0, -0.5f, 0), map.sampleMap.startUpstairA); //position correction fix .. 5/13
        player2.SetPosition(map.sampleMap.startPositionB + new Vector3(0, -0.5f, 0), map.sampleMap.startUpstairB);

        cameraController.gameObject.SetActive(true);
        yield break;
    }
	private void Start()
	{
		backgroundSound = GameObject.FindWithTag("BackgroundSound");
        if(backgroundSound != null)
		    soundManagerScript = backgroundSound.GetComponent<SoundManager>();
	}

	public void SetPlaying(bool play)
    {
        isPlaying = play;
    }

    public void GameStart()//called by cameracontroller.cs after mapscanning...
    {
        player1.FindPlayer();
        player2.FindPlayer();



        nowPlayer = player1;
        nowPlayer.isActive = true;
        
        isRunning = true;
        isPlaying = true;
        startTime = Time.time;

        if(map.parfait)
        {
            ui.mission_parfait.SetActive(true);
        }
        else
        {
            ui.mission_default.SetActive(true);
        }
        ui.inGame.SetActive(true);
        
    }
    public void UserUpdate(int cash , int stage)
    {
        user.id = Cloud.PlayerDisplayName;
        user.cash = cash;
        user.stage = stage;
        var json = JsonUtility.ToJson(user);
        StartCoroutine(jsonAdapter.API_POST("account/update", json));
    }
    public void StageClear(int stage_num , int step)//max update
    {
        StageData stage = new StageData(Cloud.PlayerDisplayName , stage_num);
       
        stage.stage_step = step;

        var json = JsonUtility.ToJson(stage);
        StartCoroutine(jsonAdapter.API_POST("stage/insert", json));
    }

    public void StageClear(int step)//update step
    {
        StageData stage = new StageData(Cloud.PlayerDisplayName, GoogleInstance.instance.nowLevel);
       
        stage.stage_step = step;

        var json = JsonUtility.ToJson(stage);
        StartCoroutine(jsonAdapter.API_POST("stage/update", json));
    }
    public void GameEnd(bool isSuccess)
    {

        Debug.Log("Game End... result is " + isSuccess);
        isRunning = false;
        isPlaying = false;
        endTime = Time.time;
        
        ui.inGame.SetActive(false);
        ui.resultPopup.SetActive(true);
		if(backgroundSound != null)
		{
			soundManagerScript.GameResultPopup();
		}		

		if (isSuccess)
        {
            //FirstClear();
            int moveCount = player1.moveCount + player2.moveCount;
            ui.SetMoveCountText(moveCount);


            xMLManager = XMLManager.ins;

            int level = GoogleInstance.instance.user.stage;

            int nowLevel = GoogleInstance.instance.nowLevel;//input level (select stage or play btn)

            
            if(xMLManager.itemDB.stepList[nowLevel].step > moveCount)
            {
                xMLManager.itemDB.stepList[nowLevel].step = moveCount;
                xMLManager.SaveItems();
            }


            if (nowLevel == level)
            {
                StageClear(GoogleInstance.instance.nowLevel, moveCount);
                GoogleInstance.instance.nowLevel++;

                UserUpdate(30, GoogleInstance.instance.nowLevel);
                



                xMLManager.SaveItems();
            }
            else
            {
                StageClear(moveCount);
                GoogleInstance.instance.nowLevel++;
            }
           
            if(nowLevel == maxLevel)
			{
				ui.nextLevelBtn.interactable = false;
			}
            
          
        }
        else
        {
            //not use this paragraph...
        }

    }

   



   

    public void FirstClear()
    {
        Achievements.FirstPlay.Unlock();
        Debug.Log("first play");
        GoogleInstance.instance.SetText("first Play Achievement");
    }

}
