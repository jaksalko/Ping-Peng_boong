using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UniRx;
using UniRx.Triggers;

public class TouchMove : MonoBehaviour
{
    
    public event Action<int> Move;

    public Text swipe;
    Vector2 startpos;
    Vector2 v;

    bool move = false;

    private void Awake()
    {
#if UNITY_EDITOR
#endif
        Debug.Log("unity");
#if UNITY_IOS
#endif
        Debug.Log("ios");
#if UNITY_ANDROID
        Debug.Log("Android");
#endif

    }

    private void FixedUpdate()//not using update method but uniRx
    {
        if(GameController.Playing)
        {
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
            {
                GetKeyBoard();
                SwipeMouse();
            }
            else
            {
                SlideScreen();
            }
        }

		

	}

    private void Start()
    {
        //uniRx code here.

    }

    void SwipeMouse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startpos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            string dir = "";
            v = (Vector2)Input.mousePosition - startpos;
            v = v.normalized;
            swipe.text = "(" + v.x + "," + v.y + ")" + dir;
            if ((v.x > -0.5 && v.x < 0.5) && (v.y > 0.5))
            {
                dir = "up";
                //Debug.Log("move up");
                if (Move != null)
                    Move(1);
            }
            else if((v.y > -0.5 && v.y < 0.5) && (v.x > 0.5))
            {
                dir = "right";
                //Debug.Log("move right");
                if (Move != null)
                    Move(2);
            }
            else if((v.x > -0.5 && v.x < 0.5) && (v.y < -0.5))
            {
                dir = "down";
                //Debug.Log("move down");
                if (Move != null)
                    Move(3);
            }
            else if ((v.y > -0.5 && v.y < 0.5) && (v.x < -0.5))
            {
                dir = "left";
                //Debug.Log("move left");
                if (Move != null)
                    Move(4);
            }
            else
            {
                dir = "nothing";
            }
            swipe.text = "(" + v.x + "," + v.y + ")" + dir;
        }
    }

    void SlideScreen()
    {
        
        int touchCount = Input.touchCount;
        if(Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            
            Touch touch = Input.GetTouch(0);
            //if (touch.Equals(null)) continue;

            string dir = "";
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startpos = touch.position;
                    
                    break;

                case TouchPhase.Moved:
                    v = touch.position;
                    break;
                case TouchPhase.Ended:
                    
                        v = touch.position - startpos;
                        v = v.normalized;

                        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
                        {
                            //좌우
                            if (v.x < 0)//좌
                            {
                                dir = "left";
                                //Debug.Log("move left");
                                if (Move != null)
                                    Move(4);
                            }
                            if (v.x > 0)
                            {
                                dir = "right";
                                //Debug.Log("move right");
                                if (Move != null)
                                    Move(2);
                            }
                        }
                        if (Mathf.Abs(v.x) < Mathf.Abs(v.y))
                        {
                            //상하
                            if (v.y < 0)
                            {
                                dir = "down";
                                //Debug.Log("move down");
                                if (Move != null)
                                    Move(3);
                            }
                            if (v.y > 0)
                            {

                                dir = "up";
                                //Debug.Log("move up");
                                if (Move != null)
                                    Move(1);
                            }
                        }

                        move = false;
                    
                    
                   

                    //else
                    //{
                    //    dir = "nothing";
                    //}
                    

                    //MoveEnd(touch);
                    break;

            }
            swipe.text = "(" + v.x + "," + v.y + ")" + dir;
        }
        //for (int i = 0; i < touchCount; i++)
        //{
            
        //    Touch touch = Input.GetTouch(0);
        //    //if (touch.Equals(null)) continue;

        //    string dir = "";
        //    switch (touch.phase)
        //    {
        //        case TouchPhase.Began:
        //            startpos = touch.position;
        //            break;

        //        case TouchPhase.Ended:
                    
        //            v = touch.position - startpos;
        //            v = v.normalized;

        //            if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        //            {
        //                //좌우
        //                if (v.x < 0)//좌
        //                {
        //                    dir = "left";
        //                    //Debug.Log("move left");
        //                    if (Move != null)
        //                        Move(4);
        //                }
        //                if (v.x > 0)
        //                {
        //                    dir = "right";
        //                    //Debug.Log("move right");
        //                    if (Move != null)
        //                        Move(2);
        //                }
        //            }
        //            if (Mathf.Abs(v.x) < Mathf.Abs(v.y))
        //            {
        //                //상하
        //                if (v.y < 0)
        //                {
        //                    dir = "down";
        //                    //Debug.Log("move down");
        //                    if (Move != null)
        //                        Move(3);
        //                }
        //                if (v.y > 0)
        //                {

        //                    dir = "up";
        //                    //Debug.Log("move up");
        //                    if (Move != null)
        //                        Move(1);
        //                }
        //            }

        //            //else
        //            //{
        //            //    dir = "nothing";
        //            //}
        //            swipe.text = "(" + v.x + "," + v.y + ")" + dir;

        //            //MoveEnd(touch);
        //            break;
              
        //    }
        //}
    }

    
    void GetKeyBoard()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("move up");
            if (Move != null)
                Move(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("move right");
            if (Move != null)
                Move(2);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("move down");
            if (Move != null)
                Move(3);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("move left");
            if (Move != null)
                Move(4);
        }
    }



}
