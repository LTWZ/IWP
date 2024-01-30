using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;

public class LightningSlime : EnemyEntity
{

    private GameObject targetPlayer;

    public float nextWaypointDistance = 3f;
    public float stopDistance = 0.5f;
    public float rootDuration = 5f;
    private float rootTimer = 0f;

    bool isMoving = false;

    public Transform enemyGFX;
    private float attackTimer = 0f;
    public float attackCooldown = 2f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;

    Seeker seeker;
    Rigidbody2D rb;

    public GameObject lightningBoltPrefab;
    public Transform iceballSpawnPoint;
    public float lightningBoltSpeed = 10f;
    public float iceballCooldown = 0.5f;
    private float nextIceballTime = 0f;

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

        // Check if the enemy is close to the player
        if (Vector3.Distance(targetPlayer.transform.position, transform.position) < 5)
        {
            EnemyMove();
            isMoving = true;  // The enemy is currently moving
        }
        else
        {
            isMoving = false;  // The enemy is not moving
        }

        // Check if the enemy is close to the player and stopped moving
        if (!isMoving && Vector2.Distance(rb.position, targetPlayer.transform.position) <= 10 && Time.time >= nextIceballTime)
        {
            ShootLightningBolts();
            AudioManager.instance.PlaySFX("Electric");
            nextIceballTime = Time.time + iceballCooldown;
        }
    }

    void ShootLightningBolts()
    {
        // Number of lightning bolts to shoot
        int lightningBoltCount = 8;
        Vector2 direction = Vector2.right; // Starting direction, adjust as needed

        for (int i = 0; i < lightningBoltCount; i++)
        {
            // Rotate the original direction by the angle between each bolt
            float angle = 360f / lightningBoltCount;
            Vector2 rotatedDirection = Quaternion.Euler(0, 0, angle * i) * direction;

            // Calculate the position for the lightning bolt
            Vector3 lightningBoltPosition = transform.position; // Adjust the distance as needed

            // Instantiate a lightning bolt
            GameObject lightningBolt = Instantiate(lightningBoltPrefab, lightningBoltPosition, Quaternion.identity);

            // Calculate the rotation angle based on the rotated direction
            float angle2 = Mathf.Atan2(rotatedDirection.y, rotatedDirection.x) * Mathf.Rad2Deg;

            // Apply the rotation to the lightning bolt
            lightningBolt.transform.rotation = Quaternion.Euler(0, 0, angle2);

            // Set the lightning bolt's velocity with the rotated direction
            lightningBolt.GetComponent<Rigidbody2D>().velocity = rotatedDirection * lightningBoltSpeed;
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
            Debug.Log(rootTimer);

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

        if (rb.velocity.x >= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.x <= 0.01f)
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

