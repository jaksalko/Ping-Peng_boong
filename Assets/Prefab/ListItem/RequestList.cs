using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
public class RequestList : MonoBehaviour
{

    public Text sender_name;
    JsonAdapter jsonAdapter = new JsonAdapter();

    public static event Action Update;
    public void Accept()
    {
        //delete from friendrequest where id =?(sender) and friend_id = ?(GameManager.instance.id)
        //insert into friend set ? (id(sender) , friend_id(GameManager.instance.id))


        FriendRequest friendRequest = new FriendRequest(sender_name.text.ToString() , GameManager.instance.id);
        var json = JsonUtility.ToJson(friendRequest);
        StartCoroutine(jsonAdapter.API_POST("friend/accept", json , callback => { if(callback != null) Update?.Invoke(); }));
    }
  
    public void Decline()
    {
        //delete from friendrequest where id =?(sender) and friend_id = ?(GameManager.instance.id)

        //if success delete , update content
        Update?.Invoke();
    }
}
