using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShuriken : MonoBehaviour
{
    private int damage = 3;
    private Transform enemyTransform;
    public float ninjastarSpeed = 10f;
    private bool isReturning = false;

    private void Start()
    {
        enemyTransform = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the dagger has hit an enemy
        if (collider.GetComponent<PlayerEntity>())
        {
            collider.GetComponent<PlayerEntity>().ChangeHealth(-damage);
        }

        //if (!isReturning)
        //{
        //    // Start returning to the player
        //    StartCoroutine(ReturnToEnemy());
        //}
    }

    //IEnumerator ReturnToEnemy()
    //{
    //    isReturning = true;

    //    Vector2 enemyPosition = enemyTransform.position;

    //    while (Vector2.Distance(transform.position, enemyPosition) > 0.1f)
    //    {
    //        Vector2 returnDirection = (enemyPosition - (Vector2)transform.position);
    //        GetComponent<Rigidbody2D>().velocity = returnDirection.normalized * ninjastarSpeed;

    //        if (returnDirection.magnitude <= 1f)
    //            break;

    //        yield return null;
    //    }

    //    // Cleanup
    //    Destroy(gameObject);
    //}
}
