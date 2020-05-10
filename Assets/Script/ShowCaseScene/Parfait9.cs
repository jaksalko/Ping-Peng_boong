using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait9 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 7;
		mapsizeW = 8;
		parfait = true;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1},
		new int[] { 1, 1, -7, 1, -8, 0, 1, 1},
		new int[] { 1, -6, 2, 2, 2, 0, 0, 1},
		new int[] { 1, 1, 2, 1, 21, 0, 0, 1},
		new int[] { 1, 1, 0, 0, 0, 0, 0, 1},
		new int[] { 1, 1, 1, -1, 1, 1, 1, 1},
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(6, -9, 2);
		startPositionB = new Vector3(6, -9, 4);
	}
}