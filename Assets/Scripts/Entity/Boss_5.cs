using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Boss_5 : EnemyEntity
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

    public GameObject warningIconPrefab;
    public float chargeCooldown = 2f; // Adjust the charge cooldown as needed
    private float nextChargeTime = 0f;
    public float chargeSpeed = 10f;
    private bool isCharging = false;

    private float phase1Timer = 10.0f;
    private float phase2Timer = 10.0f;

    public float beamoktimer = 0;
    public bool beamok = false;

    public GameObject aoefirePrefab;
    public float ninjastarSpeed = 10f;
    private bool isNinjaStarThrown = false;
    private float ninjaStarFlightTime = 2f;
    public float knifeslifetime;

    public float meteorCooldown = 0.5f;
    private float nextMeteorTime = 0f;

    private bool canmeteorspawn = true;
    private bool meteorsFalling = false;
    private bool hasSummonedFireAOE = false;
    public bool isImmuneToDamage = false;

    // Add a variable to hold the SpriteRenderer component
    private SpriteRenderer spriteRenderer;



    int enemyLayer = 7;

    Seeker seeker;
    Rigidbody2D rb;

    // Add a variable to hold the default color
    private Color defaultColor;

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
        // Get the SpriteRenderer component
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Save the default color
        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color;
        }

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
            isImmuneToDamage = true;
            spriteRenderer.color = Color.blue;
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
            hasSummonedFireAOE = false;
            isImmuneToDamage = false;
            spriteRenderer.color = defaultColor;
            Debug.Log("Transition back to Phase 1");
        }
    }

    public override void ChangeHealth(int amtChanged, bool isSelfDamage = false)
    {
        if (!isImmuneToDamage)
        {
            // ... (your existing code)
            base.ChangeHealth(amtChanged);
        }
        else
        {
            // Handle gaining back health while immune to damage
            if (amtChanged > 0)
            {
                // Gain back health logic (e.g., play a sound, visual effect, etc.)
                Debug.Log("Gaining back health while immune to damage");
                currHealth += amtChanged;
                currHealth = Mathf.Clamp(currHealth, 0, Hp);
            }
        }
    }

    void FixedUpdate()
    {
        targetPlayer = EnemyManager.GetInstance().GetPlayerReference();
        if (Vector3.Distance(targetPlayer.transform.position, transform.position) < 15)
        {
            EnemyMove();
        }

        if (!isMoving && Vector2.Distance(rb.position, targetPlayer.transform.position) <= 20)
        {
            if (Time.time >= nextChargeTime && currentState == EnemyState.Phase1 && !IsEnemyRooted)
            {
                StartCoroutine(Dash());
                nextChargeTime = Time.time + chargeCooldown;
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
                    // Additional conditions for Phase2 if needed
                }
            }
            // Add more conditions for other states as needed
        }
    }

    void SpawnMeteors(Vector3 playerPosition)
    {
        // Check if the enemy is in Phase 2 and has not summoned Fire AOE yet
        if (currentState == EnemyState.Phase2 && !hasSummonedFireAOE)
        {
            // Throw the ninja star
            StartCoroutine(SummonFireAOE());
        }
    }

    IEnumerator SummonFireAOE()
    {
        GameObject fireAOE = Instantiate(aoefirePrefab, transform.position, Quaternion.identity);

        // Set the flag to true to indicate that Fire AOE has been summoned
        hasSummonedFireAOE = true;

        // Follow the boss continuously
        while (fireAOE != null)
        {
            fireAOE.transform.position = transform.position;
            yield return null;
        }
    }




    //void StartFireKnifes()
    //{
    //    StartCoroutine(FireKnifes());
    //}

    IEnumerator Dash()
    {
        if (Time.time < nextChargeTime)
        {
            // Boss is on cooldown, cannot dash yet
            yield break;
        }

        // Spawn a warning icon at the boss's current position
        GameObject warningIcon = Instantiate(warningIconPrefab, targetPlayer.transform.position, Quaternion.identity);
        Destroy(warningIcon, 1f); // Adjust the warning icon's lifetime as needed
        Vector3 warningIconSpawnPosition = warningIcon.transform.position;

        yield return new WaitForSeconds(1f); // Adjust the delay before charging as needed

        // Calculate direction to the warning icon's spawn position
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
