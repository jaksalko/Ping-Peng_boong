using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParfaitObject : MonoBehaviour
{
    public GameObject iceBox;
    public enum State
    {
        inactive,
        active,
        clear
        
    }
    public int sequence;
    public State state;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("parfait initialize...");
        state = State.inactive;
        //renderer = this.gameObject.GetComponent<Renderer>();
        //renderer.material.color = Color.black;

    }
    /*private void FixedUpdate()
    {
        if(state == State.active)
        {
            y_rot = 1f;
            
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + y_rot, transform.rotation.eulerAngles.z));
        }
    }*/

    public void Activate()
    {
        Debug.Log("activate");
        state = State.active;
        iceBox.SetActive(false);
        //renderer.material.color = Color.white;// reveal real color

    }

    public bool GetParfait(Map map)
    {
        state = State.clear;
        if (sequence < 3)
        {
            map.parfaitBlock[sequence + 1].Activate();
            Destroy(this.gameObject);
            return false;//active next parfait
        }
        else
        {

            Destroy(this.gameObject);
            return true;//clear game
        }
            
       
        //Start Get Animation...
        

        
    }
}
