using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait12 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 7;
		mapsizeW = 7;
		parfait = true;
		map = new int[,] {
			{ 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 0, 0, 2, 2, 1},
			{ 1, 0, 0, -6, 2, 2, 1},
			{ 1, 0, 0, 22, 2, 2, 1},
			{ 1, 0, 0, -7, -8, 2, 1},
			{ 1, 1, 0, -1, 2, 2, 1},
			{ 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(1, -9, 4);
		startPositionB = new Vector3(3, -9, 5);
	}
}
