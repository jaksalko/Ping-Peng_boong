using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class MapGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] blockPrefab;
    public Indexer blockPositionPrefab;

    [Header("Camera")]
    Camera cam;
    public Slider fovSlider;
    public Vector2 fovMinMax;
    public float cameraHeight;
    Vector3 center;
    float angle;
    float distance;


    [Header("Holder")]
    public Holder positionHolder;
    public Holder firstFloorHolder;
    public Holder secondFloorHolder;
    public Holder thirdFloorHolder;
    


    [Header("MapGenerating")]
    [SerializeField] GameObject selectedPrefab;
    public SampleMap newMap;
    public List<Indexer> indexer;
    public Vector2 maxSize;
    RaycastHit hit;
    int selected_id;
    int slope_id;
    public Text warning;

    public Simulator simulator;

    bool erase = false;

    private void Awake()
    {
        maxSize = GoogleInstance.instance.maxSize;
        BlockPositionEditor();
        cam = Camera.main;
        center = new Vector3(maxSize.x / 2 -0.5f , 0f, maxSize.y / 2 - 0.5f);
        angle = -90f;
        distance = maxSize.x;

       
        
#if UNITY_EDITOR
        this.UpdateAsObservable()
            .Where(_ => !erase)
               .Where(_ => Input.GetMouseButton(0))
               .Select(ray => cam.ScreenPointToRay(Input.mousePosition))
               .Subscribe(ray => MakeBlock(ray));


        this.UpdateAsObservable()
           .Where(_ => erase)
              .Where(_ => Input.GetMouseButtonDown(0))
              .Select(ray => cam.ScreenPointToRay(Input.mousePosition))
              .Subscribe(ray => EraseBlock(ray));
#else

        this.UpdateAsObservable()
                .Where(_ => !erase)
                .Where(_ => Input.touchCount > 0)
                .Where(_ => Input.GetTouch(0).phase == TouchPhase.Moved)
               .Select(ray => cam.ScreenPointToRay(Input.GetTouch(0).position))
               .Subscribe(ray => MakeBlock(ray));

        this.UpdateAsObservable()
               .Where(_ => erase)
               .Where(_ => Input.touchCount > 0)
               .Where(_ => Input.GetTouch(0).phase == TouchPhase.Began)
              .Select(ray => cam.ScreenPointToRay(Input.GetTouch(0).position))
              .Subscribe(ray => EraseBlock(ray));
#endif
        TopView();
       
    }
    void EraseBlock(Ray ray)
    {
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.transform.CompareTag("Indexer"))
            {
                Indexer indexer = hit.transform.GetComponent<Indexer>();
                if(indexer.Floor != 0)
                {
                    switch(indexer.Floor)
                    {
                        case 1://first floor
                            for(int i = 0; i < firstFloorHolder.transform.childCount; i++)
                            {
                                Transform firstFloorObject = firstFloorHolder.transform.GetChild(i);
                                if(firstFloorObject.localPosition.x == indexer.X && firstFloorObject.localPosition.z == indexer.Z)
                                {
                                    Destroy(firstFloorObject.gameObject);
                                    indexer.Floor = 0;
                                    indexer.data = 1;//obstacle
                                    break;
                                }
                            }
                            
                            break;
                        case 2://second floor
                            for (int i = 0; i < secondFloorHolder.transform.childCount; i++)
                            {
                                Transform secondFloorObject = secondFloorHolder.transform.GetChild(i);
                                if (secondFloorObject.localPosition.x == indexer.X && secondFloorObject.localPosition.z == indexer.Z)
                                {
                                    Destroy(secondFloorObject.gameObject);
                                    indexer.Floor = 1;
                                    indexer.data = 0;
                                    break;
                                }
                            }
                           
                            break;
                        case 3://third floor
                            for (int i = 0; i < thirdFloorHolder.transform.childCount; i++)
                            {
                                Transform thirdFloorObject = thirdFloorHolder.transform.GetChild(i);
                                if (thirdFloorObject.localPosition.x == indexer.X && thirdFloorObject.localPosition.z == indexer.Z)
                                {
                                    Destroy(thirdFloorObject.gameObject);
                                    indexer.Floor = 2;
                                    indexer.data = 2;
                                    break;
                                }
                            }
                           
                            break;

                    }
                    if(indexer.data == 5)//character erase
                    {
                        if(blockPrefab[4].activeSelf && blockPrefab[4].transform.position.x == indexer.X && blockPrefab[4].transform.position.z == indexer.Z)
                        {
                            //char1 erase
                            blockPrefab[4].SetActive(false);
                            blockPrefab[4].transform.position = default;
                            indexer.Floor--;
                           
                        }
                        if (blockPrefab[5].activeSelf && blockPrefab[5].transform.position.x == indexer.X && blockPrefab[5].transform.position.z == indexer.Z)
                        {
                            //char2 erase
                            blockPrefab[5].SetActive(false);
                            blockPrefab[5].transform.position = default;
                            indexer.Floor--;
                        }

                        if (indexer.Floor == 1)
                            indexer.data = 0;
                        else if (indexer.Floor == 2)
                            indexer.data = 2;
                    }
                }
            }
        }
    }
    void MakeBlock(Ray ray)
    {
        if(Physics.Raycast(ray,out hit , 1000))
        {
            if(hit.transform.CompareTag("Indexer"))
            {
                if (selectedPrefab == null)
                    warning.text = "블럭을 선택해주세요";
                else
                {
                    warning.text = "";
                    Indexer indexer = hit.transform.GetComponent<Indexer>();
                    if (indexer.Floor == 0 && selected_id == 0)
                    {
                        GameObject selected = Instantiate(selectedPrefab, new Vector3(indexer.X, 0, indexer.Z), selectedPrefab.transform.rotation);
                        firstFloorHolder.SetParent(selected);
                        indexer.Floor = 1;
                        indexer.data = 0;
                    }
                    else if (indexer.Floor == 1 && selected_id != 0)//first floor and not selected block
                    {
                        if ((selected_id == 4 || selected_id == 5))//if character setting
                        {
                            if(!selectedPrefab.activeSelf)
                            {
                                if(selected_id == 4)
                                {
                                    newMap.startUpstairA = false;
                                    newMap.startPositionA = new Vector3(indexer.X+1, -9, indexer.Z+1);
                                }
                                else if(selected_id == 5)
                                {
                                    newMap.startUpstairB = false;
                                    newMap.startPositionB = new Vector3(indexer.X+1, -9, indexer.Z+1);
                                }
                                selectedPrefab.SetActive(true);
                                selectedPrefab.transform.position = new Vector3(indexer.X, 0.5f, indexer.Z);
                                
                                indexer.data = 5;
                            }
                            else
                            {
                                warning.text = "이미 존재합니다";
                                return;
                            }
                          

                            
                        }
                        else if(selected_id == 3)
                        {
                            GameObject selected = Instantiate(selectedPrefab, new Vector3(indexer.X, 0, indexer.Z), Quaternion.Euler(new Vector3(0,90 * slope_id , 0)));
                            secondFloorHolder.SetParent(selected);
                            indexer.data = 21 + slope_id;
                        }
                        else// 1 2  : 장애물 , 2층
                        {
                            GameObject selected = Instantiate(selectedPrefab, new Vector3(indexer.X, 0, indexer.Z), selectedPrefab.transform.rotation);
                            secondFloorHolder.SetParent(selected);
                            indexer.data = selected_id;
                            
                        }
                      
                        indexer.Floor = 2;

                        if (selected_id != 2)
                            indexer.isFull = true;
                        
                    }
                    else if (indexer.Floor == 2 && !indexer.isFull && selected_id != 0 && selected_id != 2 && selected_id != 3)
                    {
                        if ((selected_id == 4 || selected_id == 5))//character setting
                        {
                            if (!selectedPrefab.activeSelf)
                            {
                                if (selected_id == 4)
                                {
                                    newMap.startUpstairA = true;
                                    newMap.startPositionA = new Vector3(indexer.X+1, -8, indexer.Z+1);
                                }
                                else if (selected_id == 5)
                                {
                                    newMap.startUpstairB = true;
                                    newMap.startPositionB = new Vector3(indexer.X+1, -8, indexer.Z+1);
                                }
                                selectedPrefab.SetActive(true);
                                selectedPrefab.transform.position = new Vector3(indexer.X, 1.5f, indexer.Z);
                                //thirdFloorHolder.SetParent(selectedPrefab);
                                indexer.data = 5;
                            }
                            else
                            {
                                warning.text = "이미 존재합니다";
                                return;
                            }



                        }
                        else
                        {
                            GameObject selected = Instantiate(selectedPrefab, new Vector3(indexer.X, 0, indexer.Z), selectedPrefab.transform.rotation);
                            thirdFloorHolder.SetParent(selected);
                            indexer.data = 3;
                        }
                       
                        indexer.Floor = 3;
                        indexer.isFull = true;
                    }
                   
                }
               
                
                
                
            }
        }
        
    }

#region Editor
    public void BlockPositionEditor()
    {
        positionHolder.ClearHolder();
        indexer.Clear();
        for(int i = 0; i < maxSize.x; i++)//가로
        {
            for(int j = 0; j < maxSize.y; j++)//세로
            {
                Indexer newPositionBlock = Instantiate(blockPositionPrefab, new Vector3(i, 0, j), blockPositionPrefab.transform.rotation);
                newPositionBlock.SetXZ(i, j);
                newPositionBlock.name = "Quad(" + i + "," + j + ")";
                indexer.Add(newPositionBlock);
                positionHolder.SetParent(newPositionBlock.gameObject);
            }
        }

        
    }
#endregion


#region CAMERA Button Function
    public void FovControl()
    {
        float fov = Mathf.Lerp(fovMinMax.y, fovMinMax.x, fovSlider.value);
        cam.fieldOfView = fov;
    }
    public void TopView()
    {
        cam.transform.position = center + Vector3.up*cameraHeight;
        cam.transform.LookAt(center);
    }
    public void SideView()//default front view
    {
        angle = -90f;
        float x = center.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = center.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);

        cam.transform.position = new Vector3(x, cameraHeight, z);
        cam.transform.LookAt(center);
    }

    public void RightSlide()
    {
        angle += 90;
        float x = center.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = center.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);

        cam.transform.position = new Vector3(x, cameraHeight, z);
        cam.transform.LookAt(center);

    }
    public void LeftSlide()
    {
        angle -= 90;
        float x = center.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = center.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);

        cam.transform.position = new Vector3(x, cameraHeight, z);
        cam.transform.LookAt(center);
    }
#endregion

    //Block Select Button Function
    public void SelectBlockButtonClicked(int id)
    {
        erase = false;
        selected_id = id;
        selectedPrefab = blockPrefab[selected_id];
        
    }
    public void SlopeButtonClicked(int id)
    {
        erase = false;
        selected_id = 3;
        slope_id = id;
        selectedPrefab = blockPrefab[selected_id];
    }
    public void EraseButtonClicked()
    {
        erase = true;

    }
    //Simulating Button Function
    public void StartSimulatorButtonClicked()
    {
        newMap.mapsizeH = (int)maxSize.y + 2;
        newMap.mapsizeW = (int)maxSize.x + 2;

        for (int i = 0; i < newMap.mapsizeH; i++)
        {
            SampleMap.Line line = new SampleMap.Line();
            for (int j = 0; j < newMap.mapsizeW; j++)
            {
                line.line.Add(1);
            }
            newMap.lines.Add(line);
        }
            
        
        for(int i = 0; i < indexer.Count; i++)
        {
            
            newMap.lines[indexer[i].Z+1].line[indexer[i].X+1] = indexer[i].data;

        }

        newMap.parfait = false;
        newMap.LineToMap();
        simulator.StartSimulator();
    }

    //Reset Button
    public void ResetMapEditorButtonClicked()
    {
        firstFloorHolder.ClearHolder();
        secondFloorHolder.ClearHolder();
        thirdFloorHolder.ClearHolder();

        positionHolder.ClearHolder();

        blockPrefab[4].SetActive(false);
        blockPrefab[4].transform.position = default;


        blockPrefab[5].SetActive(false);
        blockPrefab[5].transform.position = default;
        for (int i = 0; i < maxSize.x; i++)
        {
            for (int j = 0; j < maxSize.y; j++)
            {
                Indexer newPositionBlock = Instantiate(blockPositionPrefab, new Vector3(j, 0, i), blockPositionPrefab.transform.rotation);
                newPositionBlock.SetXZ(j, i);
                newPositionBlock.name = "Quad(" + j + "," + i + ")";
                positionHolder.SetParent(newPositionBlock.gameObject);
            }
        }
    }
}
