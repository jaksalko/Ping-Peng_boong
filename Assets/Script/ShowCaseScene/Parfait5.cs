using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait5 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 7;
		mapsizeW = 7;
		parfait = true;
		map = new int[,] {
			{ 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 2, -7, 2, 0, 1},
			{ 1, -6, 2, 2, -1, 0, 1},
			{ 1, 2, 2, 0, 0, 0, 1},
			{ 1, 2, 24, 0, 1, 1, 1},
			{ 1, -4, 0, 0, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(5, -9, 3);
		startPositionB = new Vector3(5, -9, 5);
	}
}