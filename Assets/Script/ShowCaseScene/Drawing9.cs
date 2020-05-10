using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing9 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 7;
		mapsizeW = 8;
		parfait = false;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1},
		new int[] { 1, 1, 2, 1, 2, 0, 1, 1},
		new int[] { 1, 2, 2, 2, 2, 0, 0, 1},
		new int[] { 1, 1, 2, 1, 21, 0, 0, 1},
		new int[] { 1, 1, 0, 0, 0, 0, 0, 1},
		new int[] { 1, 1, 1, 0, 1, 1, 1, 1},
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(6, -9, 2);
		startPositionB = new Vector3(6, -9, 4);
	}
}
