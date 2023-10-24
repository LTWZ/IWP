using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test1 : PlayerEntity
{
    [Header("Ability 1")]
    public Image Skill_1_Image;
    public float cooldown1 = 5;
    bool isCooldown1 = false;
    public KeyCode ability1;

    [Header("Ability 2")]
    public Image Skill_2_Image;
    public float cooldown2 = 5;
    bool isCooldown2 = false;
    public KeyCode ability2;

    [Header("Ability 3")]
    public Image Skill_3_Image;
    public float cooldown3 = 5;
    bool isCooldown3 = false;
    public KeyCode ability3;

    [Header("Ability 4")]
    public Image Skill_4_Image;
    public float cooldown4 = 5;
    bool isCooldown4 = false;
    public KeyCode ability4;

    private void Start()
    {
        Skill_1_Image.fillAmount = 0;
        Skill_2_Image.fillAmount = 0;
        Skill_3_Image.fillAmount = 0;
        Skill_4_Image.fillAmount = 0;
        UIManager.GetInstance().onCooldown += DisableCooldown;
    }

    public void Update()
    {
        Skill1();
        Skill2();
        Skill3();
        Skill4();
    }

    public override void Skill1()
    {
        if (Input.GetKeyDown(ability1) && isCooldown1 == false)
        {
            isCooldown1 = true;
            UIManager.GetInstance().UpdateCooldownStuff(cooldown1, skillType.SKILL1);
        }
    }

    public override void Skill2()
    {
        if (Input.GetKeyDown(ability2) && isCooldown2 == false)
        {
            isCooldown2 = true;
            UIManager.GetInstance().UpdateCooldownStuff(cooldown2, skillType.SKILL2);
        }
    }

    public override void Skill3()
    {
        if (Input.GetKeyDown(ability3) && isCooldown3 == false)
        {
            isCooldown3 = true;
            UIManager.GetInstance().UpdateCooldownStuff(cooldown3, skillType.SKILL3);
        }
    }

    public override void Skill4()
    {
        if (Input.GetKeyDown(ability4) && isCooldown4 == false)
        {
            isCooldown4 = true;
            UIManager.GetInstance().UpdateCooldownStuff(cooldown4, skillType.SKILL4);
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
