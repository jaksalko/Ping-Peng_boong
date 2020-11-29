using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BClass : MonoBehaviour
{
    TestController controller;
    public int[] arr;
    public int variable;

    // Start is called before the first frame update
    void Start()
    {
        controller = TestController.GetTestController;

        arr = controller.share_arr; //call by reference
        variable = controller.share_var; //call by value
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateClickCount()
    {
        controller.before = this;

        Debug.Log("var :" + (++variable));
        Debug.Log(++arr[0]);
    }
}
