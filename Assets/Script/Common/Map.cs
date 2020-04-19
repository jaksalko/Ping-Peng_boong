using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int mapsizeH;
    public int mapsizeW;
    public bool parfait;
    public ParfaitObject[] parfaitList;
    public int[,] map;
    public bool[,] check;
    public bool checkparfait;
    public GameObject groundBlock;
    public GameObject obstacleBlock;
    public ParfaitObject parfaitBlock;
    public GameObject slope;


    public Transform groundParent;
    public Transform obstacleParent;

    public GameObject[] sample;

    public Vector3 parfaitEndPoint;
    public void GenerateMap(int index)
    {
        SampleMap1 sampleMap = sample[index].GetComponent<SampleMap1>();
        mapsizeH = sampleMap.mapsizeH;
        mapsizeW = sampleMap.mapsizeW;
        parfait = sampleMap.parfait;
        
        map = new int[mapsizeH, mapsizeW];
        check = new bool[mapsizeH, mapsizeW];
        checkparfait = false;
        for(int i = 0; i < mapsizeH; i++)
        {
            for(int j = 0; j < mapsizeW; j++)
            {
                
                map[mapsizeH-1-i,j] = sampleMap.map[i][j];
                
            }
        }

        MakeGround();
        Debug.Log("parfait? : " + parfait);
        if(parfait)
        {
            MakeParfait();
        }

    }

    void MakeGround()
    {
        for (int i = 0; i < mapsizeH; i++)
        {
            for (int j = 0; j < mapsizeW; j++)
            {
                //instantiate ground object at all of area . parent is spawnGround object
                GameObject ground = Instantiate(groundBlock, new Vector3(j, -10,i), Quaternion.identity) as GameObject;
                ground.transform.parent = groundParent;

                if (map[i, j] == 1)
                {
                    //generate obstacle
                    check[i, j] = true;
                    GameObject obstacle = Instantiate(obstacleBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    obstacle.transform.parent = obstacleParent;

                }
                else if(map[i,j] == 2)//second floor
                {
                    //generate second floor
                    
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;
                }
                else if(map[i,j] == 21)
                {
                    check[i, j] = true;
                    GameObject slope_up = Instantiate(slope, new Vector3(j, -9, i), Quaternion.Euler(new Vector3(0, 0, 0)));
                    slope_up.transform.parent = groundParent;
                }
                else if(map[i,j] == 22)
                {
                    check[i, j] = true;
                    GameObject slope_right = Instantiate(slope, new Vector3(j, -9, i), Quaternion.Euler(new Vector3(0, 90, 0)));
                    slope_right.transform.parent = groundParent;
                }
                else if (map[i, j] == 23)
                {
                    check[i, j] = true;
                    GameObject slope_down = Instantiate(slope, new Vector3(j, -9, i), Quaternion.Euler(new Vector3(0, 180, 0)));
                    slope_down.transform.parent = groundParent;
                }
                else if (map[i, j] == 24)
                {
                    check[i, j] = true;
                    GameObject slope_left = Instantiate(slope, new Vector3(j, -9, i), Quaternion.Euler(new Vector3(0, 270, 0)));
                    slope_left.transform.parent = groundParent;
                }
            }
        }
    }


    void MakeParfait()
    {
        for (int i = 0; i < mapsizeH; i++)
        {
            for (int j = 0; j < mapsizeW; j++)
            {
                if (map[i, j] == -1)
                {
                    
                    ParfaitObject parfait = Instantiate(parfaitBlock, new Vector3(j, -9, i), Quaternion.identity) as ParfaitObject;
                    parfait.sequence = 0;
                    parfait.Activate();
                    parfaitList[0] = parfait;

                    
                    parfait.transform.parent = obstacleParent;
                }
                    
                else if (map[i, j] == -2)
                {

                    ParfaitObject parfait = Instantiate(parfaitBlock, new Vector3(j, -9, i), Quaternion.identity) as ParfaitObject;
                    parfait.sequence = 1;
                    parfaitList[1] = parfait;
                    
                    
                    parfait.transform.parent = obstacleParent;
                }

                    
                else if (map[i, j] == -3)
                {

                    ParfaitObject parfait = Instantiate(parfaitBlock, new Vector3(j, -9, i), Quaternion.identity) as ParfaitObject;
                    parfait.sequence = 2;
                    parfaitList[2] = parfait;
                    
                    parfait.transform.parent = obstacleParent;
                }
                    
                else if (map[i, j] == -4)
                {

                    ParfaitObject parfait = Instantiate(parfaitBlock, new Vector3(j, -9, i), Quaternion.identity) as ParfaitObject;
                    parfait.sequence = 3;
                    parfaitList[3] = parfait;
                    parfaitEndPoint = parfait.transform.position;
                    parfait.transform.parent = obstacleParent;
                }
                    

                

            }
        }
    }
    
}
