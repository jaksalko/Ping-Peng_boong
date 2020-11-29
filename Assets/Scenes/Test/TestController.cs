using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{

    private static TestController instance = null;

    public static TestController GetTestController
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PrintBeforeState()
    {
        Debug.Log(before.variable + "  arr : " + before.arr[0]);
       
    }

    public BClass before;

    public AClass a;
    public BClass b1;
    public BClass b2;

    public int share_var;
    public int[] share_arr;

   
}
