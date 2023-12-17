using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloodPool : MonoBehaviour
{
    public int damageOverTime = 1; // Integer damage applied per second
    private float timerRate = 0.5f;
    public float radius = 5f; // Radius of effect
    public float lifetime = 3f;
    private Transform playerTransform;
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

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
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
        Vector3 playerPosition = GetPlayerPosition();
        transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
    }

    private void ApplyDamageOverTime()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        // Dictionary to store damage dealt to each enemy
        Dictionary<EnemyEntity, int> damageToEnemies = new Dictionary<EnemyEntity, int>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") || collider.CompareTag("Skill2Tutorial"))
            {
                continue; // Skip the player and Skill2Tutorial
            }

            if (collider.CompareTag("Enemy"))
            {
                // Handle the enemy logic here
                EnemyEntity enemy = collider.GetComponent<EnemyEntity>();

                if (enemy != null && DOTElapsed > timerRate)
                {
                    int damageDealt = -damageOverTime;

                    // Accumulate damage dealt to each enemy
                    if (damageToEnemies.ContainsKey(enemy))
                    {
                        damageToEnemies[enemy] += Mathf.Abs(damageDealt);
                    }
                    else
                    {
                        damageToEnemies[enemy] = Mathf.Abs(damageDealt);
                    }

                    enemy.ChangeHealth(damageDealt);
                    Debug.Log("Enemy hit by BH");
                }
            }
        }

        if (DOTElapsed > timerRate)
        {
            DOTElapsed = 0;

            // Sum up the total damage dealt to all enemies
            int totalDamageDealt = damageToEnemies.Values.Sum();

            // Add healing logic here before destroying
            PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().ChangeHealth(totalDamageDealt, true);
        }
    }

    public void SetDamageOverTime(int damage)
    {
        damageOverTime = damage;
    }

    private Vector3 GetPlayerPosition()
    {
        // Get the player's position
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            return player.transform.position;
        }

        // Return a default position if the player is not found
        return Vector3.zero;
    }
}
