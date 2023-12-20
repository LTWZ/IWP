using UnityEngine;

public class Punch : MonoBehaviour
{
    private GameObject targetPlayer;
    public int damage = 10;

    private void Update()
    {
        targetPlayer = EnemyManager.GetInstance().GetPlayerReference();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerEntity>())
        {
            // Calculate the knockback direction and force
            Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;
            float knockbackForce = 5f; // Adjust the force as needed

            // Apply knockback to the player
            collider.GetComponent<PlayerMovement>().ApplyKnockback(knockbackDirection, knockbackForce);

            // Deal damage to the player
            collider.GetComponent<PlayerEntity>().ChangeHealth(-damage);

            // Destroy the punch object upon collision with the player
            Destroy(gameObject);
        }

        if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "RoomManager" && collider.gameObject.tag != "Room" && collider.gameObject.tag != "Fireball" && collider.gameObject.tag != "Iceball" && collider.gameObject.tag != "Bullet" && collider.gameObject.tag != "BloodPool" && collider.gameObject.tag != "FireAOE")
        {
            Destroy(gameObject);
        }
    }
}
