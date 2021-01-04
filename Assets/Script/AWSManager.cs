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


[DynamoDBTable("UserInfo")]
    public class User
    {
        [DynamoDBHashKey]public string user_id { get; set; } //hash key
        [DynamoDBProperty]public string nickname { get; set; } // 유저의 닉네임 (UNIQUE)
        [DynamoDBProperty]public int boong { get; set; } // 유저의 붕 갯수
        [DynamoDBProperty]public int heart {get; set;} // 유저의 하트 갯수
        [DynamoDBProperty]public int current_stage {get; set;} // 유저가 깨야하는 스테이지
    }

public class AWSManager : MonoBehaviour
{
    public static AWSManager instance = null;
    CognitoAWSCredentials credentials;
    AmazonDynamoDBClient dbClient;
    DynamoDBContext dbContext;

    public delegate void IsNewUser(int state);
    public delegate void CreateUserCallback(bool success);
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
    }

    public void AddLogin_To_Credentials(string token)
    {
        credentials.AddLogin ("graph.facebook.com", token);
    }

    public void GetIdentityId(IsNewUser callback)
    {
        
        credentials.GetIdentityIdAsync(delegate(AmazonCognitoIdentityResult<string> result) {
            if (result.Exception != null) {
                //Exception!
                Debug.Log(result.Exception);
            }
            id = result.Response;//credential id
            
            Find_UserInfo(callback);
           
        });

       
        
    }

    public void Create_UserInfo(string nickname,CreateUserCallback callback)//call by LoadingScene(AddAccount)
    {
        User user = new User
        {
            user_id = id,
                    nickname = nickname,
                    boong = 0,
                    heart = 5,
                    current_stage = 0
        };
        // Save the book.
        dbContext.SaveAsync(user,(result)=>{
            if(result.Exception == null)
            {
                Debug.Log("Success saved db");
                callback(true);
            }                
            else
            {
                Debug.Log("DB Save Exception : " + result.Exception);
                callback(false);
            }
                

    }); 
    }
    public void Find_UserInfo(IsNewUser callback) //DB에서 캐릭터 정보 받기
    {
        
        dbContext.LoadAsync<User>(id, (AmazonDynamoDBResult<User> result) =>
        {
            // id가 abcd인 캐릭터 정보를 DB에서 받아옴
            if (result.Exception != null)
            {
                Debug.LogException(result.Exception);
                callback(-1);
            }
            if(result.Result == null)
            {
                Debug.Log("new user!");
                callback(0);
            }
            else
            {
                user = result.Result;
                Debug.Log("user data :" + user.nickname); //찾은 캐릭터 정보 중 아이템 정보 출력
                callback(1);
            }
            
        }, null);
    }
}
