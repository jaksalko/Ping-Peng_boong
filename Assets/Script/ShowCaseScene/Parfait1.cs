using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait1 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 6;
		mapsizeW = 7;
		parfait = true;
		map = new int[,] {
			{ 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 0, 1, 1, 1},
			{ 1, 1, 0, -2, 0, 1, 1},
			{ 1, -3, 0, 0, 0, -4, 1},
			{ 1, 1, 0, -1, 0, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(2, -9, 3);
		startPositionB = new Vector3(3, -9, 4);
	}
}
