using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleInstance : MonoBehaviour
{
    public Text debugTxt;

    public static GoogleInstance instance = null;
    public string id;

    public int infiniteLevel;
    public Vector2 maxSize;

    public int nowLevel;

    private void Awake()
    {
        //PlayerPrefs.SetInt("level", IslandData.lastLevel);
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
    }

    public void SetText(string txt)
    {
        debugTxt.text = debugTxt.text + "\n" + txt;

    }
}
