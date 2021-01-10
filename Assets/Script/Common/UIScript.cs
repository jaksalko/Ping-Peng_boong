using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


   

public class UIScript : MonoBehaviour
{
    protected GameManager gameManager;
    protected AWSManager awsManager;
    void Start()
    {
        gameManager = GameManager.instance;
        awsManager = AWSManager.instance;
    }
    public void Load_Island(int stage)
    {
        if(stage <= IslandData.tutorial)
        {
            SceneManager.LoadScene("Tutorial_Island");
        }
        else if(stage <= IslandData.iceCream)
        {
            SceneManager.LoadScene("Icecream_Island");
        }
        else if (stage <= IslandData.beach)
        {
            SceneManager.LoadScene("Beach_Island");
        }
        else if (stage <= IslandData.cracker)
        {
            SceneManager.LoadScene("Cracker_Island");
        }
        else if (stage <= IslandData.cottoncandy)
        {
            SceneManager.LoadScene("Cottoncandy_Island");
        }
    }

	public string Island_Name(int stage)
	{
        if (stage <= IslandData.tutorial)
        {
            return "Tutorial";
        }
        else if (stage <= IslandData.iceCream)
        {
            return "Icecream";
        }
        else if (stage <= IslandData.beach)
        {
            return "Beach";
        }
        else if (stage <= IslandData.cracker)
        {
            return "Cracker";
        }
        else if (stage <= IslandData.cottoncandy)
        {
            return "Cottoncandy";
        }

        return "";
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
