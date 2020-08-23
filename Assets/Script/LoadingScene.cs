using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;
using System.Text;

using UnityEngine.SocialPlatforms.GameCenter;


// 구글 플레이 연동
#if UNITY_ANDROID
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;


#elif UNITY_IOS
//using AppleAuth.Editor;
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
#endif

public class LoadingScene : MonoBehaviour
{
#if UNITY_IOS
    private IAppleAuthManager appleAuthManager;
#endif
    public Text appleArg;
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

#if UNITY_ANDROID
        /*PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();*/
#elif UNITY_IOS
        //initializing
        if(AppleAuthManager.IsCurrentPlatformSupported)
        {
            var deserializer = new PayloadDeserializer();
            appleAuthManager = new AppleAuthManager(deserializer);
        }

#endif

    }
    private void Update()
    {
#if UNITY_IOS
        //update the appleauthmanager instance to execute
        //pending callbacks inside unity's execution loop
        appleAuthManager?.Update();
#endif
    }
    public void GoogleLogin()//if android => playstore , ios => gamecenter
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool bSuccess) =>
            {
                if (bSuccess)
                {
                    //Debug.Log("Success : " + Social.localUser.userName);
                    GoogleInstance.instance.SetDebugText("user name :" + Social.localUser.userName);

                    GoogleInstance.instance.id = Social.localUser.userName;
                    if (!once)
                        StartCoroutine(Fader());
                }
                else
                {
                    //Debug.Log("Fall");
                    GoogleInstance.instance.SetDebugText("user name : guest");

                    
                    GoogleInstance.instance.id = "guest";
                    if (!once)
                        StartCoroutine(Fader());
                }
            });
        }
    }

    public void AppleSuccess()
    {
        appleArg.text = "success";
    }
    public void AppleError()
    {
        appleArg.text = "error";
    }
    public void AppleCredential()
    {
        appleArg.text = "credential";
    }

    public void AppleLogin()//Apple login button clicked..
    {
#if ANDROID_IOS
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        

        this.appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
        // Obtained credential, cast it to IAppleIDCredential
        var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
            // Apple User ID
            // You should save the user ID somewhere in the device
            var userId = appleIdCredential.User;
                    PlayerPrefs.SetString("AppleUserIdKey", userId);

            // Email (Received ONLY in the first login)
            var email = appleIdCredential.Email;

                    // Full name (Received ONLY in the first login)
                    var fullName = appleIdCredential.FullName;
        
            // Identity token
            var identityToken = Encoding.UTF8.GetString(
                        appleIdCredential.IdentityToken,
                        0,
                        appleIdCredential.IdentityToken.Length);

            // Authorization code
            var authorizationCode = Encoding.UTF8.GetString(
                        appleIdCredential.AuthorizationCode,
                        0,
                        appleIdCredential.AuthorizationCode.Length);

            // And now you have all the information to create/login a user in your system
        }
            },
            error =>
            {
        // Something went wrong
        var authorizationErrorCode = error.GetAuthorizationErrorCode();
            });
#endif
    }



    public void GuestLogin()
    {
        GoogleInstance.instance.id = nickname.text;
        if (!once)
            StartCoroutine(Fader());
    }
    public void OnLogOut()
    {
#if UNITY_ANDROID
        //((PlayGamesPlatform)Social.Active).SignOut();
        id.text = "Logout";
#elif UNITY_IOS

#endif
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
