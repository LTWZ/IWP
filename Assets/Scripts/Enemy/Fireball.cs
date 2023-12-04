using UnityEngine;

public class Fireball : MonoBehaviour
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

            // Destroy the fireball upon collision with the player
            Destroy(gameObject);
        }

        if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "RoomManager" && collider.gameObject.tag != "Room" && collider.gameObject.tag != "Fireball")
        {
            Destroy(gameObject);
        }
    }
}
