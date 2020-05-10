using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut4 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 5;
		mapsizeW = 7;
		parfait = false;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1},
		new int[] { 1, 0, 0, 0, 2, 2, 1},
		new int[] { 1, 0, 0, 22, 2, 2, 1},
		new int[] { 1, 0, 0, 0, 2, 2, 1},
		new int[] { 1, 1, 1, 1, 1, 1, 1}
		

		};

		startPositionA = new Vector3(1, -9, 3);
		startPositionB = new Vector3(1, -9, 1);
	}
}
