using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomMapItem : MonoBehaviour
{
    JsonAdapter jsonAdapter = new JsonAdapter();

    public Text title;
    public Text maker;
    public Text moveCount;
    public Toggle isClear;
    // Start is called before the first frame update
    public JsonData itemdata;

    public bool isPlayed;
    public void Initialize(JsonData item)
    {
        name = item.updateTime;
        itemdata = item;
        title.text = itemdata.title;
        maker.text = itemdata.nickname;
        moveCount.text = itemdata.moveCount.ToString();

        List<CustomStagePlayerData> myPlayData = GoogleInstance.instance.customStagePlayerDatas;
        isPlayed = false;
        for (int i = 0; i < myPlayData.Count; i++)
        {
            if(myPlayData[i].title == itemdata.title)
            {
                isPlayed = true;
                break;
            }
        }

    }

    public void PlayButton()
    {
        GoogleInstance.instance.playCustomData = this;
        //popularity++
        if(!isPlayed)
        {
            
            var json = JsonUtility.ToJson(itemdata);
            StartCoroutine(jsonAdapter.API_POST("map/play", json));//popularity++

        }
        SceneManager.LoadScene("CustomMapPlayScene");//customMode Scene
    }
}
