using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FriendManage : UIScript
{
    public InputField friend_nickname;
    JsonAdapter jsonAdapter = new JsonAdapter();

	public RequestList requestItem;
	public FriendList friendItem;

	public Transform requestContent;
	public Transform friendContent;

	public ChatRoom chatroom;

    private void Awake()
    {
		StartCoroutine(GetFriendList());
		StartCoroutine(GetFriendRequest());

		RequestList.Update += UpdateList;
    }

    public void SendButtonClicked()
    {
        //send request to friend;
        FriendRequest friendRequest = new FriendRequest(GoogleInstance.instance.id, friend_nickname.text);
        var json = JsonUtility.ToJson(friendRequest);
		StartCoroutine(Sennd_and_Update(json));
    }
    IEnumerator Sennd_and_Update(string json)
    {
        yield return StartCoroutine(jsonAdapter.API_POST("friend/send", json));

		UpdateList();

		yield break;
	}
    void UpdateList()
    {
		StartCoroutine(GetFriendList());
		StartCoroutine(GetFriendRequest());
	}
    public IEnumerator GetFriendRequest()
    {
       
			UnityWebRequest www = UnityWebRequest.Get("http://ec2-15-164-219-253.ap-northeast-2.compute.amazonaws.com:3000/friend/request?id=" + GoogleInstance.instance.id);
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				// Show results as text
				Debug.Log(www.downloadHandler.text);



				string fixdata = JsonHelper.fixJson(www.downloadHandler.text);
				Debug.Log(fixdata);

				FriendRequest[] datas = JsonHelper.FromJson<FriendRequest>(fixdata);
				Debug.Log("request count : " + datas.Length);

				foreach (Transform child in requestContent)
				{
					Destroy(child.gameObject);
				}


				for (int i = 0; i < datas.Length; i++)
				{
					var item = Instantiate(requestItem, default, Quaternion.identity);
					item.sender_name.text = datas[i].id;
					item.transform.SetParent(requestContent);
				}


			
		}

		

	}

	public IEnumerator GetFriendList()
	{
		

			UnityWebRequest www = UnityWebRequest.Get("http://ec2-15-164-219-253.ap-northeast-2.compute.amazonaws.com:3000/friend/list?id=" + GoogleInstance.instance.id);
		    yield return www.SendWebRequest();

		    if (www.isNetworkError || www.isHttpError)
		    {
			    Debug.Log(www.error);
		    }
		    else
		    {
			    // Show results as text
			    Debug.Log(www.downloadHandler.text);



			    string fixdata = JsonHelper.fixJson(www.downloadHandler.text);
			    Debug.Log(fixdata);

			    FriendRequest[] datas = JsonHelper.FromJson<FriendRequest>(fixdata);
			    Debug.Log("friend count : " + datas.Length);

			    foreach (Transform child in friendContent)
			    {
				    Destroy(child.gameObject);
			    }

			    for (int i = 0; i < datas.Length; i++)
			    {
				    var item = Instantiate(friendItem, default, Quaternion.identity);
                    if(datas[i].id == GoogleInstance.instance.id)
                    {
					    item.friend_name.text = datas[i].friend_id;
				    }
                    else
                    {
					    item.friend_name.text = datas[i].id;
				    }
				
				    item.transform.SetParent(friendContent);
			    }
		    }

		
	}

    public void OpenChatRoom(FriendList list)
    {
        
		
		chatroom.gameObject.SetActive(true);
		chatroom.OpenChatRoom(GoogleInstance.instance.id, list.friend_name.text.ToString());
	}
}
