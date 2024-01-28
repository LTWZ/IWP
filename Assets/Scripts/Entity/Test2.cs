using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test2 : PlayerEntity
{
    [Header("Ability 1")]
    public float cooldown1 = 5;
    bool isCooldown1 = false;
    public KeyCode ability1;
    public float teleportRange = 5f;
    public float teleportDistanceMultiplier = 5f;
    int mapLayer = 99;
    // Change this for debug mode purposes, original value is 6

    [Header("Ability 2")]
    public float cooldown2 = 5;
    bool isCooldown2 = false;
    public KeyCode ability2;
    public GameObject serratedBladePrefab;
    public float bladeSpeed = 10f;
    public int numBlades = 3;
    private bool isbladescast = false;
    public float knifeslifetime;

    [Header("Ability 3")]
    public float cooldown3 = 5;
    bool isCooldown3 = false;
    public KeyCode ability3;
    public GameObject ninjaStarPrefab;
    public float ninjastarSpeed = 10f;
    private bool isNinjaStarThrown = false;
    private float ninjaStarFlightTime = 0.5f;


    [Header("Ability 4")]
    public float cooldown4 = 5;
    public bool isCooldown4 = false;
    public KeyCode ability4;
    public float bladeStormDuration = 5f;
    private bool isBladeStormActive = false;
    private float bladeStormTimer = 0f;
    public GameObject bladestormPrefab; // Assign your bladestorm prefab in the Unity editor

    private void Start()
    {
        GetUIManager();
        UIManager.GetInstance().onCooldown += DisableCooldown;
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

        if (Input.GetKeyDown(KeyCode.Alpha0))
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
        TeleportToMousePosition();
    }

    void TeleportToMousePosition()
    {
        // Get the mouse position in world coordinates
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction and distance to the mouse position
        Vector2 teleportDirection = (mousePos - (Vector2)transform.position);
        float teleportDistance = teleportDirection.magnitude;
        teleportDistance = Mathf.Clamp(teleportDistance, 0f, 5f);

        if (Input.GetKeyDown(ability1) && isCooldown1 == false)
        {
            if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() >= 5)
            {
                // Perform a raycast to check if the teleport position is valid
                RaycastHit2D hit = Physics2D.Raycast(transform.position, teleportDirection.normalized, teleportDistance, 1 << mapLayer);

                if (hit.collider == null)
                {
                    // Teleport the character to the mouse position within the teleport range
                    transform.position = (Vector3)((Vector2)transform.position + teleportDirection.normalized * teleportDistance);
                    AudioManager.instance.PlaySFX("Roguedash");
                    isCooldown1 = true;
                    UIManager.GetInstance().UpdateCooldownStuff(cooldown1, skillType.SKILL1);
                    if (canUseskill1 == true)
                    {

                    }
                    else
                    {
                        ChangeMana(-5);
                    }
                }
                else
                {
                    // The teleport position is blocked by an obstacle
                    Debug.Log("Cannot teleport to the selected position - obstacle in the way!");
                    // You may want to add feedback to the player here
                }
            }
            else if (currMana <= 5)
            {
                // Handle not enough mana case
            }
        }
    }

    public override void Skill2()
    {
        if (Input.GetKeyDown(ability2) && isCooldown2 == false)
        {
            if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() >= 10 && isbladescast == false)
            {
                if (canUseskill2 == true)
                {

                }
                else
                {
                    // Consume mana
                    ChangeMana(-10);
                }

                // Get the mouse position in world coordinates
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                AudioManager.instance.PlaySFX("Rogueknife");
                // Summon and shoot out blades
                StartCoroutine(ShootSerratedBlades(mousePos));
            }
            else
            {
                // Not enough mana, implement feedback or other logic here
            }
        }

    }

    IEnumerator ShootSerratedBlades(Vector2 targetPosition)
    {
        isbladescast = true;
        for (int i = 0; i < numBlades; i++)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Instantiate the serrated blade at the player's position
            GameObject blade = Instantiate(serratedBladePrefab, transform.position, Quaternion.identity);

            // Calculate the direction to the target position
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

            // Set the blade's velocity
            blade.GetComponent<Rigidbody2D>().velocity = direction * bladeSpeed;

            // Calculate the angle to face the target position
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the dagger's rotation to face forward
            blade.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            // Wait for a short delay before shooting the next blade
            yield return new WaitForSeconds(0.3f);

            Destroy(blade, knifeslifetime);
        }

        isCooldown2 = true;
        UIManager.GetInstance().UpdateCooldownStuff(cooldown2, skillType.SKILL2);
        isbladescast = false;


    }

    public override void Skill3()
    {
        if (Input.GetKeyDown(ability3) && isCooldown3 == false)
        {
            if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() >= 15 && !isNinjaStarThrown)
            {

                if (canUseskill3 == true)
                {

                }
                else
                {
                    // Consume mana
                    ChangeMana(-15);
                }

                // Get the mouse position in world coordinates
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                AudioManager.instance.PlaySFX("Roguestar");

                // Throw the ninja star
                StartCoroutine(ThrowNinjaStar(mousePos));
            }
            else
            {
                // Not enough mana or ninja star already thrown, implement feedback or other logic here
            }
        }
    }

    IEnumerator ThrowNinjaStar(Vector2 targetPosition)
    {
        isNinjaStarThrown = true;

        // Get the current player position as the reference point
        Vector2 playerPosition = transform.position;

        // Instantiate the ninja star at the player's position
        GameObject ninjaStar = Instantiate(ninjaStarPrefab, playerPosition, Quaternion.identity);

        // Calculate the initial direction to the target position
        Vector2 direction = (targetPosition - playerPosition).normalized;

        // Set the ninja star's velocity
        ninjaStar.GetComponent<Rigidbody2D>().velocity = direction * ninjastarSpeed;

        // Calculate the angle to face the target position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the dagger's rotation to face forward
        ninjaStar.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // Wait for the ninja star to reach the target position
        yield return new WaitForSeconds(ninjaStarFlightTime);
        
        while (true)
        {
            playerPosition = transform.position;
            Vector2 returnDirection = (playerPosition - (Vector2)ninjaStar.transform.position);
            ninjaStar.GetComponent<Rigidbody2D>().velocity = returnDirection.normalized * ninjastarSpeed;

            if (returnDirection.magnitude <= 0.1f)
                break;

            yield return null;
        }

        // Cleanup and cooldown
        Destroy(ninjaStar);
        isCooldown3 = true;
        UIManager.GetInstance().UpdateCooldownStuff(cooldown3, skillType.SKILL3);
        isNinjaStarThrown = false;
    }

    public override void Skill4()
    {
        if (Input.GetKeyDown(ability4) && isCooldown4 == false)
        {
            if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() >= 20)
            {
                isCooldown4 = true;
                AudioManager.instance.PlaySFX("Rogueult");
                UIManager.GetInstance().UpdateCooldownStuff(cooldown4, skillType.SKILL4);
                // Get the mouse position in world coordinates
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Summon the bladestorm
                // Instantiate the bladestorm prefab at the player's position
                Instantiate(bladestormPrefab, transform.position, Quaternion.identity);
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeHealth(20);
        }
    }
    public override void UpdateMana()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeMana(20);
        }
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
