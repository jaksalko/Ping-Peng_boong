using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Amazon;
using Amazon.CognitoIdentity;
using Amazon.IdentityManagement;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.Runtime.Internal;

using Facebook.Unity;

[DynamoDBTable("UserInfo")]
    public class User
    {
        [DynamoDBHashKey]public string user_id { get; set; } //hash key
        [DynamoDBProperty]public string nickname { get; set; }
        [DynamoDBProperty]public int boong { get; set; }
        [DynamoDBProperty]public int heart {get; set;}
    }

public class AWSManager : MonoBehaviour
{
    public static AWSManager instance = null;
    CognitoAWSCredentials credentials;
    AmazonDynamoDBClient dbClient;
    DynamoDBContext dbContext;

    string id;
    public User user;
    void Awake()
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

        UnityInitializer.AttachToGameObject(this.gameObject); // Amazon Initialize
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest; //Bug fix code
        
        credentials = new CognitoAWSCredentials ( //get credentials 
            PrivateData.identitiy_pool_id, // Identity Pool ID
            RegionEndpoint.APNortheast2 // Region : Seoul
        );
        dbClient = new AmazonDynamoDBClient(credentials , RegionEndpoint.APNortheast2);
        dbContext = new DynamoDBContext(dbClient);

        //credentials.ClearCredentials();


        FB.Init(delegate() {

            if (FB.IsLoggedIn) { //User already logged in from a previous session
                Debug.Log("is Logged Get Access Token : " + AccessToken.CurrentAccessToken.TokenString);
                credentials.AddLogin ("graph.facebook.com", AccessToken.CurrentAccessToken.TokenString);

            } else {
                Debug.Log("not Logged Get Access Token : null");
            }
        });


        GetIdentityId("start ");
        


        //Facebook Initialize
        
    }

    public void SignUpWithFacebook()//SignUp Button Click Event
    {
        var perms = new List<string>(){"email"};
        FB.LogInWithReadPermissions (perms, FacebookLoginCallback);

    }
    void FacebookLoginCallback(ILoginResult result)//callback by signup button click event
    {
        if (FB.IsLoggedIn)//success get token
        {
            Debug.Log("You get Access Token : " + AccessToken.CurrentAccessToken.TokenString);
            credentials.AddLogin ("graph.facebook.com", AccessToken.CurrentAccessToken.TokenString);
            
            credentials.GetIdentityIdAsync(delegate(AmazonCognitoIdentityResult<string> cog_result) {
            if (cog_result.Exception != null) {
                //Exception!
                Debug.Log(cog_result.Exception);
            }
            id = cog_result.Response;
            Find_UserInfo();


            });


        }
        else//error...
        {
            Debug.Log("FB Login error");
        }
    }

    public void GetIdentityId(string call)
    {
        
        credentials.GetIdentityIdAsync(delegate(AmazonCognitoIdentityResult<string> result) {
            if (result.Exception != null) {
                //Exception!
                Debug.Log(result.Exception);
            }
            id = result.Response;
            Debug.Log(call + "identityID : " + id);
           
        });

       
        
    }

    public void Create_UserInfo(string nickname)
    {
        User user = new User
        {
            user_id = id,
                    nickname = nickname,
                    boong = 0,
                    heart = 5
        };
        // Save the book.
        dbContext.SaveAsync(user,(result)=>{
            if(result.Exception == null)
                Debug.Log("Success saved db");
            else
                Debug.Log("DB Save Exception : " + result.Exception);

    }); 
    }
    public void Find_UserInfo() //DB에서 캐릭터 정보 받기
    {
        User user;
        dbContext.LoadAsync<User>(id, (AmazonDynamoDBResult<User> result) =>
        {
            // id가 abcd인 캐릭터 정보를 DB에서 받아옴
            if (result.Exception != null)
            {
                Debug.LogException(result.Exception);
                
            }
            if(result.Result == null)
            {
                Debug.Log("new user!");
                
            }
            else
            {
                user = result.Result;
                Debug.Log("user data :" + user.nickname); //찾은 캐릭터 정보 중 아이템 정보 출력

            }
            
        }, null);
    }
}
