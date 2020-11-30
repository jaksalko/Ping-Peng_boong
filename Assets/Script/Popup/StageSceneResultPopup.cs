using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSceneResultPopup : UIScript
{
    public Text moveCount;

    public void ShowResultPopup(int count)
    {
        moveCount.text = count.ToString();
        gameObject.SetActive(true);
    }

    public void GoLobbyButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void NextStageButtonClicked()
    {
        Load_Island(GoogleInstance.instance.nowLevel);
    }
}
