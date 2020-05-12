using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNose : MonoBehaviour
{
    public Animator animator;

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("collision nose");
            animator.SetInteger("action", 3);
        }
    }
}
