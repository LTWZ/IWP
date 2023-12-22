using UnityEngine;

public class EnemyDagger : MonoBehaviour
{
    public int damage = 10;
    public float bleedingDamagePerSecond = 5f;
    public float bleedingDuration = 5f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the dagger has hit an enemy
        if (collider.GetComponent<PlayerEntity>())
        {
            collider.GetComponent<PlayerEntity>().ChangeHealth(-damage);

            if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "RoomManager" && collider.gameObject.tag != "Iceball" && collider.gameObject.tag != "Room" && collider.gameObject.tag != "Fireball")
            {
                Destroy(gameObject);
            }

            // Start the bleeding effect
            StartBleedingEffect(collider.GetComponent<PlayerEntity>());
        }
    }

    void StartBleedingEffect(PlayerEntity player)
    {
        // Check if the enemy already has a bleeding effect
        BleedingEffectEnemy existingBleedingEffect = player.GetComponent<BleedingEffectEnemy>();

        if (existingBleedingEffect == null)
        {
            // Create a new BleedingEffect component on the enemy
            BleedingEffectEnemy bleedingEffect = player.gameObject.AddComponent<BleedingEffectEnemy>();

            // Set bleeding parameters
            bleedingEffect.StartBleeding(bleedingDamagePerSecond, bleedingDuration);
        }
        else
        {
            // If the enemy already has a bleeding effect, extend its duration or modify as needed
            existingBleedingEffect.ExtendBleedingDuration(bleedingDuration);
        }
    }
}
