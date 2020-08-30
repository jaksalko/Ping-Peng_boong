using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing2 : SampleMap
{
	public override void init()
	{
		base.init();
		mapsizeH = 5;
		mapsizeW = 7;
		parfait = false;
		map = new int[,] {
			{ 1, 1, 1, 1, 1, 1, 1},
			{ 1, 2, 0, 2, 24, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 1},
			{ 1, 2, 0, 2, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1}
		};

		startPositionA = new Vector3(1, -9, 2);
		startPositionB = new Vector3(3, -9, 2);
	}
}
