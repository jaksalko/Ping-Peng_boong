using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut3 : SampleMap
{
	// Start is called before the first frame update
	public override void init()
	{
		base.init();
		mapsizeH = 7;
		mapsizeW = 5;
		parfait = false;
		map = new int[][] {
		new int[] { 1, 1, 1, 1, 1},
		new int[] { 1, 0, 0, 1, 1},
		new int[] { 1, 0, 0, 1, 1},
		new int[] { 1, 0, 0, 0, 1},
		new int[] { 1, 0, 0, 1, 1},
		new int[] { 1, 0, 0, 1, 1},
		new int[] { 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(1, -9, 5);
		startPositionB = new Vector3(1, -9, 1);
	}
}