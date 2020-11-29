using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CubeMove_manager : MonoBehaviour
{
	//public Map _samplemap;
	public int adjustX;
	public int adjustZ;
	public float speed;
	public Text moveCountText;
	public GameObject player;
	public GameObject playerCamera;

	public int mapsizeH = 17;
	public int mapsizeW = 12;
	public int[][] map = new int[][] {
		new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },//아래
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
	int PlayerPosI;		// vertical
	int PlayerPosJ;		// horizental
	int movedir;
	int count = 0;
	[SerializeField]
	bool isMoving = false;
    
	Vector3 targetPos;
	

    // Start is called before the first frame update
    void Start()
    {
		//map = _samplemap.map;
		//mapsize = _samplemap.mapsize;

		FindPlayer();
		//player.transform.position = new Vector3(PlayerPosI + adjustX, -9, PlayerPosJ + adjustZ);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if(isMoving)
		{
			player.transform.position = Vector3.MoveTowards(player.transform.position, targetPos, speed);
			if (player.transform.position == targetPos)
			{
				isMoving = false;
				player.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
			}
		}
		else
		{
			PlayerControl();
		}

		moveCountText.text = "Move : " + count;
	}

	public void FindPlayer()
	{
		PlayerPosJ = (int)player.transform.position.x + adjustX;
		PlayerPosI = -(int)player.transform.position.z + adjustZ;

		Debug.Log("PlayerPosI : " + PlayerPosI + " PlayerPosJ : " + PlayerPosJ);
	}

	void PlayerControl()
	{
		if (Input.GetKeyDown(KeyCode.W) || movedir == 1)
		{
			player.transform.rotation = Quaternion.Euler(-90, 0, 0);

			movedir = 0;
			count++;
			map[PlayerPosI][PlayerPosJ] = 0;

			while (map[PlayerPosI - 1][PlayerPosJ] == 0)
			{
				PlayerPosI--;
			}

			Debug.Log(PlayerPosI + " " + PlayerPosJ);
			targetPos = new Vector3(PlayerPosJ - adjustX, player.transform.position.y, - PlayerPosI + adjustZ);
			map[PlayerPosI][PlayerPosJ] = 2;
			isMoving = true;
		}
		else if (Input.GetKeyDown(KeyCode.S) || movedir == 2)
		{
			player.transform.rotation = Quaternion.Euler(-90, 180, 0);

			movedir = 0;
			count++;
			map[PlayerPosI][PlayerPosJ] = 0;

			while (map[PlayerPosI + 1][PlayerPosJ] == 0)
			{
				PlayerPosI++;
			}
			Debug.Log(PlayerPosI + " " + PlayerPosJ);
			targetPos = new Vector3(PlayerPosJ - adjustX, player.transform.position.y, -PlayerPosI + adjustZ);
			map[PlayerPosI][PlayerPosJ] = 2;
			isMoving = true;
		}
		else if (Input.GetKeyDown(KeyCode.A) || movedir == 3)
		{
			player.transform.rotation = Quaternion.Euler(-90, -90, 0);

			movedir = 0;
			count++;
			map[PlayerPosI][PlayerPosJ] = 0;

			while (map[PlayerPosI][PlayerPosJ - 1] == 0)
			{
				PlayerPosJ--;
			}
			Debug.Log(PlayerPosI + " " + PlayerPosJ);
			targetPos = new Vector3(PlayerPosJ - adjustX, player.transform.position.y, -PlayerPosI + adjustZ);
			map[PlayerPosI][PlayerPosJ] = 2;
			isMoving = true;
		}
		else if (Input.GetKeyDown(KeyCode.D) || movedir == 4)
		{
			player.transform.rotation = Quaternion.Euler(-90, 90, 0);

			movedir = 0;
			count++;
			map[PlayerPosI][PlayerPosJ] = 0;

			while (map[PlayerPosI][PlayerPosJ + 1] == 0)
			{
				PlayerPosJ++;
			}
			Debug.Log(PlayerPosI + " " + PlayerPosJ);
			targetPos = new Vector3(PlayerPosJ - adjustX, player.transform.position.y, -PlayerPosI + adjustZ);
			map[PlayerPosI][PlayerPosJ] = 2;
			isMoving = true;
		}
	}

	public void PressArrowBtn(string dir)
	{
		switch(dir)
		{
			case "up":
				movedir = 1;
				break;
			case "down":
				movedir = 2;
				break;
			case "left":
				movedir = 3;
				break;
			case "right":
				movedir = 4;
				break;
			default:
				movedir = 0;
				break;
		}
	}

	public void ResetScene()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	public void SwitchPlayer(string playerName)
	{
		if(!isMoving)
		{
			player = GameObject.Find(playerName);
			//playerCamera.GetComponent<CameraController>().target = player.transform;
			FindPlayer();
		}
		else
		{
			Debug.Log("is moving");
		}

		Debug.Log("-----------------------------------");
		string array = "";
		for (int i = 1; i <= mapsizeH; i++)
		{
			for (int j = 1; j <= mapsizeW; j++)
			{
				array += map[i][j];
				array += " ";
			}
			Debug.Log(array);
			array = "";
		}
		Debug.Log("-----------------------------------");
	}
}
