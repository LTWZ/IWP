using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Boss_3 : EnemyEntity
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

    public GameObject BHPrefab;
    public Transform fireballSpawnPoint;
    public float fireballSpeed = 10f;
    public float fireballCooldown = 0.5f;
    private float nextFireballTime = 0f;

    private float phase1Timer = 5.0f;
    private float phase2Timer = 10.0f;

    public GameObject indicatorPrefab;
    public float indicatorDuration = 1.0f;

    public GameObject beamPrefab2;
    public GameObject beamtemp;
    public float beamoktimer = 0;
    public bool beamok = false;

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

        if (!isMoving && Vector2.Distance(rb.position, targetPlayer.transform.position) <= 10)
        {
            if (Time.time >= nextFireballTime)
            {
                if (currentState == EnemyState.Phase1)
                {
                    StartBlackHoleSequence();
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
            if (preparingBeam == null)
            {
                preparingBeam = StartCoroutine(DelayBeam());
            }
        }
    }

    IEnumerator DelayBeam()
    {
        Vector2 playerPos = targetPlayer.transform.position;
        Vector2 directionVector = playerPos - (Vector2)gameObject.transform.position;
        Vector2 posToSpawnIn = (playerPos + (Vector2)gameObject.transform.position) / 2;

        // Instantiate the tempbeam at the calculated position
        GameObject tempbeam = Instantiate(beamtemp, posToSpawnIn, Quaternion.identity);
        float widthOfBeam = tempbeam.GetComponentInChildren<SpriteRenderer>().sprite.rect.width;
        float lengthofBeam = directionVector.magnitude * 110;
        float offSetScale = lengthofBeam / widthOfBeam;

        // Set the scale and rotation of tempbeam
        tempbeam.transform.localScale = new Vector2(offSetScale, tempbeam.transform.localScale.y);
        Vector2 dir = directionVector.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tempbeam.transform.eulerAngles = new Vector3(0, 0, angle);

        // Store the position for later use
        Vector3 tempBeamSpawnPosition = tempbeam.transform.position;

        yield return new WaitForSeconds(1);

        // Destroy tempbeam after using its position
        Destroy(tempbeam);

        // Instantiate beamPrefab2 at the stored position
        GameObject beam = Instantiate(beamPrefab2, tempBeamSpawnPosition, Quaternion.identity);
        float widthOfBeam2 = beam.GetComponentInChildren<SpriteRenderer>().sprite.rect.width;
        float lengthofBeam2 = directionVector.magnitude * 110;
        float offSetScale2 = lengthofBeam2 / widthOfBeam2;

        // Set the scale and rotation of beamPrefab2
        beam.transform.localScale = new Vector2(offSetScale2, beam.transform.localScale.y);
        beam.transform.eulerAngles = new Vector3(0, 0, angle);

        preparingBeam = null;
    }


    void StartBlackHoleSequence()
    {
        StartCoroutine(BlackHoleSequence());
    }

    IEnumerator BlackHoleSequence()
    {
        // Get the player's position
        Vector2 playerPos = targetPlayer.transform.position;

        // Calculate a random offset within a specified range
        float offsetX = Random.Range(-1f, 1f);
        float offsetY = Random.Range(-1f, 1f);

        // Apply the offset to the player's position
        Vector2 spawnPos = playerPos + new Vector2(offsetX, offsetY);

        // Instantiate the indicator at the random position
        GameObject indicator = Instantiate(indicatorPrefab, spawnPos, Quaternion.identity);

        // Wait for the indicator to be destroyed before casting the black hole
        yield return StartCoroutine(DestroyIndicator(indicator));

        // Instantiate the black hole at the random position
        GameObject blackHole = Instantiate(BHPrefab, spawnPos, Quaternion.identity);

        // Add a script to control the black hole's behavior
        BlackHoleScript blackHoleScript = blackHole.GetComponent<BlackHoleScript>();
        if (blackHoleScript != null)
        {
            // Customize the black hole's behavior (e.g., damage, pull force)
            blackHoleScript.SetDamageOverTime(1); // Adjust the damage as needed
            blackHoleScript.SetPullForce(10); // Adjust the pull force as needed
        }
    }

    // Coroutine to destroy the indicator after a short delay
    IEnumerator DestroyIndicator(GameObject indicator)
    {
        // Wait for a short delay (you can adjust the duration as needed)
        yield return new WaitForSeconds(indicatorDuration);

        // Destroy the indicator
        Destroy(indicator);
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
