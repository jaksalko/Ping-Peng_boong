using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomsSceneResultPopup : MonoBehaviour
{
    JsonAdapter jsonAdapter = new JsonAdapter();

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
        SceneManager.LoadScene("CustomMapPlayScene");//customMode Scene
    }

    public void PushButtonClicked()
    {
        JsonData jsonData = GoogleInstance.instance.playCustomData.itemdata;

        var json = JsonUtility.ToJson(jsonData);
        StartCoroutine(jsonAdapter.API_POST("map/push", json));
        StartCoroutine(jsonAdapter.API_POST("editorPlay/push", json));

        //map push++
        //candy++
        //show ad...
    }
}
