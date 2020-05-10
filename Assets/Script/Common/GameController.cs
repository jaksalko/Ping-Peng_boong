using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public CameraController cameraController;
    
    public UiController ui;
    public Player player1;
    public Player player2;
    public Map map;

    public Player nowPlayer;

   

    private bool isRunning;
    public static bool Running
    {
        get => instance.isRunning;

    }

    public float startTime, endTime;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
        map.GenerateMap(PlayerPrefs.GetInt("level", 0));
        player1.SetPosition(map.sampleMap.startPositionA, map.sampleMap.startUpstairA);
        player2.SetPosition(map.sampleMap.startPositionB, map.sampleMap.startUpstairB);

    }

    

    public void GameStart()//called by cameracontroller.cs after mapscanning...
    {
        
        nowPlayer = player1;
        nowPlayer.isActive = true;
        isRunning = true;
        startTime = Time.time;
        ui.inGame.SetActive(true);
        
    }

    public void GameEnd(bool isSuccess)
    {
        Debug.Log("Game End... result is " + isSuccess);
        isRunning = false;
        endTime = Time.time;
        
        ui.inGame.SetActive(false);
        ui.resultPopup.SetActive(true);
        if(isSuccess)
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 0) + 1);
        }
        else
        {
            //not use this paragraph...
        }

    }

    public void GameStop(bool stop)
    {
        isRunning = !stop;
        ui.inGame.SetActive(!stop);
        ui.popup.SetActive(stop);
    }

}
