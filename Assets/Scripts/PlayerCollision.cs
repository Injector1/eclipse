using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PlayerCollision : MonoBehaviour
{
    List<string> animations = new List<string>{
        "isShooting",
        "isMoving"
    };
    Animator animator;

    void StopAnimations()
    {
        animator = GetComponent<Animator>();

        foreach (var anim in animations)
        {
            animator.SetBool(anim, false);
        }
    }

    void Death()
    {
        StopAnimations();
        animator.SetBool("isDead", true);
        // Add delay (1 sec)
        
        //throw new NotImplementedException();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var tag = collision.gameObject.tag;
        Debug.Log($"Collision with object \"{tag}\"");
        Death();
    }
}
