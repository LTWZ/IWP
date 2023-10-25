using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;

    [SerializeField] MouseController mouseController;
    [SerializeField] float movement_speed;
    private SpriteRenderer sr;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform HandPivotParent;
    [SerializeField] Transform HandPivot;
    private Vector2 moveDir;
    public Test1 Player;

    private void Awake()
    {
        instance = this;
    }

    public static PlayerMovement GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // To process inputs
        ProcessInputs();
        UpdatePivot();
        animator.SetFloat("Horizontal_movement", moveDir.x);
        animator.SetFloat("Vertical_movement", moveDir.y);
        animator.SetFloat("Speed", moveDir.sqrMagnitude);
    }

    public Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void UpdatePivot()
    {
        Vector3 dir = mouseController.GetDirection().normalized;
        float rotZ = (-Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg) + 90f;
        HandPivotParent.rotation = Quaternion.Euler(0, 0, rotZ);
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

        moveDir = new Vector2(moveXaxis, moveYaxis); //to do
    }

    public Transform GetHandPivotParent()
    {
        return HandPivotParent;
    }

    public Transform GetHandPivot()
    {
        return HandPivot;
    }
    void Move()
    {
        rb.MovePosition(rb.position + moveDir * movement_speed * Time.deltaTime);
    }

}
