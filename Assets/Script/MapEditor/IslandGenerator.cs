using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandGenerator : MonoBehaviour
{
    public Vector2 maxSize; // input max size width and height
    public InputField input;
    public List<int> splitToInt;
    string row = ""; // width one line
    
    public SampleMap sample;

    int characterCount = 0;

    private void Awake()
    {
        
        sample.mapsizeH = (int)maxSize.y;
        sample.mapsizeW = (int)maxSize.x;

    }
  
    public void SplitLine(string datas)
    {
        row = datas;

        string[] split = row.Split(' ');
       
        for(int i = 0; i < split.Length; i++)
        {
            int data = int.Parse(split[i]);
            if (splitToInt.Count == maxSize.x * maxSize.y)
            {
                Debug.Log("done map data");
                SampleMap newMap = Instantiate(sample, default);
                newMap.gameObject.transform.SetParent(transform);
                break;

            }



            splitToInt.Add(data);

            int x = splitToInt.Count / (int)maxSize.x;
            int z = (splitToInt.Count - 1) % (int)maxSize.x;

            if(data >= BlockNumber.parfaitA && data <= BlockNumber.parfaitD)
            {
                sample.parfait = true;
            }
            else if(data >= BlockNumber.upperParfaitA && data <= BlockNumber.upperParfaitD)
            {
                sample.parfait = true;
            }

            if (characterCount == 0 && (data == BlockNumber.character || data == BlockNumber.upperCharacter))
            {
                

                switch (data)
                {



                    case BlockNumber.character://27
                        sample.startPositionA = new Vector3(x, -9, z);
                        sample.startUpstairA = false;
                        break;
                    case BlockNumber.upperCharacter://38
                        sample.startPositionA = new Vector3(x, -8, z);
                        sample.startUpstairA = true;
                        break;


                }
                characterCount++;
            }
            else if (characterCount == 1 && (data == BlockNumber.character || data == BlockNumber.upperCharacter))
            {
                switch (data)
                {
                    case BlockNumber.character://27
                        sample.startPositionB = new Vector3(x, -9, z);
                        sample.startUpstairB = false;
                        break;
                    case BlockNumber.upperCharacter://38
                        sample.startPositionB = new Vector3(x, -8, z);
                        sample.startUpstairB = true;
                        break;


                }
                characterCount++;
            }


        }


    }
    #region Editor
    public void GenerateMap(string datas)
    {
        Debug.Log(datas);

        SplitLine(datas);


    }
    #endregion

}
