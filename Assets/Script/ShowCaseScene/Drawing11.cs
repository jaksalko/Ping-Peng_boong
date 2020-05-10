using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing11 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 7;
		mapsizeW = 7;
		parfait = false;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1},
		new int[] { 1, 1, 1, 2, 1, 1, 1},
		new int[] { 1, 2, 2, 2, 2, 2, 1},
		new int[] { 1, 2, 2, 2, 3, 2, 1},
		new int[] { 1, 0, 0, 0, 0, 21, 1},
		new int[] { 1, 0, 0, 0, 0, 0, 1},
		new int[] { 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(3, -9, 1);
		startPositionB = new Vector3(1, -9, 1);
	}
}
