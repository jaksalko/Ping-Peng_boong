using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    Player player;
    int dir;
    Map map;

#region Change Object(Snow , cracker , parfait) List

    List<Vector2> erasedSnowList;
    List<KeyValuePair<Vector2, int>> changeCrackerList;
    int beforeParfaitOrder;

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


    List<Vector2> beforeSnow; // 후 입력
    List<KeyValuePair<Vector2, int>> beforeCracker; // 후 입력
    //남은 눈의 갯수는 복구된 체크 배열로 다시 계산가능
    //사라진 눈의 재생성도 마찬가지
    //파르페 얼음은 오더로 계산가능
    //크래커가 문제...

    public MoveCommand(Player p, Map m , int direction)
    {
        player = p;

        map = m;
        dir = direction;

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

        beforeParfaitOrder = GameController.ParfaitOrder;

        beforeSnow = new List<Vector2>();
        beforeCracker = new List<KeyValuePair<Vector2, int>>();
    }

    public void SetLaterData(List<Vector2> snowList , List<KeyValuePair<Vector2, int>> crackerList)//call by player? GameController?
    {
        beforeSnow.AddRange(snowList);
        beforeCracker.AddRange(crackerList);
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
        throw new System.NotImplementedException();
    }
}
