using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class DeformedEyeball : EnemyEntity
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

    public GameObject iceballPrefab;
    public Transform iceballSpawnPoint;
    public float iceballSpeed = 5f;
    public float iceballhomingSpeed = 4f;
    public float iceballCooldown = 0.5f;
    private float nextIceballTime = 0f;

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

        if (collision.GetComponent<Beam>())
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
            ShootIceball();
            nextIceballTime = Time.time + iceballCooldown;
        }
    }

    void ShootIceball()
    {
        // Number of iceballs to shoot
        int iceballCount = 3;
        Vector2 direction = (targetPlayer.transform.position - iceballSpawnPoint.position).normalized;

        // Calculate the angle between the original direction and spread the shots
        float angle = 5f;

        for (int i = 0; i < iceballCount; i++)
        {
            // Rotate the original direction by the calculated angle
            Vector2 rotatedDirection = Quaternion.Euler(0, 0, (-(angle / 2) * i * angle) + angle * iceballCount / 2f) * direction;

            // Calculate the position for the iceball
            Vector3 iceballPosition = iceballSpawnPoint.position; // Adjust the distance as needed

            // Instantiate an iceball
            GameObject iceball = Instantiate(iceballPrefab, iceballPosition, Quaternion.identity);

            // Set the iceball's velocity with the rotated direction
            iceball.GetComponent<Rigidbody2D>().velocity = rotatedDirection * iceballSpeed;

            // Start homing after a specific delay with a maximum lifetime of 5 seconds
            float homingDelay = i * 0.75f; // Adjust the delay for each projectile
            StartCoroutine(HomingAfterDelay(iceball, iceballhomingSpeed, targetPlayer, homingDelay, 5f));
        }
    }

    IEnumerator HomingAfterDelay(GameObject iceball, float speed, GameObject target, float delay, float maxLifetime)
    {
        float elapsedTime = 0f;

        yield return new WaitForSeconds(delay);

        while (iceball != null && target != null && elapsedTime < maxLifetime)
        {
            // Update the iceball's velocity to home in on the player's position
            Vector2 direction = (target.transform.position - iceball.transform.position).normalized;
            iceball.GetComponent<Rigidbody2D>().velocity = direction * speed;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy the iceball when it reaches its maximum lifetime
        Destroy(iceball);
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

