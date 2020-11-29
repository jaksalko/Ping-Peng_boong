using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
static class BlockNumber
{

    public const int
        //0층 블럭
        normal = 0,
        cloudUp = 1, cloudRight = 2, cloudDown = 3, cloudLeft = 4,
        cracked = 5,
        broken = 6,

        //1층 블럭
        upperNormal = 10,
        upperCloudUp = 11, upperCloudRight = 12, upperCloudDown = 13, upperCloudLeft = 14,
        upperCracked = 15,
        upperBroken = 16,
        slopeUp = 17, slopeRight = 18, slopeDown = 19, slopeLeft = 20,
        parfaitA = 21, parfaitB = 22, parfaitC = 23, parfaitD = 24,
        obstacle = 26,

        //** character
        characterA = 28, characterB = 29, character = 27,

        //2층 블럭
        upperCharacter = 38,
        upperParfaitA = 31, upperParfaitB = 32, upperParfaitC = 33, upperParfaitD = 34,
        upperObstacle = 36;

    public static int[] firstlevel = new int[2] { normal, cracked };
    public static int[] secondLevel = new int[2] { upperNormal, upperCracked };
    public static int[] slopeLevel = new int[4] { slopeUp, slopeRight, slopeDown, slopeLeft };

    public static List<int> GetThirdFloorThroughBlock(int dir, bool onCloud)
    {
        List<int> throughBlockList = new List<int>();

        for (int i = 0; i <= 3; i++)
        {
            if (Mathf.Abs(dir - i) != 2)
            {
                throughBlockList.Add(upperCloudUp + i);
                throughBlockList.Add(cloudUp + i);
            }
        }

        return throughBlockList;
    }

    public static List<int> GetUpstairThroughBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;
        List<int> throughBlockList = new List<int>();

        if(!onCloud)
        {
            throughBlockList.AddRange(secondLevel);
            throughBlockList.Add(upperParfaitA + parfaitOrder);            
        }

        switch (dir)
        {
            case 0:
                throughBlockList.Add(slopeDown);
                break;
            case 1:
                throughBlockList.Add(slopeLeft);
                break;
            case 2:
                throughBlockList.Add(slopeUp);
                break;
            case 3:
                throughBlockList.Add(slopeRight);
                break;
        }
        

        for (int i = 0; i <= 3; i++)
        {
            if (Mathf.Abs(dir - i) != 2)
            {
                throughBlockList.Add(upperCloudUp + i);
                throughBlockList.Add(cloudUp + i);
            }
        }

        return throughBlockList;
    }
    public static List<int> GetDownstairThroughBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;

        List<int> throughBlockList = new List<int>();

        if(!onCloud)
        {
            throughBlockList.AddRange(firstlevel);
            throughBlockList.Add(parfaitA + parfaitOrder);
        }
        
        throughBlockList.Add(slopeUp + dir);
        

        for (int i = 0; i <= 3; i++)
        {
            if (Mathf.Abs(dir - i) != 2)
            {
                throughBlockList.Add(cloudUp + i);
            }
        }

        return throughBlockList;
    }

    #region Stop Block List

    public static List<int> GetThirdFloorStopBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;

        List<int> stopBlockList = new List<int>();
        stopBlockList.AddRange(secondLevel);
        stopBlockList.AddRange(firstlevel);

        stopBlockList.Add(parfaitA + parfaitOrder);//first floor parfait
        stopBlockList.Add(upperParfaitA + parfaitOrder);//first floor parfait

        

        return stopBlockList;
    }

    public static List<int> GetUpstairStopBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;

        List<int> stopBlockList = new List<int>();


        if (onCloud)
        {
            stopBlockList.AddRange(secondLevel);
            stopBlockList.Add(upperParfaitA + parfaitOrder);
        }

        stopBlockList.AddRange(firstlevel);//normal , cracked
        stopBlockList.Add(parfaitA + parfaitOrder);//first floor parfait



        stopBlockList.Add(character);

        
        return stopBlockList;
    }

    public static List<int> GetDownstairStopBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;

        List<int> stopBlockList = new List<int>();

        if (onCloud)
        {
            stopBlockList.AddRange(firstlevel);
            stopBlockList.Add(parfaitA + parfaitOrder);
        }
        
        return stopBlockList;
    }

    #endregion
}
