using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    public float explosionTime = 2.0f;  // Time until detonation
    public float damage = 10.0f;         // Amount of damage to deal
    [SerializeField] LayerMask enemyLayer;         // Specify the enemy layer
    private float slowdownFactor = 0.5f;

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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius, enemyLayer);

        // Deal damage to each enemy in the AoE
        foreach (Collider2D hitCollider in hitColliders)
        {
            EnemyEntity enemy = hitCollider.GetComponent<EnemyEntity>();
            if (enemy != null)
            {
                enemy.ChangeHealth(-3);
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.GetComponent<EnemyEntity>().ApplySpeedModifier(1.0f);
        }
    }
}
