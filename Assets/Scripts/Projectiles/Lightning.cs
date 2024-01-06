using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public int damage = 10;
    public Enemy_1 enemy;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collided object is an enemy
        if (collider.GetComponent<PlayerEntity>())
        {
            collider.GetComponent<PlayerEntity>().ChangeHealth(-damage);
            collider.GetComponent<PlayerEntity>().isplayerRooted = true;
        }

       

        if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "RoomManager" && collider.gameObject.tag != "Skill1Tutorial" && collider.gameObject.tag != "Room" && collider.gameObject.tag != "Iceball" && collider.gameObject.tag != "Fireball")
        {
            Destroy(gameObject);
        }
    }
}
