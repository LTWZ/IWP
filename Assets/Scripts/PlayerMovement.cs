using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    private float knockbackDuration = 0.5f;
    private float knockbackForce = 5f;
    private bool isKnockbackActive = false;
    private Vector2 knockbackDirection;
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
    public float finalbossslowDuration = 2f;
    public float speedDuration = 2f;
    public float rootedDuration = 2f;
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

        //Debug.Log(speedDuration);
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
        if (GetComponent<PlayerEntity>().isplayerSlowedByFinalBossAOE == true)
        {
            SetMovementSpeed(originalMovementSpeed / 1.5f);
            finalbossslowDuration -= Time.deltaTime;
            Debug.Log("slow duration: " + finalbossslowDuration);

            if (finalbossslowDuration <= 0)
            {
                finalbossslowDuration = 2f;
                PlayerManager playerManager = PlayerManager.GetInstance();
                PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
                player.isplayerSlowedByFinalBossAOE = false;
                ResetSpeedModifier(); // Reset speed when slow effect ends
            }
        }
        else if (GetComponent<PlayerEntity>().isplayerSpedUp == true)
        {
            SetMovementSpeed(0);
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
        else if (GetComponent<PlayerEntity>().isplayerRooted == true)
        {
            SetMovementSpeed(0);
            rootedDuration -= Time.deltaTime;
            Debug.Log("speed duration: " + rootedDuration);

            if (rootedDuration <= 0)
            {
                rootedDuration = 2f;
                PlayerManager playerManager = PlayerManager.GetInstance();
                PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
                player.isplayerRooted = false;
                ResetSpeedModifier(); // Reset speed when speed up effect ends
            }
        }
        else
        {
            ResetSpeedModifier(); // Reset speed when no effect is active
        }

        //if (Input.GetKeyDown(KeyCode.Minus))
        //{
        //    PlayerPrefs.DeleteAll();
        //    PlayerPrefs.SetFloat("VolumeValue", 1);
        //    PlayerPrefs.SetFloat("SFXValue", 1);
        //}

    }





    void Knockback()
    {
        // Apply knockback force in the specified direction
        rb.velocity = knockbackDirection * knockbackForce;
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        // Set knockback direction and force
        knockbackDirection = direction;
        knockbackForce = force;

        // Start knockback
        StartCoroutine(StartKnockback());
    }

    IEnumerator StartKnockback()
    {
        isKnockbackActive = true;

        // Wait for the specified knockback duration
        yield return new WaitForSeconds(knockbackDuration);

        // End knockback
        isKnockbackActive = false;
        rb.velocity = Vector2.zero; // Reset velocity after knockback
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
        if (isKnockbackActive)
        {
            // Apply knockback force when active
            Knockback();
        }
        else
        {
            // To process physics calculations
            Move();
        }
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
        if (!isKnockbackActive)
        {
            rb.MovePosition(rb.position + moveDir * currentMovementSpeed * Time.deltaTime);
        }
    }

}
