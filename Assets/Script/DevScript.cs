using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DevScript : MonoBehaviour
{
    

   
    public void LevelUp()
    {
        
        int level = PlayerPrefs.GetInt("level", 0);
        if(level <= 29)
        {
            PlayerPrefs.SetInt("level", level + 1);
            SceneManager.LoadScene("GameScene");

        }
        
    }
    public void LevelDown()
    {
        int level = PlayerPrefs.GetInt("level", 0);
        if(level > 0)
        {
            PlayerPrefs.SetInt("level", level - 1);
            SceneManager.LoadScene("GameScene");
        }
        
    }
}
