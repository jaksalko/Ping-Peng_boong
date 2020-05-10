using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UiController : MonoBehaviour
{
    public GameObject inGame;
    public GameObject resultPopup;
    public GameObject popup;

    bool mini = false;

    public void MiniMapButton()
    {
        mini = GameController.instance.cameraController.MiniMapView(mini);
    }

    public void ChangeCharacter()
    {
        Player now = GameController.instance.nowPlayer;

        if(!now.Moving())
        {
            Debug.Log("change Character");
            GameController.instance.nowPlayer.isActive = false;
            
            if (now == GameController.instance.player1)
            {
                GameController.instance.nowPlayer = GameController.instance.player2;
            }
            else
            {
                GameController.instance.nowPlayer = GameController.instance.player1;
            }
            GameController.instance.nowPlayer.isActive = true;

            Debug.Log("player 1 : " + GameController.instance.player1.isActive);
            Debug.Log("player 2 : " + GameController.instance.player2.isActive);
        }
        
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Pause()
    {

    }

    public void Resume()
    {

    }

    public void GoLobby()
    {
        SceneManager.LoadScene("MainScene");
    }






}
