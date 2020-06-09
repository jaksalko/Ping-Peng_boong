using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.UI;


public class SlideMove : MonoBehaviour
{
    public Text buffer;
    public Text vector;
    public Text dir;

    Vector2 down;
    bool click;
    Vector2 up;
    int buf;

    // Start is called before the first frame update
    void Start()
    {
        buf = 0;
#if UNITY_EDITOR
        var mouseDownStream = this.UpdateAsObservable()
            .Where(_ => !click)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => Input.mousePosition)
            .Subscribe(_ => { down = _; click = true; } );

        var mouseUpStream = this.UpdateAsObservable()
            .Where(_ => click)
            .Where(_ => Input.GetMouseButtonUp(0))
            .Select(_ => Input.mousePosition)
            .Subscribe(_ => { up = _; SetText(); click = false; });

#else
        var touchDownStream = this.UpdateAsObservable()
            .Where(_ => !click)
            .Where(_ => Input.touchCount > 0)
            .Where(_ => Input.GetTouch(0).phase == TouchPhase.Began)
            .Select(_ => Input.GetTouch(0))
            .Subscribe(_ => { down = _.position; click = true; } );

        var touchUpStream = this.UpdateAsObservable()
            .Where(_ => click)
            .Where(_ => Input.touchCount > 0)
            .Where(_ => Input.GetTouch(0).phase == TouchPhase.Ended)
            .Select(_ => Input.mousePosition)
            .Subscribe(_ => { up = _; SetText(); click = false; });
#endif

    }

    public void SetText()
    {
        buf++;
        buffer.text = buf.ToString();
        if (buf % 2 == 0)
            buffer.color = Color.red;
        else
            buffer.color = Color.blue;
        Vector2 normalized = (up - down).normalized;
        vector.text = "vector :" + (up - down).normalized;
        if(normalized.x < -0.5)
        {
            
                dir.text = "left";
            
        }
        else if(normalized.x > 0.5)
        {
            dir.text = "right";
        }
        else
        {
            if(normalized.y>0)
            {
                dir.text = "up";
            }
            else
            {
                dir.text = "down";
            }
        }
    }
}
