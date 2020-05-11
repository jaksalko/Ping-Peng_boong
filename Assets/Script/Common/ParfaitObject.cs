using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParfaitObject : MonoBehaviour
{
    [SerializeField]
    Renderer renderer;

    float y_rot = 0;
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
    private void FixedUpdate()
    {
        if(state == State.active)
        {
            y_rot = 1f;
            
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + y_rot, transform.rotation.eulerAngles.z));
        }
    }

    public void Activate()
    {
        Debug.Log("activate");
        state = State.active;
        for(int i = 0; i < renderer.materials.Length; i++)
        {
            Color color = renderer.materials[i].color;
            color.a = 1f;
            renderer.materials[i].color = color;
           
        }
        //renderer.material.color = Color.white;// reveal real color

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
