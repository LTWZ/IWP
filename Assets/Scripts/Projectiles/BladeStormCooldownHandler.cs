using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeStormCooldownHandler : MonoBehaviour
{
    private System.Action cooldownHandler;

    public void SetCooldownHandler(System.Action handler)
    {
        cooldownHandler = handler;
    }

    private void Start()
    {
        StartCoroutine(DelayCooldown());
    }

    private IEnumerator DelayCooldown()
    {
        // Adjust this delay value based on your BladeStorm's intended lifetime
        float delay = 3f; // Example: 5 seconds

        yield return new WaitForSeconds(delay);

        // Call the cooldown handler after the delay
        cooldownHandler?.Invoke();
    }
}