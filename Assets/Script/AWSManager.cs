using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
         [DynamoDBProperty]public int heart_time {get; set;} // 하트 충전 타이머
        [DynamoDBProperty]public int current_stage {get; set;} // 유저가 깨야하는 스테이지
        [DynamoDBProperty]public string log_out {get; set;} //로그 아웃 시간 yyyy/MM/dd HH:mm
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

    bool isPaused = false;
    bool isQuit = false;
    void Awake()
    {
        Debug.Log("Single Class Awake..." + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));//Set instance
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
                    current_stage = 0,
                    log_out = DateTime.Now.ToString("yyyy/MM/dddd HH:mm:ss"),
                    heart_time = 600 // 10분 
                    
        };
        // Save the book.
        dbContext.SaveAsync(user,(result)=>{
            if(result.Exception == null)
            {
                this.user = user;
                StartCoroutine(StartTimer());
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
                StartCoroutine(StartTimer());
                Debug.Log("user data :" + user.nickname); //찾은 캐릭터 정보 중 아이템 정보 출력
                callback(1);
            }
            
        }, null);
    }

    public void Update_UserInfo()
    {
        user.log_out = DateTime.Now.ToString("yyyy/MM/dddd HH:mm:ss");
        dbContext.SaveAsync<User>(user,(res)=>
            {
                if(res.Exception == null)
                {
                    Debug.Log("success update");
                    isQuit = true;
                }
                else
                {
                    Debug.Log(res.Exception);
                }
                     
        });
    }
    float wait_second = 1f;
    public IEnumerator StartTimer()
    {
        while(true)
        {
            if(user.heart < 5)
            {
                user.heart_time -= 1;
                if(user.heart_time == 0)
                {
                    user.heart++;
                    user.heart_time = 600;
                }
            }
            else
            {
                user.heart_time = 600;
            }
            Debug.Log("heart time " + user.heart_time);
            

            yield return new WaitForSeconds(wait_second);
        }
    }

    void OnApplicationQuit()
    {
        Update_UserInfo();
        Debug.Log("application quit");

        if(!isQuit)
            Application.CancelQuit();
        
    }
    void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            Update_UserInfo();
            Debug.Log("application pause");
            isPaused = true;
        }
        else
        {
            isPaused = false;
            Debug.Log("application resume");
        }
    }

}
