using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movement_speed;
    private SpriteRenderer sr;
    public Animator animator;
    private Vector2 inputvalues; //chanbge name ltr


    public Rigidbody2D rb;
    public Vector2 moveDir;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // To process inputs
        ProcessInputs();

        animator.SetFloat("Horizontal_movement", moveDir.x);
        animator.SetFloat("Vertical_movement", moveDir.y);
        animator.SetFloat("Speed", moveDir.sqrMagnitude);
    }


    void FixedUpdate()
    {
        // To process physics calculations
        Move();
    }

    void ProcessInputs()
    {
        float moveXaxis = Input.GetAxisRaw("Horizontal");
        float moveYaxis = Input.GetAxisRaw("Vertical");

        inputvalues = new Vector2(moveXaxis, moveYaxis);
        moveDir = new Vector2(moveXaxis, moveYaxis); //to do
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveDir * movement_speed * Time.deltaTime);
    }

}
