using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMove : MonoBehaviour
{
    private Vector2[] firstFingerPosition; // first finger position
    private Vector2[] lastFingerPosition; // last finger position
    private float angle;
    private float swipeDistanceX;
    private float swipeDistanceY;
    private float SWIPE_DISTANCE_X_CONST = 60;
    private float SWIPE_DISTANCE_Y_CONST = 150;
    private int touchFingerId = -1;

    public event System.Action<int> Move;

    
    private void FixedUpdate()
    {
        if(GameController.Running)
        {
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                GetKeyBoard();
            }
            else
            {
                SlideScreen();
            }
        }
       
        
        
    }

    void SlideScreen()
    {
        int touchCount = Input.touchCount;
        for (int i = 0; i < touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.Equals(null)) continue;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("Touch Began : " + i);
                    touchFingerId = touch.fingerId;
                    firstFingerPosition[i] = touch.position;
                    lastFingerPosition[i] = touch.position;
                    break;
                case TouchPhase.Moved:
                    if (touch.fingerId == touchFingerId)
                    {
                        lastFingerPosition[i] = touch.position;
                        swipeDistanceX = Mathf.Abs((lastFingerPosition[i].x - firstFingerPosition[i].x));
                        swipeDistanceY = Mathf.Abs((lastFingerPosition[i].y - firstFingerPosition[i].y));
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    Debug.Log("Touch End : " + i);
                    if (touch.fingerId == touchFingerId)
                    {
                        touchFingerId = -1;
                        angle = Mathf.Atan2((lastFingerPosition[i].x - firstFingerPosition[i].x), (lastFingerPosition[i].y - firstFingerPosition[i].y)) * 57.2957795f;

                        if (angle > -30 && angle < 30 && swipeDistanceY > SWIPE_DISTANCE_Y_CONST)
                        {
                            Debug.Log("up  swipe...");
                            if (Move != null)
                                Move(1);
                        }
                        else if (angle > 150 || angle < -150 && swipeDistanceY > SWIPE_DISTANCE_Y_CONST)
                        {
                            Debug.Log("down  swipe...");
                            if (Move != null)
                                Move(3);
                        }
                        else if (angle <= -50 && angle >= -110 && swipeDistanceX > SWIPE_DISTANCE_X_CONST)
                        {
                            Debug.Log("left swipe...");
                            if (Move != null)
                                Move(4);
                        }
                        else if (angle >= 50 && angle <= 110 && swipeDistanceX > SWIPE_DISTANCE_X_CONST)
                        {
                            Debug.Log("right swipe...");
                            if (Move != null)
                                Move(2);
                        }
                    }
                    break;
            }
        }
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
