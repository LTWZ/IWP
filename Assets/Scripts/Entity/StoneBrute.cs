using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;

public class StoneBrute : EnemyEntity
{
    private GameObject targetPlayer;

    public float nextWaypointDistance = 3f;
    public float stopDistance = 0.5f;
    public float rootDuration = 5f;
    private float rootTimer = 0f;

    public Transform enemyGFX;
    private float attackTimer = 0f;
    public float attackCooldown = 2f;
    public bool isImmuneToDamage = false;

    // Variables for immunity toggle
    private float immunityToggleTimer = 0f;
    private float immunityToggleCooldown = 5f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;

    Seeker seeker;
    Rigidbody2D rb;

    public bool IsEnemyRooted = false;

    [Header("HP Code")]
    private TextMeshProUGUI HB_valuetext;

    private void Start()
    {
        SetTarget();
        currHealth = Hp;
        currSpeed = speed;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //targetPlayer = EnemyManager.GetInstance().GetPlayerReference();

        InvokeRepeating("UpdatePath", 0f, .5f);
        InvokeRepeating("ToggleImmunity", 0f, 5f); // Invoke the toggle every 5 seconds
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
        }

        // Check for a root ability (you can use a different trigger condition)
        if (IsEnemyRooted == true)
        {
            rootTimer = rootDuration;
        }
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

        // Update color based on immunity
        if (isImmuneToDamage)
        {
            ChangeEnemyColor(Color.blue);
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

    private void ToggleImmunity()
    {
        // Toggle immunity and reset the timer
        isImmuneToDamage = !isImmuneToDamage;
        immunityToggleTimer = immunityToggleCooldown;
    }

    public override void ChangeHealth(int amtChanged, bool isSelfDamage = false)
    {
        if (!isImmuneToDamage)
        {
            base.ChangeHealth(amtChanged);
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
        if (collision.gameObject == targetPlayer && !isImmuneToDamage)
        {
            if (Time.time >= attackTimer)
            {
                Debug.Log("Take dmg");
                targetPlayer.GetComponent<PlayerEntity>().ChangeHealth(-attackValue);

                // Set the next allowed attack time
                attackTimer = Time.time + attackCooldown;
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
        // Assuming your StoneBrute has a SpriteRenderer component
        SpriteRenderer enemyRenderer = GetComponentInChildren<SpriteRenderer>();

        if (enemyRenderer != null)
        {
            enemyRenderer.color = color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the child object of the StoneBrute.");
        }
    }
}
