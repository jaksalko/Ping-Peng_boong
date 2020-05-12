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
    public bool isMoving = false;

    [SerializeField]
    bool upstair = false;//if player located in second floor --> true

    public ParticleSystem moveParticle;
	public GameObject crashParticle;


    public Animator animator;
    public GameObject nose;
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
    [SerializeField]
    bool third_drop;
    public bool Moving()
    {
        return isMoving;
    }
    [SerializeField]
    public bool isActive = false;
    Vector3 targetPos;

    int getDirection = -1;

    // Start is called before the first frame update
    //public Animator animator;


    void Start()
    {
        state = State.Idle;
        stateChange = false;
        third_drop = false;

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

    
    public void SetPosition(Vector3 startpos , bool upstair)
    {
        transform.position = startpos;
        this.upstair = upstair;
    }

    public void FindPlayer()
    {
        

        posX = (int)transform.position.x;
        posZ = (int)transform.position.z;
        map[posZ, posX] = 5;
        check[posZ, posX] = true;
        Debug.Log(gameObject.name + "   Vertical : " + posZ + " Horizental : " + posX + "5 mark : " + map[posZ,posX]);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameController.Running)
            return;

        if (isMoving)
        {
			//이동 시 발생하는 particle control
			if(!moveParticle.isPlaying)
			{
				moveParticle.Play();
				moveParticle.loop = true;
				var main = moveParticle.main;
				// main.startColor = new Color(1, 0.4f, 0.7f);	// skin마다 color 바꿔주도록 할 것!!
			}

			if (cc.isGrounded)	// 바닥에 붙어있으면 움직임
            {
                //Debug.Log("is grounded");
                cc.Move(speed * Time.deltaTime * dir);
            }
            else				// 바닥이 없으면 떨어짐 (여기다 쿵! 넣으면되는데 지금 잘 작동이 안 되서 넣으면 안 됨)
            {
                //Debug.Log("is not grounded!!!!");
                cc.Move(speed * Time.deltaTime * Vector3.down);
            }
            float distance = Vector3.Distance(transform.position, targetPos);
            //transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
            if (distance < 0.25f)
            {
                SetPlayerMarker();
                //Debug.Log("Arrive... target position : " + targetPos + "  distance : " + distance);
                isMoving = false;

				//이동 시 발생하는 particle control
				moveParticle.loop = false;

				transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);


                
                if (CheckStageClear(stage.parfait))
                {
                    GameController.instance.GameEnd(true);
                }

                if (stateChange)
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
                    if(posX == other.posX && posZ == other.posZ)//위치가 같고 갈라질 상황이 아니다? 무조건 같이 붙어 있는 상태
                    {
                        if(transform.position.y > other.transform.position.y)//움직인 놈이 더 위에 있다
                        {
                            state = State.Slave;
                            other.state = State.Master;
                            transform.SetParent(other.transform);
                        }
                        else//움직인 놈이 더 아래에 있다.
                        {
                            state = State.Master;
                            other.state = State.Slave;
                            other.transform.SetParent(transform);
                        }
                    }
                }

                map[posZ, posX] = 5;
                map[other.posZ, other.posX] = 5;


            }
        }
        

    }
    private void LateUpdate()
    {
        if (isMoving)
        {
            animator.SetBool("move", true);
            nose.SetActive(true);
        }
        else
        {
            animator.SetBool("move", false);
            nose.SetActive(false);
            animator.SetInteger("action", 0);
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

                if(other.upstair)
                {
                    third_drop = true;
                }
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
        //Debug.Log("Set Player Marker : " + gameObject.name + "  (" + posZ + "," + posX + ")");
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

		float y1 = -9.5f;//animation character modification.. 5/13
        float y2 = -8.5f;//

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



            while ((next <= 0 && next > -5) || next == checkSlopeNumber )
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
                    if(next == 5)
                    {
                        posX -= step[direction, 1];
                        posZ -= step[direction, 0];


                        targetPos = new Vector3(posX, y1, posZ);

                        isMoving = true;



                        return;
                    }

                    upstair = true;
                    break;
                }

               
                check[posZ, posX] = true;


                if (SetEndPoint_Paint() && !stage.parfait)
                    break;




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

                if ((next == 2 || next < -4) && state == State.Master)
                {
                    Debug.Log("state change   " + posZ + "," + posX);
                    stateChange = true;
                }


                Debug.Log("set pos");
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

                //Debug.Log("posz :" + posZ + "  posx : " + posX + " value : " + map[posZ, posX]);


                next = map[posZ + step[direction, 0], posX + step[direction, 1]];

                if (map[posZ, posX] == downSlopeNumer)
                {
                    if(next == 5)
                    {
                        posX -= step[direction, 1];
                        posZ -= step[direction, 0];


                        targetPos = new Vector3(posX, y2, posZ);

                        isMoving = true;

                        

                        return;
                    }
                    upstair = false;
                    break;//While break
                }

                
                check[posZ, posX] = true;


                if (map[posZ, posX] <= 0 && map[posZ,posX] > -5)//다음이 1층 또는 1층 파
                {
                    Debug.Log("drop");
                    upstair = false;
                    targetPos = new Vector3(posX, y1, posZ);
                    
                    isMoving = true;

                    //Debug.Log("target position in drop : " + targetPos);

                    return;

                }


                if (third_drop)
                {
                    upstair = true;
                    Debug.Log("third floor drop... mean slave drop in second floor");
                    targetPos = new Vector3(posX, y2, posZ);

                    isMoving = true;

                    third_drop = false;

                    return;
                }


                if (map[posZ, posX] == 5 && !other.upstair && state == State.Idle)//next block is other player and stay first floor..
                {

                    targetPos = new Vector3(posX, y2, posZ);
                    
                    isMoving = true;
                    //Debug.Log("target position in other player : " + targetPos);
                    return;//end method...
                }


                if (SetEndPoint_Paint() && !stage.parfait)
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
                if(!third_drop)
                    targetPos = new Vector3(posX, y2, posZ);
                else
                {
                    third_drop = false;
                    targetPos = new Vector3(posX, y2+1, posZ);
                }
                
                isMoving = true;
               
            }

        }
        
    }


    void Up_2()
    {
        //Debug.Log(" up move!");
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        dir = Vector3.forward;

        CheckMove(0);
        stone = false;
        
        //Debug.Log("target position in Up Method : " + targetPos);
    }

    void Right_2()
    {
        //Debug.Log(" right move!");
        transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        dir = Vector3.right;

        CheckMove(1);
        stone = false;
        //Debug.Log("target position in Right Method : " + targetPos);
    }


    void Down_2()
    {
        //Debug.Log(" down move!");
        transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        dir = Vector3.back;

        CheckMove(2);
        stone = false;
        Debug.Log("target position in Down Method : " + targetPos);
    }


    void Left_2()
    {
        //Debug.Log(" left move!");
        transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
        dir = Vector3.left;


        CheckMove(3);
        stone = false;
        Debug.Log("target position in Left Method : " + targetPos);
    }














    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Parfait"))
        {
            ParfaitObject parfait = other.GetComponent<ParfaitObject>();
            if (parfait.state == ParfaitObject.State.active)
            {
                if (parfait.GetParfait())//if true end game
                {
                    Debug.Log("end parfait mode.");
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Parfait"))
        {
            ParfaitObject parfait = other.GetComponent<ParfaitObject>();
            if (parfait.state == ParfaitObject.State.active)
            {
                if (parfait.GetParfait())//if true end game
                {
                    Debug.Log("end parfait mode.");
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


	private IEnumerator CrashEffect()
	{
		Debug.Log("Crash");
		crashParticle.SetActive(true);
		yield return new WaitForSeconds(1f);
		crashParticle.SetActive(false);
	}
   



}
