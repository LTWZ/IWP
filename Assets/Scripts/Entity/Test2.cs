using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : PlayerEntity
{
    [Header("Ability 1")]
    public float cooldown1 = 5;
    bool isCooldown1 = false;
    public KeyCode ability1;
    public float teleportRange = 10f;

    [Header("Ability 2")]
    public float cooldown2 = 5;
    bool isCooldown2 = false;
    public KeyCode ability2;

    [Header("Ability 3")]
    public float cooldown3 = 5;
    bool isCooldown3 = false;
    public KeyCode ability3;

    [Header("Ability 4")]
    public float cooldown4 = 5;
    bool isCooldown4 = false;
    public KeyCode ability4;

    private void Start()
    {
        GetUIManager();
        UIManager.GetInstance().onCooldown += DisableCooldown;
    }

    public int GetCurrMana()
    {
        return currMana;
    }

    public void Update()
    {
        UpdateHP();
        UpdateMana();
        if (DialogueManager.isActive == false)
        {
            Skill1();
            Skill2();
            Skill3();
            Skill4();

        }
        else if (DialogueManager.isActive == true)
        {

        }
    }

    public override void Skill1()
    {
        TeleportToMousePosition();
    }

    void TeleportToMousePosition()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = 0.5f; // Adjust the height as needed

        // Calculate the direction and distance to the mouse position
        Vector3 teleportDirection = mousePosition - transform.position;
        float teleportDistance = Mathf.Min(teleportRange, teleportDirection.magnitude);

        // Teleport the character to the mouse position within the teleport range
        transform.position = transform.position + teleportDirection.normalized * teleportDistance;
    }

    public override void Skill2()
    {

    }

    public override void Skill3()
    {

    }

    public override void Skill4()
    {

    }

    public override void UpdateHP()
    {

    }
    public override void UpdateMana()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeMana(-20);
        }
        else if (Input.GetKeyDown(KeyCode.L))
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
