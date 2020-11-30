using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomsSceneResultPopup : MonoBehaviour
{
    public Text moveCount;
    public Text candyText;
    public Button retryButton;

    public void ShowResultPopup(int count)
    {
        moveCount.text = count.ToString();

        //user.candy == 0   --> AdButton active
        //else              --> retryButton active

        gameObject.SetActive(true);
    }

    public void GoLobbyButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void RetryButtonClicked()
    {
        //candy--
        //reload this custom stage...
    }

    public void AdButtonClicked()
    {
        //candy++
        //show ad...
    }
}
