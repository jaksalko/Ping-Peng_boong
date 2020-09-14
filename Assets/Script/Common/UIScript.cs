using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


    public static class IslandData
    {
        public const int
            tutorial = 4,
            iceCream = 15;
        public const string
            stage1 = "Tutorial_Island",
            stage2 = "Beach_Island";


    }



public class UIScript : MonoBehaviour
{
    public void Load_Island(int stage)
    {
        if(stage < IslandData.tutorial)
        {
            SceneManager.LoadScene("Tutorial_Island");
        }
        else if(stage < IslandData.iceCream)
        {
            SceneManager.LoadScene("Beach_Island");
        }
    }
}
