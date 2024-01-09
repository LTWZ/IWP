using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEffects : MonoBehaviour
{
    public float explosionTime = 5.0f;  // Time until detonation  // Amount of damage to deal
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

        // Destroy the AoE object
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.GetComponent<EnemyEntity>().ApplySpeedModifier(0.5f);
            collision.GetComponent<EnemyEntity>().isEnemySlowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.GetComponent<EnemyEntity>().ApplySpeedModifier(1.0f);
            collision.GetComponent<EnemyEntity>().isEnemySlowed = false;
        }
    }
}

