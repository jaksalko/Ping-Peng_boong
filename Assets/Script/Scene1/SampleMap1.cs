using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SampleMap1 : MonoBehaviour
{
	public int mapsizeH = 17;
	public int mapsizeW = 12;
	public bool parfait = true;
	public int[][] map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },//아
		new int[] { 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1 },
		new int[] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 13, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14, 1 },
		new int[] { 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 11, 0, 12, 0, 0, 1 },
		new int[] { 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1 },
		new int[] { 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
	};

    
}
