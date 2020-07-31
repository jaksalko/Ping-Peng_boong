using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait3 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 5;
		mapsizeW = 6;
		parfait = true;
		map = new int[,] {
			{ 1, 1, 1, 1, 1, 1},
			{ 1, -2, 22, -7, 1, 1},
			{ 1, 0, 0, 0, -1, 1},
			{ 1, 0, 0, 2, -8, 1},
			{ 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(1, -9, 1);
		startPositionB = new Vector3(1, -9, 2);
	}
}