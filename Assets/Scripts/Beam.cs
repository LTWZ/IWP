using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public int damage = 10;
    public Enemy_1 enemy;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collided object is an enemy
        if (collider.GetComponent<EnemyEntity>())
            collider.GetComponent<EnemyEntity>().ChangeHealth(-damage);

        if (collider.gameObject.tag != "Player" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "RoomManager" && collider.gameObject.tag != "Skill1Tutorial" && collider.gameObject.tag != "Room")
        {
            Destroy(gameObject);
        }


    }
}
