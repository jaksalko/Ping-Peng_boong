using System.Collections;
using System.Collections.Generic;// let us use lists
using UnityEngine;
using System.Xml;               // basic xml attributes
using System.Xml.Serialization; // access xmlserializer
using System.IO;                //file management
using System.Text;
using UnityEngine.UI;

public class XMLManager : MonoBehaviour
{

    public static XMLManager ins = null;//terrible singleton pattern
                                        // Use this for initialization
    private void Awake()
    {
        Debug.Log("XMLManager awake");

        if (ins == null)
        {
            Debug.Log("instance is null");
            ins = this;
        }
        else if (ins != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //list of items 
    public ItemDatabase itemDB;

    //save function
    public void SaveItems()
    {
        //open new xml file
        string path;
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));



#if UNITY_EDITOR
        using (FileStream stream = new FileStream(
            Application.dataPath + "/XML/item_data.xml", FileMode.Create))
        {
            StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(sw, itemDB); // put into sml files)
            sw.Close();//important :)
        }


#elif UNITY_IOS || UNITY_ANDROID
          
        path = Application.persistentDataPath;
        
        if(!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

            using (FileStream stream = new FileStream(
            path + "/item_data.xml", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);
                serializer.Serialize(sw, itemDB); // put into sml files)
                sw.Close();//important :)
            }
#endif


    }
    //load function
    public void LoadItems()
    {
        string path;
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));




#if UNITY_EDITOR

        try
        {
            FileStream stream = new FileStream(
           Application.dataPath + "/XML/item_data.xml", FileMode.Open);
            itemDB = serializer.Deserialize(stream) as ItemDatabase;
            stream.Close();

        }
        catch(System.Exception e)
        {
            Debug.LogWarning(e.ToString());
            itemDB.Initialize();
            SaveItems();
        }




#elif UNITY_IOS || UNITY_ANDROID

        
        try
        {
        path = Application.persistentDataPath;
        //path += "datas";
        FileStream stream = new FileStream(
            path + "/item_data.xml", FileMode.Open);
            itemDB = serializer.Deserialize(stream) as ItemDatabase;
            stream.Close();
        }
        catch(System.Exception e)
        {
            GoogleInstance.instance.SetText(e.ToString());
            itemDB.Initialize();
            SaveItems();
        }

#endif



    }


}

[System.Serializable]
public class StageStep
{
    public int stage =0;
    public int step =0;//min

   

    

    public void SetStep(int current)
    {
        if(current < step)
        {
            step = current;
        }
    }

}

[System.Serializable]
public class ItemDatabase
{
    //List<Dictionary<string, object>> data;

    [XmlArray("StageStepList")]
    public List<StageStep> stepList = new List<StageStep>();

    [XmlElement("level")]
    public int level;

    
    public void Initialize()//NewGame
    {
        //data = CSVReader.Read("makemoneydatasheet3");
        Debug.Log("initialize");
        level = 0;
        for(int i = 0; i <= IslandData.lastLevel; i++)
        {
            StageStep stageStep = new StageStep();
            stageStep.stage = i;
            stageStep.step = 9999;
            stepList.Add(stageStep);
        }
    }

    public void LevelUp()
    {
        level++;
        
    }
 
         


}