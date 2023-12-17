using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;

    [SerializeField] MouseController mouseController;
    [SerializeField] float movement_speed;
    [SerializeField] float currentMovementSpeed;
    private float originalMovementSpeed;
    private SpriteRenderer sr;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform HandPivotParent;
    [SerializeField] Transform HandPivot;
    private Vector2 moveDir;
    public Test1 Player;
    public Test2 Player2;
    public Test3 Player3;
    public bool isMovementEnabled = true;
    public float slowDuration = 2f;
    public float speedDuration = 2f;
    private float slowTimer = 0f;

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
        currentMovementSpeed = movement_speed;
        originalMovementSpeed = movement_speed;
    }

    void Update()
    {

        Debug.Log(speedDuration);
        if (!isMovementEnabled || DialogueManager.isActive)
        {
            StopPlayer();
            return;
        }

        // To process inputs
        ProcessInputs();
        UpdatePivot();
        UpdateAnimator();

        if (GetComponent<PlayerEntity>().isplayerSlowed == true)
        {
            SetMovementSpeed(originalMovementSpeed / 2);
            slowDuration -= Time.deltaTime;
            Debug.Log("slow duration: " + slowDuration);

            if (slowDuration <= 0)
            {
                slowDuration = 2f;
                PlayerManager playerManager = PlayerManager.GetInstance();
                PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
                player.isplayerSlowed = false;
                ResetSpeedModifier(); // Reset speed when slow effect ends
            }
        }
        else if (GetComponent<PlayerEntity>().isplayerSpedUp == true)
        {
            SetMovementSpeed(originalMovementSpeed * 1.5f);
            speedDuration -= Time.deltaTime;
            Debug.Log("speed duration: " + speedDuration);

            if (speedDuration <= 0)
            {
                speedDuration = 2f;
                PlayerManager playerManager = PlayerManager.GetInstance();
                PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
                player.isplayerSpedUp = false;
                ResetSpeedModifier(); // Reset speed when speed up effect ends
            }
        }
        else
        {
            ResetSpeedModifier(); // Reset speed when no effect is active
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetFloat("VolumeValue", 1);
            PlayerPrefs.SetFloat("SFXValue", 1);
        }

    }

    public void SetMovementSpeed(float newSpeed)
    {
        currentMovementSpeed = newSpeed;
    }
    public void ResetSpeedModifier()
    {
        SetMovementSpeed(originalMovementSpeed);
    }

    void StopPlayer()
    {
        moveDir = Vector2.zero;
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
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
        rb.MovePosition(rb.position + moveDir * currentMovementSpeed * Time.deltaTime);
    }

}
