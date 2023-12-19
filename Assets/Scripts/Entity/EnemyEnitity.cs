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
    private bool isFlashing = false;

    /// <summary>
    /// Change the health of the enemyEntity. Use negative value to represent reducing health and positive to represent adding health.
    /// </summary>
    /// 

    public virtual void ChangeHealth(int amtChanged)
    {
        if (isFlashing == false)
        {
            // Pass the enemy game object to FlashEnemy
            StartCoroutine(FlashEnemy(gameObject));
        }

        currHealth += amtChanged;
        Debug.Log(currHealth);

        currHealth = Mathf.Clamp(currHealth, 0, Hp);

        if (currHealth <= 0)
        {
            PlayerEntity player = FindObjectOfType<PlayerEntity>();
            if (player != null)
            {
                // Gain back a random amount of mana
                int manaGained = Random.Range(10, 21); // Adjust the range as needed
                player.ChangeMana(manaGained);
            }

            // Check if the enemy is part of the room before reducing the enemy count
            if (roomReference != null)
            {
                roomReference.ReduceEnemy();
            }

            // Die logic here
        }
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

    /// <summary>
    /// Apply the speed modifier according to the value inputted. e.g. 0.1 means 10% of the original speed. 2.0 means 200% of the original speed.
    /// </summary>
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

    /// <summary>
    /// Get the attack value of the enemy.
    /// </summary>
    public int GetAttackValue()
    {
        return attackValue;
    }

    /// <summary>
    /// Set the room reference to whatever room that created this is.
    /// </summary>
    public void SetRoomReference(Room theRoomReference)
    {
        roomReference = theRoomReference;
    }
}
