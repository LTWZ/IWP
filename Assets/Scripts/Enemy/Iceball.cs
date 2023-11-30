using UnityEngine;

public class Iceball : MonoBehaviour
{
    private GameObject targetPlayer;
    public int damage = 1;

    private void Update()
    {
        targetPlayer = EnemyManager.GetInstance().GetPlayerReference();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerEntity>())
        {
            collider.GetComponent<PlayerEntity>().ChangeHealth(-damage);
            PlayerMovement.GetInstance().Player.isplayerSlowed = true;
            // Destroy the fireball upon collision with the player
            Destroy(gameObject);
        }

        if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "RoomManager" && collider.gameObject.tag != "Iceball")
        {
            Destroy(gameObject);
        }
    }
}