using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public Animator animator;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player.isMoving)
        {
            animator.SetBool("move", true);
        }
        else
        {
            animator.SetBool("move", false);
        }
    }
}
