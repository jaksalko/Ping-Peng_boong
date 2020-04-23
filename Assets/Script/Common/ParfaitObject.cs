using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParfaitObject : MonoBehaviour
{
    [SerializeField]
    Renderer renderer;

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

    public void Activate()
    {
        Debug.Log("activate");
        state = State.active;
        renderer.material.color = Color.white;// reveal real color

    }

    public bool GetParfait()
    {
        state = State.clear;
        if (sequence < 3)
        {
            GameController.instance.map.parfaitBlock[sequence + 1].Activate();
            Destroy(this.gameObject);
            return false;
        }
        else
        {

            Destroy(this.gameObject);
            return true;
        }
            
       
        //Start Get Animation...
        

        
    }
}
