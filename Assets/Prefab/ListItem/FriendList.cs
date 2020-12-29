using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendList : MonoBehaviour
{
   
    public Text friend_name;

    public void DeleteFriend()
    {
        //delete from friend where id =?(friend_name) and friend_id = ?(GameManager.instance.id)
        //or
        //delete from friend where id =?(GameManager.instance.id) and friend_id = ?(friend_name)


    }

}
