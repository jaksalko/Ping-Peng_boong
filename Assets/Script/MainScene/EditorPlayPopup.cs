using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class EditorPlayPopup : MonoBehaviour
{
    public void ClickLevel(int level)
    {

        GoogleInstance.instance.infiniteLevel = level;
        SceneManager.LoadScene("CustomMapPlayScene");

    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
   
}
