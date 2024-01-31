using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] protected int Hp;
    [SerializeField] protected float speed;
    protected int currHealth;
    protected float currSpeed;
    protected float slowdownFactor;
    [SerializeField] Healthbar healthbar;
    [SerializeField] protected int attackValue;
    [SerializeField] NavMeshAgent navMeshAgent;
    private Room roomReference;
    private FinalBossRoom roomReferenceBoss;
    private bool isFlashing = false;
    [SerializeField] Animator deathAnimation;
    [SerializeField] GameObject deathAnimationPrefab;
        // New field for tracking whether the enemy is slowed
    public bool isEnemySlowed = false;
    // New field for storing the original color
    private Color originalColor;
    private bool shouldResetColor = false;

    // New field for tracking the enemyRenderer
    private SpriteRenderer enemyRenderer;

    private void Awake()
    {
        // Retrieve the SpriteRenderer component of the enemy object
        enemyRenderer = GetComponentInChildren<SpriteRenderer>();
        if (enemyRenderer != null)
        {
            // Store the original color
            originalColor = enemyRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the child object of the enemy.");
        }
    }

    // Change the health of the enemyEntity. Use negative value to represent reducing health and positive to represent adding health.

    public virtual void ChangeHealth(int amtChanged, bool isSelfDamage = false)
    {
        if (!isSelfDamage && isFlashing == false)
        {
            // Pass the enemy game object to FlashEnemy
            StartCoroutine(FlashEnemy(gameObject));
            AudioManager.instance.PlaySFX("EnemyHit");
        }

        currHealth += amtChanged;
        Debug.Log(currHealth);

        currHealth = Mathf.Clamp(currHealth, 0, Hp);

        if (currHealth <= 0)
        {
            DeathAnimation();

            PlayerEntity player = FindObjectOfType<PlayerEntity>();
            if (player != null)
            {

                if (GetComponent<Enemy_3>())
                {

                }
                else
                {
                    // Gain back a random amount of mana
                    int manaGained = Random.Range(1,6);
                    int coinget = Random.Range(1, 3);
                    player.ChangeCoins(coinget);
                    player.ChangeMana(manaGained);// Adjust the range as needed
                }

            }

            // Check if the enemy is part of the room before reducing the enemy count
            if (roomReference != null)
            {
                roomReference.ReduceEnemy();
            }
            if (roomReferenceBoss != null)
            {
                roomReferenceBoss.ReduceEnemy();
            }
            AudioManager.instance.PlaySFX("EnemyDie");
            // Die logic here
            Destroy(gameObject);
        }
    }

    public virtual void DeathAnimation()
    {
        deathAnimation.SetTrigger("isDead");

        Instantiate(deathAnimationPrefab, transform.position, Quaternion.identity);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private IEnumerator FlashEnemy(GameObject enemy)
    {
        // Set the flag to indicate that the coroutine is running
        isFlashing = true;

        if (enemy != null)
        {
            // Retrieve the SpriteRenderer component of the enemy object
            SpriteRenderer enemyRenderer = enemy.GetComponentInChildren<SpriteRenderer>();

            if (enemyRenderer != null)
            {
                // Number of times the enemy will flash
                int numberOfFlashes = 2;

                // Original color of the enemy sprite
                Color originalColor = enemyRenderer.color;

                for (int i = 0; i < numberOfFlashes; i++)
                {
                    // Toggle the color alpha (transparency)
                    enemyRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

                    // Wait for a short duration
                    yield return new WaitForSeconds(0.1f);

                    // Toggle back to the original color
                    enemyRenderer.color = originalColor;

                    // Wait for a short duration
                    yield return new WaitForSeconds(0.1f);
                }

                // Ensure enemy is visible at the end
                enemyRenderer.color = originalColor;
            }
            else
            {
                Debug.LogError("SpriteRenderer component not found on the child object of the enemy.");
            }
        }
        else
        {
            Debug.LogError("Enemy object is null.");
        }

        // Set the flag to indicate that the coroutine has finished
        isFlashing = false;
    }


    // Apply the speed modifier according to the value inputted. e.g. 0.1 means 10% of the original speed. 2.0 means 200% of the original speed.
    public void ApplySpeedModifier(float speedChanged)
    {
        currSpeed = speed * speedChanged;
    }

    //public void ApplySlowdown(float factor)
    //{
    //    // Adjust the enemy's movement speed based on the factor
    //    slowdownFactor = factor;
    //}

    public virtual void SetTarget()
    {

    }

    protected virtual void Update()
    {
        if (healthbar) {
            healthbar.SetMinandMax(0, Hp);
            healthbar.UpdateContent(currHealth);
        }
        UpdateHPEnemy();
        //navMeshAgent.updateRotation = false;
    }

    //void FixedUpdate()
    //{
    //    // Adjust the enemy's speed based on the slowdownFactor
    //    currSpeed = speed * slowdownFactor;
    //}

    public virtual void Skill1()
    {
        
    }

    public virtual void Skill2()
    {

    }

    public virtual void Skill3()
    {

    }

    public virtual void Skill4()
    {

    }

    public virtual void UpdateHPEnemy()
    {

    }

    public virtual void UpdateSpeedEnemy()
    {

    }

    //Get the attack value of the enemy.
    public int GetAttackValue()
    {
        return attackValue;
    }

    //Set the room reference to whatever room that created this is.
    public void SetRoomReference(Room theRoomReference)
    {
        roomReference = theRoomReference;
    }

    public void SetRoomReferenceBoss(FinalBossRoom theRoomReference)
    {
        roomReferenceBoss = theRoomReference;
    }
}
