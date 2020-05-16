using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int mapsizeH;
    public int mapsizeW;
    public bool parfait;
    //public ParfaitObject[] parfaitList;
    public int[,] map;
    public bool[,] check;
    public bool checkparfait;
    public GameObject groundBlock;
    public GameObject obstacleBlock;
    public ParfaitObject[] parfaitBlock;
    public GameObject slope;
    

    public Transform groundParent;
    public Transform obstacleParent;

    public SampleMap[] sample;

    public Vector3 parfaitEndPoint;
    public Vector3 centerOfMap;
    public Transform minimapTarget;

    public Transform[] design;

    public SampleMap sampleMap;

    public void GenerateMap(int index)
    {
        sample[index].init();
        sampleMap = sample[index];
        mapsizeH = sampleMap.mapsizeH;
        mapsizeW = sampleMap.mapsizeW;
        parfait = sampleMap.parfait;

        //Debug.Log("map index : " + PlayerPrefs.GetInt("level", 0) + " , " + index);

        map = new int[mapsizeH, mapsizeW];
        //Debug.Log("map : " + mapsizeH + " , " + mapsizeW + "parfait? ");
        check = new bool[mapsizeH, mapsizeW];

        centerOfMap = new Vector3((float)(mapsizeW-1)/2, -10 , (float)(mapsizeH-1)/2);
        minimapTarget.position = centerOfMap;

        design[0].position = centerOfMap + new Vector3(0, -0.5f, 0);
        design[1].localPosition = design[1].localPosition + new Vector3(0, 0, centerOfMap.z + 8);
        design[2].localPosition = design[2].localPosition + new Vector3(0, 0, -(centerOfMap.z + 8));
        checkparfait = false;
        for(int i = 0; i < mapsizeH; i++)
        {
            for(int j = 0; j < mapsizeW; j++)
            {
//                Debug.Log(mapsizeH - 1 - i);
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
                else if(map[i,j] == 3)//second floor obstacle
                {
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;

                    check[i, j] = true;
                    GameObject obstacle = Instantiate(obstacleBlock, new Vector3(j, -8, i), Quaternion.identity) as GameObject;
                    obstacle.transform.parent = obstacleParent;
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
                    parfaitBlock[0].gameObject.SetActive(true);
                    parfaitBlock[0].gameObject.transform.position = new Vector3(j, -9, i);
                    parfaitBlock[0].sequence = 0;
                    parfaitBlock[0].Activate();
                 
                }
                    
                else if (map[i, j] == -2)
                {
                    parfaitBlock[1].gameObject.SetActive(true);
                    parfaitBlock[1].gameObject.transform.position = new Vector3(j, -9, i);
                    parfaitBlock[1].sequence = 1;
                  
                }

                    
                else if (map[i, j] == -3)
                {
                    parfaitBlock[2].gameObject.SetActive(true);
                    parfaitBlock[2].gameObject.transform.position = new Vector3(j, -9, i);
                    parfaitBlock[2].sequence = 2;
                    
                   
                }
                    
                else if (map[i, j] == -4)
                {
                    parfaitBlock[3].gameObject.SetActive(true);
                    parfaitBlock[3].gameObject.transform.position = new Vector3(j, -9, i);
                    parfaitBlock[3].sequence = 3;
                    parfaitEndPoint = parfaitBlock[3].transform.position + new Vector3(0,-0.5f,0);
                    
                    
                }
                else if (map[i, j] == -5)
                {
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;


                    parfaitBlock[0].gameObject.SetActive(true);
                    parfaitBlock[0].gameObject.transform.position = new Vector3(j, -8, i);
                    parfaitBlock[0].sequence = 0;
                    parfaitBlock[0].Activate();


                    
                }

                else if (map[i, j] == -6)
                {
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;

                    parfaitBlock[1].gameObject.SetActive(true);
                    parfaitBlock[1].gameObject.transform.position = new Vector3(j, -8, i);
                    parfaitBlock[1].sequence = 1;
                }


                else if (map[i, j] == -7)
                {
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;

                    parfaitBlock[2].gameObject.SetActive(true);
                    parfaitBlock[2].gameObject.transform.position = new Vector3(j, -8, i);
                    parfaitBlock[2].sequence = 2;

                }

                else if (map[i, j] == -8)
                {
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;

                    parfaitBlock[3].gameObject.SetActive(true);
                    parfaitBlock[3].gameObject.transform.position = new Vector3(j, -8, i);
                    parfaitBlock[3].sequence = 3;
                    parfaitEndPoint = parfaitBlock[3].transform.position + new Vector3(0, -0.5f, 0);
                }




            }
        }
    }
    
}
