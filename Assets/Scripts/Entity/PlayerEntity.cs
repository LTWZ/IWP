using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEntity : MonoBehaviour
{
    [SerializeField] protected int Hp;
    [SerializeField] protected int Coins;
    [SerializeField] protected int HealthPotionCount;
    [SerializeField] protected int ManaPotionCount;
    [SerializeField] protected int speed;
    [SerializeField] protected int Mana;
    protected int currentHP;

    [Header("Mana Code")]
    [SerializeField] protected int maxMana = 100;
    protected int currMana;
    protected int currCoins;
    protected int currHealthPotions;
    protected int currManaPotions;
    private UIManager uiManager;
    public bool canUseskill1 = false;
    public bool canUseskill2 = false;
    public bool canUseskill3 = false;
    public bool canUseskill4 = false;
    public bool isplayerSlowed = false;
    public bool isplayerSlowedByFinalBossAOE = false;
    public bool isplayerSpedUp = false;
    public bool isplayerRooted = false;
    private bool isFlashing = false;

    [SerializeField] AbitiliesSet abitiliesSet;
    private void Awake()
    {
        currentHP = Hp;
        currMana = Mana;
        currCoins = Coins;
        currHealthPotions = HealthPotionCount;
        currManaPotions = ManaPotionCount;
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log(GetCurrentManaPercentage());
            if (GetCurrManaPotionAmt() > 0 && GetCurrentManaPercentage() < 100f)
            {
                ChangeManaPotionAmt(-1);
                ChangeMana(20);
            }
            else
            {
                Debug.Log("NO MANA POTION LEFT");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (GetCurrHPPotionAmt() > 0 && GetCurrentHealthPercentage() < 100f) 
            {
                ChangeHealthPotionAmt(-1);
                ChangeHealth(20, true);
            }
            else
            {
                Debug.Log("NO MANA POTION LEFT");
            }
        }
    }

    public int GetCurrMana()
    {
        return currMana;
    }

    public int GetCurrHealth()
    {
        return currentHP;
    }

    public int GetCurrCoins()
    {
        return currCoins;
    }

    public int GetCurrHPPotionAmt()
    {
        return currHealthPotions;
    }

    public int GetCurrManaPotionAmt()
    {
        return currManaPotions;
    }

    public float GetCurrentHealthPercentage()
    {
        return (float)currentHP / Hp * 100f;
    }

    public float GetCurrentManaPercentage()
    {
        return (float)currMana / maxMana * 100f;
    }

    public void SetCurrHealth(int loadedCurrHP)
    {
        currentHP = loadedCurrHP;
    }

    public void SetCurrMana(int loadedCurrMana)
    {
        currMana = loadedCurrMana;
    }

    public void SetCurrCoins(int loadedCurrCoins)
    {
        currCoins = loadedCurrCoins;
    }

    public void SetHPPotionAmt(int loadedHPPotionAmt)
    {
        currHealthPotions = loadedHPPotionAmt;
    }

    public void SetManaPotionAmt(int loadedManaPotionAmt)
    {
        currManaPotions = loadedManaPotionAmt;
    }

    public AbitiliesSet GetAbilitiesSet()
    {
        return abitiliesSet;
    }

    public void GetUIManager()
    {
        if (UIManager.GetInstance() != null)
        {
            uiManager = UIManager.GetInstance();
            uiManager.UpdateHealthDisplay(currentHP, Hp);
            uiManager.UpdateManaDisplay(currMana, maxMana);
            uiManager.UpdateCoinsDisplay(currCoins);
            uiManager.UpdateHPPotionDisplay(currHealthPotions);
            uiManager.UpdateManaPotionDisplay(currManaPotions);
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

    public virtual void UpdateCoins()
    {

    }

    public virtual void UpdateManaPotionsAmt()
    {

    }

    public virtual void UpdateHPPotionsAmt()
    {

    }



    public virtual void UpdateManaTutorial()
    {

    }

    /// <summary>
    /// Change the health of the playerEntity. Use negative value to represent reducing health and positive to represent adding health.
    /// </summary>
    public void ChangeHealth(int amtChanged, bool isPlayerDamage = false)
    {
        if (!isPlayerDamage && !isFlashing)
        {
            StartCoroutine(FlashPlayer());
        }

        currentHP += amtChanged;

        if (currentHP > Hp)
        {
            currentHP = Hp;
        }

        if (currentHP <= 0)
        {
            SceneManager.LoadScene("GameOver");
            Destroy(PlayerManager.GetInstance().gameObject);
            Destroy(EnemyManager.GetInstance().gameObject);
            Destroy(LevelManager.GetInstance().gameObject);
            Destroy(WeaponManager.GetInstance().gameObject);
        }

        uiManager.UpdateHealthDisplay(currentHP, Hp);
    }

    private IEnumerator FlashPlayer()
    {
        // Set the flag to indicate that the coroutine is running
        isFlashing = true;

        // Get the current player
        GameObject currentPlayer = PlayerManager.GetInstance().GetCurrentPlayer();

        if (currentPlayer != null)
        {
            // Retrieve the SpriteRenderer component of the child object
            SpriteRenderer playerRenderer = currentPlayer.GetComponentInChildren<SpriteRenderer>();

            if (playerRenderer != null)
            {
                // Number of times the player will flash
                int numberOfFlashes = 2;

                // Original color of the player sprite
                Color originalColor = playerRenderer.color;

                for (int i = 0; i < numberOfFlashes; i++)
                {
                    // Toggle the color alpha (transparency)
                    playerRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

                    // Wait for a short duration
                    yield return new WaitForSeconds(0.1f);

                    // Toggle back to the original color
                    playerRenderer.color = originalColor;

                    // Wait for a short duration
                    yield return new WaitForSeconds(0.1f);
                }

                // Ensure player is visible at the end
                playerRenderer.color = originalColor;
            }
            else
            {
                Debug.LogError("SpriteRenderer component not found on the child object of the current player.");
            }
        }
        else
        {
            Debug.LogError("Current player not found.");
        }

        // Set the flag to indicate that the coroutine has finished
        isFlashing = false;
    }

    public void ChangeMana(int amtChanged)
    {
        currMana += amtChanged;

        if (currMana > maxMana)
        {
            currMana = maxMana;
        }

        //if (currMana <= 0)
        //{
        //    // die ofc
        //}

        currMana = Mathf.Clamp(currMana, 0, maxMana);
        uiManager.UpdateManaDisplay(currMana, maxMana);
    }

    public void ChangeCoins(int amtChanged)
    {
        currCoins += amtChanged;
        uiManager.UpdateCoinsDisplay(currCoins);
    }

    public void ChangeHealthPotionAmt(int amtChanged)
    {
        currHealthPotions += amtChanged;
        uiManager.UpdateHPPotionDisplay(currHealthPotions);
    }

    public void ChangeManaPotionAmt(int amtChanged)
    {
        currManaPotions += amtChanged;
        uiManager.UpdateManaPotionDisplay(currManaPotions);
    }

    // this is an issue u need fix now. OnTriggerEnter dont trigger bcos both ur enemy and ur player has a boxcollider that is not isTrigger
    // so both cant go into each other, hence not triggering OnTirggerENter. you need to like find a way to implement a dummy isTrigger hitbox.
    // However, this is dumb cos like. Your hitbox would be bigger than what u look like. also, u need to do it not on the parent unless
    // u have a sprite as a child, and u add that as the actual collider or something. ur parent have the isTrigger. btw this my method, ask randall first.
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<EnemyEntity>())
    //    {
    //        if (Time.time >= attackTimer)
    //        {
    //            Debug.Log("Take dmg");
    //            ChangeHealth(-collision.GetComponent<EnemyEntity>().GetAttackValue());
    //            UpdateHP();

    //            // Set the next allowed attack time
    //            attackTimer = Time.time + attackCooldown;
    //        }
    //        else
    //        {
    //            // Optionally, you can provide feedback that the enemy is on cooldown
    //            Debug.Log("Enemy is on cooldown");
    //        }
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{

    //}
}