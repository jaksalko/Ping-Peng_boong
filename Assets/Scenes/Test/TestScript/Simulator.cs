using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

using UnityEngine.SceneManagement;

using UniRx;
using UniRx.Triggers;

[Serializable]
public class JsonData
{
    public int num;//pk
    public string id;
    public int height;
    public int width;
    public string value;
    public string posA;
    public string posB;
    public int moveCount;
    public int difficulty;

    public void DataToString()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }

    public SampleMap MakeSampleMap()
    {
        SampleMap sampleMap = new SampleMap();
        sampleMap.mapsizeW = width;
        sampleMap.mapsizeH = height;
        sampleMap.parfait = false;
        
        sampleMap.map = new int[height, width];
        sampleMap.startPositionA = StringToPosition(posA);
        sampleMap.startPositionB = StringToPosition(posB);
        if(CharToIndex(posA[3]) == 0)
        {
            sampleMap.startUpstairA = false;
        }
        else
        {
            sampleMap.startUpstairA = true;
        }
        if (CharToIndex(posB[3]) == 0)
        {
            sampleMap.startUpstairB = false;
        }
        else
        {
            sampleMap.startUpstairB = true;
        }
        int index = 0;
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                //sampleMap.map[i, j] = value[index]-'0';
                sampleMap.map[i, j] = CharToIndex(value[index]);
                //Debug.Log(i + "," + j + " : " + sampleMap.map[i, j]);
                index++;
            }
        }
        return sampleMap;
    }

    int CharToIndex(char value)
    {
        int ascii = (int)value - 97;
        return ascii;

    }

    Vector3 StringToPosition(string position_upstair)
    {
        Vector3 v = new Vector3(CharToIndex(position_upstair[0]),
            CharToIndex(position_upstair[1]),
            CharToIndex(position_upstair[2])
            );
      

        return v;
    }


}

[Serializable]
public class MapCount
{
    public int count;
}
public class Simulator : MonoBehaviour
{
    public SimulatorCameraController cameraController;

    public static Simulator instance;
    public GameObject simulatorUI;
    public Player player1;
    public Player player2;
    public Player nowPlayer;

    public Map simulatingMap;

    public SimulatorObject simulatorObject;
    public GameObject generatorObject;
    public MapGenerator mapGenerator;
    SimulatorObject subSimulatorObject;
    public GameObject successPopup;
    public Text MoveCountTxt;

    //public UiController ui;
    //public CameraController cameraController;
    Vector3 backUpPositionA;
    Vector3 backUpPositionB;


    private bool isRunning;
    public static bool Running
    {
        get { return instance.isRunning; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);



        

    }
    void ChangeOutline()
    {
        
        if (nowPlayer == player1)
        {
            player1.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().sharedMaterial.SetFloat("_OutlineWidth", 1.08f);
            player2.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().sharedMaterial.SetFloat("_OutlineWidth", 1.0f);
        }
        else
        {
            player2.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().sharedMaterial.SetFloat("_OutlineWidth", 1.08f);
            player1.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().sharedMaterial.SetFloat("_OutlineWidth", 1.0f);
        }
    }
    public void StartSimulator()
    {

        //set Player...
        generatorObject.SetActive(false);
        simulatorObject.gameObject.SetActive(true);

        simulatingMap.GenerateMap(0);
        player1.SetSimulatorData();
        player2.SetSimulatorData();

        backUpPositionA = player1.transform.position;
        backUpPositionB = player2.transform.position;
        player1.SetPosition(simulatingMap.sampleMap.startPositionA + new Vector3(0, -0.5f, 0), simulatingMap.sampleMap.startUpstairA); //position correction fix .. 5/13
        player2.SetPosition(simulatingMap.sampleMap.startPositionB + new Vector3(0, -0.5f, 0), simulatingMap.sampleMap.startUpstairB);

        
        
        
        //ui.inGame.SetActive(true);
        

        cameraController.gameObject.SetActive(true);

        this.UpdateAsObservable()
           .Subscribe(_ => ChangeOutline());

    }
    public void GameStart()
    {
        simulatorUI.SetActive(true);

        player1.FindPlayer();
        player2.FindPlayer();

        nowPlayer = player1;
        nowPlayer.isActive = true;

        isRunning = true;

    }
    public void SimulatingSuccess()//Can Make Map
    {
        isRunning = false;
        Debug.Log("can build!");
        int count = player1.moveCount + player2.moveCount;
        int level = (count / 5) + 1;
        //StartCoroutine(INSERTMAP());
        MoveCountTxt.text = "이동횟수 : " + count + "\n" + "난이도 : " + level;
        successPopup.SetActive(true);

    }
    public void MakeMapButton()
    {
        StartCoroutine(INSERTMAP());
        
    }
    public void GoToLobbyButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void GenerateModeButton()
    {
        isRunning = false;
        player1.transform.position = backUpPositionA;
        player2.transform.position = backUpPositionB;

        subSimulatorObject = Instantiate(simulatorObject, transform.position, Quaternion.identity);
        mapGenerator.newMap = subSimulatorObject.simulatingMap.sample[0];

        Destroy(simulatorObject.gameObject);
        simulatorObject = subSimulatorObject;
        simulatorObject.Adapting();
        generatorObject.SetActive(true);
        simulatorObject.gameObject.SetActive(false);
    }
    public void ResetButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator INSERTMAP()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://ec2-15-164-219-253.ap-northeast-2.compute.amazonaws.com:3000/count");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            string result = www.downloadHandler.text;
            result = result.Replace("[", "").Replace("]","");
            result = result.Replace("(*)", "");
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            Debug.Log(result);
            MapCount count = JsonUtility.FromJson<MapCount>(result);

            JsonData jsonData = new JsonData();
            jsonData.num = count.count+1;//get latest number

            /*if (GoogleInstance.instance.id == null || GoogleInstance.instance.id == "")
                jsonData.id = "guest";
            else*/
                jsonData.id = GoogleInstance.instance.id;//get google id

            jsonData.height = simulatingMap.sampleMap.mapsizeH;
            jsonData.width = simulatingMap.sampleMap.mapsizeW;

            Vector3 startA = simulatingMap.sampleMap.startPositionA;
            jsonData.posA = PositionToString(startA, simulatingMap.sampleMap.startUpstairA);

            Vector3 startB = simulatingMap.sampleMap.startPositionB;
            jsonData.posB = PositionToString(startB, simulatingMap.sampleMap.startUpstairB);

            jsonData.moveCount = player1.moveCount + player2.moveCount;

            jsonData.difficulty = (jsonData.moveCount / 5) + 1;
            if (jsonData.difficulty > 5)
                jsonData.difficulty = 5;

            string arrayToString = "";
            for (int i = 0; i < jsonData.height; i++)
            {
                for (int j = 0; j < jsonData.width; j++)
                {

                    //arrayToString += simulatingMap.sampleMap.map[i, j].ToString();
                    arrayToString += IndexToChar(simulatingMap.sampleMap.map[i, j]);
                }
            }
            jsonData.value = arrayToString;
            var json = JsonUtility.ToJson(jsonData);

            yield return StartCoroutine(POST("http://ec2-15-164-219-253.ap-northeast-2.compute.amazonaws.com:3000/test/", json));

            SceneManager.LoadScene("MainScene");
            //successPopup.SetActive(true);
        }
    }
    string PositionToString(Vector3 pos , bool upstair)
    {
        string s = "";
        s += IndexToChar((int)pos.x);
        s += IndexToChar((int)pos.y);
        s += IndexToChar((int)pos.z);
        if (upstair)
            s += IndexToChar(1);
        else
            s += IndexToChar(0);

        return s;
    }
    char IndexToChar(int value)
    {
        
        char ascii = Convert.ToChar(value + 97);
        return ascii;
    }
   
    IEnumerator POST(string url, string bodyJsonString)
    {
        Debug.Log(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        Debug.Log("Status Code: " + request.responseCode);
    }
    public void ChangeCharacter()
    {
        

        if (!nowPlayer.Moving())
        {
            Debug.Log("change Character");
            nowPlayer.isActive = false;

            if (nowPlayer == player1)
            {
                nowPlayer = player2;
            }
            else
            {
                nowPlayer = player1;
            }
            nowPlayer.isActive = true;

            Debug.Log("player 1 : " + player1.isActive);
            Debug.Log("player 2 : " + player2.isActive);
        }
        else
        {
            Debug.Log("Can't change!");
        }

    }

}
