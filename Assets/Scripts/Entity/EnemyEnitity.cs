using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] protected int Hp;
    [SerializeField] protected float speed;
    protected int currHealth;
    protected float currSpeed;
    protected float slowdownFactor;
    [SerializeField] Healthbar healthbar;
    [SerializeField] protected int attackValue;

    private void Awake()
    {
        if (EnemyManager.GetInstance() != null)
        {
            Hp = EnemyManager.GetInstance().LoadHP();
        }
        if (EnemyManager.GetInstance() != null)
        {
            speed = EnemyManager.GetInstance().LoadSpeed();
        }
    }


    /// <summary>
    /// Change the health of the enemyEntity. Use negative value to represent reducing health and positive to represent adding health.
    /// </summary>
    public void ChangeHealth(int amtChanged)
    {
        currHealth += amtChanged;
        Debug.Log(currHealth);

        currHealth = Mathf.Clamp(currHealth, 0, Hp);
        // btw since u have reference to hp/maxhp. if u want u can go ahead and add more functionality like if they below certain
        // hp, they trigger something.

        if (currHealth <= 0)
        {
            // die ofc
        }
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


    protected virtual void Update()
    {
        if (healthbar) {
            healthbar.SetMinandMax(0, Hp);
            healthbar.UpdateContent(currHealth);
        }
        UpdateHPEnemy();
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
}
