using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Mana Code")]
    private Slider Mana_slider;
    private GameObject Mana_slider_GO;
    [SerializeField] TextMeshProUGUI Mana_valuetext;
    public int maxMana = 100;
    private int currMana;
    [SerializeField] Healthbar healthbar;

    private void Start()
    {
        Skill_1_Image.fillAmount = 0;
        Skill_2_Image.fillAmount = 0;
        Skill_3_Image.fillAmount = 0;
        Skill_4_Image.fillAmount = 0;
        UIManager.GetInstance().onCooldown += DisableCooldown;
        currMana = maxMana;
        Mana_slider_GO = GameObject.FindGameObjectWithTag("ManaBar");
        Mana_slider = Mana_slider_GO.GetComponentInChildren<Slider>();
    }

    public int GetCurrMana()
    {
        return currMana;
    }

    public void SetCurrMana(int currMana)
    {
        this.currMana = currMana;
    }

    public void Update()
    {
        Skill1();
        Skill2();
        Skill3();
        Skill4();
        UpdateHP();
        UpdateMana();
    }

    public override void Skill1()
    {
        if (Input.GetKeyDown(ability1) && isCooldown1 == false)
        {
            if (PlayerMovement.GetInstance().Player.currMana >= 5)
            {
                isCooldown1 = true;
                UIManager.GetInstance().UpdateCooldownStuff(cooldown1, skillType.SKILL1);
                currMana -= 5;
            }
            else if (PlayerMovement.GetInstance().Player.currMana <= 5)
            {

            }

        }
    }

    public override void Skill2()
    {
        if (Input.GetKeyDown(ability2) && isCooldown2 == false)
        {
            if (PlayerMovement.GetInstance().Player.currMana >= 10)
            {
                isCooldown2 = true;
                UIManager.GetInstance().UpdateCooldownStuff(cooldown2, skillType.SKILL2);
                currMana -= 10;
            }
            else if (PlayerMovement.GetInstance().Player.currMana <= 10)
            {

            }

        }
    }

    public override void Skill3()
    {
        if (Input.GetKeyDown(ability3) && isCooldown3 == false)
        {
            if (PlayerMovement.GetInstance().Player.currMana >= 15)
            {
                isCooldown3 = true;
                UIManager.GetInstance().UpdateCooldownStuff(cooldown3, skillType.SKILL3);
                currMana -= 15;
            }
            else if (PlayerMovement.GetInstance().Player.currMana <= 15)
            {

            }
        }
    }

    public override void Skill4()
    {
        if (Input.GetKeyDown(ability4) && isCooldown4 == false)
        {
            if (PlayerMovement.GetInstance().Player.currMana >= 20)
            {
                isCooldown4 = true;
                UIManager.GetInstance().UpdateCooldownStuff(cooldown4, skillType.SKILL4);
                currMana -= 20;
            }
            else if (PlayerMovement.GetInstance().Player.currMana <= 20)
            {

            }
        }
    }

    public override void UpdateHP()
    {
        if (healthbar)
        {
            healthbar.SetMinandMax(0, Hp);
            healthbar.UpdateContent(currentHP);
        }


    }
    public override void UpdateMana()
    {
        Mana_valuetext.text = currMana.ToString() + "/" + maxMana.ToString();

        Mana_slider.value = currMana;
        Mana_slider.maxValue = maxMana;

        // testing
        if (Input.GetKeyDown(KeyCode.K))
        {
            currMana -= 20;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            currMana += 20;
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
