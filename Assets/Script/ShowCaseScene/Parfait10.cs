using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait10 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 8;
		mapsizeW = 6;
		parfait = true;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1},
		new int[] { 1, 0, 0, 0, 0, 1},
		new int[] { 1, 0, -5, 0, 23, 1},
		new int[] { 1, 0, 0, 0, -6, 1},
		new int[] { 1, -7, 0, 0, 0, 1},
		new int[] { 1, 21, 0, -8, 0, 1},
		new int[] { 1, 0, 0, 0, 0, 1},
		new int[] { 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(4, -9, 3);
		startPositionB = new Vector3(1, -9, 4);
	}
}

