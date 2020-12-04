using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EditorPlayPopup : MonoBehaviour
{
    public CustomMapItem item_prefab;
    public Transform itemList;

    public InputField searchText;
    public Transform searchTab;

    List<CustomMapItem> customMapItems = new List<CustomMapItem>();

    private void OnEnable()
    {
        MakeCustomeMapItem();
    }

    public void ExitButton()
    {
        int listCount = customMapItems.Count;
        for (int i = 0; i < listCount; i++)
        {
            Destroy(customMapItems[i].gameObject);
        }
        customMapItems.Clear();
        
        gameObject.SetActive(false);
    }
    public void ClickLevel(int level)
    {

        GoogleInstance.instance.infiniteLevel = level;
        SceneManager.LoadScene("CustomMapPlayScene");

    }

    public void MakeCustomeMapItem()
    {
        Debug.Log("make map");
        List<JsonData> datas = GoogleInstance.instance.customMapdatas;
        for (int i = 0; i < datas.Count ; i++)
        {
            CustomMapItem newItem = Instantiate(item_prefab);
            newItem.Initialize(datas[i]);
            customMapItems.Add(newItem);
            newItem.transform.SetParent(itemList);
        }
    }

    public void Search()
    {
        int listCount = searchTab.childCount;
        for(int i = 0; i < listCount; i++)
        {
            searchTab.GetChild(0).transform.SetParent(itemList);
        }

        for(int i = 0; i < customMapItems.Count; i++)
        {
            if(customMapItems[i].title.text.Contains(searchText.text))
            {
                customMapItems[i].transform.SetParent(searchTab);
            }
        }
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
   
}
