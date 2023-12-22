using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Boss_4 : EnemyEntity
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

    public Transform fireballSpawnPoint;
    public GameObject fireballPrefab;
    public int numBlades = 8;
    public float fireballSpeed = 10f;
    public float fireballCooldown = 0.5f;
    private float nextFireballTime = 0f;

    private float phase1Timer = 5.0f;
    private float phase2Timer = 10.0f;

    public float beamoktimer = 0;
    public bool beamok = false;

    public GameObject ninjaStarPrefab;
    public float ninjastarSpeed = 10f;
    private bool isNinjaStarThrown = false;
    private float ninjaStarFlightTime = 2f;
    public float knifeslifetime;

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

    Coroutine preparingBeam;
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

        if (beamok == true)
        {
            beamoktimer += Time.deltaTime;
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

        if (!isMoving && Vector2.Distance(rb.position, targetPlayer.transform.position) <= 20)
        {
            if (Time.time >= nextFireballTime)
            {
                if (currentState == EnemyState.Phase1)
                {
                    FireKnifes();
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

    void SpawnMeteors(Vector3 playerPosition)
    {
        // Check if the enemy is in Phase 2 before spawning meteors
        if (currentState == EnemyState.Phase2)
        {
            // Get the mouse position in world coordinates
            Vector2 playerPos = targetPlayer.transform.position;

            // Throw the ninja star
            StartCoroutine(ThrowNinjaStar(playerPos));
        }
    }

    IEnumerator ThrowNinjaStar(Vector2 targetPosition)
    {
        isNinjaStarThrown = true;

        // Get the current player position as the reference point
        Vector2 enemyPosition = transform.position;

        // Instantiate the ninja star at the player's position
        GameObject ninjaStar = Instantiate(ninjaStarPrefab, enemyPosition, Quaternion.identity);

        // Calculate the initial direction to the target position
        Vector2 direction = (targetPosition - enemyPosition).normalized;

        // Set the ninja star's velocity
        ninjaStar.GetComponent<Rigidbody2D>().velocity = direction * ninjastarSpeed;

        // Calculate the angle to face the target position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the dagger's rotation to face forward
        ninjaStar.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // Wait for the ninja star to reach the target position
        yield return new WaitForSeconds(ninjaStarFlightTime);

        while (true)
        {
            enemyPosition = transform.position;
            Vector2 returnDirection = (enemyPosition - (Vector2)ninjaStar.transform.position);
            ninjaStar.GetComponent<Rigidbody2D>().velocity = returnDirection.normalized * ninjastarSpeed;

            if (returnDirection.magnitude <= 0.1f)
                break;

            yield return null;
        }

        // Cleanup and cooldown
        Destroy(ninjaStar);

        isNinjaStarThrown = false;
    }




    //void StartFireKnifes()
    //{
    //    StartCoroutine(FireKnifes());
    //}

    void FireKnifes()
    {
        int numKnives = 5; // Adjust the number of knives as needed
        float circleRadius = 2f; // Adjust the radius of the circle
        Vector2 directionToPlayer = ((Vector2)targetPlayer.transform.position - (Vector2)fireballSpawnPoint.position).normalized;

        for (int i = 0; i < numKnives; i++)
        {
            // Calculate the angle for each knife in the circle
            float angle = (360f / numKnives) * i;
            Vector3 circlePosition = fireballSpawnPoint.position + (Quaternion.Euler(0, 0, angle) * (Vector2.right * circleRadius));

            // Instantiate a knife at the calculated position
            GameObject knife = Instantiate(fireballPrefab, circlePosition, Quaternion.identity);

            // Set the knife's velocity towards the player
            Vector2 directionToPlayerFromCircle = ((Vector2)targetPlayer.transform.position - (Vector2)circlePosition).normalized;
            knife.GetComponent<Rigidbody2D>().velocity = directionToPlayerFromCircle * fireballSpeed;

            // Calculate the rotation angle to make the knife face forward
            float rotationAngle = Mathf.Atan2(directionToPlayerFromCircle.y, directionToPlayerFromCircle.x) * Mathf.Rad2Deg;

            // Set the knife's rotation to face forward
            knife.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

            Destroy(knife, knifeslifetime);
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
