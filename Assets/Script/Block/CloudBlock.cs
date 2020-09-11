using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CloudBlock : Block
{
    public int direction;

    public static event Action<GameObject> Exit;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player onCloud_player = other.gameObject.GetComponent<Player>();
            if(!onCloud_player.onCloud && !onCloud_player.isMoving && onCloud_player.isActive)//call only one time
            {
                Debug.Log("ride cloud" + num);
                onCloud_player.onCloud = true;
                //StartCoroutine(onCloud_player.MoveByCloud(num));
                onCloud_player.MoveByCloud(num);
            }

            //if (onCloud_player.onCloud && !onCloud_player.isMoving && !onCloud_player.isActive)
            //{
            //    onCloud_player.MoveByCloud(num);
            //}



        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player onCloud_player = other.gameObject.GetComponent<Player>();
            Debug.Log("exit cloud");
            if(onCloud_player.isActive)
                onCloud_player.onCloud = false;

            
            Exit?.Invoke(onCloud_player.gameObject);
        }
    }
}
