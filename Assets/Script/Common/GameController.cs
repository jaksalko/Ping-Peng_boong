﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    XMLManager xMLManager;

    [Header("Controller Field")]
    public CameraController cameraController;
    public UiController ui;

    [Header("Game Component Field")]
    public MapLoader mapLoader;
    public Player player1;
    public Player player2;
    public Player nowPlayer;

    Map map;
    public Map GetMap() { return map; }
    int snow_total, snow_remain;
    int moveCount;

    [SerializeField]
    int parfaitOrder;
    public static int ParfaitOrder
    {
        get => instance.parfaitOrder;
        set => instance.parfaitOrder = value;
    }

    public bool customMode;
    public bool editorMode;
    public int maxLevel;



    private bool isPlaying;
    public static bool Playing
    {
        get => instance.isPlaying;
    }

    public float startTime, endTime;


    [Header("Sound Field")]
    [SerializeField]
    GameObject backgroundSound;
    SoundManager soundManagerScript;
    GameManager gameManager = GameManager.instance;


    JsonAdapter jsonAdapter = new JsonAdapter();
    UserData user;

    [SerializeField]
    Vector2 down;
    [SerializeField]
    Vector2 up;
    [SerializeField]
    bool click;

    InputHandler handler;
    public MoveCommand moveCommand;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();


        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        handler = new InputHandler();
        SwipeStream();
        StartCoroutine(GameSetting());


        



    }
    
    void SwipeStream()
    {

#if UNITY_EDITOR
        var mouseDownStream = this.UpdateAsObservable()
                .Where(_ => !EventSystem.current.IsPointerOverGameObject()
                    && !click
                    && Input.GetMouseButtonDown(0))

                .Select(_ => Input.mousePosition)
                .Subscribe(_ => { down = _; click = true; });

        var mouseUpStream = this.UpdateAsObservable()
            .Where(_ => click && Input.GetMouseButtonUp(0))

            .Select(_ => Input.mousePosition)
            .Subscribe(_ => { up = _; MakeMoveCommand(); click = false; });

#elif UNITY_ANDROID || UNITY_IOS
        var touchDownStream = this.UpdateAsObservable()
          
            .Where(_ => !click)
            .Where(_ => Input.touchCount > 0)
            .Where(_ => !EventSystem.current.IsPointerOverGameObject(0))
            .Where(_ => Input.GetTouch(0).phase == TouchPhase.Began)
            .Select(_ => Input.GetTouch(0))
            .Subscribe(_ => { down = _.position; click = true; } );

        var touchUpStream = this.UpdateAsObservable()
         
            .Where(_ => click)
            .Where(_ => Input.touchCount > 0)
            .Where(_ => Input.GetTouch(0).phase == TouchPhase.Ended)
            .Select(_ => Input.mousePosition)
            .Subscribe(_ => { up = _; MakeMoveCommand(); click = false; });
#endif

    }
    void MakeMoveCommand()
    {
        int dir = NormalizeSwipe();

        //make command;
        if (!nowPlayer.Moving() && isPlaying && dir != -1)
        {
            moveCommand = new MoveCommand(nowPlayer, map, dir);

            handler.ExecuteCommand(moveCommand);
            moveCount++;
        }

    }


    public void MakeUndoCommand()   // press undo btn -> make undo command
    {
        if (!nowPlayer.Moving() && isPlaying)
        {
            Debug.Log("undo command made");
            handler.UndoCommand();
            moveCount--;
        }
    }



    int NormalizeSwipe()
    {
        if (Vector2.Distance(up, down) <= 30)//민감도
        {
            return -1;
        }
        Vector2 normalized = (up - down).normalized;


        if (normalized.x < -0.5)
        {
            return 3;
            //isMove = PlayerControl(3); //left
        }
        else if (normalized.x > 0.5)
        {
            return 1;
            //isMove = PlayerControl(1); //right
        }
        else
        {
            if (normalized.y > 0)
            {
                return 0;
                //isMove = PlayerControl(0); //up

            }
            else
            {
                return 2;
                //isMove = PlayerControl(2); // down
            }

        }


    }
    IEnumerator GameSetting()
    {

        BackgroundSoundSetting();
       
        // map 생성
        if (customMode)
        {
            map = mapLoader.CustomPlayMap();
        }
        else if (editorMode)
        {
            map = mapLoader.EditorMap();
        }
        else
            map = mapLoader.GenerateMap(gameManager.nowLevel);//map 생성



        //데이터 초기화 (Remain / Total / MoveCount)
        snow_remain = snow_total = RemainCheck();
        moveCount = 0;
        // player.FindPlayer 가 실행되면 자동으로 2개가 사라짐 이 전까지는 remain == total
        // 실행위치는 GameStart CameraController에 의해서 실행됨.


        //character 위치에 맵 데이터가 노멀블럭으로 되어있는데 캐릭터 데이터로 전환 ? **데이터가 캐릭터면 바꿀필요 없음
        //체크 true로 변경
        //snow_remain 변경?
        int AposX = (int)map.startPositionA.x;
        int AposZ = (int)map.startPositionA.z;

        int BposX = (int)map.startPositionB.x;
        int BposZ = (int)map.startPositionB.z;

        map.UpdateCheckArray(width: AposX, height: AposZ, true);
        map.UpdateCheckArray(width: BposX, height: BposZ, true);

        snow_remain = RemainCheck();

        //캐릭터 배치 (active)
        player1.SetPosition(
           startpos: map.startPositionA);

        player2.SetPosition(
            startpos: map.startPositionB);



        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);


        //카메라 활성화
        cameraController.gameObject.SetActive(true);

        yield break;
    }


    public int RemainCheck()//남은 눈 체크 --> 이동
    {
        int remain = 0;

        for (int i = 0; i < map.mapsizeH; i++)
        {
            for (int j = 0; j < map.mapsizeW; j++)
            {
                if (!map.check[i, j])
                {
                    remain++;
                }
            }
        }

        if (remain == 0) GameEnd();
        else
        {
            snow_remain = remain;
            ui.SetRemainText(snow_remain, snow_total);
        }

        return remain;
    }





    public void SetPlaying(bool play)
    {
        isPlaying = play;
    }

    public void GameStart()//called by cameracontroller.cs after mapscanning...
    {

        //if(!simulating)
        ui.SetRemainText(remain: snow_remain, total: snow_total);

        nowPlayer = player1;
        nowPlayer.isActive = true;


        isPlaying = true;
        startTime = Time.time;

        if (map.parfait)
        {
            ui.mission_parfait.SetActive(true);
        }
        else
        {
            ui.mission_default.SetActive(true);
        }
        ui.inGame.SetActive(true);

    }

    #region JSON
    public void UserUpdate(int cash, int stage)//cash : +cash , stage : nowStage
    {
        UserData user = new UserData(gameManager.user.id, cash, change_heart: 0, stage);
        var json = JsonUtility.ToJson(user);
        StartCoroutine(jsonAdapter.API_POST("account/update", json, callback => {

            gameManager.user.cash += cash;
            gameManager.user.stage = stage;

        }));

    }

    public void StageClear(int stage_num, int step)//max update
    {
        StageData stage = new StageData(gameManager.user.id, stage_num, step);

        var json = JsonUtility.ToJson(stage);
        StartCoroutine(jsonAdapter.API_POST("stage/insert", json, callback => { }));
    }

    public void StageClear(int step)//update step
    {
        StageData stage = new StageData(gameManager.user.id, gameManager.nowLevel, step);

        var json = JsonUtility.ToJson(stage);
        StartCoroutine(jsonAdapter.API_POST("stage/update", json, callback => { }));
    }
    #endregion


    public void GameEnd()
    {

        isPlaying = false;
        endTime = Time.time;
        Debug.Log("Game End... PlayTime : " + (endTime - startTime));

        ui.GameEnd(moveCount, customMode, editorMode);

        if (customMode)
        {
            //클리어 에디터 모드 맵 데이터 추가하기 (클리어 한 맵 데이터를 처리하기 위해서)
            CustomMapItem nowPlayData = GameManager.instance.playCustomData;

            if (!nowPlayData.isPlayed)
            {
                CustomStagePlayerData newPlayerData = new CustomStagePlayerData(gameManager.user.id, nowPlayData.itemdata.title, false);
                gameManager.customStagePlayerDatas.Add(newPlayerData);
                var json = JsonUtility.ToJson(newPlayerData);

                StartCoroutine(jsonAdapter.API_POST("editorPlay/add", json, callback => {



                }));//add tuple
            }

            if (moveCount < nowPlayData.itemdata.moveCount)
            {
                nowPlayData.itemdata.moveCount = moveCount;
                var json = JsonUtility.ToJson(nowPlayData.itemdata);

                StartCoroutine(jsonAdapter.API_POST("map/clear", json, callback => { }));//change moveCount
            }
            //if !is
            //Add Tuple Customclear DB
            //


            //clear custom map user update(cash+? map clear 여부)

        }
        else if (editorMode)//mapLoader.editorMap 생성하기
        {
            //??
        }
        else//stage mode
        {
            int level = gameManager.user.stage;
            int nowLevel = gameManager.nowLevel;//input level (select stage or play btn)

            if (nowLevel == level)//지금 스테이지 레벨 == 유저의 도전해야할 레벨
            {
                StageClear(gameManager.nowLevel, moveCount);//stage clear data insert (stage , step)
                gameManager.nowLevel++;
                UserUpdate(30, gameManager.nowLevel);//cash +30 & clear stage +1

                

            }
            else
            {
                StageClear(moveCount);//stage clear data Update (step)
                gameManager.nowLevel++;
            }

            if (nowLevel == maxLevel)//???
            {
                ui.nextLevelBtn.interactable = false;
            }
        }


        if (backgroundSound != null)
        {
            soundManagerScript.GameResultPopup();
        }






    }

    void BackgroundSoundSetting()
    {
        //Background sound
        backgroundSound = GameObject.FindWithTag("BackgroundSound");
        if (backgroundSound != null)
            soundManagerScript = backgroundSound.GetComponent<SoundManager>();
    }





 
}
