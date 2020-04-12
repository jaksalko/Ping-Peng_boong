using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int mapsizeH;
    public int mapsizeW;
    public bool parfait;
    public GameObject[] parfaitList;
    public int[,] map;

    public GameObject groundBlock;
    public GameObject obstacleBlock;
    public GameObject parfaitBlock;

    public Transform groundParent;
    public Transform obstacleParent;

    public GameObject[] sample;

    public void GenerateMap(int index)
    {
        SampleMap1 sampleMap = sample[index].GetComponent<SampleMap1>();
        mapsizeH = sampleMap.mapsizeH;
        mapsizeW = sampleMap.mapsizeW;
        parfait = sampleMap.parfait;
        
        map = new int[mapsizeH, mapsizeW];
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
                    GameObject obstacle = Instantiate(obstacleBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    obstacle.transform.parent = obstacleParent;

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
                if (map[i, j] == 11)
                {
                    
                    GameObject parfait = Instantiate(parfaitBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    parfaitList[0] = parfait;
                    parfait.SetActive(false);
                    parfait.GetComponent<Renderer>().material.color = Color.red;
                    parfait.transform.parent = obstacleParent;
                }
                    
                else if (map[i, j] == 12)
                {
                    
                    GameObject parfait = Instantiate(parfaitBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    parfaitList[1] = parfait;
                    parfait.SetActive(false);
                    parfait.GetComponent<Renderer>().material.color = Color.black;
                    parfait.transform.parent = obstacleParent;
                }

                    
                else if (map[i, j] == 13)
                {
                    
                    GameObject parfait = Instantiate(parfaitBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    parfaitList[2] = parfait;
                    parfait.SetActive(false);
                    parfait.GetComponent<Renderer>().material.color = Color.black;
                    parfait.transform.parent = obstacleParent;
                }
                    
                else if (map[i, j] == 14)
                {
                    
                    GameObject parfait = Instantiate(parfaitBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    parfaitList[3] = parfait;
                    parfait.SetActive(false);
                    parfait.GetComponent<Renderer>().material.color = Color.black;
                    parfait.transform.parent = obstacleParent;
                }
                    

                

            }
        }
    }
    
}
