using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing6 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 6;
		mapsizeW = 6;
		parfait = false;
		map = new int[,] {
			{ 1, 1, 1, 1, 1, 1},
			{ 1, 1, 2, 0, 2, 1},
			{ 1, 2, 24, 0, 21, 1},
			{ 1, 1, 0, 0, 0, 1},
			{ 1, 1, 0, 0, 0, 1},
			{ 1, 1, 1, 1, 1, 1}
		};

		startPositionA = new Vector3(4, -9, 1);
		startPositionB = new Vector3(2, -9, 2);
	}
}
