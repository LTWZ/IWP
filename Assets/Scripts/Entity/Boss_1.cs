using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Boss_1 : EnemyEntity
{
    private GameObject targetPlayer;

    public float nextWaypointDistance = 3f;
    public float stopDistance = 0.5f;
    public float rootDuration = 5f;
    private float rootTimer = 0f;

    public Transform enemyGFX;
    private float attackTimer = 0f;
    public float attackCooldown = 2f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;

    bool isMoving = false;
    bool phase1isactive = false;
    bool phase2isactive = false;

    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    public float fireballSpeed = 10f;
    public float fireballCooldown = 0.5f;
    private float nextFireballTime = 0f;

    private float phase1Timer = 5.0f;
    private float phase2Timer = 10.0f;

    public GameObject meteorPrefab;
    public GameObject meteorIndicatorPrefab;

    public float meteorCooldown = 0.5f;
    private float nextMeteorTime = 0f;

    private bool canmeteorspawn = true;
    private bool meteorsFalling = false;

    int enemyLayer = 7;

    Seeker seeker;
    Rigidbody2D rb;

    public bool IsEnemyRooted = false;

    [Header("HP Code")]
    private TextMeshProUGUI HB_valuetext;

    private EnemyState currentState = EnemyState.Phase1;

    private List<GameObject> activeIndicatorsAndMeteors = new List<GameObject>();

    public enum EnemyState
    {
        Phase1,
        Phase2
    }


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

    protected override void Update()
    {
        base.Update();

        switch (currentState)
        {
            case EnemyState.Phase1:
                UpdatePhase1();
                break;

            case EnemyState.Phase2:
                UpdatePhase2();
                break;

            // Add more states as needed

            default:
                break;
        }
    }

    void UpdatePhase1()
    {
        phase1Timer -= Time.deltaTime;
        Debug.Log("Phase 1 Timer: " + phase1Timer);

        if (phase1Timer <= 0)
        {
            phase1Timer = 5.0f;
            currentState = EnemyState.Phase2;
            canmeteorspawn = true;
            Debug.Log("Transition to Phase 2");
        }
    }

    void UpdatePhase2()
    {
        phase2Timer -= Time.deltaTime;
        Debug.Log("Phase 2 Timer: " + phase2Timer);

        if (phase2Timer <= 0)
        {
            phase2Timer = 10.0f;
            currentState = EnemyState.Phase1;
            Debug.Log("Transition back to Phase 1");
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

        if (!isMoving && Vector2.Distance(rb.position, targetPlayer.transform.position) <= 10)
        {
            if (Time.time >= nextFireballTime)
            {
                if (currentState == EnemyState.Phase1)
                {
                    ShootFireball();
                    nextFireballTime = Time.time + fireballCooldown;
                }
            }

            if (currentState == EnemyState.Phase2)
            {
                // Check the meteor cooldown before spawning a meteor
                if (Time.time >= nextMeteorTime && canmeteorspawn == true)
                {
                    if (phase2Timer < 2f)
                    {
                        canmeteorspawn = false;
                    }
                    else
                    {
                        SpawnMeteors(targetPlayer.transform.position);
                        nextMeteorTime = Time.time + meteorCooldown;
                    }
                }
                else if (canmeteorspawn == false)
                {

                }
            }
            // Add more conditions for other states as needed
        }

    }

    void SpawnMeteorAfterDelay(Vector3 playerPosition)
    {
        // Calculate a position around the player for the meteor
        Vector3 meteorSpawnPosition = playerPosition;

        // Display the meteor indicator
        GameObject indicator = DisplayMeteorIndicator(meteorSpawnPosition);

        // Introduce a delay before spawning the meteor
        float delay = 0.5f;
        float meteorDuration = 2f; // Adjust the duration as needed
        StartCoroutine(SpawnMeteorAfterDelayCoroutine(meteorSpawnPosition, delay, indicator, meteorDuration));
    }

    GameObject DisplayMeteorIndicator(Vector3 indicatorPosition)
    {
        // Instantiate and display the meteor indicator at the specified position.
        // This can be a UI element, a sprite, or any other visual cue.
        GameObject indicator = Instantiate(meteorIndicatorPrefab, indicatorPosition, Quaternion.identity);

        // Add the indicator to the active list
        activeIndicatorsAndMeteors.Add(indicator);

        // Return the indicator GameObject so we can reference it later
        return indicator;
    }

    IEnumerator SpawnMeteorAfterDelayCoroutine(Vector3 meteorSpawnPosition, float delay, GameObject indicator, float meteorDuration)
    {
        yield return new WaitForSeconds(delay);

        // Instantiate the meteor at the specified position
        GameObject meteor = Instantiate(meteorPrefab, meteorSpawnPosition, Quaternion.identity);

        AudioManager.instance.PlaySFX("Meteor");

        // Destroy the indicator after spawning the meteor
        Destroy(indicator);

        // Remove the meteor from the active list
        activeIndicatorsAndMeteors.Remove(indicator);

        // Add the meteor to the active list
        activeIndicatorsAndMeteors.Add(meteor);

        // Set a timer for destroying the meteor
        yield return new WaitForSeconds(meteorDuration);

        // Destroy the meteor
        Destroy(meteor);

        // Remove the meteor from the active list
        activeIndicatorsAndMeteors.Remove(meteor);
    }
    void SpawnMeteors(Vector3 playerPosition)
    {
        // Check if the enemy is in Phase 2 before spawning meteors
        if (currentState == EnemyState.Phase2)
        {
            SpawnMeteorAfterDelay(playerPosition);
        }
    }


    void ShootFireball()
    {

        AudioManager.instance.PlaySFX("FireballEnemy");

        // Instantiate a fireball
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);

        // Calculate the direction to the player
        Vector2 direction = (targetPlayer.transform.position - fireballSpawnPoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Debug the direction
        Debug.Log("Fireball Direction: " + direction);

        // Check if the fireball would spawn inside the enemy
        float minDistanceToEnemy = 2f; // Adjust this value based on your needs

        // Declare adjustedSpawnPosition here
        Vector2 adjustedSpawnPosition;

        // Cast a ray from fireballSpawnPoint towards the player
        RaycastHit2D hit = Physics2D.Raycast(fireballSpawnPoint.position, direction, Mathf.Infinity, 1 << enemyLayer);

        if (hit.collider != null && hit.distance < minDistanceToEnemy)
        {
            // If too close, adjust the spawn position using the point of intersection
            adjustedSpawnPosition = hit.point + direction * minDistanceToEnemy;
        }
        else
        {
            // If no intersection, use the original spawn position
            adjustedSpawnPosition = fireballSpawnPoint.position;
        }

        // Set the adjusted spawn position
        fireball.transform.position = adjustedSpawnPosition;

        // Set the fireball's velocity
        fireball.GetComponent<Rigidbody2D>().velocity = direction * fireballSpeed;
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
