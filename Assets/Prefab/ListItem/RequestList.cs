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
        //delete from friendrequest where id =?(sender) and friend_id = ?(GoogleInstance.instance.id)
        //insert into friend set ? (id(sender) , friend_id(GoogleInstance.instance.id))
        FriendRequest friendRequest = new FriendRequest(sender_name.text.ToString() , GoogleInstance.instance.id);
        var json = JsonUtility.ToJson(friendRequest);
        StartCoroutine(Accept_and_Update(json));
    }
    IEnumerator Accept_and_Update(string json)
    {
        yield return StartCoroutine(jsonAdapter.API_POST("friend/accept", json));
        Update?.Invoke();
        yield break;
    }
    public void Decline()
    {
        //delete from friendrequest where id =?(sender) and friend_id = ?(GoogleInstance.instance.id)

        //if success delete , update content
        Update?.Invoke();
    }
}
