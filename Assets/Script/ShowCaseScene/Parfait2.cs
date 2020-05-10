using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait2 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 5;
		mapsizeW = 7;
		parfait = true;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1},

		new int[] { 1, -8, 0, -7, 24, 0, 1},
		new int[] { 1, 0, -1, 0, 0, 0, 1},
		new int[] { 1, -6, 0, 2, 1, 1, 1},

		new int[] { 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(1, -9, 2);
		startPositionB = new Vector3(3, -9, 2);
	}
}
