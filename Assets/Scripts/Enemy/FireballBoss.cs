using UnityEngine;

public class FireballBoss : MonoBehaviour
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
            collider.GetComponent<PlayerEntity>().ChangeHealth(-damage);
            collider.GetComponent<PlayerEntity>().isplayerRooted = true;

            // Destroy the fireball upon collision with the player
            Destroy(gameObject);
        }

        if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "RoomManager" && collider.gameObject.tag != "Room" && collider.gameObject.tag != "Fireball" && collider.gameObject.tag != "Iceball" && collider.gameObject.tag != "Bullet" && collider.gameObject.tag != "BloodPool" && collider.gameObject.tag != "FireAOE")
        {
            Destroy(gameObject);
        }
    }
}
