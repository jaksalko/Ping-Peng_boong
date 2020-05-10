using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait8 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 7;
		mapsizeW = 6;
		parfait = true;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1},
		new int[] { 1, 0, 0, 0, 1, 1},
		new int[] { 1, 0, -1, 0, 1, 1},
		new int[] { 1, 0, 1, -3, -4, 1},
		new int[] { 1, 0, -2, 0, 1, 1},
		new int[] { 1, 0, 0, 0, 1, 1},
		new int[] { 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(1, -9, 5);
		startPositionB = new Vector3(1, -9, 1);
	}
}

