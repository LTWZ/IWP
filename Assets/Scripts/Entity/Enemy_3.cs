using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NavMeshPlus;
using UnityEngine.AI;
using System;
using System.Collections;

public class Enemy_3 : EnemyEntity
{
    private GameObject targetPlayer;
    public event Action<Enemy_3> OnEnemyDeath;

    public float rootDuration = 5f;
    private float rootTimer = 0f;

    public float stopDistance = 0.5f;
    private float attackTimer = 0f;
    public float attackCooldown = 2f;

    public bool IsEnemyRooted = false;
    public bool isenemydefeated = false;


    public Transform enemyGFX;

    [Header("HP Code")]
    private TextMeshProUGUI HB_valuetext;

    // Reference to the sprite renderer component
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        SetTarget();
        currHealth = Hp;
        currSpeed = speed;
        InvokeRepeating("UpdateTargetPosition", 0f, .5f);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void UpdateTargetPosition()
    {
        SetTarget();
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

        if (IsEnemyRooted)
        {
            Invoke("EndRoot", rootDuration);
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

    private void EndRoot()
    {
        IsEnemyRooted = false;
    }

    void FixedUpdate()
    {

    }

    public void EnemyMove()
    {
        if (IsEnemyRooted)
        {
            return;
        }

        // Update the root timer
        if (IsEnemyRooted)
        {
            Debug.Log(rootTimer);

            if (rootTimer <= 0)
            {
                IsEnemyRooted = false;
            }
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

    }

}


