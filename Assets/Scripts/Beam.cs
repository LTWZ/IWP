using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public int damage = 1;
    public Enemy_1 enemy;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collided object is an enemy
        if (collider.GetComponent<EnemyEntity>())
            collider.GetComponent<EnemyEntity>().ChangeHealth(-1);

        if (collider.gameObject.tag != "Player")
        {
            Debug.Log(collider.gameObject);
            Destroy(gameObject);
        }

    }
}
