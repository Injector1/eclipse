using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollision : MonoBehaviour
{
    void Death()
    {
        throw new NotImplementedException();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var tag = collision.gameObject.tag;
        Debug.Log($"Collision with object \"{tag}\"");
        Death();
    }
}
