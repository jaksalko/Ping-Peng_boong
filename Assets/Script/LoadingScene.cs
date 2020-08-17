using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 구글 플레이 연동
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LoadingScene : MonoBehaviour
{
    public Text id;
    public Image fade;
    public Image title;
    public float minSize;
    public float maxSize;
    bool once;
    public InputField nickname;
    private void Awake()
    {
        StartCoroutine(Interpolation());

        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

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
                    if (!once)
                        StartCoroutine(Fader());
                }
                else
                {
                    Debug.Log("Fall");
                    id.text = "Fail";
                    GoogleInstance.instance.id = "guest";
                    if (!once)
                        StartCoroutine(Fader());
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
        ((PlayGamesPlatform)Social.Active).SignOut();
        id.text = "Logout";
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
