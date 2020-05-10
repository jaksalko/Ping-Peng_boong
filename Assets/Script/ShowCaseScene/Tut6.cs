using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut6 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 5;
		mapsizeW = 9;
		parfait = true;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1},
		new int[] { 1, 0, 1, 0, 0, -2, 0, 0, 1},
		new int[] { 1, 0, 1, 0, -4, 0, 1, 0, 1},
		new int[] { 1, 0, 0, -1, 0, -3, 1, 0, 1},
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1}
		

		};

		startPositionA = new Vector3(1, -9, 3);
		startPositionB = new Vector3(7, -9, 1);
	}
}
