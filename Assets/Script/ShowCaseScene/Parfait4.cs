using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait4 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 5;
		mapsizeW = 7;
		parfait = true;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1},

		new int[] { 1, 0, 22, 2, -8, 2, 1},
		new int[] { 1, 0, 0, -5, 3, -7, 1},
		new int[] { 1, 0, 0, 2, -6, 2, 1},

		new int[] { 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(1, -9, 1);
		startPositionB = new Vector3(1, -9, 2);
	}
}

