using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float speed;
    //public Text moveCountText;



    Map stage;

    int mapsizeH;
    int mapsizeW;
    int[,] map;
    bool[,] check;
    int posZ;     // vertical
    int posX;     // horizental

    //int count = 0;
    [SerializeField]
    bool isMoving = false;

    [SerializeField]
    bool upstair = false;//if player located in second floor --> true





    CharacterController cc;
    Vector3 dir;

    [SerializeField]
    Player other;

    public enum State
    {
        Idle,//no interaction
        Master,//in interaction and state is master
        Slave//in interaction and state is slave...
    }
    [SerializeField]
    State state;
    [SerializeField]
    bool stateChange;
    
    public bool Moving()
    {
        return isMoving;
    }
    [SerializeField]
    public bool isActive = false;
    Vector3 targetPos;

    int getDirection = -1;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        stateChange = false;
        isMoving = false;
        
        //isActive = false;

        cc = GetComponent<CharacterController>();
        stage = GameController.instance.map;
        mapsizeH = stage.mapsizeH;
        mapsizeW = stage.mapsizeW;
        map = stage.map;
        check = stage.check;


        FindObjectOfType<TouchMove>().Move += PlayerControl;

        FindPlayer();
    }


    public void FindPlayer()
    {
        posX = (int)transform.position.x;
        posZ = (int)transform.position.z;
        map[posZ, posX] = 5;
        check[posZ, posX] = true;
        Debug.Log(gameObject.name + "   Vertical : " + posZ + " Horizental : " + posX);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameController.Running)
            return;

        if (isMoving)
        {
            if (cc.isGrounded)
            {
                //Debug.Log("is grounded");
                cc.Move(speed * Time.deltaTime * dir);
            }
            else
            {
                //Debug.Log("is not grounded!!!!");
                cc.Move(speed * Time.deltaTime * Vector3.down);
            }
            float distance = Vector3.Distance(transform.position, targetPos);
            //transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
            if (distance < 0.25f)
            {
                SetPlayerMarker();
                Debug.Log("Arrive... target position : " + targetPos + "  distance : " + distance);
                isMoving = false;
                transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);

                if(stateChange)
                {
                    state = State.Idle;
                    other.state = State.Idle;
                    stateChange = false;
                    other.transform.SetParent(null);
                    GameController.instance.ui.ChangeCharacter();
                    other.PlayerControl(getDirection);
                    
                    
                }
                else
                {
                    if(posX == other.posX && posZ == other.posZ)
                    {
                        if(upstair)
                        {
                            state = State.Slave;
                            other.state = State.Master;
                            transform.SetParent(other.transform);
                        }
                        else
                        {
                            state = State.Master;
                            other.state = State.Slave;
                            other.transform.SetParent(transform);
                        }
                    }
                }

                map[posZ, posX] = 5;
                map[other.posZ, other.posX] = 5;
                if (CheckStageClear(stage.parfait))
                {
                    GameController.instance.GameEnd(true);
                }
            }
        }

    }

    bool CheckStageClear(bool parfait)
    {
        if (!parfait)
        {
            for (int i = 0; i < mapsizeH; i++)
            {
                for (int j = 0; j < mapsizeW; j++)
                {
                    if (!check[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        else//parfait mode clear
        {
            return stage.checkparfait;

        }

    }

    bool stone = false;//call playercontrol once...

    void PlayerControl(int direction)//direction 1 : u 2: r 3 : d 4 : l
    {
        
        if (!isMoving && isActive && !stone)
        {
            stone = true;
            getDirection = direction;

            if(state == State.Slave)
            {
                
                state = State.Idle;
                other.state = State.Idle;
                transform.SetParent(null);
            }

            Debug.Log("move direction : " + direction);
            switch (direction)
            {
                case 1:
                    Up_2();
                    break;
                case 2:
                    Right_2();
                    break;
                case 3:
                    Down_2();
                    break;
                 case 4:
                    Left_2();
                    break;
            }

        }
      
        


    }

    bool SetEndPoint_Paint()
    {
        for (int i = 0; i < mapsizeH; i++)
        {
            for (int j = 0; j < mapsizeW; j++)
            {
                if (!check[i, j])
                {
                    return false;
                }
            }
        }
        return true;

    }

    void SetPlayerMarker()
    {
        Debug.Log("Set Player Marker : " + gameObject.name + "  (" + posZ + "," + posX + ")");
        map[posZ, posX] = 5;
        if(state != State.Idle)
        {
            other.posX = posX;
            other.posZ = posZ;
        }
    }

   

    void CheckMove(int direction)
    {
        int[,] step = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        int next = map[posZ + step[direction, 0], posX + step[direction, 1]];
        float y1 = -9f;
        float y2 = -8f;

        int checkSlopeNumber = 0;
        int downSlopeNumer = 0;

        switch (direction)
        {
            case 0:
                checkSlopeNumber = 21;
                downSlopeNumer = 23;
                break;
            case 1:
                checkSlopeNumber = 22;
                downSlopeNumer = 24;
                break;
            case 2:
                checkSlopeNumber = 23;
                downSlopeNumer = 21;
                break;
            case 3:
                checkSlopeNumber = 24;
                downSlopeNumer = 22;

                break;
        }

        if (!upstair)
        {
            if (map[posZ, posX] < 20)
                map[posZ, posX] = 0;



            while (next == 0 || next == checkSlopeNumber || next < 0)
            {
                //Debug.Log("while...");
                if (direction == 0)
                {
                    posZ++;
                    //Debug.Log("posz++" + posZ);
                }
                else if (direction == 1)
                {
                    posX++;

                }
                else if (direction == 2)
                {
                    posZ--;

                }
                else if (direction == 3)
                {
                    posX--;

                }
                next = map[posZ + step[direction, 0], posX + step[direction, 1]];

                if (map[posZ, posX] == checkSlopeNumber)
                {
                    upstair = true;
                    break;
                }

                check[posZ, posX] = true;


                if (SetEndPoint_Paint())
                    break;




            }

            if(map[posZ,posX] == 2 && state == State.Master)
            {
                Debug.Log("state change   " + posZ + "," + posX);
                stateChange = true;
            }

            if (upstair)//1층에서 가다가 경사로를 만남
            {
                switch (direction)
                {
                    case 0:
                        Up_2();
                        break;
                    case 1:
                        Right_2();
                        break;
                    case 2:
                        Down_2();
                        break;
                    case 3:
                        Left_2();
                        break;
                }
                return;
            }
            else
            {
                targetPos = new Vector3(posX, y1, posZ);
                //SetPlayerMarker();
                isMoving = true;
            }


        }
        else if (upstair)
        {
            if (map[posZ, posX] < 20)
                map[posZ, posX] = 2;

            while (next == 2 || next == downSlopeNumer || next == 0
                || next < 0 || (next == 5 && !other.upstair && state == State.Idle))
            {
                //Debug.Log("while...");
                if (direction == 0)
                {
                    posZ++;
                    //Debug.Log("z++" + posZ);
                }
                else if (direction == 1)
                {
                    posX++;

                }
                else if (direction == 2)
                {
                    posZ--;

                }
                else if (direction == 3)
                {
                    posX--;

                }

                Debug.Log("posz :" + posZ + "  posx : " + posX + " value : " + map[posZ, posX]);
                next = map[posZ + step[direction, 0], posX + step[direction, 1]];



                if (map[posZ, posX] == downSlopeNumer)
                {
                    
                    upstair = false;
                    break;//While break
                }

                check[posZ, posX] = true;

                if (map[posZ, posX] == 0)//다음이 1층/
                {
                    upstair = false;
                    targetPos = new Vector3(posX, y1, posZ);
                    
                    isMoving = true;

                    Debug.Log("target position in drop : " + targetPos);

                    return;

                }


                if (map[posZ, posX] == 5 && !other.upstair && state == State.Idle)//next block is other player and stay first floor..
                {
                    

                    targetPos = new Vector3(posX, y2, posZ);
                    
                    isMoving = true;
                    Debug.Log("target position in other player : " + targetPos);
                    return;//end method...
                }



                if (SetEndPoint_Paint())
                    break;
            }

            if (!upstair)//while break --> upstair false...
            {
                switch (direction)
                {
                    case 0:
                        Up_2();
                        break;
                    case 1:
                        Right_2();
                        break;
                    case 2:
                        Down_2();
                        break;
                    case 3:
                        Left_2();
                        break;
                }

                return;
            }
            else
            {
                targetPos = new Vector3(posX, y2, posZ);
                
                isMoving = true;
               
            }

        }
        
    }


    void Up_2()
    {
        Debug.Log(" up move!");
        transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
        dir = Vector3.forward;

        CheckMove(0);
        stone = false;
        
        Debug.Log("target position in Up Method : " + targetPos);
    }

    void Right_2()
    {
        Debug.Log(" right move!");
        transform.rotation = Quaternion.Euler(new Vector3(-90f, 90f, 0f));
        dir = Vector3.right;

        CheckMove(1);
        stone = false;
        Debug.Log("target position in Right Method : " + targetPos);
    }


    void Down_2()
    {
        Debug.Log(" down move!");
        transform.rotation = Quaternion.Euler(new Vector3(-90f, 180f, 0f));
        dir = Vector3.back;

        CheckMove(2);
        stone = false;
        Debug.Log("target position in Down Method : " + targetPos);
    }


    void Left_2()
    {
        Debug.Log(" left move!");
        transform.rotation = Quaternion.Euler(new Vector3(-90f, 270f, 0f));
        dir = Vector3.left;


        CheckMove(3);
        stone = false;
        Debug.Log("target position in Left Method : " + targetPos);
    }
















    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Parfait"))
        {
            ParfaitObject parfait = other.GetComponent<ParfaitObject>();
            if (parfait.state == ParfaitObject.State.active)
            {
                if (parfait.GetParfait())//if true end game
                {
                    targetPos = stage.parfaitEndPoint;
                    stage.checkparfait = true;
                }

            }
            else
            {
                Debug.Log("pass inactive parfait...");
            }
        }
    }

   



}
