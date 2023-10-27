
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy_1 : EnemyEntity
{
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 4.0f;
    public float detectionDistance = 5.0f;
    public float returnToPatrolDistance = 8.0f;
    public Transform[] patrolPoints; // Assign patrol points in the Inspector.

    private Transform player;
    private Rigidbody2D rb;
    private int currentPatrolPoint = 0;
    private bool isChasing = false;

    [Header("HP Code")]
    private TextMeshProUGUI HB_valuetext;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currHealth = Hp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }


    public override void UpdateHPEnemy()
    {
        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
