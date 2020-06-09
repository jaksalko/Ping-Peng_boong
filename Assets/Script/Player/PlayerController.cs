using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerController : MonoBehaviour
{
    public TouchMove touchMove;


    SampleMap currentMap; public SampleMap Map { set { currentMap = value; } }
    public Player player_black;
    public Player player_red;

    [SerializeField]
    Player activePlayer;

    [SerializeField]
    float speed;

    [SerializeField]
    Vector2 down;
    [SerializeField]
    Vector2 up;
    [SerializeField]
    bool click;

    bool move;

    // Start is called before the first frame update
    void Start()
    {

        Player.setPosition += SetPosition;
        Player.tryMove += TryMove;
        Player.collisionEvent += StopMove;
        player_black.playerPosition = new Player.PlayerPosition(player_black, currentMap.startPositionA + new Vector3(0, -0.5f, 0));
        player_red.playerPosition = new Player.PlayerPosition(player_red, currentMap.startPositionB + new Vector3(0, -0.5f, 0));

        activePlayer = player_black;

        int floor_black = 0;
        int floor_red = 0;

        if (currentMap.startUpstairA)
            floor_black = 1;
        if (currentMap.startUpstairB)
            floor_red = 1;


        player_black.playerState = new Player.PlayerState(Player.State.Idle, floor_black, true);
        player_black.Speed = speed;
        player_red.playerState = new Player.PlayerState(Player.State.Idle, floor_red, false);
        player_red.Speed = speed;



        //Reactive stream
#if UNITY_EDITOR
        var mouseDownStream = this.UpdateAsObservable()
            .Where(_ => !click)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => Input.mousePosition)
            .Subscribe(_ => { down = _; click = true; });

        var mouseUpStream = this.UpdateAsObservable()
            .Where(_ => click)
            .Where(_ => Input.GetMouseButtonUp(0))
            .Select(_ => Input.mousePosition)
            .Subscribe(_ => { up = _; PlayerMove(); click = false; });

#else
        var touchDownStream = this.UpdateAsObservable()
            .Where(_ => !click)
            .Where(_ => Input.touchCount > 0)
            .Where(_ => Input.GetTouch(0).phase == TouchPhase.Began)
            .Select(_ => Input.GetTouch(0))
            .Subscribe(_ => { down = _.position; click = true; } );

        var touchUpStream = this.UpdateAsObservable()
            .Where(_ => click)
            .Where(_ => Input.touchCount > 0)
            .Where(_ => Input.GetTouch(0).phase == TouchPhase.Ended)
            .Select(_ => Input.mousePosition)
            .Subscribe(_ => { up = _; PlayerMove(); click = false; });
#endif

    }
    void PlayerMove()
    {
        Vector2 normalized = (up - down).normalized;
        Vector3 dir;

        if (normalized.x < -0.5)
        {
            //left
            activePlayer.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
            dir = Vector3.left;
        }
        else if (normalized.x > 0.5)
        {
            //right
            activePlayer.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            dir = Vector3.right;         
        }
        else
        {
            if (normalized.y > 0)
            {
                //up
                activePlayer.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                dir = Vector3.forward;            
            }
            else
            {
                //down
                activePlayer.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                dir = Vector3.back;          
            }
        }


        StartCoroutine(PlayerMoving(dir));
    }

    
    IEnumerator PlayerMoving(Vector3 dir)
    {
        float playerSpeed = activePlayer.Speed;
        
        move = true;
        while(move)
        {
            activePlayer.cc.Move(Time.deltaTime * playerSpeed * dir);
            yield return null;

        }
        
        yield break;

    }
    void SetPosition(Player player ,Vector3 vector)//callback
    {
        Debug.Log("set position");
        player.transform.position = vector;
    }
    void TryMove()//callback
    {

    }
    void StopMove()
    {
        activePlayer.transform.position = new Vector3((int)activePlayer.transform.position.x, (int)activePlayer.transform.position.y, (int)activePlayer.transform.position.z);
        move = false;
    }

    
}
