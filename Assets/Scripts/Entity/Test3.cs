using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test3 : PlayerEntity
{
    [Header("Ability 1")]
    public float cooldown1 = 5;
    bool isCooldown1 = false;
    public KeyCode ability1;
    public float dashSpeed = 10f;
    public int dashDamage = 5;
    public float dashCollisionRadius = 1.0f; // Set the appropriate radius for checking collisions
    public LayerMask enemyLayer;
    private bool isMoving = false;// Adjust the default dash speed as needed

    [Header("Ability 2")]
    public float cooldown2 = 5;
    bool isCooldown2 = false;
    public KeyCode ability2;
    public GameObject aoeMovementPrefab;

    [Header("Ability 3")]
    public float cooldown3 = 5;
    bool isCooldown3 = false;
    public KeyCode ability3;
    public GameObject FireAOEPrefab;


    [Header("Ability 4")]
    public float cooldown4 = 5;
    public bool isCooldown4 = false;
    public KeyCode ability4;
    public GameObject bloodpoolPrefab; // Assign your bladestorm prefab in the Unity editor

    private void Start()
    {
        GetUIManager();
        UIManager.GetInstance().onCooldown += DisableCooldown;
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    public override void Update()
    {
        base.Update();
        UpdateHP();
        UpdateMana();
        UpdateCoins();
        UpdateHPPotionsAmt();
        UpdateManaPotionsAmt();

        // Check if the current scene is a tutorial scene
        bool isTutorialScene = IsTutorialScene();

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrManaPotionAmt() >= 1)
            {
                PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().ChangeManaPotionAmt(-1);
                PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().ChangeMana(20);
            }
            else
            {

            }
        }

        // If the scene is not a tutorial scene and the dialogue is not active, allow abilities
        if (!isTutorialScene && !DialogueManager.isActive)
        {
            Skill1();
            Skill2();
            Skill3();
            Skill4();
        }
        else
        {
            if (canUseskill1 == true)
            {
                Skill1();
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL2);
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL3);
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL4);
                UIManager.GetInstance().ToggleImage(1, true);
            }
            else
            {
                UIManager.GetInstance().AddBackSkillFillAmount(skillType.SKILL2);
                UIManager.GetInstance().AddBackSkillFillAmount(skillType.SKILL3);
                UIManager.GetInstance().AddBackSkillFillAmount(skillType.SKILL4);
                UIManager.GetInstance().ToggleImage(1, false);
            }
            if (canUseskill2 == true)
            {
                Skill2();
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL1);
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL3);
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL4);
                UIManager.GetInstance().ToggleImage(2, true);
            }
            else
            {
                UIManager.GetInstance().AddBackSkillFillAmount(skillType.SKILL1);
                UIManager.GetInstance().ToggleImage(2, false);
            }
            if (canUseskill3 == true)
            {
                Skill3();
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL1);
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL2);
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL4);
                UIManager.GetInstance().ToggleImage(3, true);
            }
            else
            {
                UIManager.GetInstance().ToggleImage(3, false);
            }
            if (canUseskill4 == true)
            {
                Skill4();
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL1);
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL2);
                UIManager.GetInstance().ResetSkillFillAmount(skillType.SKILL3);
                UIManager.GetInstance().ToggleImage(4, true);
            }
            else
            {
                UIManager.GetInstance().ToggleImage(4, false);
            }
            // Optionally handle the case when the scene is a tutorial or dialogue is active
        }

        // Check if the player is currently moving
        isMoving = Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f;

    }

    // Method to check if the current scene is a tutorial scene
    private bool IsTutorialScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Check by scene name
        if (currentScene.name == "TutorialLevel" || currentScene.name == "WizardTutorial" || currentScene.name == "RogueTutorial" || currentScene.name == "FighterTutorial")
        {
            return true;
        }

        // Alternatively, check by scene build index
        // if (currentScene.buildIndex == yourTutorialSceneBuildIndex)
        // {
        //     return true;
        // }

        return false;
    }

    public override void Skill1()
    {
        if (isMoving)
        {
            DashToMousePosition();
        }
    }

    void DashToMousePosition()
    {
        // Get the mouse position in world coordinates
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction to the mouse position
        Vector2 dashDirection = GetFourDirectionalDashDirection(mousePos - (Vector2)transform.position);

        if (Input.GetKeyDown(ability1) && isCooldown1 == false)
        {
            if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() >= 5)
            {
                // Dash towards the mouse position
                StartCoroutine(DashRoutine(dashDirection));
                AudioManager.instance.PlaySFX("Warriordash");
            }
            else if (currMana <= 5)
            {
                // Handle not enough mana case
            }
        }
    }

    Vector2 GetFourDirectionalDashDirection(Vector2 inputDirection)
    {
        float angle = Vector2.SignedAngle(Vector2.up, inputDirection);

        // Round the angle to the nearest 90 degrees
        if (angle >= -45f && angle < 45f)
        {
            return Vector2.up; // Up
        }
        else if (angle >= 45f && angle < 135f)
        {
            return Vector2.left; // Left
        }
        else if (angle >= 135f || angle < -135f)
        {
            return Vector2.down; // Down
        }
        else
        {
            return Vector2.right; // right
        }
    }

    HashSet<Collider2D> damagedEnemies = new HashSet<Collider2D>();

    IEnumerator DashRoutine(Vector2 dashDirection)
    {
        float dashDuration = 0.5f; // Adjust as needed

        // Disable the player's ability to control movement during the dash
        // You may want to adjust this based on your game's logic
        // For example, disable the player's Rigidbody2D or CharacterController

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            // Move the character continuously in the dash direction with a controllable speed
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);

            // Check for collisions with enemies
            CheckCollisionsWithEnemies();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Clear the set of damaged enemies after the dash is complete
        damagedEnemies.Clear();

        // Re-enable player control after the dash is complete

        // Set cooldown and update UI
        isCooldown1 = true;
        UIManager.GetInstance().UpdateCooldownStuff(cooldown1, skillType.SKILL1);

        // Use mana if the skill was successful
        if (canUseskill1 == true)
        {

        }
        else
        {
           ChangeMana(-5);
        }
        
    }

    void CheckCollisionsWithEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, dashCollisionRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Check if the enemy has already been damaged during this dash
            if (!damagedEnemies.Contains(enemy))
            {
                // Deal damage to the enemy
                enemy.GetComponent<EnemyEntity>().ChangeHealth(-dashDamage);

                // Add the enemy to the set to mark it as damaged
                damagedEnemies.Add(enemy);
            }
        }
    }

    public override void Skill2()
    {
        if (Input.GetKeyDown(ability2) && isCooldown2 == false)
        {
            if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() >= 10)
            {
                // Get the mouse position in world coordinates
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Instantiate the AoE Prefab at the mouse position
                GameObject aoeObject = Instantiate(aoeMovementPrefab, mousePos, Quaternion.identity);

                PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().isplayerSpedUp = true;

                AudioManager.instance.PlaySFX("Warriorspeed");

                isCooldown2 = true;
                UIManager.GetInstance().UpdateCooldownStuff(cooldown2, skillType.SKILL2);

                if (canUseskill2 == true)
                {

                }
                else
                {
                    // Consume mana
                    ChangeMana(-10);
                }
            }
            else
            {
                // Not enough mana, implement feedback or other logic here
            }
        }

    }

    public override void Skill3()
    {
        if (Input.GetKeyDown(ability3) && isCooldown3 == false)
        {
            if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrHealth() > 30)
            {

                isCooldown3 = true;
                UIManager.GetInstance().UpdateCooldownStuff(cooldown3, skillType.SKILL3);
                // Get the mouse position in world coordinates
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Summon the bladestorm
                // Instantiate the bladestorm prefab at the player's position
                Instantiate(FireAOEPrefab, transform.position, Quaternion.identity);

                AudioManager.instance.PlaySFX("Warriorfire");
                if (canUseskill3 == true)
                {

                }
                else
                {
                    // Consume health
                    ChangeHealth(-30, true);
                }
            }
            else
            {
                // Not enough mana or ninja star already thrown, implement feedback or other logic here
            }
        }
    }

    public override void Skill4()
    {
        if (Input.GetKeyDown(ability4) && isCooldown4 == false)
        {
            if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() >= 20)
            {
                isCooldown4 = true;
                UIManager.GetInstance().UpdateCooldownStuff(cooldown4, skillType.SKILL4);
                // Get the mouse position in world coordinates
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Summon the bladestorm
                // Instantiate the bladestorm prefab at the player's position
                Instantiate(bloodpoolPrefab, transform.position, Quaternion.identity);

                AudioManager.instance.PlaySFX("WarriorAOE");
                if (canUseskill4 == true)
                {

                }
                else
                {
                    // Consume mana
                    ChangeMana(-20);
                }


            }
            else
            {
                // Not enough mana or ninja star already thrown, implement feedback or other logic here
            }
        }
    }



    public override void UpdateHP()
    {

    }
    public override void UpdateMana()
    {

    }

    void DisableCooldown(skillType whatSkill)
    {
        switch (whatSkill)
        {
            case skillType.SKILL1:
                isCooldown1 = false;
                break;
            case skillType.SKILL2:
                isCooldown2 = false;
                break;
            case skillType.SKILL3:
                isCooldown3 = false;
                break;
            case skillType.SKILL4:
                isCooldown4 = false;
                break;
        }
    }
}

