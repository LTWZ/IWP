using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;
using System.Collections;

public class BigBrute : EnemyEntity
{

    private GameObject targetPlayer;

    public float nextWaypointDistance = 3f;
    public float stopDistance = 0.5f;
    public float rootDuration = 5f;
    private float rootTimer = 0f;

    public Transform enemyGFX;
    private float attackTimer = 0f;
    public float attackCooldown = 2f;
    public GameObject warningIconPrefab;

    public float chargeCooldown = 5f; // Adjust the charge cooldown as needed
    private float nextChargeTime = 0f;
    public float chargeSpeed = 10f;
    private bool isCharging = false;

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

        if (Time.time >= nextChargeTime)
        {
            StartCoroutine(Charge());
            nextChargeTime = Time.time + chargeCooldown;
        }
    }


    IEnumerator Charge()
    {

        if (Time.time < nextChargeTime)
        {
            // Boss is on cooldown, cannot dash yet
            yield break;
        }

        // Spawn a warning icon at the boss's current position
        GameObject warningIcon = Instantiate(warningIconPrefab, targetPlayer.transform.position, Quaternion.identity);
        Destroy(warningIcon, 0.5f); // Adjust the warning icon's lifetime as needed
        Vector3 warningIconSpawnPosition = warningIcon.transform.position;

        yield return new WaitForSeconds(0.5f); // Adjust the delay before charging as needed

        // Calculate direction to the player
        Vector2 chargeDirection = ((Vector2)warningIconSpawnPosition - rb.position).normalized;

        // Apply force to charge
        rb.AddForce(chargeDirection * chargeSpeed, ForceMode2D.Impulse);

        // Set the next allowed charge time
        nextChargeTime = Time.time + chargeCooldown;

        // Reset charging status after a short delay (you can adjust the delay as needed)
        yield return new WaitForSeconds(1f);
        isCharging = false;
        rb.velocity = Vector2.zero;
    }

    //void ResetChargeStatus()
    //{
    //    isCharging = false;
    //    rb.velocity = Vector2.zero;
    //}

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
            if (Time.time >= attackTimer)
            {
                Debug.Log("Take dmg");
                targetPlayer.GetComponent<PlayerEntity>().ChangeHealth(-attackValue);
                //ChangeHealth(-collision.GetComponent<EnemyEntity>().GetAttackValue());
       

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
}
