using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestCode : MonoBehaviour //player test code
{
    int direction = -1; // 0 up 1 right 2 down 3 left
    bool upstair = false; // false : 1 floor, true : 2 floor
    int posX, posZ; // X - horizontal Z - vertical
    int parfaitOrder = 0; // parfait step 0~4(clear)
    int[,] map;
    Vector3 targetPosition = default;

    public List<int> through;
    public List<int> stop;

    [SerializeField]
    Player other;

    public enum State
    {
        Idle,//no interaction
        Master,//in interaction and state is master
        Slave//in interaction and state is slave...
    }
    public State state;


    void CharacterMove()
    {
        SettingBlockLevel();

        CompareWithNextBlock();
    }

    void CompareWithNextBlock()
    {
        float firstFloorY = -9.5f;//animation character modification.. 5/13
        float secondFloorY = -8.5f;//
        int[,] step = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        int next = map[posZ + step[direction,0] ,  posX + step[direction,1] ];
        int nextnext = map[posZ + (step[direction, 0] * 2), posX + (step[direction, 1] * 2)];

        if(IsThrough(next) && ThroughCheckChangeState(next, nextnext))// next block is through block
        {
           
            posZ += step[direction, 0];
            posX += step[direction, 1];
            //check[posZ, posX] = true;
            //if (SetEndPoint_Paint() && !stage.parfait)
            //    break;
            CompareWithNextBlock();
            return;
            
           
        }
        else if(IsStop(next))// next block is stop block
        {

            StopCheckChangeState(next);

            posZ += step[direction, 0];
            posX += step[direction, 1];
            //check[posZ, posX] = true;
            //if (SetEndPoint_Paint() && !stage.parfait)
            //    break;
        }
        else// next block is cant block
        {
            if((!upstair && next == BlockNumber.character) || (upstair && next == BlockNumber.upperCharacter))
            {
                //actionnum = 4;
            }
            else if(!upstair && state == State.Master)
            {
                //state change
                //other character move
            }
            else
            {
                //actionnum = 3;
            }
        }


        if (upstair)
        {
            targetPosition = new Vector3(posX, secondFloorY, posZ);
        }
        else
        {
            targetPosition = new Vector3(posX, firstFloorY, posZ);
        }
        //isMoving = true;
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
    bool ThroughCheckChangeState(int next , int nextnext)//next block is throughlevel and check if block is slopeBlock?
    {

        //through
        if(upstair && next >= BlockNumber.slopeUp && next <= BlockNumber.slopeLeft)
        {
            upstair = false;
            SettingBlockLevel();

            //if next block is slope block(can through)
            if (IsThrough(nextnext) || IsStop(nextnext))// next next block is can position
            {
                return true;
            }
            else//cant through slope block because next next block cannot position
            {
                upstair = true;
                SettingBlockLevel();
                //actionnum = 1;
                return false;
            }

            
        }
        else if(!upstair && next >= BlockNumber.slopeUp && next <= BlockNumber.slopeLeft)
        {
            upstair = true;
            SettingBlockLevel();

            if (IsThrough(nextnext) || IsStop(nextnext))
            {
                return true;
            }
            else
            {
                upstair = false;
                SettingBlockLevel();
                return false;
            }
        }
        else
        {
            return true;
        }


    }
    void StopCheckChangeState(int next)
    {
        //stop
        if (upstair && next == BlockNumber.character)
        {
            //state = State.Slave;
            //other.state = State.Master;
            //character master/slave
            //actionnum = 2; //ride motion
        }
        else if (upstair &&
            (next == BlockNumber.normal || next == BlockNumber.cracked
            || next == BlockNumber.parfaitA + parfaitOrder))
        {
            upstair = false;
            //actionnum = 5; // drop motion
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

        if(upstair)
        {
            switch(direction)
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
            through.Add(BlockNumber.slopeUp + direction);
            through.Add(BlockNumber.parfaitA + parfaitOrder);
        }

        
    }
    void StopSetting()
    {
        stop.Clear();

        if(upstair)
        {
            stop.AddRange(BlockNumber.firstlevel);//normal , cracked
            stop.Add(BlockNumber.parfaitA + parfaitOrder);


            stop.Add(BlockNumber.character);
            for(int i = 0; i <= 3; i++)
            {
                if(Mathf.Abs(direction -i) != 2)
                {
                    stop.Add(BlockNumber.upperCloudUp +i );
                }
            }
        }
        else
        {
            for (int i = 0; i <= 3; i++)
            {
                if (Mathf.Abs(direction - i) != 2)
                {
                    stop.Add(BlockNumber.cloudUp + i);
                }
            }
        }
    }


}
