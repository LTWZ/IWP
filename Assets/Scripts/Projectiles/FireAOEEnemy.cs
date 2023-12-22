using System.Collections;
using UnityEngine;

public class FireAOEEnemy : MonoBehaviour
{
    public int damageOverTime = 1; // Integer damage applied per second
    private float timerRate = 0.5f;
    public float radius = 5f; // Radius of effect
    public float lifetime = 3f;
    private Transform enemyTransform;
    public bool isbladestormdestroyed; // Duration of the black hole

    private float destroyTime;
    private float DOTElapsed;

    private void Start()
    {
        DOTElapsed = 1;
        destroyTime = Time.time + lifetime; // Calculate the time to destroy the black hole
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
                continue; // Skip the enemy and Skill2Tutorial
            }

            if (collider.CompareTag("Player"))
            {
                // Handle the player logic here
                PlayerEntity player = collider.GetComponent<PlayerEntity>();
                if (player != null && DOTElapsed > timerRate)
                {
                    player.ChangeHealth(-damageOverTime);
                    // Find all Boss_5 components in the scene
                    Boss_5[] bosses = GameObject.FindObjectsOfType<Boss_5>();
                    foreach (Boss_5 boss in bosses)
                    {
                        boss.ChangeHealth(damageOverTime, true);
                    }
                    Debug.Log("Player hit by Fire AOE");
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
        if (enemyTransform != null)
        {
            return enemyTransform.position;
        }

        // Return a default position if the enemy transform is not set
        return Vector3.zero;
    }
}