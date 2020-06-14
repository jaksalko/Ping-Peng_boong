using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{

    public static Simulator instance;
    public GameObject simulatorUI;
    public Player player1;
    public Player player2;
    public Player nowPlayer;

    public Map simulatingMap;

    public GameObject simulatorObject;
    public GameObject generatorObject;

    private bool isRunning;
    public static bool Running
    {
        get { return instance.isRunning; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


    }

    public void StartSimulator()
    {

        //set Player...
        generatorObject.SetActive(false);
        simulatorObject.SetActive(true);

        simulatingMap.GenerateMap(0);
        player1.SetSimulatorData();
        player2.SetSimulatorData();

        player1.SetPosition(simulatingMap.sampleMap.startPositionA + new Vector3(0, -0.5f, 0), simulatingMap.sampleMap.startUpstairA); //position correction fix .. 5/13
        player2.SetPosition(simulatingMap.sampleMap.startPositionB + new Vector3(0, -0.5f, 0), simulatingMap.sampleMap.startUpstairB);

        nowPlayer = player1;
        nowPlayer.isActive = true;
        isRunning = true;
        simulatorUI.SetActive(true);

        player1.FindPlayer();
        player2.FindPlayer();
    }

    public void SimulatingSuccess()//Can Make Map
    {

    }

    
    
}
