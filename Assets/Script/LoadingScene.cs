using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 구글 플레이 연동

using UnityEngine.Networking;
using System.Text.RegularExpressions;

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

    public Button make_account_button;
    public Button play_button;


    private void Awake()
    {

       
        StartCoroutine(Interpolation());//Animation Effect


      
        
    }
    /*void CloudInitializeCompleted()
    {
        Cloud.OnInitializeComplete -= CloudInitializeCompleted;

      

        Debug.LogWarning("initialize completed");

        JsonAdapter jsonAdapter = new JsonAdapter();
        StartCoroutine(jsonAdapter.API_GET("account/checkid?id=" + Cloud.PlayerID, callback => { IsNewUser(callback); }));

    }
    public void IsNewUser(string callback)// call by loading
    {
        
        if (callback != null)
        {
            make_account_button.gameObject.SetActive(true);
            // create new account
            
        }
        else
        {
            GameManager.instance.UpdateUserData(Cloud.PlayerID);
            play_button.gameObject.SetActive(true);
        }
        
        
    }
    public void IsUniqueNickname(string callback)//call by add AddAccount button
    {
        if(callback != null)
        {
            //addAccountText.text = "created successfully";
            GameManager.instance.UpdateUserData(Cloud.PlayerID);


            addAccountPanel.SetActive(false);
            make_account_button.gameObject.SetActive(false);
            play_button.gameObject.SetActive(true);
            
        }
        else
        {
            addAccountText.text = "이미 존재하는 닉네임입니다.";
        }
    }
    public void AddAccount()
    {

        string nickname_regex = "^[a-zA-Z가-힣0-9]{1}[a-zA-Z가-힣0-9]{1,7}$";
        Regex regex = new Regex(nickname_regex);

        if(regex.IsMatch(nickname.text))
        {
            JsonAdapter jsonAdapter = new JsonAdapter();
            UserData newAccount = new UserData(userid: Cloud.PlayerID, nick: nickname.text);
            var json = JsonUtility.ToJson(newAccount);


            StartCoroutine(jsonAdapter.API_POST("account/add", json , callback => { IsUniqueNickname(callback); }));
        }
        {
            addAccountText.text = "한글,영어,숫자 포함 최소 2자, 최대 8자입니다";
        }

      


    }
    */

    public void GameStart()
    {
        StartCoroutine(Fader());
    }
    public void ActiveMakeAccountPanel()
    {
        addAccountPanel.SetActive(true);

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

        yield break;
        
    }
}
