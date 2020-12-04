using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevData : MonoBehaviour
{
    public Player p1;
    public Player p2;

    public Text p1Text;
    public Text p2Text;

    private void Awake()
    {
        if (GoogleInstance.instance.id == "TestPlayerID")
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {
        p1Text.text = "Player1 Data\n isMoving : " + p1.Moving() + "\n isActive : " + p1.isActive
            + "\n dir : " + p1.direction + "\n onCloud : " + p1.onCloud + "\n isLock : " + p1.isLock
            + "\n stateChange : "+p1.stateChange+"\n temp : " + p1.temp;

        p2Text.text = "Player2 Data\n isMoving : " + p2.Moving() + "\n isActive : " + p2.isActive
            + "\n dir : " + p2.direction + "\n onCloud : " + p2.onCloud + "\n isLock : " + p2.isLock
             + "\n stateChange : " + p2.stateChange + "\n temp : " + p2.temp;
    }
}
