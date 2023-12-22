using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloodPoolEnemy : MonoBehaviour
{
    public int damageOverTime = 1; // Integer damage applied per second
    private float timerRate = 0.5f;
    public float radius = 5f; // Radius of effect
    public float lifetime = 3f;
    private Transform enemyTransform;
    public bool isbladestormdestroyed;// Duration of the black hole

    private float destroyTime;
    private float DOTElapsed;

    // Added healing variables
    private int totalDamageDealt;
    private int healingAmount;

    private void Start()
    {
        DOTElapsed = 1;
        destroyTime = Time.time + lifetime; // Calculate the time to destroy the black hole
        totalDamageDealt = 0;
        healingAmount = 0;
    }

    public void SetEnemyTransform(Transform enemy)
    {
        enemyTransform = enemy;
    }

    private void Update()
    {
        ApplyDamageOverTime();

        // Check if it's time to destroy the black hole
        if (Time.time >= destroyTime)
        {

            Destroy(gameObject);
        }

        DOTElapsed += Time.deltaTime;

        // Move the bladestorm to follow the player
        Vector3 enemyPosition = GetEnemyPosition();
        transform.position = new Vector3(enemyPosition.x, enemyPosition.y, transform.position.z);
    }

    private void ApplyDamageOverTime()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);


        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Skill2Tutorial"))
            {
                continue; // Skip the player and Skill2Tutorial
            }

            if (collider.CompareTag("Player"))
            {
                // Handle the enemy logic here
                EnemyEntity enemy = collider.GetComponent<EnemyEntity>();
                PlayerEntity player = collider.GetComponent<PlayerEntity>();

                if (player != null && enemy != null && DOTElapsed > timerRate)
                {
                    player.ChangeHealth(-damageOverTime);
                    enemy.ChangeHealth(damageOverTime);
                    Debug.Log("Enemy hit by BH");
                }
            }
        }

        if (DOTElapsed > timerRate)
        {
            DOTElapsed = 0;
        }
    }

    public void SetDamageOverTime(int damage)
    {
        damageOverTime = damage;
    }

    private Vector3 GetEnemyPosition()
    {
        // Get the player's position
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");

        if (enemy != null)
        {
            return enemy.transform.position;
        }

        // Return a default position if the player is not found
        return Vector3.zero;
    }
}
