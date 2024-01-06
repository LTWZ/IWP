using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    public float explosionTime = 2.0f;  // Time until detonation
    public int damage = 10;         // Amount of damage to deal
    [SerializeField] LayerMask enemyLayer;         // Specify the enemy layer
    private float slowdownFactor = 0.5f;
    public float radius = 1.0f;

    private float timer;

    void Start()
    {
        timer = explosionTime;
    }

    public void ApplyDebuff(float factor)
    {
        slowdownFactor = factor;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Detonate();
        }
        //Debug.Log(enemyLayer.value);
    }

    void Detonate()
    {
        // Find all colliders within the AoE circle
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        // Deal damage to each enemy in the AoE
        foreach (Collider2D hitCollider in hitColliders)
        {
            EnemyEntity enemy = hitCollider.GetComponent<EnemyEntity>();
            if (enemy != null)
            {
                enemy.ChangeHealth(-damage);
            }


        }

        // Destroy the AoE object
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.GetComponent<EnemyEntity>().ApplySpeedModifier(0.5f);
            collision.GetComponent<EnemyEntity>().isEnemySlowed = true;
            collision.GetComponent<EnemyEntity>().EnemySlowed();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.GetComponent<EnemyEntity>().ApplySpeedModifier(1.0f);
            collision.GetComponent<EnemyEntity>().isEnemySlowed = false;
            collision.GetComponent<EnemyEntity>().EnemySlowed();
        }
    }
}
