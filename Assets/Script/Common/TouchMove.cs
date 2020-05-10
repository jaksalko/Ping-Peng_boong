using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public Text swipe;
    Vector2 startpos;
    Vector2 v;
     
    
    private void FixedUpdate()
    {
        if(GameController.Running)
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

		swipe.text = "(" + v.x + "," + v.y + ")";

	}

    void SwipeMouse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startpos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            v = (Vector2)Input.mousePosition - startpos;
            v = v.normalized;

            if((v.x > -0.5 && v.x < 0.5) && (v.y > 0.5))
            {
                Debug.Log("move up");
                if (Move != null)
                    Move(1);
            }
            else if((v.y > -0.5 && v.y < 0.5) && (v.x > 0.5))
            {
                Debug.Log("move right");
                if (Move != null)
                    Move(2);
            }
            else if((v.x > -0.5 && v.x < 0.5) && (v.y < -0.5))
            {
                Debug.Log("move down");
                if (Move != null)
                    Move(3);
            }
            else if ((v.y > -0.5 && v.y < 0.5) && (v.x < -0.5))
            {
                Debug.Log("move left");
                if (Move != null)
                    Move(4);
            }
        }
    }

    void SlideScreen()
    {
        int touchCount = Input.touchCount;
        for (int i = 0; i < touchCount; i++)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.Equals(null)) continue;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startpos = touch.position;
                    break;
                
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    Debug.Log("Touch End : " + i);
                    v = touch.position - startpos;
                    v = v.normalized;

                    if ((v.x > -0.5 && v.x < 0.5) && (v.y > 0.5))
                    {
                        Debug.Log("move up");
                        if (Move != null)
                            Move(1);
                    }
                    else if ((v.y > -0.5 && v.y < 0.5) && (v.x > 0.5))
                    {
                        Debug.Log("move right");
                        if (Move != null)
                            Move(2);
                    }
                    else if ((v.x > -0.5 && v.x < 0.5) && (v.y < -0.5))
                    {
                        Debug.Log("move down");
                        if (Move != null)
                            Move(3);
                    }
                    else if ((v.y > -0.5 && v.y < 0.5) && (v.x < -0.5))
                    {
                        Debug.Log("move left");
                        if (Move != null)
                            Move(4);
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
