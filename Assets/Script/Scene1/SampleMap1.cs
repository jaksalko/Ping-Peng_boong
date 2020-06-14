using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SampleMap1 : SampleMap
{

    public override void init()
    {
        base.init();
		mapsizeH = 17;
		mapsizeW = 12;
		parfait = false;
		/*map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		new int[] { 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1 },
		new int[] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1 },
		new int[] { 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
		};
        */
		startPositionA = new Vector3(1, -9, 1);
		startPositionB = new Vector3(1, -9, 2);
	}
    
   


}
