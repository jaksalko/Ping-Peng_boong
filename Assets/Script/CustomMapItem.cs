using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomMapItem : MonoBehaviour
{
    public Text title;
    public Text maker;
    public Text moveCount;
    public Toggle isClear;
    // Start is called before the first frame update
    public JsonData itemdata;

    public void Initialize(JsonData item)
    {
        itemdata = item;
        title.text = itemdata.title;
        maker.text = itemdata.nickname;
        moveCount.text = itemdata.moveCount.ToString();
    }

    public void PlayButton()
    {
        GoogleInstance.instance.infiniteLevel = 1;
        SceneManager.LoadScene("CustomMapPlayScene");//customMode Scene
    }
}
