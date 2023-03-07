using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed = 100.0f;
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        h *= Mathf.Sqrt(1 - (v * v) / 2.0f);
        v *= Mathf.Sqrt(1 - (h * h) / 2.0f);

        Vector2 dir = new Vector2(h, v);
        GetComponent<Rigidbody2D>().velocity = dir * _speed;
    }
}
