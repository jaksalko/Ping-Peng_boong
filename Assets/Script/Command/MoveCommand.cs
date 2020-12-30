using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveCommand : ICommand
{
	Player player;
	int dir;
	Map map;
	Block[,] blocks;

	#region Change Object(Snow , cracker , parfait) List

	List<Vector2> erasedSnowList;
	int beforeParfaitOrder;
	Vector2[] parfaitPos;

#endregion


#region Character Data   1. state(State)     2. onCloud(bool)      3. temp(int)     4. transform(Transform)

Vector3 beforePositionA;
	Vector3 beforePositionB;

	Player.State beforeStateA;// idle , master , slave
	Player.State beforeStateB;

	bool beforeOnCloudA;
	bool beforeOnCloudB;

	int beforeTempA;
	int beforeTempB;

	#endregion

	List<KeyValuePair<Vector2, int>> beforeParfaitPos;
	List<Vector2> beforeSnow; // 후 입력
	List<KeyValuePair<Vector2, Vector2>> beforeCracker; // 후 입력
													//남은 눈의 갯수는 복구된 체크 배열로 다시 계산가능
													//사라진 눈의 재생성도 마찬가지
													//파르페 얼음은 오더로 계산가능
													//크래커가 문제...

	public MoveCommand(Player p, Map m, int direction)
	{
		player = p;

		map = m;
		dir = direction;

		beforePositionA = player.transform.position;

		beforeStateA = player.state;

		beforeTempA = player.temp;

		beforeOnCloudA = player.onCloud;

		/*
        Player player1 = GameController.instance.player1;
        Player player2 = GameController.instance.player2;

        beforePositionA = player1.transform.position;
        beforePositionB = player2.transform.position;

        beforeStateA = player1.state;
        beforeStateB = player2.state;

        beforeTempA = player1.temp;
        beforeTempB = player2.temp;

        beforeOnCloudA = player1.onCloud;
        beforeOnCloudB = player2.onCloud;
		*/

		beforeParfaitOrder = GameController.ParfaitOrder;
		parfaitPos = map.FindParfaitBlocks();

		beforeSnow = new List<Vector2>();

		beforeCracker = map.FindCrackerBlocks();
	}

	public void SetLaterData(List<Vector2> snowList, List<KeyValuePair<Vector2, int>> crackerList)  //call by player
	{
		beforeSnow.AddRange(snowList);
		//Map -> ErasedSnowList
	}
	public void Execute()
	{
		player.Move(map, dir);
	}

	public void Undo()
	{
		/*RETURN TO BEFORE STATE
         * GameController
         * Player(Both)
        */

		// throw new System.NotImplementedException();

		int posX = (int)player.transform.position.x;
		int posZ = (int)player.transform.position.z;
		map.SetBlockData(posX, posZ, beforeTempA);

		player.transform.position = beforePositionA;

		player.state = beforeStateA;

		player.onCloud = beforeOnCloudA;

		// parfait
		if (GameController.ParfaitOrder != beforeParfaitOrder)
		{
			int pre = beforeParfaitOrder;
			int now = GameController.ParfaitOrder;

			GameObject[] parfaits;
			parfaits = GameObject.FindGameObjectsWithTag("Parfait");

			Transform[] children = parfaits[pre].GetComponentsInChildren<Transform>(true);
			children[1].gameObject.SetActive(true);

			GameObject preObj = parfaits[pre].gameObject;
			preObj.GetComponent<ParfaitBlock>().Activate();

			GameObject nowObj = parfaits[now].gameObject;
			nowObj.GetComponent<ParfaitBlock>().Deactivate();

			GameController.ParfaitOrder = beforeParfaitOrder;
			Vector2 bp_pos = parfaitPos[beforeParfaitOrder];

			int blockType;
			if (beforeParfaitOrder == 0)
			{
				blockType = BlockNumber.parfaitA;
				map.SetBlockData((int)bp_pos.y, (int)bp_pos.x, blockType);
			}
			else if (beforeParfaitOrder == 1)
			{
				blockType = BlockNumber.parfaitB;
				map.SetBlockData((int)bp_pos.y, (int)bp_pos.x, blockType);
			}
			else if (beforeParfaitOrder == 2)
			{
				blockType = BlockNumber.parfaitC;
				map.SetBlockData((int)bp_pos.y, (int)bp_pos.x, blockType);
			}
			else if (beforeParfaitOrder == 3)
			{
				blockType = BlockNumber.parfaitD;
				map.SetBlockData((int)bp_pos.y, (int)bp_pos.x, blockType);
			}
		}

		// snow
		GameObject[] grounds;
		grounds = GameObject.FindGameObjectsWithTag("Ground");
		foreach (GameObject ground in grounds)
		{
			Vector2 groundpos = new Vector2(ground.transform.position.z, ground.transform.position.x);
			int blockType = map.GetBlockData((int)groundpos.y, (int)groundpos.x);
			if (blockType == BlockNumber.normal || blockType == BlockNumber.upperNormal)
			{
				int index = beforeSnow.IndexOf(groundpos);
				if (index > -1)
				{
					map.UpdateCheckArray((int)groundpos.y, (int)groundpos.x, false);
					GameController.instance.RemainCheck();

					Transform[] children = ground.transform.GetChild(0).GetComponentsInChildren<Transform>(true);
					foreach (Transform child in children)
					{
						child.gameObject.SetActive(true);
					}
				}
			}
		}

		// cracker
		List<KeyValuePair<Vector2, Vector2>> afterCracker = map.FindCrackerBlocks();

		var changeList = beforeCracker.Except(afterCracker);

		foreach (KeyValuePair<Vector2, Vector2> cracker in changeList)
		{
			Vector2 crackerpos = new Vector2(cracker.Key.y, cracker.Key.x);

			int blockType = map.GetBlockData((int)crackerpos.x, (int)crackerpos.y);
			if (blockType == BlockNumber.broken || cracker.Value.y == BlockNumber.cracked)
			{
				map.SetBlockData((int)crackerpos.x, (int)crackerpos.y, BlockNumber.cracked);
			}
			else if (blockType == BlockNumber.broken || cracker.Value.y == BlockNumber.upperCracked)
			{
				map.SetBlockData((int)crackerpos.x, (int)crackerpos.y, BlockNumber.upperCracked);
			}

			map.SetCrackerCount((int)crackerpos.x, (int)crackerpos.y, (int)cracker.Value.x);

		}

	}


}