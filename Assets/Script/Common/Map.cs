using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.IO;
using System.Net;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

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


}


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
    public GameObject groundBlock_sky;
    public GameObject ground_half_sky;
    public GameObject ground_half;
    public GameObject obstacleBlock;

    public GameObject secondfloor_block;

    public ParfaitObject[] parfaitBlock;
    public GameObject slope;

    public GameObject cloud;
    public GameObject cracked;
    public GameObject broken;

    public Transform groundParent;
    public Transform obstacleParent;

    public SampleMap[] sample;

    public Vector3 parfaitEndPoint;
    public Vector3 centerOfMap;
    public Transform minimapTarget;

    public Transform[] design;

    public SampleMap sampleMap;

    public Transform waterQuad;

    /*public void InfiniteMap()
    {
        StartCoroutine(GET());
    }*/

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
        waterQuad.position = centerOfMap;
        waterQuad.localScale = new Vector3(mapsizeW, mapsizeH, 1);
        
        design[0].position = centerOfMap + new Vector3(0, -0.5f, 0);
        design[1].localPosition = design[1].localPosition + new Vector3(0, 0, centerOfMap.z + 8);
        design[2].localPosition = design[2].localPosition + new Vector3(0, 0, -(centerOfMap.z + 8));
        checkparfait = false;
        for(int i = 0; i < mapsizeH; i++)
        {
            for(int j = 0; j < mapsizeW; j++)
            {
//                Debug.Log(mapsizeH - 1 - i);
                map[i,j] = sampleMap.map[i,j];
                
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
        for(int i = 0; i < mapsizeW; i++)
        {
            GameObject ground;
            GameObject half_ground;
            if (i == 0 || i == (mapsizeW-1))
            {
                for(int j = 0; j < mapsizeH; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        ground = Instantiate(groundBlock_sky, new Vector3(i, -10, j), Quaternion.identity) as GameObject;
                        half_ground = Instantiate(ground_half_sky, new Vector3(i, -9.25f, j), Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        ground = Instantiate(groundBlock, new Vector3(i, -10, j), Quaternion.identity) as GameObject;
                        half_ground = Instantiate(ground_half, new Vector3(i, -9.25f, j), Quaternion.identity) as GameObject;
                    }
                    check[j,i] = true;
                    ground.transform.parent = groundParent;
                    half_ground.transform.parent = groundParent;
                }
            }
            else
            {
                int j = 0;
                check[j, i] = true;
                if ((i + j) % 2 == 0)
                {
                    ground = Instantiate(groundBlock_sky, new Vector3(i, -10, j), Quaternion.identity) as GameObject;
                    half_ground = Instantiate(ground_half_sky, new Vector3(i, -9.25f, j), Quaternion.identity) as GameObject;
                }
                else
                {
                    ground = Instantiate(groundBlock, new Vector3(i, -10, j), Quaternion.identity) as GameObject;
                    half_ground = Instantiate(ground_half, new Vector3(i, -9.25f, j), Quaternion.identity) as GameObject;
                }
                ground.transform.parent = groundParent;
                half_ground.transform.parent = groundParent;
                j = mapsizeH-1;
                check[j, i] = true;
                if ((i + j) % 2 == 0)
                {
                    ground = Instantiate(groundBlock_sky, new Vector3(i, -10, j), Quaternion.identity) as GameObject;
                    half_ground = Instantiate(ground_half_sky, new Vector3(i, -9.25f, j), Quaternion.identity) as GameObject;
                }
                else
                {
                    ground = Instantiate(groundBlock, new Vector3(i, -10, j), Quaternion.identity) as GameObject;
                    half_ground = Instantiate(ground_half, new Vector3(i, -9.25f, j), Quaternion.identity) as GameObject;
                }
                ground.transform.parent = groundParent;
                half_ground.transform.parent = groundParent;
            }
        }


        for (int i = 1; i < mapsizeH-1; i++)
        {
            for (int j = 1; j < mapsizeW-1; j++)
            {
                //instantiate ground object at all of area . parent is spawnGround object
                

                //0층 빌드
                if(map[i,j] == 0 || map[i,j] > 6)// not cloud , cracked , broken (0 floor)
                {
                    GameObject ground;
                    if ((i + j) % 2 == 0)
                    {
                        ground = Instantiate(groundBlock_sky, new Vector3(j, -10, i), Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        ground = Instantiate(groundBlock, new Vector3(j, -10, i), Quaternion.identity) as GameObject;
                    }
                    ground.transform.parent = groundParent;
                }
                else//cloud , cracked or broken (0 floor)
                {
                    GameObject ground;

                    if(map[i,j] == BlockNumber.cracked)
                    {
                        ground = Instantiate(cracked, new Vector3(j, -10, i), Quaternion.identity) as GameObject;
                        ground.transform.parent = groundParent;

                        CrackedBlock crackedBlock = ground.GetComponent<CrackedBlock>();
                        crackedBlock.x = j;
                        crackedBlock.z = i;
                        crackedBlock.num = map[i, j];

                    }
                    else if(map[i,j] == BlockNumber.broken)
                    {
                        ground = Instantiate(broken, new Vector3(j, -10, i), Quaternion.identity) as GameObject;
                        check[i, j] = true;
                        ground.transform.parent = obstacleParent;
                    }
                    else
                    {
                        //cloud
                        ground = Instantiate(cloud, new Vector3(j, -10, i), Quaternion.Euler(new Vector3(0, 90 * (map[i,j] - BlockNumber.cloudUp), 0))) as GameObject;
                        ground.transform.parent = groundParent;
                        check[i, j] = true;

                        ground.GetComponent<CloudBlock>().num = map[i, j];
                    }
                    
                }
               
                
                
                //1층,2층 빌드
                if (map[i, j] == BlockNumber.obstacle)
                {
                    //generate obstacle
                    check[i, j] = true;
                    GameObject obstacle = Instantiate(obstacleBlock, new Vector3(j, -8.7f, i), obstacleBlock.transform.rotation) as GameObject;
                    obstacle.transform.parent = obstacleParent;

                }
                else if (map[i, j] == BlockNumber.upperObstacle)//second floor obstacle
                {
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;

                    check[i, j] = true;
                    GameObject obstacle = Instantiate(obstacleBlock, new Vector3(j, -7.7f, i), obstacleBlock.transform.rotation) as GameObject;
                    obstacle.transform.parent = obstacleParent;
                }
              
                else if(map[i,j] == BlockNumber.upperNormal)//second floor
                {
                    //generate second floor
                    
                    GameObject second_ground = Instantiate(secondfloor_block, new Vector3(j, -9, i), secondfloor_block.transform.rotation) as GameObject;
                    second_ground.transform.parent = groundParent;
                }
                else if (map[i, j] == BlockNumber.upperCharacter)//second floor with character?
                {
                    if (sampleMap.startPositionA.z == i && sampleMap.startPositionA.x == j && sampleMap.startUpstairA)
                    {
                        Debug.Log("char A : " + i + "," + j);
                        GameObject second_ground = Instantiate(secondfloor_block, new Vector3(j, -9, i), secondfloor_block.transform.rotation) as GameObject;
                        second_ground.transform.parent = groundParent;
                    }
                    if (sampleMap.startPositionB.z == i && sampleMap.startPositionB.x == j && sampleMap.startUpstairB)
                    {
                        Debug.Log("char B : " + i + "," + j);
                        GameObject second_ground = Instantiate(secondfloor_block, new Vector3(j, -9, i), secondfloor_block.transform.rotation) as GameObject;
                        second_ground.transform.parent = groundParent;
                    }
                }
                else if(map[i,j] >= BlockNumber.slopeUp && map[i, j] <= BlockNumber.slopeLeft)
                {
                    check[i, j] = true;
                    GameObject slopeBlock = Instantiate(slope, new Vector3(j, -9, i), Quaternion.Euler(new Vector3(0, 90 * (map[i,j] - BlockNumber.slopeUp) , 0)));
                    slopeBlock.transform.parent = groundParent;
                }
                else if(map[i,j] == BlockNumber.upperCracked)
                {
                    GameObject crackedBlock = Instantiate(cracked, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    crackedBlock.transform.parent = groundParent;
                }
                else if (map[i, j] == BlockNumber.upperBroken)
                {
                    check[i, j] = true;
                    GameObject brokenBlock = Instantiate(broken, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    brokenBlock.transform.parent = obstacleParent;
                }
                else if (map[i, j] >= BlockNumber.upperCloudUp && map[i, j] <= BlockNumber.upperCloudLeft)
                {
                    check[i, j] = true;
                    GameObject upperCloudBlock = Instantiate(cloud, new Vector3(j, -9, i), Quaternion.Euler(new Vector3(0, 90 * (map[i, j] - BlockNumber.upperCloudUp), 0)));
                    upperCloudBlock.transform.parent = groundParent;
                    upperCloudBlock.GetComponent<CloudBlock>().num = map[i, j];
                }
                /*else if (map[i, j] >= BlockNumber.parfaitA && map[i, j] <= BlockNumber.parfaitD)
                {
                    
                    GameObject parfait = Instantiate(cloud, new Vector3(j, -9, i), Quaternion.Euler(new Vector3(0, 90 * (map[i, j] - BlockNumber.upperCloudUp), 0)));
                    parfait.transform.parent = groundParent;
                    //parfait.GetComponent<CloudBlock>().num = map[i, j];
                }
                else if (map[i, j] >= BlockNumber.upperParfaitA && map[i, j] <= BlockNumber.upperParfaitD)
                {
                    
                    GameObject upperParfait = Instantiate(cloud, new Vector3(j, -7.7f, i), Quaternion.Euler(new Vector3(0, 90 * (map[i, j] - BlockNumber.upperCloudUp), 0)));
                    upperParfait.transform.parent = groundParent;
                    //upperCloudBlock.GetComponent<CloudBlock>().num = map[i, j];
                }*/
                //추가해야할 것 : 구름길 (0층 1층 ) , 부서지는 블럭(0층 1층) , 
            }
        }
    }


    void MakeParfait()
    {
        for (int i = 0; i < mapsizeH; i++)
        {
            for (int j = 0; j < mapsizeW; j++)
            {
                
                if (map[i, j] == BlockNumber.parfaitA)
                {
                    parfaitBlock[0].gameObject.SetActive(true);
                    parfaitBlock[0].gameObject.transform.position = new Vector3(j, -9, i);
                    parfaitBlock[0].sequence = 0;
                    parfaitBlock[0].Activate();
                 
                }
                    
                else if (map[i, j] == BlockNumber.parfaitB)
                {
                    parfaitBlock[1].gameObject.SetActive(true);
                    parfaitBlock[1].gameObject.transform.position = new Vector3(j, -9, i);
                    parfaitBlock[1].sequence = 1;
                  
                }

                    
                else if (map[i, j] == BlockNumber.parfaitC)
                {
                    parfaitBlock[2].gameObject.SetActive(true);
                    parfaitBlock[2].gameObject.transform.position = new Vector3(j, -9, i);
                    parfaitBlock[2].sequence = 2;
                    
                   
                }
                    
                else if (map[i, j] == BlockNumber.parfaitD)
                {
                    parfaitBlock[3].gameObject.SetActive(true);
                    parfaitBlock[3].gameObject.transform.position = new Vector3(j, -9, i);
                    parfaitBlock[3].sequence = 3;
                    parfaitEndPoint = parfaitBlock[3].transform.position + new Vector3(0,-0.5f,0);
                    
                    
                }
                else if (map[i, j] == BlockNumber.upperParfaitA)
                {
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;


                    parfaitBlock[0].gameObject.SetActive(true);
                    parfaitBlock[0].gameObject.transform.position = new Vector3(j, -8, i);
                    parfaitBlock[0].sequence = 0;
                    parfaitBlock[0].Activate();


                    
                }

                else if (map[i, j] == BlockNumber.upperParfaitB)
                {
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;

                    parfaitBlock[1].gameObject.SetActive(true);
                    parfaitBlock[1].gameObject.transform.position = new Vector3(j, -8, i);
                    parfaitBlock[1].sequence = 1;
                }


                else if (map[i, j] == BlockNumber.upperParfaitC)
                {
                    GameObject second_ground = Instantiate(groundBlock, new Vector3(j, -9, i), Quaternion.identity) as GameObject;
                    second_ground.transform.parent = groundParent;

                    parfaitBlock[2].gameObject.SetActive(true);
                    parfaitBlock[2].gameObject.transform.position = new Vector3(j, -8, i);
                    parfaitBlock[2].sequence = 2;

                }

                else if (map[i, j] == BlockNumber.upperParfaitD)
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


    public IEnumerator InfiniteMAP(int level)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://ec2-15-164-219-253.ap-northeast-2.compute.amazonaws.com:3000/map/difficulty?difficulty="+level+"&nickname="+GoogleInstance.instance.id);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;

            //Get data and convert to samplemap list..

            
            string fixdata = fixJson(www.downloadHandler.text);
            JsonData[] datas = JsonHelper.FromJson<JsonData>(fixdata);
            Debug.Log(datas.Length);
            JsonData selectedData = datas[UnityEngine.Random.Range(0, datas.Length)];
            sampleMap = selectedData.MakeSampleMap();
            selectedData.DataToString();


            /*foreach (var data in datas)
            {
                data.DataToString();
            }*/


            mapsizeH = sampleMap.mapsizeH;
            mapsizeW = sampleMap.mapsizeW;
            parfait = sampleMap.parfait;

            //Debug.Log("map index : " + PlayerPrefs.GetInt("level", 0) + " , " + index);

            map = new int[mapsizeH, mapsizeW];
            //Debug.Log("map : " + mapsizeH + " , " + mapsizeW + "parfait? ");
            check = new bool[mapsizeH, mapsizeW];

            centerOfMap = new Vector3((float)(mapsizeW - 1) / 2, -10, (float)(mapsizeH - 1) / 2);
            minimapTarget.position = centerOfMap;

            design[0].position = centerOfMap + new Vector3(0, -0.5f, 0);
            design[1].localPosition = design[1].localPosition + new Vector3(0, 0, centerOfMap.z + 8);
            design[2].localPosition = design[2].localPosition + new Vector3(0, 0, -(centerOfMap.z + 8));
            checkparfait = false;

            //bool aSet = false;
            for (int i = 0; i < mapsizeH; i++)
            {
                for (int j = 0; j < mapsizeW; j++)
                {

                    map[i, j] = sampleMap.map[i, j];

                    //                Debug.Log(mapsizeH - 1 - i);
                    /*if (map[i,j] ==5 && !aSet)
                    {
                        Debug.Log("hihihi");
                        sampleMap.startPositionA = new Vector3(j, -9f, i);
                        aSet = true;
                    }
                    else if(map[i, j] == 5 && aSet)
                    {
                        sampleMap.startPositionB = new Vector3(j, -9f, i);
                    }*/


                    

                }
            }

            MakeGround();
            Debug.Log("parfait? : " + parfait);
            if (parfait)
            {
                MakeParfait();
            }
        }

        yield break;
    }



    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }


}