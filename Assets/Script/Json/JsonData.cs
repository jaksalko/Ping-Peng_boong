﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class JsonData
{
    public string title = "";//pk
    public string nickname = "";//user nickname
    public DateTime updateTime;

    public int height;//map height
    public int width;//map width
    public string value = "";//map index value to string

    public string posA = "";//character a position to string
    public string posB = "";//character b position to string

    public int moveCount;//simulating game move count
    public int difficulty;//game difficulty from movecount
    public int parfait;//default 0 == false
   

    public JsonData(string nickname , string title , Map map, DateTime time , int move , int dif)
    {
        this.nickname = nickname;
        this.title = title;
        updateTime = time;

        height = map.mapsizeH;
        width = map.mapsizeW;

        moveCount = move;
        difficulty = dif;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                value += IndexToChar(map.GetBlockData(j,i));
            }
        }

        posA = PositionToString(map.startPositionA);
        posB = PositionToString(map.startPositionB);

        parfait = map.parfait ? 1 : 0;

    }

    public void DataToString()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }

    public Map MakeSampleMap()
    {
        int[,] datas = new int[height, width];
        int index = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {

                datas[i, j] = CharToIndex(value[index]);

                index++;
            }
        }

        Map newMap = new Map(
            size: new Vector2(height, width),
            isParfait: parfait == 0 ? false : true,
            posA: StringToPosition(posA),
            posB: StringToPosition(posB),
            datas: datas
            );


        return newMap;
    }

    int CharToIndex(char value)
    {
        int ascii = (int)value - 65; // a -> 0
        return ascii;

    }
    public char IndexToChar(int value)
    {

        char ascii = Convert.ToChar(value + 65);// 0 -> A
        return ascii;
    }

    string PositionToString(Vector3 pos)
    {
        string s = "";
        s += IndexToChar((int)pos.x);
        s += IndexToChar((int)pos.y);
        s += IndexToChar((int)pos.z);
        

        return s;
    }
    Vector3 StringToPosition(string pos)
    {
        Vector3 v = new Vector3(CharToIndex(pos[0]),
            CharToIndex(pos[1]),
            CharToIndex(pos[2])
            );


        return v;
    }


}

#region User Skin Data

public class UserSkinData
{
    public int rid;
    public string userid;
    public int skinid_1;
    public Time gettime_1;
    public int skinid_2;
    public Time gettime_2;
    public int skinid_3;
    public Time gettime_3;
    public int skinid_4;
    public Time gettime_4;
    public int skinid_5;
    public Time gettime_5;
}

#endregion

#region Community Json Data

[Serializable]
public class UserData
{
    public string id;
    public int cash;
    public int candy;
    public string nickname;
    public int stage;
    //public int change;

    public UserData(string userid , string nick)
    {
        id = userid;
        nickname = nick;
        cash = 0;
        candy = 5;
        stage = 0;
    }

    public UserData(string userid, int cash , int stage)
    {
        id = userid;
        this.cash = cash;
        this.stage = stage;
    }
}

[Serializable]
public class Chat
{
    public string id_1;
    public string id_2;
    public string text;
    public DateTime time;

    public Chat(string i1, string i2, string txt)
    {
        id_1 = i1;
        id_2 = i2;
        text = txt;
        time = DateTime.Now;
    }
}
[Serializable]
public class FriendRequest
{
    public string id;
    public string friend_id;

    public FriendRequest(string i, string f)
    {
        id = i;
        friend_id = f;
    }
}

[Serializable]
public class StageData
{
    public string id;
    public int stage_num;
    public int stage_step;

    public StageData(string i, int num)//
    {
        id = i;
        stage_num = num;
        stage_step = 99;
    }

    public StageData(string i, int num , int step)
    {
        id = i;
        stage_num = num;
        stage_step = step;
    }
}

#endregion

