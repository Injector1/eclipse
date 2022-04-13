using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 2f;

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
