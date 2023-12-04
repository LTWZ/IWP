using UnityEngine;

public class Meteor : MonoBehaviour
{
    private GameObject targetPlayer;
    public int damage = 10; // Adjust damage as needed
    public float damageInterval = 1f; // Adjust the interval between damage ticks

    private bool playerInside = false;
    private float damageTimer = 0f;

    private void Update()
    {
        targetPlayer = EnemyManager.GetInstance().GetPlayerReference();

        // Check if the player is inside the meteor
        if (playerInside)
        {
            // Update the damage timer
            damageTimer -= Time.deltaTime;

            // Deal damage at intervals
            if (damageTimer <= 0f)
            {
                DealDamageOverTime();
                damageTimer = damageInterval;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider is the player
        if (collider.GetComponent<PlayerEntity>())
        {
            // Set the player as inside the meteor
            playerInside = true;
        }

        // Check if the collider is not an enemy, skill, room manager, room, or another meteor
        if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "RoomManager" && collider.gameObject.tag != "Room" && collider.gameObject.tag != "Meteor")
        {
            // Destroy the meteor upon collision with other objects
            //Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Check if the collider is the player
        if (collider.GetComponent<PlayerEntity>())
        {
            // Set the player as outside the meteor
            playerInside = false;
        }
    }

    private void DealDamageOverTime()
    {
        // Deal damage to the player over time
        targetPlayer.GetComponent<PlayerEntity>().ChangeHealth(-damage);
    }
}