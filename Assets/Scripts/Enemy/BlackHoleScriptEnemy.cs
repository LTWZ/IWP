using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleScriptEnemy : MonoBehaviour
{
    public float pullForce = 1f; // Force to pull objects towards the black hole
    public int damageOverTime = 1; // Integer damage applied per second
    private int timerRate = 1;
    public float radius = 5f; // Radius of effect
    public float lifetime = 3f; // Duration of the black hole

    private float destroyTime;
    private float DOTElapsed;
    // Time to destroy the black hole

    private void Start()
    {
        DOTElapsed = 1;
        destroyTime = Time.time + lifetime; // Calculate the time to destroy the black hole
    }

    private void Update()
    {
        ApplyPullForce();

        // Check if it's time to destroy the black hole
        if (Time.time >= destroyTime)
        {
            Destroy(gameObject); // Destroy the black hole GameObject
        }

        DOTElapsed += Time.deltaTime;
    }

    private void ApplyPullForce()
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
                PlayerEntity player = collider.GetComponent<PlayerEntity>();

                if (player != null)
                {
                    Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

                    if (rb != null)
                    {
                        // Calculate force direction
                        Vector2 direction = (transform.position - rb.transform.position).normalized;
                        rb.AddForce(direction * pullForce);
                        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 50f);
                    }

                    if (DOTElapsed > timerRate)
                    {
                        player.ChangeHealth(-damageOverTime);
                        Debug.Log("boss_2 hit by BH");
                    }
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

    public void SetPullForce(float force)
    {
        pullForce = force;
    }
}