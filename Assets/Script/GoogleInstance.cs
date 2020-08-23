using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleInstance : MonoBehaviour
{
    
    public static GoogleInstance instance = null;
    public string id;

    public int infiniteLevel;
    public Vector2 maxSize;

    public Text debug;

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
    }

    public void SetDebugText(string txt)
    {
        debug.text = txt;
    }
}
