using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 구글 플레이 연동
using CloudOnce;
using UnityEngine.Networking;

public class LoadingScene : MonoBehaviour
{
    public Text id;
    public Image fade;
    public Image title;
    public float minSize;
    public float maxSize;
    bool once;
    public InputField nickname;
    public Text addAccountText;
    public GameObject addAccountPanel;

   

    private void Awake()
    {

        JsonAdapter.GET += IsNew;
        JsonAdapter.POST += IsUnique;
        StartCoroutine(Interpolation());//Animation Effect


        Cloud.OnInitializeComplete += CloudInitializeCompleted;

        Cloud.Initialize(false, true);
        //Cloud.Initialize();
        
    }
    public void IsNew(bool add)// call by loading
    {
        JsonAdapter.GET -= IsNew;
        if (add)
        {
            // create new account
            addAccountPanel.SetActive(true);
        }
        else
        {

            Debug.Log("already exist");
            GoogleInstance.instance.id = Cloud.PlayerID;
            
            JsonAdapter.POST -= IsUnique;
            SceneManager.LoadScene("MainScene");

            //Load Lobby Scene
        }
        
        
    }
    public void IsUnique(bool unique)//call by add AddAccount button
    {
        if(unique)
        {
            addAccountText.text = "created successfully";
            GoogleInstance.instance.id = Cloud.PlayerID;
            JsonAdapter.POST -= IsUnique;


            SceneManager.LoadScene("MainScene");
        }
        else
        {
            addAccountText.text = "already exist!";
        }
    }
    public void AddAccount()
    {
        JsonAdapter jsonAdapter = new JsonAdapter();
        UserData newAccount = new UserData(userid : Cloud.PlayerID , nick : nickname.text);       
        var json = JsonUtility.ToJson(newAccount);

        
        StartCoroutine(jsonAdapter.API_POST("account/add",json));


    }
    public void CloudInitializeCompleted()
    {
        Cloud.OnInitializeComplete -= CloudInitializeCompleted;
        GoogleInstance.instance.SetText("initialized completed ! " + Cloud.PlayerID);

        Debug.LogWarning("initialize completed");

        string userId = Cloud.PlayerID;

        JsonAdapter jsonAdapter = new JsonAdapter();
        StartCoroutine(jsonAdapter.API_GET("account/checkid?id="+userId,callback => { }));

    }
   
    public void OnLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool bSuccess) =>
            {
                if (bSuccess)
                {
                    Debug.Log("Success : " + Social.localUser.userName);
                    id.text = Social.localUser.userName;
                    GoogleInstance.instance.id = Social.localUser.userName;
                    GoogleInstance.instance.SetText(Social.localUser.userName);
                    if (!once)
                        StartCoroutine(Fader());
                }
                else
                {
                    Debug.Log("Fall");
                    id.text = "Fail";
                    GoogleInstance.instance.SetText("fail login");
                    
                }
            });
        }
    }
    public void GuestLogin()
    {
        GoogleInstance.instance.id = nickname.text;
        if (!once)
            StartCoroutine(Fader());
    }
    public void OnLogOut()
    {
        GoogleInstance.instance.id = Cloud.PlayerID;
        StartCoroutine(Fader());
    }

    public void LoadMainScene()
    {
        if(!once)
            StartCoroutine(Fader());
    }
    IEnumerator Interpolation()
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime *1.2f;
            float interpolation = Mathf.Abs(Mathf.Sin(t));

            float size = Mathf.Lerp(minSize, maxSize, interpolation);
            title.transform.localScale = new Vector3(size, size, 1);
            yield return null;
        }

    }
    IEnumerator Fader()
    {
        once = true;
        float t = 0;
        Color c = fade.color;

        while(t < 1)
        {
            t += Time.deltaTime * 0.7f;
            c.a = t;
            fade.color = c;
            yield return null;
        }

        SceneManager.LoadScene("MainScene");
        
    }
}
