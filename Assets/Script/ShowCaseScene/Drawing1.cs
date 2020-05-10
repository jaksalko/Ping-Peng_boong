using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing1 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 6;
		mapsizeW = 7;
		parfait = false;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1},
		new int[] { 1, 1, 1, 0, 1, 1, 1},
		new int[] { 1, 1, 0, 0, 0, 1, 1},
		new int[] { 1, 0, 0, 0, 0, 0, 1},
		new int[] { 1, 1, 0, 0, 0, 1, 1},
		
		new int[] { 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(2, -9, 3);
		startPositionB = new Vector3(3, -9, 4);
	}
}
