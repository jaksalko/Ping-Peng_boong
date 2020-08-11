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
    public bool infiniteMode;
    //public int infiniteLevel;

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

	private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
        if (!infiniteMode)
        {
            map.GenerateMap(PlayerPrefs.GetInt("level", 0));
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
        ui.inGame.SetActive(true);
        
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
            int level = PlayerPrefs.GetInt("level", 0);
            int nextlevel;
            //Showcase scope Level admin
            if (level < 5)
                PlayerPrefs.SetInt("level", level + 1);
            else if (level == 5)//drawing easy
            {
                nextlevel = Random.Range(6, 11);
                PlayerPrefs.SetInt("except", 11 - nextlevel);
                PlayerPrefs.SetInt("level", nextlevel);
            }
            else if(level >5 && level < 11)//parfait easy
            {
                nextlevel = Random.Range(11, 16);
                while(PlayerPrefs.GetInt("except",0) == 16-nextlevel)
                {
                    nextlevel = Random.Range(11, 16);
                }
                PlayerPrefs.SetInt("except", 16 - nextlevel);
                PlayerPrefs.SetInt("level", nextlevel);

            }
            else if (level > 10 && level < 16)//drawing normal
            {
                nextlevel = Random.Range(16, 19);
                while (PlayerPrefs.GetInt("except", 0) == 19 - nextlevel)
                {
                    nextlevel = Random.Range(16, 19);
                }
                PlayerPrefs.SetInt("except", 19 - nextlevel);
                PlayerPrefs.SetInt("level", nextlevel);

            }
            else if (level > 15 && level < 19)//parfait normal
            {
                nextlevel = Random.Range(19, 22);
                while (PlayerPrefs.GetInt("except", 0) == 22 - nextlevel)
                {
                    nextlevel = Random.Range(19, 22);
                }
                PlayerPrefs.SetInt("except", 22 - nextlevel);
                PlayerPrefs.SetInt("level", nextlevel);

            }
            else if (level > 18 && level < 22)//drawing hard
            {
                nextlevel = Random.Range(22, 25);
                while (PlayerPrefs.GetInt("except", 0) == 25 - nextlevel)
                {
                    nextlevel = Random.Range(22, 25);
                }
                PlayerPrefs.SetInt("except", 25 - nextlevel);
                PlayerPrefs.SetInt("level", nextlevel);

            }
            else if (level > 21 && level < 25)//parfait hard
            {
                nextlevel = Random.Range(25, 28);
                while (PlayerPrefs.GetInt("except", 0) == 28 - nextlevel)
                {
                    nextlevel = Random.Range(25, 28);
                }
                PlayerPrefs.SetInt("except", 28 - nextlevel);
                PlayerPrefs.SetInt("level", nextlevel);

            }
            else if (level > 24 && level < 28)//drawing super hard
            {
              
                
                PlayerPrefs.SetInt("level", 28);

            }
            else if(level == 28)
            {
                PlayerPrefs.SetInt("level", 29);
            }
            else if(level >= 29)
            {
                PlayerPrefs.SetInt("level", 0);

            }




            //Default level admin
            //PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 0) + 1);
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
