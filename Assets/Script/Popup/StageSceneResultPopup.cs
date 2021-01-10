using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StageSceneResultPopup : UIScript
{
    public TextMeshProUGUI moveCount;
    public Image[] starImage;

    public void ShowResultPopup(int move_count , int star_count)
    {
        moveCount.text = move_count.ToString();
        starImage[star_count].gameObject.SetActive(true);
        gameObject.SetActive(true);

    }

    public void GoLobbyButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ReplayButtonClicked()
    {
        
        if(awsManager.user.heart > 0)
        {
            awsManager.user.heart -= 1;
            Load_Island(GameManager.instance.nowLevel);
        }
    }
    public void NextStageButtonClicked()
    {
        gameManager.nowLevel++;
        Load_Island(GameManager.instance.nowLevel);
    }
}
