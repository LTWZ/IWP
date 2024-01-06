using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;
using System.Collections;

public class GhostBrute : EnemyEntity
{

    private GameObject targetPlayer;

    private bool isFirstTouch = true;

    public float nextWaypointDistance = 3f;
    public float stopDistance = 0.5f;
    public float rootDuration = 5f;
    private float rootTimer = 0f;

    public Transform enemyGFX;
    private float attackTimer = 0f;
    public float attackCooldown = 6f;
    public int attackDmg;
    private bool isAboutToAttack = false;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;

    Seeker seeker;
    Rigidbody2D rb;

    public bool IsEnemyRooted = false;

    [Header("HP Code")]
    private TextMeshProUGUI HB_valuetext;

    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        SetTarget();
        currHealth = Hp;
        currSpeed = speed;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //targetPlayer = EnemyManager.GetInstance().GetPlayerReference();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }



    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public override void SetTarget()
    {
        targetPlayer = EnemyManager.GetInstance().GetPlayerReference();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Beam>() || collision.GetComponent<TrapBullet>())
        {
            IsEnemyRooted = true;
            ChangeColorWhenRooted();
        }

        // Check for a root ability (you can use a different trigger condition)
        if (IsEnemyRooted == true)
        {
            rootTimer = rootDuration;
        }

        if (isFirstTouch && collision.gameObject == targetPlayer)
        {
            // First touch, initiate the attack preparation
            isFirstTouch = false;
            attackTimer = Time.time + 0.5f; // Set the attack timer 0.5 seconds into the future
        }
    }

    private IEnumerator DelayedChangeColor(Color newColor, float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.color = newColor;
    }

    private void ChangeColorWhenRooted()
    {
        // Wait for 0.5 seconds before changing the color to red
        StartCoroutine(DelayedChangeColor(Color.red, 0.5f));
    }

    void FixedUpdate()
    {
        targetPlayer = EnemyManager.GetInstance().GetPlayerReference();
        if (Vector3.Distance(targetPlayer.transform.position, transform.position) < 15)
        {
            EnemyMove();
        }
    }


    protected override void Update()
    {
        base.Update();

        // Update color based on the flag
        if (isAboutToAttack)
        {
            ChangeEnemyColor(Color.red);
        }
        else
        {
            ChangeEnemyColor(Color.white);
        }
    }

    public void EnemyMove()
    {
        if (IsEnemyRooted == true)
        {
            rb.velocity = Vector2.zero;
        }

        // Update the root timer
        if (IsEnemyRooted == true)
        {
            rootTimer -= Time.deltaTime;
            //Debug.Log(rootTimer);

            if (rootTimer <= 0)
            {
                IsEnemyRooted = false;
            }
        }

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * currSpeed * Time.deltaTime;

        if (Vector2.Distance(rb.position, targetPlayer.transform.position) <= stopDistance)
        {
            rb.velocity = Vector2.zero; // Stop the enemy
        }
        else
        {
            rb.AddForce(force);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
    }


    public override void UpdateHPEnemy()
    {
        if (currHealth <= 0)
        {
            //RoomManager.GetInstance().EnemyDefeated();
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == targetPlayer)
        {
            if (Time.time >= attackTimer - 0.5f && Time.time < attackTimer)
            {
                // Enemy is about to attack, change color to red
                isAboutToAttack = true;
            }

            if (Time.time >= attackTimer)
            {
                // Reset color immediately after the attack
                ChangeEnemyColor(Color.white);

                Debug.Log("Take dmg");
                targetPlayer.GetComponent<PlayerEntity>().ChangeHealth(-attackDmg);

                // Set the next allowed attack time
                attackTimer = Time.time + attackCooldown;

                // Reset the flag for the next attack
                isAboutToAttack = false;
            }
            else
            {
                // Optionally, you can provide feedback that the enemy is on cooldown
                Debug.Log("Enemy is on cooldown");
            }
        }
    }

    private void ChangeEnemyColor(Color color)
    {
        // Assuming your GhostBrute has a SpriteRenderer component
        SpriteRenderer enemyRenderer = GetComponentInChildren<SpriteRenderer>();

        if (enemyRenderer != null)
        {
            enemyRenderer.color = color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the child object of the GhostBrute.");
        }
    }
}
