using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;



using Amazon;
using Amazon.CognitoIdentity;
using Amazon.IdentityManagement;

using Facebook.Unity;

using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using System.Text;
using System.Security.Cryptography;
using AppleAuth.Interfaces;



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
    CognitoAWSCredentials credentials;
    public Button facebook_login_button;
    private IAppleAuthManager appleAuthManager;

    AWSManager awsManager = AWSManager.instance;
    GameManager gameManager = GameManager.instance;

    private void Awake()
    {
        
        StartCoroutine(Interpolation());//Animation Effect
#if UNITY_IOS
/*
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            this.appleAuthManager = new AppleAuthManager(deserializer);    
        }

        if(PlayerPrefs.GetString("AppleUserIdKey") != null)
        {
            Debug.Log("Can quick login with apple");
            QuickAppleLogin();
        }
        else
        {
            Debug.Log("first access");
        }
*/
#endif
    


        
    }

    void Start()
    {
        Debug.Log(PlayerPrefs.GetString("nickname",null));
        if(PlayerPrefs.GetString("nickname","") == "")//no account
        {
            facebook_login_button.gameObject.SetActive(true);
        }
        else
        {
            play_button.gameObject.SetActive(true);
        }
    }
    /*
    private void Update()
    {
        this.appleAuthManager?.Update();
    }
    */
    private void QuickAppleLogin()
    {
        var quickLoginArgs = new AppleAuthQuickLoginArgs();

        this.appleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>
            {
                // Received a valid credential!
                // Try casting to IAppleIDCredential or IPasswordCredential

                // Previous Apple sign in credential
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
                    credentials.AddLogin("appleid.apple.com", identityToken);
                    credentials.GetIdentityIdAsync(delegate(AmazonCognitoIdentityResult<string> result) {
                        if (result.Exception != null) {
                            //Exception!
                            Debug.Log("no identity id!");
                        }
                        string identityId = result.Response;
                        Debug.Log("identity ID :" + identityId);
                        Debug.Log("Success Quick Login");
                    });
                }

                // Saved Keychain credential (read about Keychain Items)
                //var passwordCredential = credential as IPasswordCredential;
            },
            error =>
            {
                // Quick login failed. The user has never used Sign in With Apple on your app. Go to login screen
            });
    }
    public void SignUpWithApple()
    {
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
                    credentials.AddLogin("appleid.apple.com", identityToken);
                    credentials.GetIdentityIdAsync(delegate(AmazonCognitoIdentityResult<string> result) {
                        if (result.Exception != null) {
                            //Exception!
                            Debug.Log("no identity id!");
                        }
                        string identityId = result.Response;
                        Debug.Log(identityId);
                    });
                            }
            },
            error =>
            {
                // Something went wrong
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
            });
        
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

    */
    public void AddAccount()
    {

        string nickname_regex = "^[a-zA-Z가-힣0-9]{1}[a-zA-Z가-힣0-9]{1,7}$";
        Regex regex = new Regex(nickname_regex);

        if(regex.IsMatch(nickname.text))
        {
            //중복 체크
            awsManager.Create_UserInfo(nickname.text);            
        }
        {
            addAccountText.text = "한글,영어,숫자 포함 최소 2자, 최대 8자입니다";
        }

      


    }

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
