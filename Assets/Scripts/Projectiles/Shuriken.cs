using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private int damage = 5;
    private Transform playerTransform;
    public float ninjastarSpeed = 10f;
    private bool isReturning = false;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the dagger has hit an enemy
        if (collider.GetComponent<EnemyEntity>())
        {
            collider.GetComponent<EnemyEntity>().ChangeHealth(-damage);
        }

        //if (collider.gameObject.tag == "Map" && !isReturning)
        //{
        //    // Start returning to the player
        //    StartCoroutine(ReturnToPlayer());
        //}
    }

    IEnumerator ReturnToPlayer()
    {
        isReturning = true;

        Vector2 playerPosition = playerTransform.position;

        while (Vector2.Distance(transform.position, playerPosition) > 0.1f)
        {
            Vector2 returnDirection = (playerPosition - (Vector2)transform.position);
            GetComponent<Rigidbody2D>().velocity = returnDirection.normalized * ninjastarSpeed;

            if (returnDirection.magnitude <= 0.1f)
                break;

            yield return null;
        }

        // Cleanup
        Destroy(gameObject);
    }
}
