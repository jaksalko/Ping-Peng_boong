using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing10 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 8;
		mapsizeW = 6;
		parfait = false;
		map = new int[,] {
			{ 1, 1, 1, 1, 1, 1},
			{ 1, 0, 0, 0, 0, 1},
			{ 1, 0, 2, 0, 23, 1},
			{ 1, 0, 0, 0, 2, 1},
			{ 1, 2, 0, 0, 0, 1},
			{ 1, 21, 0, 2, 0, 1},
			{ 1, 0, 0, 0, 0, 1},
			{ 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(4, -9, 3);
		startPositionB = new Vector3(1, -9, 4);
	}
}

