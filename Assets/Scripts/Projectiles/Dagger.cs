using UnityEngine;

public class Dagger : MonoBehaviour
{
    public int damage = 10;
    public float bleedingDamagePerSecond = 5f;
    public float bleedingDuration = 5f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the dagger has hit an enemy
        if (collider.GetComponent<EnemyEntity>())
        {
            collider.GetComponent<EnemyEntity>().ChangeHealth(-damage);

            if (collider.gameObject.tag != "Player" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "RoomManager" && collider.gameObject.tag != "Iceball" && collider.gameObject.tag != "Room")
            {
                Destroy(gameObject);
            }

            // Start the bleeding effect
            StartBleedingEffect(collider.GetComponent<EnemyEntity>());
        }
    }

    void StartBleedingEffect(EnemyEntity enemy)
    {
        // Check if the enemy already has a bleeding effect
        BleedingEffect existingBleedingEffect = enemy.GetComponent<BleedingEffect>();

        if (existingBleedingEffect == null)
        {
            // Create a new BleedingEffect component on the enemy
            BleedingEffect bleedingEffect = enemy.gameObject.AddComponent<BleedingEffect>();

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
