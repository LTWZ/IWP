using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Boss_2 : EnemyEntity
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

    public GameObject iceballPrefab;
    public Transform iceballSpawnPoint;
    public float iceballSpeed = 8f;
    public float iceballCooldown = 0.5f;
    private List<int> icicleCounts = new List<int> { 3, 5, 7, 4, 6}; // Define your icicle count sequence
    private int currentIcicleIndex = 0;
    private float nextIceballTime = 0f;
    public bool isImmuneToDamage = false;


    private float phase1Timer = 5.0f;
    private float phase2Timer = 10.0f;

    public GameObject icecrystalPrefab; // Reference to the ice crystal prefab
    private GameObject activeIceCrystal; // Reference to the active ice crystal
                                            // Health of the ice crystal
    private bool isSummoningIceCrystal = false;
    private bool hasSummonedFinalIceCrystal = false;// Health of each ice crystal

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

        // Check for phase transitions based on health
        if (currHealth <= Hp / 2 && !phase2isactive)
        {
            currentState = EnemyState.Phase2;
            phase2isactive = true;
            phase1isactive = false;
            isImmuneToDamage = true;
            Debug.Log("Transition to Phase 2");
        }

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
        // Your Phase 1 logic here

        // Check for phase transition based on health
        if (currHealth <= Hp / 2)
        {
            currentState = EnemyState.Phase2;
            isImmuneToDamage = true;
            Debug.Log("Transition to Phase 2");
        }
    }

    void UpdatePhase2()
    {
        // Your Phase 2 logic here

        if (!isSummoningIceCrystal && !hasSummonedFinalIceCrystal)
        {
            SummonIceCrystal();
        }
    }

    public override void ChangeHealth(int amtChanged, bool isSelfDamage = false)
    {
        if (!isImmuneToDamage)
        {
            base.ChangeHealth(amtChanged);
        }
    }

    void SummonIceCrystal()
    {
        // Define the radius around the player where the ice crystal can be summoned
        float summonRadius = 4.0f; // Adjust the radius as needed

        // Calculate a random position within the summon radius
        Vector2 randomOffset = Random.insideUnitCircle * summonRadius;
        Vector3 summonPosition = targetPlayer.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        // Instantiate an ice crystal at the calculated position
        activeIceCrystal = Instantiate(icecrystalPrefab, summonPosition, Quaternion.identity);
        isSummoningIceCrystal = true;

        // Subscribe to the ice crystal's destruction event
        IceCrystal iceCrystalComponent = activeIceCrystal.GetComponent<IceCrystal>();
        if (iceCrystalComponent != null)
        {
            iceCrystalComponent.OnIceCrystalDestroyed += IceCrystalDestroyed;
        }
    }

    void IceCrystalDestroyed()
    {
        // Unsubscribe from the ice crystal's destruction event
        IceCrystal iceCrystalComponent = activeIceCrystal.GetComponent<IceCrystal>();
        if (iceCrystalComponent != null)
        {
            iceCrystalComponent.OnIceCrystalDestroyed -= IceCrystalDestroyed;
        }

        // Set isSummoningIceCrystal to false
        isSummoningIceCrystal = false;

        isImmuneToDamage = false;

        // Deal damage to the boss when the ice crystal is destroyed
        ChangeHealth(-10);

        isImmuneToDamage = true;
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
            if (Time.time >= nextIceballTime)
            {
                if (currentState == EnemyState.Phase1)
                {
                    int icicleCount = icicleCounts[currentIcicleIndex];
                    ShootIcicle(icicleCount);

                    // Increment the index, and loop back to the beginning if necessary
                    currentIcicleIndex = (currentIcicleIndex + 1) % icicleCounts.Count;

                    // Set the next allowed iceball time
                    nextIceballTime = Time.time + iceballCooldown;

                }
            }

            if (currentState == EnemyState.Phase2)
            {
                // Check the meteor cooldown before spawning a meteor
            }
            // Add more conditions for other states as needed
        }

    }



    void ShootIcicle(int icicleCount)
    {
        Vector2 direction = (targetPlayer.transform.position - iceballSpawnPoint.position).normalized;

        // Calculate the angle between the original direction and spread the shots
        float angle = 90f;

        for (int i = 0; i < icicleCount; i++)
        {
            // Rotate the original direction by the calculated angle
            float currentAngle = (-(angle / 2) + (angle / (icicleCount - 1)) * i);
            Vector2 rotatedDirection = Quaternion.Euler(0, 0, currentAngle) * direction;

            // Calculate the position for the iceball
            Vector3 iceballPosition = iceballSpawnPoint.position; // Adjust the distance as needed

            // Instantiate an iceball
            GameObject iceball = Instantiate(iceballPrefab, iceballPosition, Quaternion.identity);

            // Set the iceball's forward direction to face the player
            iceball.transform.up = rotatedDirection;

            // Check if the iceball would spawn inside the enemy
            float minDistanceToEnemy = 2f; // Adjust this value based on your needs

            // Declare adjustedSpawnPosition here
            Vector2 adjustedSpawnPosition;

            // Cast a ray from iceballSpawnPoint towards the player
            RaycastHit2D hit = Physics2D.Raycast(iceballSpawnPoint.position, rotatedDirection, Mathf.Infinity, 1 << enemyLayer);

            if (hit.collider != null && hit.distance < minDistanceToEnemy)
            {
                // If too close, adjust the spawn position using the point of intersection
                adjustedSpawnPosition = hit.point + rotatedDirection * minDistanceToEnemy;
            }
            else
            {
                // If no intersection, use the original spawn position
                adjustedSpawnPosition = iceballSpawnPoint.position;
            }

            // Set the adjusted spawn position
            iceball.transform.position = adjustedSpawnPosition;

            // Set the iceball's velocity
            iceball.GetComponent<Rigidbody2D>().velocity = rotatedDirection * iceballSpeed;
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
        if (currHealth < 1)
        {
            hasSummonedFinalIceCrystal = true;
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
