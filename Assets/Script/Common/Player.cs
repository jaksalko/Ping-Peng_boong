using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour , IMoveable
{

    public enum State
    {
        Idle,//no interaction
        Master,//in interaction and state is master
        Slave//in interaction and state is slave...
    }
    Vector3[] dir = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

    [SerializeField]
    CharacterController cc;
    CheckAnimationState stateMachine;

    [Header("Character Infomation")]
    public float speed;   
    bool isPlayingParticle = false;

    public int actionnum;   
    public int getDirection = -1;
    public int direction = -1;

    Vector3 targetPos;
    

    [Header("Character State")]
    [SerializeField]
    bool isMoving = false; public bool Moving() { return isMoving; }
    [SerializeField]
    public bool isActive = false;

   
    [SerializeField] 
    public State state { get; set; }    
    [SerializeField]
    public bool onCloud = false;
    public bool isLock = false;
    public int temp;

    [Header("Character Sound")]
	public AudioClip crashSound;
	public AudioClip departureSound;
	public AudioClip fallSound;
	public AudioClip slideSound;
    [SerializeField]
	AudioSource playerAudio;
	private bool isSlideSoundPlaying;

    [Header("Character Particle System")]
    public ParticleSystem moveParticle;
	public ParticleSystem crashParticle;
	public GameObject bumpParticle;
    public Animator animator;
    
	
    
    [SerializeField]
    Player other;
    
    [SerializeField]
    bool simulating;

    

    void AnimationEnd()
    {
        //Debug.Log("Animation End...");
        animator.SetInteger("action", 0);
        actionnum = 0;

	}
  
    
   
   
    void Start()
    {
        state = State.Idle;


        cc = GetComponent<CharacterController>();
        playerAudio = GetComponent<AudioSource>();
        stateMachine = animator.GetBehaviour<CheckAnimationState>();
        stateMachine.player = this;
        stateMachine.ActionEnd += AnimationEnd;
  


    }
    public void Move(Map map, int direction)//call by GameController Command
    {
        //if (!simulating && !GameController.Playing)
        //return false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))//remove this
        {

            this.direction = getDirection = direction;

            if (state == State.Slave)
            {
                Debug.Log("slave move");
                state = State.Idle;
                other.state = State.Idle;
                transform.SetParent(null);

                /*if (other.upstair)
                {
                    //thirdFloor = true;
                }*/

                playerAudio.Stop();

            }

            Debug.Log("move direction : " + direction);
            transform.rotation = Quaternion.Euler(new Vector3(0f, direction * 90, 0f));

            //CharacterMove(map.map);
            
            map.SetBlockData((int)transform.position.x, (int)transform.position.z, temp);
            targetPos =  map.GetDestination(this, transform.position);
            Debug.Log("target position : " + targetPos);
            isMoving = true;
        }
    }

    /*void CharacterMove(Map map)
    {
        map.map[posZ, posX] = temp;//set initializedMap data

        SettingBlockLevel();//stop , through block list setting
        CompareWithNextBlock(map); // set target position ... 재
    }*/




    public void SetPosition(Vector3 startpos)
    {
        transform.position = startpos;

        switch(startpos.y)
        {
            case 0:
                temp = BlockNumber.normal;
                break;
                
            case 1:
                temp = BlockNumber.upperNormal;
                break;
        }
        
    }

    public void FindPlayer()
    {
        

      

        /*if (upstair)
            map[posZ, posX] = BlockNumber.upperCharacter;
        else
            map[posZ, posX] = BlockNumber.character;

        Debug.Log(map[posZ, posX] + "," + other.map[posZ, posX]);*/

    

        //check[posZ, posX] = true;//이미 맵 생성할때 체크 트루로 해둠 근데 데이터는 노멀블럭 또는 위노멀블
        /*if(!simulating)
        {
            Debug.Log("check remain : " + RemainCheck());
            GameController.instance.ui.SetRemainText(RemainCheck(), total);

        }*/

        //        Debug.Log(gameObject.name + "   Vertical : " + posZ + " Horizental : " + posX + "5 mark : " + map[posZ,posX]);
    }

    void CharacterControllerMovement()
    {


        if (cc.isGrounded)  // 바닥에 붙어있으면 움직임
        {
            Debug.Log("dir : " + direction);
            cc.Move(speed * Time.deltaTime * dir[direction]);
        }
        else                // 바닥이 없으면 떨어짐 (여기다 쿵! 넣으면되는데 지금 잘 작동이 안 되서 넣으면 안 됨)
        {
            //Debug.Log("is not grounded!!!!");
            cc.Move(speed * Time.deltaTime * Vector3.down);
        }

        if (direction % 2 == 0)//vector.forward , vector.back ==> z 움직임 
        {
           

            int x = Mathf.RoundToInt(transform.position.x);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            Debug.Log("X : " + transform.position.x);
        }
        else
        {
           

            int z = Mathf.RoundToInt(transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
            Debug.Log("Z : " + transform.position.z);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!simulating && !GameController.Playing)
            return;
        if (simulating && !Simulator.Running)
            return;

        if (isMoving)
        {
           
            CharacterControllerMovement();

            float distance = Vector3.Distance(transform.position, targetPos);
            
            if (distance < 0.25f)//arrive condition
            {
                transform.position = targetPos;
                isMoving = false;
                //temp and block data change in map script(SetDestination end...) 

                //맵 데이터 치환
                //temp = map[posZ, posX];

                /*
                if (upstair)
                    map[posZ, posX] = BlockNumber.upperCharacter;
                else
                    map[posZ, posX] = BlockNumber.character;
                */

                //상태 변경
                

               
                
                animator.SetBool("move", false);
                Debug.Log("Arrive... target position : " + targetPos + "  distance : " + distance);
               
               

				//이동 시 발생하는 particle control
				moveParticle.loop = false;

                GameController.instance.RemainCheck();

                /*if(RemainCheck() == 0)
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
                }*/

                if (state == State.Master)
                {
                    other.temp = transform.position.y == 0 ? BlockNumber.character : BlockNumber.upperCharacter;

                    //change character?

                    //other == State.Slave
                    other.Move(GameController.instance.GetMap(), direction);
                    //둘 다 Idle로 변경됨.
                }
                else//Idle 상태 - slave는 움직일때 풀림
                {
                    Vector3 otherPos = other.transform.position;
                    Vector3 myPos = transform.position;
                    //master 에서 other.move로 idle상태로 other이 움직이기 때문에 모든 move는 else로 들어오게 되어있음.
                    //Other에서 불리느냐 this에서 불리느냐의 차이
                    if(otherPos.x == myPos.x && otherPos.z == myPos.z)
                    {
                        if(myPos.y < otherPos.y)
                        {
                            state = State.Master;
                            other.state = State.Slave;

                            other.transform.SetParent(transform);
                        }
                        else
                        {
                            state = State.Slave;
                            other.state = State.Master;

                            transform.SetParent(other.transform);
                        }
                    }
                    else if(other.isLock && otherPos + dir[other.direction] != myPos)
                    {
                        other.isLock = false;
                        other.Move(GameController.instance.GetMap(), other.direction);
                    }
                }

                /*상태 변경
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
                */



                /*
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
                 */
            }
        }
        

    }


    /*void CallBackOtherExitCloud(GameObject player)
    {
        Debug.Log(gameObject + "/" +player + " disconnect Action with method");
        CloudBlock.Exit -= CallBackOtherExitCloud;

        if (player == gameObject)//나는 안움직여 
        {
            Debug.Log("same player");
            //nothing
        }
        else//뒤에 있던 캐릭터만 움직
        {
            Debug.Log("different player");
            onCloud = true;//블록 설정을 위한 input (stay 에서 호출하지 않으므로 설정)

            MoveByCloud(temp);
        }
       
    }*/

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
                    animator.SetInteger("action", actionnum);//actionnum = 5 drop...

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
        else//isMoving == false
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



    /*public void MoveByCloud(int block_num)
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

    }*/


  



    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Parfait"))
        {
            ParfaitBlock parfait = collider.GetComponent<ParfaitBlock>();
            if (parfait.state == ParfaitBlock.State.active)
            {
                if (!simulating)
                {
                    GameController.instance.ui.ParfaitDone();
                }
                parfait.ActiveNextParfait();

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
