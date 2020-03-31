using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour
{
	public GameObject obstacleBlock;
	public int adjustX;
	public int adjustY;

	public int mapsizeH = 15;
	public int mapsizeW = 10;
	public int[][] map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		new int[] { 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1 },
		new int[] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1 },
		new int[] { 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
		new int[] { 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
	};

	// Start is called before the first frame update
	void Start()
    {
		for (int i = 1; i <= mapsizeH; i++)
		{
			for (int j = 1; j <= mapsizeW; j++)
			{
				if (map[i][j] == 1)
				{
					GameObject obstacle = Instantiate(obstacleBlock, new Vector3(j - 5, -9, -i + 5), Quaternion.identity) as GameObject;
					obstacle.transform.parent = gameObject.transform;
					
				}
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
