using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SampleMap : MonoBehaviour
{


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


    /*
    * 파르페 -1 -2 -3 -4
    * 1층 0
    * 2층 2
    * 슬로프 상우하좌 21 22 23 24
    * 장애물 1
    * 2층 장애물 3
    * 2층 파르페 -5 -6 -7 -8
    * */
    
    public int[,] map;

    public void LineToMap()
    {
        map = new int[mapsizeH, mapsizeW];
        for(int i = 0; i < mapsizeH; i++)
        {
            for(int j = 0; j < mapsizeW; j++)
            {
                map[i, j] = lines[i].line[j];
            }
        }

    }

    public virtual void init()
    {

    }
}
