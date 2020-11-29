﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




[Serializable]
public class Map : MonoBehaviour, IMap
{
    int[,] step;
    public int mapsizeH;
    public int mapsizeW;

    public bool parfait = false;


    public Vector3 startPositionA;//    y축 -9 : 1 층 , -8 : 2층 
    public Vector3 startPositionB;

    public bool startUpstairA = false;
    public bool startUpstairB = false;

    [Serializable]
    public class Line
    {
        public List<int> line = new List<int>();
    }

    public List<Line> lines = new List<Line>();

    public int[,] map;//not use this variable
    public bool[,] check;

    Block[,] blocks;

    public List<Vector2> snowList;
    public List<KeyValuePair<Vector2, int>> crackerList;

    private void Awake()
    {
        Debug.Log("Activate");
        step = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        blocks = new Block[mapsizeH, mapsizeW];//로더의 MakeGround에서 채워짐
        check = new bool[mapsizeH, mapsizeW];//마찬가지

        snowList = new List<Vector2>();
        crackerList = new List<KeyValuePair<Vector2, int>>();
    }

    public Map(Vector2 size, bool isParfait, Vector3 posA, Vector3 posB, int[,] datas)//로더에서 생성
    {
        mapsizeH = (int)size.x;
        mapsizeW = (int)size.y;
        parfait = isParfait;
        startPositionA = posA;
        startPositionB = posB;

        startUpstairA = startPositionA.y > -9 ? true : false;//안쓸거임
        startUpstairB = startPositionB.y > -9 ? true : false;//안쓸거임


        map = datas;//안쓸거임

        //blocks = new Block[mapsizeH, mapsizeW];//로더의 MakeGround에서 채워짐
        //check = new bool[mapsizeH, mapsizeW];//마찬가지
        //check?

    }






    public void LineToMap()
    {
        map = new int[mapsizeH, mapsizeW];
        for (int i = 0; i < mapsizeH; i++)
        {
            for (int j = 0; j < mapsizeW; j++)
            {
                map[i, j] = lines[i].line[j];
            }
        }

    }

    public void MapToLine()
    {
        int dataCount = mapsizeH * mapsizeW;
        for (int i = 0; i < mapsizeH; i++)
        {
            Line newLine = new Line();

            lines.Add(newLine);
            for (int j = 0; j < mapsizeW; j++)
            {
                lines[i].line.Add(map[i, j]);
            }
        }
    }



    public virtual void init()
    {
        LineToMap();
    }


    bool isEndGame()
    {
        for (int i = 0; i < mapsizeH; i++)
        {
            for (int j = 0; j < mapsizeW; j++)
            {
                if (!check[i, j])
                {
                    Debug.Log("is false : " + i + "," + j);
                    return false;
                }
            }
        }
        Debug.Log("end game");
        return true;
    }

    List<int> GetThroughBlockList(int floor, int getDirection, bool onCloud)
    {
        switch (floor)
        {
            case 0:
                return BlockNumber.GetDownstairThroughBlock(getDirection, onCloud);
            case 1:
                return BlockNumber.GetUpstairThroughBlock(getDirection, onCloud);
            case 2:
                return BlockNumber.GetThirdFloorThroughBlock(getDirection, onCloud);
                

        }

        return new List<int>();
    }



    List<int> GetStopBlockList(int floor, int getDirection, bool onCloud)
    {
        switch (floor)
        {
            case 0:
                return BlockNumber.GetDownstairStopBlock(getDirection, onCloud);
            case 1:
                return BlockNumber.GetUpstairStopBlock(getDirection, onCloud);
            case 2:
                return BlockNumber.GetThirdFloorStopBlock(getDirection, onCloud);

        }

        return new List<int>();
    }

    bool CheckNextBlock(List<int> checkList, int data)
    {
        for (int i = 0; i < checkList.Count; i++)
        {
            if (checkList[i] == data)
                return true;
        }
        return false;
    }

    bool ChangeState(int next, int nextnext, Player player, ref Vector3 pos)
    {

        int floor = (int)pos.y;
        int direction = player.getDirection;
        int posX = (int)pos.x;
        int posZ = (int)pos.z;

        switch (floor)
        {
            case 0:
                if (next >= BlockNumber.parfaitA && next <= BlockNumber.parfaitD)
                {
                    //change block data parfait to normal
                    blocks[posZ + step[direction, 0], posX + step[direction, 1]].Data = BlockNumber.normal; //pos 위치가 아닌 한칸 이동한 위치ㄹ
                    //parfaitorder++;
                    GameController.ParfaitOrder++;


                    return true;
                }
                else if(next >= BlockNumber.cloudUp && next <= BlockNumber.cloudLeft)
                {
                    player.getDirection = (next % 10) - 1; // cloud direction...
                    player.onCloud = true;

                    return true;
                }
                else if (next >= BlockNumber.slopeUp && next <= BlockNumber.slopeLeft)
                {
                    int nextFloor = floor + 1;
                    if (CheckNextBlock(GetThroughBlockList(nextFloor, player.getDirection, player.onCloud), nextnext) || CheckNextBlock(GetStopBlockList(nextFloor, player.getDirection, player.onCloud), nextnext))//다음은 지나갈 수 있는 블럭
                    {
                        Debug.Log("floor : 1");
                        //다음 블럭은 올라갈 수 있다
                        pos.y += 1;
                        return true;


                    }
                    else
                    {
                        Debug.Log("cant climb slope...");
                        //올라갈 수 없다 --> 슬로프를 올라가서는 안되므로 false 를 반환.
                        return false;
                    }
                    //player upstair --> true (floor = 1)
                    //if state==master --> other.floor = 2
                    //블럭 리스트 업데이트

                    //슬로프 앞에가 막혀있다면
                    //리턴 false
                    //upstair --> false (floor = 0)
                    //if state==master --> other.floor = 1
                    //다시 블럭리스트 업데이트 
                }
                else
                {
                    return true;
                }

            case 1:
                if (next >= BlockNumber.upperParfaitA && next <= BlockNumber.upperParfaitD)
                {
                    blocks[posZ + step[direction, 0], posX + step[direction, 1]].Data = BlockNumber.upperNormal;
                    GameController.ParfaitOrder++;

                    return true;
                }
                else if (next >= BlockNumber.cloudUp && next <= BlockNumber.cloudLeft)
                {
                    player.getDirection = (next % 10) - 1; // cloud direction...
                    player.onCloud = true;
                    pos.y -= 1;

                    return true;
                }
                else if (next >= BlockNumber.upperCloudUp && next <= BlockNumber.upperCloudLeft)
                {
                    player.getDirection = (next % 10) - 1; // cloud direction...
                    player.onCloud = true;

                    return true;
                }
                else if (next >= BlockNumber.slopeUp && next <= BlockNumber.slopeLeft)
                {
                    int nextFloor = floor - 1;
                    if (CheckNextBlock(GetThroughBlockList(nextFloor, player.getDirection, player.onCloud), nextnext) || CheckNextBlock(GetStopBlockList(nextFloor, player.getDirection, player.onCloud), nextnext))//다음은 지나갈 수 있는 블럭
                    {
                        //다음 블럭은 내려갈 수 있다
                        pos.y -= 1;
                        return true;

                    }
                    else
                    {
                        //내려갈 수 없다 --> 슬로프를 내력가서는 안되므로 false 를 반환.
                        return false;
                    }
                }
                else
                {
                    return true;
                }

            case 2://2층에서는 through 로 들어올 수 없음.?
                if (next >= BlockNumber.cloudUp && next <= BlockNumber.cloudLeft)
                {
                    player.getDirection = (next % 10) - 1; // cloud direction...
                    player.onCloud = true;
                    pos.y -= 2;

                    return true;
                }
                else if (next >= BlockNumber.upperCloudUp && next <= BlockNumber.upperCloudLeft)
                {
                    player.getDirection = (next % 10) - 1; // cloud direction...
                    player.onCloud = true;
                    pos.y -= 1;
                    return true;
                }
                return false;

        }

        return false;//error
    }

    
    public Vector3 GetDestination(Player player, Vector3 pos)
    {
        Debug.Log("player name : " + player.name + " position : " + pos + " player dir : " + player.getDirection);
        int direction = player.getDirection;
       
        int floor = (int)pos.y;
        int posX = (int)pos.x;
        int posZ = (int)pos.z;

        //blocks[posZ, posX].Data = player.temp;//이거 문제임 한번만 불려야 하는데... player.cs 151 line
        Debug.Log((posZ + step[direction, 0]) + "," + (posX + step[direction, 1]) + " DATA : " + GetBlockData(x: posX + step[direction, 1], z: posZ + step[direction, 0]));
        int next = GetBlockData(x: posX + step[direction, 1], z: posZ+ step[direction, 0]);
        int nextnext = GetBlockData(x: posX + step[direction, 1] * 2, z: posZ + step[direction, 0] * 2);

        if(CheckNextBlock(GetThroughBlockList(floor, direction, player.onCloud), next) && ChangeState(next, nextnext, player , ref pos))//다음은 지나갈 수 있는 블럭
        {
            //지나갈 수 있는 블럭
            
            posX += step[direction, 1];
            posZ += step[direction, 0];
            UpdateCheckTrue(width: posX, height: posZ);

            pos = new Vector3(posX, pos.y, posZ);
            //if not endpoint recursive next point
            if (!isEndGame())
                return GetDestination(player, pos);
            
            
        }
        else if (CheckNextBlock(GetStopBlockList(floor, direction, player.onCloud), next))//다음은 멈춰야하는 블럭
        {
            player.onCloud = false; // stop 이면 무조건 oncloud 에서 벗어남.

            posX += step[direction, 1];
            posZ += step[direction, 0];

            switch (floor)
            {
                case 0://솜사탕 위였으면 1단계 블럭 또는 열려있는 파르페  솜사탕 위가 아니면 솜사탕에서 멈춤 충돌 모션은
                    //actionnum = 3; //
                    
                    if (next >= BlockNumber.parfaitA && next <= BlockNumber.parfaitD)
                    {
                        player.actionnum = 3;//crash : 3
                        blocks[posZ, posX].Data = BlockNumber.normal;
                        GameController.ParfaitOrder++;
                    }
                    else
                    {
                        player.actionnum = 3;//crash : 3
                    }
                    break;
                case 1://drop 1-> 0 or ride character
                    if(next == BlockNumber.character)
                    {
                        //ride motion
                        player.actionnum = 2;//ride : 2

                        //player state          Idle --> Slave
                        //other player state    Idle --> Master
                    }
                    else if(next >= BlockNumber.normal && next <= BlockNumber.cracked)
                    {
                        pos.y -= 1;
                        player.actionnum = 5;//drop : 5
                    }
                    else if (next >= BlockNumber.parfaitA && next <= BlockNumber.parfaitD)
                    {
                        pos.y -= 1;
                        player.actionnum = 5;//drop : 5
                        blocks[posZ, posX].Data = BlockNumber.normal;
                        GameController.ParfaitOrder++;
                    }
                    else if (next >= BlockNumber.upperParfaitA && next <= BlockNumber.upperParfaitD)//onCloud(2층)에서 2층 파레페 먹고 멈
                    {
                        player.actionnum = 3;// crash : 3
                        blocks[posZ, posX].Data = BlockNumber.upperNormal;
                        GameController.ParfaitOrder++;
                    }
                    else
                    {
                        player.actionnum = 3;// crash : 3
                    }

                    break;
                case 2://drop 2-> 1 or 0
                    if (next >= BlockNumber.normal && next <= BlockNumber.cracked)
                    {
                        player.actionnum = 5;//drop : 5
                        pos.y -= 2;
                    }
                    else if(next >= BlockNumber.parfaitA && next <= BlockNumber.parfaitD)
                    {
                        player.actionnum = 5;//drop : 5
                        pos.y -= 2;
                        blocks[posZ, posX].Data = BlockNumber.normal;
                        GameController.ParfaitOrder++;
                    }
                    else if(next >= BlockNumber.upperNormal && next <= BlockNumber.upperCracked)
                    {
                        player.actionnum = 5;//drop : 5
                        pos.y -= 1;
                    }
                    else if (next >= BlockNumber.upperParfaitA && next <= BlockNumber.upperParfaitD)
                    {
                        player.actionnum = 5;//drop : 5
                        pos.y -= 1;
                        blocks[posZ, posX].Data = BlockNumber.upperNormal;
                        GameController.ParfaitOrder++;
                    }
                    else
                    {
                        player.actionnum = 3;//crash : 3
                    }
                    break;
            }//end switch



            //StopCheckChangeState();
            pos = new Vector3(posX, pos.y, posZ);
            UpdateCheckTrue(width: posX, height: posZ);
            


            


        }
        else//cant block
        {
            if((pos.y == 0 && next == BlockNumber.character) || (pos.y == 1 &&next == BlockNumber.upperCharacter))
            {
                player.actionnum = 4; // character끼리 충돌 : 4
            }
            else
            {
                player.actionnum = 3;
            }
        }

        //pos = new Vector3(posX, pos.y, posZ);

        //temp
        player.temp = blocks[posZ, posX].Data;
        switch((int)pos.y)
        {
            case 0:
                blocks[posZ, posX].Data = BlockNumber.character;
                break;
            case 1:
                blocks[posZ, posX].Data = BlockNumber.upperCharacter;
                break;
            case 2:
                blocks[posZ, posX].Data = BlockNumber.upperCharacter;
                break;
        }
        //remaincheck는 도착한 후




        GameController.instance.moveCommand.SetLaterData(snowList, crackerList);

        snowList.Clear();
        crackerList.Clear();

        return pos;
        
    }
    public void UpdateCheckTrue(int width, int height)
    {
        if (!check[height, width])
        {
            snowList.Add(new Vector2(height, width));
            check[height, width] = true;
        }
    }

    public void UpdateCheckArray(int width, int height, bool isCheck)
    {
        Debug.Log(height + "," + width + "  is checked " + isCheck);

        

        check[height, width] = isCheck;
    }

    public int GetBlockData(int x, int z)
    {
        if (x < mapsizeW && x >=0 && z < mapsizeH && z >= 0)
            return blocks[z, x].Data;
        else
            return BlockNumber.obstacle;
    }

    public void SetBlockData(int x, int z , int value)
    {
        blocks[z, x].Data = value;
    }

    public void SetBlocks(int x, int z , Block block)
    {
        blocks[z, x] = block;
    }
}


