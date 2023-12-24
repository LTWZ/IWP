using System.Collections;
using UnityEngine;

public class EnemyAOEFinalBoss : MonoBehaviour
{
    public int damageOverTime = 1;
    private float timerRate = 0.5f;
    public float radius = 5f;
    public float lifetime = 3f;
    private Transform playerTransform;
    public bool isBladestormDestroyed;

    private float destroyTime;
    private float DOTElapsed;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        DOTElapsed = 1;
        destroyTime = Time.time + lifetime;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Start the flashing coroutine
        StartCoroutine(FlashSprite(0.25f)); // Change 0.5f to your desired interval
    }

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }

    private void Update()
    {
        ApplyDamageOverTime();

        if (Time.time >= destroyTime)
        {
            Destroy(gameObject);
        }

        DOTElapsed += Time.deltaTime;
    }

    private void ApplyDamageOverTime()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Skill2Tutorial"))
            {
                continue;
            }

            if (collider.CompareTag("Player"))
            {
                PlayerEntity player = collider.GetComponent<PlayerEntity>();
                if (player != null && DOTElapsed > timerRate)
                {
                    int damageDealt = -damageOverTime;
                    player.isplayerSlowedByFinalBossAOE = true;
                    player.ChangeHealth(damageDealt);
                    Debug.Log("Enemy hit by BH");
                }
            }
        }

        if (DOTElapsed > timerRate)
        {
            DOTElapsed = 0;
        }
    }

    public void SetDamageOverTime(int damage)
    {
        damageOverTime = damage;
    }

    IEnumerator FlashSprite(float interval)
    {
        while (true)
        {
            // Reduce opacity
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f); // Adjust the alpha value (0.5f) as needed

            yield return new WaitForSeconds(0.1f); // Adjust the duration of the reduced opacity

            // Restore opacity
            spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(interval);
        }
    }
}