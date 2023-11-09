using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [SerializeField] protected int Hp;
    [SerializeField] protected int speed;
    [SerializeField] protected int Mana;
    protected int currentHP;

    [Header("Mana Code")]
    [SerializeField] protected int maxMana = 100;
    protected int currMana;
    private UIManager uiManager;

    private void Awake()
    {
        currentHP = Hp;
        currMana = Mana;
    }

    public void GetUIManager()
    {
        if (UIManager.GetInstance() != null)
        {
            uiManager = UIManager.GetInstance();
            uiManager.UpdateHealthDisplay(currentHP, Hp);
            uiManager.UpdateManaDisplay(currMana, maxMana);
        }
    }

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

    public virtual void UpdateHP()
    {
        
    }

    public virtual void UpdateMana()
    {

    }

    /// <summary>
    /// Change the health of the playerEntity. Use negative value to represent reducing health and positive to represent adding health.
    /// </summary>
    public void ChangeHealth(int amtChanged)
    {
        currentHP += amtChanged;

        if (currentHP > Hp)
        {
            currentHP = Hp;
        }

        if (currentHP <= 0)
        {
            // die ofc
        }

        uiManager.UpdateHealthDisplay(currentHP, Hp);
    }

    public void ChangeMana(int amtChanged)
    {
        currMana += amtChanged;

        if (currentHP > Hp)
        {
            currentHP = Hp;
        }

        if (currentHP <= 0)
        {
            // die ofc
        }

        uiManager.UpdateManaDisplay(currMana, Mana);
    }

    // this is an issue u need fix now. OnTriggerEnter dont trigger bcos both ur enemy and ur player has a boxcollider that is not isTrigger
    // so both cant go into each other, hence not triggering OnTirggerENter. you need to like find a way to implement a dummy isTrigger hitbox.
    // However, this is dumb cos like. Your hitbox would be bigger than what u look like. also, u need to do it not on the parent unless
    // u have a sprite as a child, and u add that as the actual collider or something. ur parent have the isTrigger. btw this my method, ask randall first.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyEntity>())
        {
            Debug.Log("Take dmg");
            ChangeHealth(-collision.GetComponent<EnemyEntity>().GetAttackValue());
            UpdateHP();
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // update on timer here
    }
}
