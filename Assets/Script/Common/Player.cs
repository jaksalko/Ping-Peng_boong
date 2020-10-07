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
    [Header("Character Infomation")]
    public float speed;
    int posZ;     // vertical
    int posX;     // horizental
    public int moveCount = 0;
    bool isPlayingParticle = false;
    public int actionnum;
    public int parfaitOrder = 0; // parfait step 0~4(clear)
    int getDirection = -1;

    //public Text moveCountText;

    [Header("Map Infomation")]
    Map stage;
    int mapsizeH;
    int mapsizeW;
    int[,] map;
    
    bool[,] check;
    public List<int> through;
    public List<int> stop;
    [SerializeField]
    int temp;
    int total = 0;


    [Header("Character State")]
    [SerializeField]
    public bool isMoving = false;
    [SerializeField]
    bool upstair = false;//if player located in second floor --> true
    [SerializeField]
    State state;
    [SerializeField]
    bool stateChange;
    [SerializeField]
    bool thirdFloor;
    [SerializeField]
    public bool onCloud;
    [SerializeField]
    public bool isActive = false;

	[Header("Character Sound")]
	public AudioClip crashSound;
	public AudioClip departureSound;
	public AudioClip fallSound;
	public AudioClip slideSound;
	private AudioSource playerAudio;
	private bool isSlideSoundPlaying;

	public enum State
    {
        Idle,//no interaction
        Master,//in interaction and state is master
        Slave//in interaction and state is slave...
    }
    public bool Moving()
    {
        return isMoving;
    }

    [Header("Character Particle System")]
    public ParticleSystem moveParticle;
	public ParticleSystem crashParticle;
	public GameObject bumpParticle;
    public Animator animator;
    
	
    CharacterController cc;
    Vector3[] dir = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    int[,] step = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
    [SerializeField]
    Player other;

    Vector3 targetPos;



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
//        Debug.Log("Animation End...");
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
        thirdFloor = false;

        isMoving = false;
        onCloud = false;
      

        cc = GetComponent<CharacterController>();

        if(!simulating)
        {
            stateMachine = animator.GetBehaviour<CheckAnimationState>();
            stateMachine.player = this;
            stateMachine.ActionEnd += AnimationEnd;
        }

        total = RemainCheck();

		playerAudio = GetComponent<AudioSource>();

		/*
        map.ObserveEveryValueChanged(data => map[posZ, posX])
            .Subscribe(_ => Debug.Log("change my position :" + posZ + "," + posX +" : " + _));

        map.ObserveEveryValueChanged(data => map[other.posZ, other.posX])
        .Subscribe(_ => Debug.Log("change other position :" + other.posZ + "," + other.posX + " : " + _));
        */


#if UNITY_EDITOR
		var mouseDownStream = this.UpdateAsObservable()
                .Where(_ => !EventSystem.current.IsPointerOverGameObject())
               
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
    
        

        //Reactive stream


    }


    void PlayerMove()
    {
        //Debug.Log("distance : " + Vector2.Distance(up, down));
        bool isMove = false;

        if(Vector2.Distance(up, down) <= 30)//민감도
        {
            return;
        }
        Vector2 normalized = (up - down).normalized;
        

        if (normalized.x < -0.5)
        {
            isMove = PlayerControl(3); //left
        }
        else if (normalized.x > 0.5)
        {
            isMove = PlayerControl(1); //right
        }
        else
        {
            if (normalized.y > 0)
            {
                isMove = PlayerControl(0); //up

            }
            else
            {
                isMove = PlayerControl(2); // down
            }

        }

        if (isMove)
            moveCount++;


        
    }
    bool PlayerControl(int direction)//0 : u // 1 : r // 2 : d // 3 : l
    {
        if (!simulating && !GameController.Playing)
            return false;

        if (isActive && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {

            getDirection = direction;

            if (state == State.Slave)
            {
                Debug.Log("slave move");
                state = State.Idle;
                other.state = State.Idle;
                transform.SetParent(null);

                if(other.upstair)
                {
                    thirdFloor = true;
                }

				playerAudio.Stop();

			}

            Debug.Log("move direction : " + getDirection);
            transform.rotation = Quaternion.Euler(new Vector3(0f, getDirection * 90 , 0f));

            CharacterMove();

            return true;

        }
        else
        {
            //getDirection = -1;
            return false;
        }




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

        if (map == null)
            Debug.Log("null exception");

        temp = map[posZ, posX];
        if (upstair)
            map[posZ, posX] = BlockNumber.upperCharacter;
        else
            map[posZ, posX] = BlockNumber.character;

        Debug.Log(map[posZ, posX] + "," + other.map[posZ, posX]);

    

        check[posZ, posX] = true;
        if(!simulating)
            GameController.instance.ui.SetRemainText(RemainCheck(), total);

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
                cc.Move(speed * Time.deltaTime * dir[getDirection]);
            }
            else				// 바닥이 없으면 떨어짐 (여기다 쿵! 넣으면되는데 지금 잘 작동이 안 되서 넣으면 안 됨)
            {
                //Debug.Log("is not grounded!!!!");
                cc.Move(speed * Time.deltaTime * Vector3.down);
            }
            float distance = Vector3.Distance(transform.position, targetPos);
            
            if (distance < 0.25f)//arrive condition
            {
                /*if(state == State.Master)
                {
                    other.posX = posX;
                    other.posZ = posZ;

                    
                }*/
                temp = map[posZ, posX];
               
                if (upstair)
                    map[posZ, posX] = BlockNumber.upperCharacter;
                else
                    map[posZ, posX] = BlockNumber.character;

                if (state == State.Master)
                {
                    other.posX = posX;
                    other.posZ = posZ;

                    other.temp = map[posZ, posX];
                }
                Debug.Log("arrive target position...temp : " + temp);
                Debug.Log("other temp : " + other.temp);
                Debug.Log(map[posZ, posX] + "," +other.posX + "," +other.posZ +":"+ other.map[other.posZ, other.posX]);

                /*
                     if (other.upstair)
                         map[other.posZ, other.posX] = BlockNumber.upperCharacter;
                     else
                         map[other.posZ, other.posX] = BlockNumber.character;

               */



                
                animator.SetBool("move", false);
                Debug.Log("Arrive... target position : " + targetPos + "  distance : " + distance);
               
                //isMoving = false;

				//이동 시 발생하는 particle control
				moveParticle.loop = false;

				transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);

                if(RemainCheck() == 0)
                {
                    if (!simulating)
                        GameController.instance.GameEnd(true);
                    else
                        Simulator.instance.SimulatingSuccess();
                }
                else
                {
                    if (!simulating)
                        GameController.instance.ui.SetRemainText(RemainCheck(), total);
                }
                
                /*if (CheckStageClear())
                {
                    if (!simulating)
                        GameController.instance.GameEnd(true);
                    else
                        Simulator.instance.SimulatingSuccess();
                }*/

                if (stateChange)//state == master !upstair 에서 (갈수없는 블럭을 제외한)2층의 블럭과 부딪히면 true
                {
                    Debug.Log("state change");

                    state = State.Idle;
                    other.state = State.Idle;
                    stateChange = false;
                    other.transform.SetParent(null);

                    isMoving = false;

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
//                        Debug.Log("slave master");

                        if(transform.position.y > other.transform.position.y)//움직인 놈이 더 위에 있다
                        {
                            state = State.Slave;
                            other.state = State.Master;
                            transform.SetParent(other.transform);

                            isMoving = false;


                            if (!simulating)
                                GameController.instance.ui.ChangeCharacter();
                            else
                                Simulator.instance.ChangeCharacter();

                         

                        }
                        else//움직인 놈이 더 아래에 있다.
                        {
                            state = State.Master;
                            other.state = State.Slave;
                            other.transform.SetParent(transform);

                          
                        
                        }

                       
                    }
                  

                   
                }


                isMoving = false;

                if(other.onCloud && other.temp == temp)
                {
                    if(upstair)
                    {
                        if (map[posZ + step[getDirection, 0], posX + step[getDirection, 1]] == BlockNumber.upperCharacter)
                        {
                            Debug.Log(name + " connect Action with method");
                            CloudBlock.Exit += CallBackOtherExitCloud;
                        }
                    }
                    else
                    {
                        if(map[posZ + step[getDirection, 0], posX + step[getDirection, 1]] == BlockNumber.character)
                        {
                            Debug.Log(name + " connect Action with method");
                            CloudBlock.Exit += CallBackOtherExitCloud;
                        }
                    }
                }

            }
        }
        

    }
    void CallBackOtherExitCloud(GameObject player)
    {
        Debug.Log(gameObject + "/" +player + " disconnect Action with method");
        CloudBlock.Exit -= CallBackOtherExitCloud;

        if (player == gameObject)
        {
            Debug.Log("same player");
            //nothing
        }
        else
        {
            Debug.Log("different player");
            onCloud = true;//블록 설정을 위한 input (stay 에서 호출하지 않으므로 설정)
            MoveByCloud(temp);
        }
       
    }
    private void LateUpdate()
    {
        animator.SetBool("move", isMoving);
        if (isMoving)
        {
			if (actionnum == 5)//drop
            {
				if (!playerAudio.isPlaying)
				{
					playerAudio.loop = true;
					playerAudio.clip = slideSound;

					playerAudio.Play();
					isSlideSoundPlaying = true;
				}

				float distance = Vector3.Distance(transform.position, targetPos + new Vector3(0, 1, 0));
                if (distance < 1f)//거리가 1일때부터 드랍 애니메이션 실행 , 움직이고 있던상태에서 애니메이션 실행
                {
                    animator.SetInteger("action", actionnum);

					if (isSlideSoundPlaying)
					{
						playerAudio.Stop();
						isSlideSoundPlaying = false;
					}
					if (!playerAudio.isPlaying)
					{
						playerAudio.loop = false;
						playerAudio.clip = fallSound;

						playerAudio.Play();
					}
				}
            }
			else
			{
				if (!playerAudio.isPlaying)
				{
					playerAudio.loop = true;
					playerAudio.clip = slideSound;

					playerAudio.Play();
					isSlideSoundPlaying = true;
				}
			}
            
        }
        else
        {
			if(isSlideSoundPlaying)
			{
				playerAudio.Stop();
				isSlideSoundPlaying = false;
			}

			if (actionnum !=5)//이미 전에 실행해서 드랍만 예외처리
			    animator.SetInteger("action", actionnum);
            
			//이동 시 발생하는 particle control
			switch(actionnum)
			{
				case 3:
					if (!isPlayingParticle)
					{
//						Debug.Log("play crash particle");
						// crashParticle.Play();
						isPlayingParticle = true;
					}

					break;
				case 4:
					if (!isPlayingParticle)
					{
//						Debug.Log("play bump particle");
						bumpParticle.SetActive(true);
						Invoke("BumpParticleControl", 4.5f);
						//bumpParticle.Play();
						isPlayingParticle = true;
					}

					if (!playerAudio.isPlaying)
					{
						playerAudio.loop = false;
						playerAudio.clip = crashSound;

						playerAudio.Play();
					}
					break;
				default:
					break;
			}
			
		}


    }

    int RemainCheck()
    {
        int remain = 0;
        for (int i = 0; i < mapsizeH; i++)
        {
            for (int j = 0; j < mapsizeW; j++)
            {
                if(!check[i,j])
                {
                    remain++;
                }
            }
        }

        return remain;
    }

    bool CheckStageClear()
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

   
    public void CrackedBlockisBroken(int x , int z , int block_num)
    {
        map[z, x] = block_num;
        other.map[z, x] = block_num;
        Debug.Log(block_num + " is changed!");
    }


    public void MoveByCloud(int block_num)
    {
        
        Debug.Log("Moved by Cloud");
        
        getDirection = (block_num % 10) - 1;

        through.Clear();
        through.Add(block_num);

        if (upstair)
        {
            switch (getDirection)
            {
                case 0:
                    through.Add(BlockNumber.slopeDown);
                    break;
                case 1:
                    through.Add(BlockNumber.slopeLeft);
                    break;
                case 2:
                    through.Add(BlockNumber.slopeUp);
                    break;
                case 3:
                    through.Add(BlockNumber.slopeRight);
                    break;
            }
          
        }
        else
        {
           
            through.Add(BlockNumber.slopeUp + getDirection);
           
        }
        StopSetting();

        
        map[posZ, posX] = temp;//set initializedMap data

        

        CompareWithNextBlock();



      

        //set targetposition
        //isMoving = true;

    }

    void CharacterMove()
    {
        SettingBlockLevel();

        
        map[posZ, posX] = temp;//set initializedMap data

        CompareWithNextBlock();
    }

    void CompareWithNextBlock()
    {
        float firstFloorY = -9.5f;//animation character modification.. 5/13
        float secondFloorY = -8.5f;//
        float thirdFloorY = -7.5f;
     

        int next = map[posZ + step[getDirection, 0], posX + step[getDirection, 1]];
        

        if (IsThrough(next) && ThroughCheckChangeState(next))// next block is through block
        {

            posZ += step[getDirection, 0];
            posX += step[getDirection, 1];
            check[posZ, posX] = true;

            if (!SetEndPoint_Paint())
            {
                CompareWithNextBlock();
                return;
            }
            
                
           
            


        }
        else if (IsStop(next))// next block is stop block
        {

            StopCheckChangeState(next);

            posZ += step[getDirection, 0];
            posX += step[getDirection, 1];
            check[posZ, posX] = true;
            
        }
        else// next block is cant block
        {
            if ((!upstair && next == BlockNumber.character) || (upstair && next == BlockNumber.upperCharacter))
            {
                actionnum = 4;//캐릭터//bump
            }
            else if (!upstair && state == State.Master/* && (next >= BlockNumber.upperNormal && next <= BlockNumber.upperParfaitD)*/ )
            {
                stateChange = true;
                actionnum = 3;//crash
                //state change
                //other character move
            }
            else
            {
                actionnum = 3;//crash
            }

           

           
        }

        if(thirdFloor)
        {
            targetPos = new Vector3(posX, thirdFloorY, posZ);
        }
        else if (upstair)
        {
            targetPos = new Vector3(posX, secondFloorY, posZ);
        }
        else
        {
            targetPos = new Vector3(posX, firstFloorY, posZ);
        }
        Debug.Log("targetposition : " + targetPos);
        isMoving = true;
        //cant go 



    }
    bool IsThrough(int next)
    {
        for (int i = 0; i < through.Count; i++)
        {
            if (through[i] == next)
                return true;

        }

        return false;

    }
    bool IsStop(int next)
    {
        for (int i = 0; i < stop.Count; i++)
        {
            if (stop[i] == next)
                return true;
        }

        return false;
    }
    bool ThroughCheckChangeState(int next)//next block is throughlevel and check if block is slopeBlock?
    {

      
        int nextnext = map[posZ + (step[getDirection, 0] * 2), posX + (step[getDirection, 1] * 2)];
        //int next = map[posZ + step[getDirection, 0], posX + step[getDirection, 1]];
        //meet parfait block
        if (!upstair)
        {
            if(next >= BlockNumber.parfaitA && next <= BlockNumber.parfaitD)
            {
                map[posZ + step[getDirection, 0], posX + step[getDirection, 1]] = BlockNumber.normal;
                parfaitOrder++;
                other.parfaitOrder++;
                SettingBlockLevel();
            }
        }
        else
        {
            if(next >= BlockNumber.upperParfaitA && next <= BlockNumber.upperParfaitD)
            {
                map[posZ + step[getDirection, 0], posX + step[getDirection, 1]] = BlockNumber.upperNormal;
                parfaitOrder++;
                other.parfaitOrder++;
                SettingBlockLevel();

            }
        }

        //through
        if (upstair && next >= BlockNumber.slopeUp && next <= BlockNumber.slopeLeft)
        {
            upstair = false;
            if(state == State.Master)
            {
                other.thirdFloor = false;
            }
            SettingBlockLevel();

            //if next block is slope block(can through)
            if (IsThrough(nextnext) || IsStop(nextnext))// next next block is can position
            {
                return true;
            }
            else//cant through slope block because next next block cannot position
            {
                upstair = true;
                if (state == State.Master)
                {
                    other.thirdFloor = true;
                }
                SettingBlockLevel();
                //actionnum = 1;
                return false;
            }


        }
        else if (!upstair && next >= BlockNumber.slopeUp && next <= BlockNumber.slopeLeft)
        {
            upstair = true;
            if (state == State.Master)
            {
                other.thirdFloor = true;
            }
            SettingBlockLevel();

            if (IsThrough(nextnext) || IsStop(nextnext))
            {
                return true;
            }
            else
            {
                upstair = false;
                if (state == State.Master)
                {
                    other.thirdFloor = false;
                }
                SettingBlockLevel();
                return false;
            }
        }
        else
        {
            return true;
        }


    }
    void StopCheckChangeState(int next)//cloud or ride or drop
    {
        //stop


        if (upstair && next == BlockNumber.character)
        {

            actionnum = 2; //ride motion
            Debug.Log("master slave!");
        }
        else if (upstair &&
            (next == BlockNumber.normal || next == BlockNumber.cracked
            || next == BlockNumber.parfaitA + parfaitOrder || (next >= BlockNumber.cloudUp && next <= BlockNumber.cloudLeft)))
        {
            upstair = false;
            thirdFloor = false;
            other.thirdFloor = false;
            actionnum = 5; // drop motion

            if(next == BlockNumber.parfaitA + parfaitOrder)
            {
                map[posZ + step[getDirection, 0], posX + step[getDirection, 1]] = BlockNumber.normal;
                parfaitOrder++;
                other.parfaitOrder++;
            }
        }
        else if (thirdFloor &&
            (next == BlockNumber.upperNormal || next == BlockNumber.upperCracked ||
            next == BlockNumber.upperParfaitA + parfaitOrder ||
            (next >= BlockNumber.upperCloudUp && next <= BlockNumber.upperCloudLeft))
            )
        {
            if (next == BlockNumber.upperParfaitA + parfaitOrder)
            {
                map[posZ + step[getDirection, 0], posX + step[getDirection, 1]] = BlockNumber.upperNormal;
                parfaitOrder++;
                other.parfaitOrder++;
            }


            thirdFloor = false;
            other.thirdFloor = false;
            actionnum = 5;
        }
    }
    void SettingBlockLevel()//player state change callback method
    {
        ThroughSetting();
        StopSetting();
    }

    void ThroughSetting()
    {
        through.Clear();

        if(thirdFloor)
        {
            //nothing to add through list
        }
        else if (upstair)
        {
            switch (getDirection)
            {
                case 0:
                    through.Add(BlockNumber.slopeDown);
                    break;
                case 1:
                    through.Add(BlockNumber.slopeLeft);
                    break;
                case 2:
                    through.Add(BlockNumber.slopeUp);
                    break;
                case 3:
                    through.Add(BlockNumber.slopeRight);
                    break;
            }
            through.AddRange(BlockNumber.secondLevel);
            through.Add(BlockNumber.upperParfaitA + parfaitOrder);
        }
        else
        {
            through.AddRange(BlockNumber.firstlevel);
            through.Add(BlockNumber.slopeUp + getDirection);
            through.Add(BlockNumber.parfaitA + parfaitOrder);
        }


    }
    void StopSetting()
    {
        stop.Clear();

        if(thirdFloor)
        {
            stop.AddRange(BlockNumber.secondLevel);
            stop.AddRange(BlockNumber.firstlevel);

            stop.Add(BlockNumber.parfaitA + parfaitOrder);//first floor parfait
            stop.Add(BlockNumber.upperParfaitA + parfaitOrder);//first floor parfait

            for (int i = 0; i <= 3; i++)
            {
                if (Mathf.Abs(getDirection - i) != 2)
                {
                    stop.Add(BlockNumber.upperCloudUp + i);
                    stop.Add(BlockNumber.cloudUp + i);
                }
            }


        }
        else if (upstair)
        {
            if(onCloud)
            {
                stop.AddRange(BlockNumber.secondLevel);
                stop.Add(BlockNumber.upperParfaitA + parfaitOrder);
            }

            stop.AddRange(BlockNumber.firstlevel);//normal , cracked
            stop.Add(BlockNumber.parfaitA + parfaitOrder);//first floor parfait



            stop.Add(BlockNumber.character);
            
            for (int i = 0; i <= 3; i++)
            {
                if (Mathf.Abs(getDirection - i) != 2)
                {
                    stop.Add(BlockNumber.upperCloudUp + i);
                    stop.Add(BlockNumber.cloudUp + i);
                }
            }
        }
        else
        {
            if(onCloud)
            {
                stop.AddRange(BlockNumber.firstlevel);
                stop.Add(BlockNumber.parfaitA + parfaitOrder);
            }
            for (int i = 0; i <= 3; i++)
            {
                if (Mathf.Abs(getDirection - i) != 2)
                {
                    stop.Add(BlockNumber.cloudUp + i);
                }
            }
        }
    }














  

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Parfait"))
        {
            ParfaitObject parfait = collider.GetComponent<ParfaitObject>();
            if (parfait.state == ParfaitObject.State.active)
            {
                if (!simulating)
                    GameController.instance.ui.ParfaitDone();
                if (parfait.GetParfait(stage))//if true end game
                {
                    Debug.Log("end parfait mode.");
                   
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
