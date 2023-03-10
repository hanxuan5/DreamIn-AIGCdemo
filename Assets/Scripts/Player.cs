using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Instance
    public static Player Instance;

    //Movement
    public float SPEED = 100.0f;
    private bool _canMove = true;
    Animator animator;

    void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    // Initialize Animator
    void Start () {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove == false)
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool up = v > 0;
        bool down = v < 0;
        bool right = h > 0;
        bool left = h < 0;

        animator.SetBool("up", up);
        animator.SetBool("down", down);
        animator.SetBool("right", right);
        animator.SetBool("left", left);

        h *= Mathf.Sqrt(1 - (v * v) / 2.0f);
        v *= Mathf.Sqrt(1 - (h * h) / 2.0f);

        Vector2 dir = new Vector2(h, v);
        GetComponent<Rigidbody2D>().velocity = dir * SPEED;
    }

    public void DisableMovement()
    {
        _canMove = false;
    }

    public void EnableMovement()
    {
        _canMove = true;
    }
}
