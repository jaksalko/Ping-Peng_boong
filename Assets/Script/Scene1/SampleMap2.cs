using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMap2 : SampleMap
{



    /*
     * 파르페 -1 -2 -3 -4
     * 1층 0
     * 2층 2
     * 슬로프 상우하좌 21 22 23 24
     * 장애물 1
     * 2층 장애물 3
     * 2층 파르페 -5 -6 -7 -8
     * */
    public override void init()
    {
        base.init();
		mapsizeH = 7;
		mapsizeW = 5;
		parfait = true;
		/*map = new int[][]
		{
			new int[] { 1, 1, 1, 1, 1},
			new int[] { 1, -6, -7, 2, 1},
			new int[] { 1, 2, 1, 21, 1},//1 23
			new int[] { 1, -1, 1, 0, 1},
			new int[] { 1, 0, 1, 23, 1},
			new int[] { 1, 2, 1, -8, 1},
			new int[] { 1, 1, 1, 1, 1}
			//new int[] { 1, 1, 1, 1, 1},
			//new int[] { 1, 0, 0, 0, 1},
			//new int[] { 1, 0, 1, 0, 1},//1 23
			//new int[] { 1, 0, 1, 2, 1},
			//new int[] { 1, 0, 1, 21, 1},
			//new int[] { 1, 0, 1, 0, 1},
			//new int[] { 1, 1, 1, 1, 1}

		};*/

		startPositionA = new Vector3(1, -8, 1);
		startUpstairA = true;
		startPositionB = new Vector3(1, -9, 2);
	}

   
}
