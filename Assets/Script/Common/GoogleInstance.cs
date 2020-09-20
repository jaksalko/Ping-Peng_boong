using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleInstance : MonoBehaviour
{
    public Text debugTxt;

    public static GoogleInstance instance = null;
    public string id; // id == user.id

    public int infiniteLevel;
    public Vector2 maxSize;

    public int nowLevel;

    public UserData user;
    public List<StageData> stages;

    public GameObject canvas;
    private void Awake()
    {
        
        Debug.Log("Single Class Awake...");//Set instance
        if (instance == null)
        {
            Debug.Log("Single instance is null");
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Single instance is not Single.. Destroy gameobject!");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//Dont destroy this singleton gameobject :(

        for(int i = 0; i < IslandData.lastLevel; i++)
        {
            stages.Add(new StageData(user.id , i));
        }
        
    }

    public void SetText(string txt)
    {
        debugTxt.text = debugTxt.text + "\n" + txt;

    }
}
