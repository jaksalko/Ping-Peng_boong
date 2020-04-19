using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMap2 : SampleMap1
{
    // Start is called before the first frame update
    void Awake()
    {
        mapsizeH = 7;
        mapsizeW = 5;
		parfait = false;
		map = new int[][]
		{
			new int[] { 1, 1, 1, 1, 1},
            new int[] { 1, 0, 22, 2, 1},
            new int[] { 1, 0, 1, 2, 1},//1 23
			new int[] { 1, 0, 1, 0, 1},
            new int[] { 1, 0, 1, 0, 1},
            new int[] { 1, 2, 1, 0, 1},
            new int[] { 1, 1, 1, 1, 1}
			//new int[] { 1, 1, 1, 1, 1},
			//new int[] { 1, 0, 0, 0, 1},
			//new int[] { 1, 0, 1, 0, 1},//1 23
			//new int[] { 1, 0, 1, 2, 1},
			//new int[] { 1, 0, 1, 21, 1},
			//new int[] { 1, 0, 1, 0, 1},
			//new int[] { 1, 1, 1, 1, 1}

		};
    }
	//map = new int[] { 1, 1, 1, 1, 1},
	//		new int[] { 1, 0, 22, 2, 1},
	//		new int[] { 1, 0, 1, 2, 1},//1 23
	//		new int[] { 1, 2, 1, 21, 1},
	//	    new int[] { 1, 21, 1, 0, 1},
	//	    new int[] { 1, 0, 1, 0, 1},
	//	    new int[] { 1, 1, 1, 1, 1}
}
