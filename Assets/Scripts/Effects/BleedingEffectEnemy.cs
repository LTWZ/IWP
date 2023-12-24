using UnityEngine;

public class BleedingEffect : MonoBehaviour
{
    private float bleedingDamagePerSecond;
    private float bleedingDuration;
    private float remainingDuration;
    private float timeBetweenTicks = 0.5f;  // Adjust this value as needed
    private float nextTickTime;

    public void StartBleeding(float damagePerSecond, float duration)
    {
        bleedingDamagePerSecond = damagePerSecond;
        bleedingDuration = duration;
        remainingDuration = duration;
        nextTickTime = Time.time + timeBetweenTicks;  // Set initial tick time

        // Start the bleeding effect (you might play particle effects, animations, etc.)
        // ...
    }

    public void ExtendBleedingDuration(float extensionDuration)
    {
        // Extend the bleeding duration
        remainingDuration += extensionDuration;

        // Optionally, you might modify the visual representation of the bleeding effect
        // ...
    }

    private void Update()
    {
        // Update the bleeding effect over time
        if (remainingDuration > 0)
        {
            if (Time.time >= nextTickTime)
            {
                ApplyBleedingDamage();
                nextTickTime = Time.time + timeBetweenTicks;  // Set next tick time
            }

            remainingDuration -= Time.deltaTime;
        }
        else
        {
            // Stop the bleeding effect (you might stop particle effects, animations, etc.)
            Destroy(this);
        }
    }

    void ApplyBleedingDamage()
    {
        // Calculate the damage to apply, rounding up
        int damageToApply = Mathf.CeilToInt(bleedingDamagePerSecond * timeBetweenTicks);

        // Apply damage over time to the object with this effect
        // This might be adjusted based on your game's health system
        GetComponent<PlayerEntity>().ChangeHealth(-damageToApply);
    }
}
