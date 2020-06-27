using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour
{

    public float speed;
    //public Text moveCountText;
    public int moveCount = 0;


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
	public ParticleSystem crashParticle;
	public GameObject bumpParticle;
	bool isPlayingParticle = false;

    public Animator animator;
    
	public int actionnum;
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


    [SerializeField]
    Vector2 down;
    [SerializeField]
    Vector2 up;
    [SerializeField]
    bool click;

    [SerializeField]
    bool simulating;

    private CheckAnimationState stateMachine;
    void AnimationEnd()
    {
        Debug.Log("Animation End...");
        animator.SetInteger("action", 0);
        actionnum = 0;
        
    }
    public void SetSimulatorData()
    {
        
        stage = Simulator.instance.simulatingMap;
        mapsizeH = stage.mapsizeH;
        mapsizeW = stage.mapsizeW;
        map = stage.map;
        check = stage.check;

        stateMachine = animator.GetBehaviour<CheckAnimationState>();
        stateMachine.player = this;
        stateMachine.ActionEnd += AnimationEnd;

    }
    public void SetMap()
    {
        stage = GameController.instance.map;

        mapsizeH = stage.mapsizeH;
        mapsizeW = stage.mapsizeW;
        map = stage.map;
        Debug.Log(mapsizeH + "," + mapsizeW + " (MAP :" + stage.map.Length);
        check = stage.check;
    }
    void Start()
    {
        state = State.Idle;
        stateChange = false;
        third_drop = false;

        isMoving = false;
        
      

        cc = GetComponent<CharacterController>();

        if(!simulating)
        {
            stateMachine = animator.GetBehaviour<CheckAnimationState>();
            stateMachine.player = this;
            stateMachine.ActionEnd += AnimationEnd;
        }
       
      


        


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

#elif UNITY_ANDROID || UNITY_IOS
        var touchDownStream = this.UpdateAsObservable()
            
            .Where(_ => !click)
            .Where(_ => Input.touchCount > 0)
            .Where(_ => !EventSystem.current.IsPointerOverGameObject(0))
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
        Debug.Log("distance : " + Vector2.Distance(up, down));
        bool isMove = false;

        if(Vector2.Distance(up, down) <= 30)
        {
            return;
        }
        Vector2 normalized = (up - down).normalized;
        

        if (normalized.x < -0.5)
        {
            //left
            isMove = PlayerControl(4);
        }
        else if (normalized.x > 0.5)
        {
            //right
            isMove = PlayerControl(2);
        }
        else
        {
            if (normalized.y > 0)
            {
                //up
                isMove = PlayerControl(1);

            }
            else
            {
                //down
                isMove = PlayerControl(3);
            }

        }

        if (isMove)
            moveCount++;


        
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
        Debug.Log(name + posZ+","+posX);
        map[posZ, posX] = 5;
        check[posZ, posX] = true;
//        Debug.Log(gameObject.name + "   Vertical : " + posZ + " Horizental : " + posX + "5 mark : " + map[posZ,posX]);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!simulating && !GameController.Running)
            return;
        if (simulating && !Simulator.Running)
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
                animator.SetBool("move", false);
                //Debug.Log("Arrive... target position : " + targetPos + "  distance : " + distance);
                isMoving = false;

				//이동 시 발생하는 particle control
				moveParticle.loop = false;

				transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);


                
                if (CheckStageClear(stage.parfait))
                {
                    if (!simulating)
                        GameController.instance.GameEnd(true);
                    else
                        Simulator.instance.SimulatingSuccess();
                }

                if (stateChange)
                {
                    state = State.Idle;
                    other.state = State.Idle;
                    stateChange = false;
                    other.transform.SetParent(null);
                    if (!simulating)
                        GameController.instance.ui.ChangeCharacter();
                    else
                        Simulator.instance.ChangeCharacter();
                    other.PlayerControl(getDirection);
                    

                    
                }
                else
                {
                    if(posX == other.posX && posZ == other.posZ)//위치가 같고 갈라질 상황이 아니다? 무조건 같이 붙어 있는 상태
                    {
                        Debug.Log("slave master");
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
        animator.SetBool("move", isMoving);
        if (isMoving)
        {
            
			isPlayingParticle = false;
            if (actionnum == 5)//drop
            {
                float distance = Vector3.Distance(transform.position, targetPos + new Vector3(0, 1, 0));
                if (distance < 1f)//거리가 1일때부터 드랍 애니메이션 실행 , 움직이고 있던상태에서 애니메이션 실행
                {
                    animator.SetInteger("action", actionnum);
                }
            }
            // nose.SetActive(true);
        }
        else
        {
			
            if(actionnum !=5)//이미 전에 실행해서 드랍만 예외처리
			    animator.SetInteger("action", actionnum);
            
			//이동 시 발생하는 particle control
			switch(actionnum)
			{
				case 3:
					if (!isPlayingParticle)
					{
						Debug.Log("play crash particle");
						// crashParticle.Play();
						isPlayingParticle = true;
					}
					break;
				case 4:
					if (!isPlayingParticle)
					{
						Debug.Log("play bump particle");
						bumpParticle.SetActive(true);
						Invoke("BumpParticleControl", 4.5f);
						//bumpParticle.Play();
						isPlayingParticle = true;
					}
					break;
				default:
					break;
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

   

    bool PlayerControl(int direction)//direction 1 : u 2: r 3 : d 4 : l
    {
        
        if (isActive && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            
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

            return true;

        }
        else
        {
            return false;
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



            while ((next <= 0 && next > -5) || next == checkSlopeNumber )//1층 또는 1층 파르페 또는 올라갈수있는 슬로프
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

						actionnum = 1;  // slop 올라가려는데 다른 캐릭터 있으면 stop
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

                if ((next == 2 || next < -4) && state == State.Master)//2층 또는 2층 파르페 이고 내가 마스터
                {
                    Debug.Log("state change   " + posZ + "," + posX);
                    stateChange = true;
                }


                Debug.Log("set pos");

                targetPos = new Vector3(posX, y1, posZ);
				//SetPlayerMarker();
				if (next == 5)
					actionnum = 4;  // 다른 플레이어랑 부딪혀 bump (1층에서)
				else
					actionnum = 3;  // 벽이랑 부딪혀 crash (1층에서)
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

						actionnum = 1;	// slop 내려가는데 다른 캐릭터 있으면 stop
                        isMoving = true;

                        

                        return;
                    }
                    upstair = false;
                    break;//While break
                }

                
                check[posZ, posX] = true;


                if (map[posZ, posX] <= 0 && map[posZ,posX] > -5)//다음이 1층 또는 1층 파르페면 드p
                {
                    Debug.Log("drop");
                    upstair = false;
                    targetPos = new Vector3(posX, y1, posZ);

					Debug.Log("drop animation 없음");
					actionnum = 5;	// 2층에서 1층으로 떨어지면 drop animation 추가 필요
					isMoving = true;

                    //Debug.Log("target position in drop : " + targetPos);

                    return;

                }


                if (third_drop)
                {
                    upstair = true;
                    Debug.Log("third floor drop... mean slave drop in second floor");
                    targetPos = new Vector3(posX, y2, posZ);

					actionnum = 5;
					isMoving = true;

                    third_drop = false;

                    return;
                }


                if (map[posZ, posX] == 5 && !other.upstair && state == State.Idle)//next block is other player and stay first floor..
                {

                    targetPos = new Vector3(posX, y2, posZ);

					actionnum = 2;	// 다른 캐릭터에게 업힐 때 ride
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
				{
					targetPos = new Vector3(posX, y2, posZ);
					if (next == 5)
						actionnum = 4;  // 다른 플레이어랑 부딪혀 bump (2층에서)
					else
						actionnum = 3;  // 벽이랑 부딪혀 crash (2층에서)
				}
                    
                else
                {
                    Debug.Log("??????");
                    third_drop = false;
                    targetPos = new Vector3(posX, y2+1, posZ);
					actionnum = 0;
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
        //stone = false;
        
        //Debug.Log("target position in Up Method : " + targetPos);
    }

    void Right_2()
    {
        //Debug.Log(" right move!");
        transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        dir = Vector3.right;

        CheckMove(1);
        //stone = false;
        //Debug.Log("target position in Right Method : " + targetPos);
    }


    void Down_2()
    {
        //Debug.Log(" down move!");
        transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        dir = Vector3.back;

        CheckMove(2);
        //stone = false;
        Debug.Log("target position in Down Method : " + targetPos);
    }


    void Left_2()
    {
        //Debug.Log(" left move!");
        transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
        dir = Vector3.left;


        CheckMove(3);
        //stone = false;
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

	private void BumpParticleControl()
	{
		bumpParticle.SetActive(false);
	}
}
