using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait11 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 7;
		mapsizeW = 7;
		parfait = true;
		map = new int[,] {
			{ 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, -8, 1, 1, 1},
			{ 1, -5, 2, 2, 2, 2, 1},
			{ 1, 2, 2, 2, 3, 2, 1},
			{ 1, -2, 0, 0, -3, 21, 1},
			{ 1, 0, 0, 0, 0, 0, 1},
			{ 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(3, -9, 1);
		startPositionB = new Vector3(1, -9, 1);
	}
}