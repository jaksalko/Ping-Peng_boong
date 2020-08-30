using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parfait7 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 6;
		mapsizeW = 7;
		parfait = true;
		map = new int[,] {
			{ 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, -5, 1, 1},
			{ 1, -4, 0, 22, 2, -6, 1},
			{ 1, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 1, 2, 2, -7, 1},
			{ 1, 1, 1, 1, 1, 1, 1}

		};

		startPositionA = new Vector3(4, -9, 2);
		startPositionB = new Vector3(1, -9, 2);
	}
}

