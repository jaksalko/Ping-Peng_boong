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
    public Vector2 maxSize;
    RaycastHit hit;
    int selected_id;

    public Text warning;

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
                    if (indexer.floor == 0 && selected_id == 0)
                    {
                        GameObject selected = Instantiate(selectedPrefab, new Vector3(indexer.X, 0, indexer.Z), selectedPrefab.transform.rotation);
                        firstFloorHolder.SetParent(selected);
                        indexer.floor = 1;
                    }
                    else if (indexer.floor == 1 && selected_id != 0)
                    {
                        GameObject selected = Instantiate(selectedPrefab, new Vector3(indexer.X, 0, indexer.Z), selectedPrefab.transform.rotation);
                        secondFloorHolder.SetParent(selected);
                        indexer.floor = 2;
                        if (selected_id != 1)//기본 블럭이 아니면 더이상 쌓을 수 없음
                            indexer.isFull = true;
                    }
                    else if (indexer.floor == 2 && !indexer.isFull && selected_id != 0 && selected_id != 1 && selected_id != 3)
                    {
                        GameObject selected = Instantiate(selectedPrefab, new Vector3(indexer.X, 0, indexer.Z), selectedPrefab.transform.rotation);
                        thirdFloorHolder.SetParent(selected);
                        indexer.floor = 3;
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
        for(int i = 0; i < maxSize.x; i++)
        {
            for(int j = 0; j < maxSize.y; j++)
            {
                Indexer newPositionBlock = Instantiate(blockPositionPrefab, new Vector3(j, 0, i), blockPositionPrefab.transform.rotation);
                newPositionBlock.SetXZ(j, i);
                newPositionBlock.name = "Quad(" + j + "," + i + ")";
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

    #region Block Select Button Function
    public void SelectBlockButtonClicked(int id)
    {
        selected_id = id;
        selectedPrefab = blockPrefab[selected_id];
    }
    #endregion
}
