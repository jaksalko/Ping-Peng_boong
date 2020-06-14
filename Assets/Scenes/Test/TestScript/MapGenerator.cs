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

    private void Awake()
    {
        cam = Camera.main;
        center = new Vector3(maxSize.x / 2, 0f, maxSize.y / 2);
        angle = -90f;
        distance = maxSize.x;

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0))
            .Select(ray => cam.ScreenPointToRay(Input.mousePosition))
            .Subscribe(ray => MakeBlock(ray));

    }
    void MakeBlock(Ray ray)
    {
        if(Physics.Raycast(ray,out hit , 1000))
        {
            if(hit.transform.CompareTag("Indexer"))
            {
                if (selectedPrefab == null)
                    warning.text = "Please select a block below";
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
                    else if (indexer.Floor == 1 && selected_id != 0)
                    {
                        if ((selected_id == 4 || selected_id == 5))//character setting
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
                                //secondFloorHolder.SetParent(selectedPrefab);
                                indexer.data = 5;
                            }
                            else
                            {
                                warning.text = "already exist";
                                return;
                            }
                          

                            
                        }
                        else if(selected_id == 3)
                        {
                            GameObject selected = Instantiate(selectedPrefab, new Vector3(indexer.X, 0, indexer.Z), Quaternion.Euler(new Vector3(0,90 * slope_id , 0)));
                            secondFloorHolder.SetParent(selected);
                            indexer.data = 21 + slope_id;
                        }
                        else// 1 2  : 2층 , 장애물 
                        {
                            GameObject selected = Instantiate(selectedPrefab, new Vector3(indexer.X, 0, indexer.Z), selectedPrefab.transform.rotation);
                            secondFloorHolder.SetParent(selected);
                            indexer.data = selected_id;
                            
                        }
                      
                        indexer.Floor = 2;
                        if (selected_id != 1)//기본 블럭이 아니면 더이상 쌓을 수 없음
                            indexer.isFull = true;
                        
                    }
                    else if (indexer.Floor == 2 && !indexer.isFull && selected_id != 0 && selected_id != 1 && selected_id != 3)
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
                                warning.text = "already exist";
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
        for(int i = 0; i < maxSize.x; i++)
        {
            for(int j = 0; j < maxSize.y; j++)
            {
                Indexer newPositionBlock = Instantiate(blockPositionPrefab, new Vector3(j, 0, i), blockPositionPrefab.transform.rotation);
                newPositionBlock.SetXZ(j, i);
                newPositionBlock.name = "Quad(" + j + "," + i + ")";
                indexer.Add(newPositionBlock);
                positionHolder.SetParent(newPositionBlock.gameObject);
            }
        }

        
    }
    #endregion


    #region CAMERA Button Function
    public void FovControl()
    {
        float fov = Mathf.Lerp(fovMinMax.x, fovMinMax.y, fovSlider.value);
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
        selected_id = id;
        selectedPrefab = blockPrefab[selected_id];
        
    }
    public void SlopeButtonClicked(int id)
    {
        selected_id = 3;
        slope_id = id;
        selectedPrefab = blockPrefab[selected_id];
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
