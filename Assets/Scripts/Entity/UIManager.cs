using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Log("RAN");
        Debug.Log(healthbarValueText.text);
        skill1.fillAmount = 0;
        skill2.fillAmount = 0;
        skill3.fillAmount = 0;
        skill4.fillAmount = 0;
        LoadSkillImages();
    }

    [SerializeField] Image skill1;
    [SerializeField] Image skill2;
    [SerializeField] Image skill3;
    [SerializeField] Image skill4;
    [SerializeField] Slider healthbarSlider;
    [SerializeField] TextMeshProUGUI healthbarValueText;
    [SerializeField] Slider manaSlider;
    [SerializeField] TextMeshProUGUI manaValueText;

    public delegate void OnCooldown(skillType whichSkill);
    public OnCooldown onCooldown;

    public void LoadSkillImages()
    {
        PlayerManager playerManager = PlayerManager.GetInstance();
        if (playerManager != null)
        {

            PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
            Debug.Log(player);
            Debug.Log(playerManager.GetCurrentPlayer());
            Debug.Log(playerManager);

            skill1.sprite = player.GetAbilitiesSet().ability1.abilitySprite;
            skill2.sprite = player.GetAbilitiesSet().ability2.abilitySprite;
            skill3.sprite = player.GetAbilitiesSet().ability3.abilitySprite;
            skill4.sprite = player.GetAbilitiesSet().ability4.abilitySprite;
        }
    }
    public void UpdateCooldownStuff(float timer, skillType whichSkill)
    {
        Image imageSkill = null;
        switch (whichSkill)
        {
            case skillType.SKILL1:
                imageSkill = skill1;
                break;
            case skillType.SKILL2:
                imageSkill = skill2;
                break;
            case skillType.SKILL3:
                imageSkill = skill3;
                break;
            case skillType.SKILL4:
                imageSkill = skill4;
                break;
        }
        StartCoroutine(StartCooldown(timer, imageSkill, whichSkill));
    }

    IEnumerator StartCooldown(float cooldownTimer, Image whatSkillImage, skillType theskillType)
    {
        float maxTimer = cooldownTimer;
        float timer = maxTimer;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            whatSkillImage.fillAmount = Mathf.Lerp(0, 1, timer/maxTimer);
            yield return null;
        }
        onCooldown?.Invoke(theskillType);
    }

    public void UpdateHealthDisplay(int currHp, int maxHp)
    {
        healthbarSlider.value = (float)currHp / maxHp;
        healthbarValueText.text = currHp.ToString() + "/" + maxHp.ToString();
        Debug.Log(healthbarValueText.text);
    }

    public void UpdateManaDisplay(int currMana, int maxMana)
    {
        manaSlider.value = (float)currMana / maxMana;
        manaValueText.text = currMana.ToString() + "/" + maxMana.ToString();
    }

    private void Update()
    {

        //Debug.Log(healthbarValueText.text);
    }

    public void ResetSkillFillAmount(skillType whichSkill)
    {
        Image imageSkill = null;

        switch (whichSkill)
        {
            case skillType.SKILL1:
                imageSkill = skill1;
                break;
            case skillType.SKILL2:
                imageSkill = skill2;
                break;
            case skillType.SKILL3:
                imageSkill = skill3;
                break;
            case skillType.SKILL4:
                imageSkill = skill4;
                break;
        }

        if (imageSkill != null)
        {
            imageSkill.fillAmount = 1;
        }
    }

    public void AddBackSkillFillAmount(skillType whichSkill)
    {
        Image imageSkill = null;

        switch (whichSkill)
        {
            case skillType.SKILL1:
                imageSkill = skill1;
                break;
            case skillType.SKILL2:
                imageSkill = skill2;
                break;
            case skillType.SKILL3:
                imageSkill = skill3;
                break;
            case skillType.SKILL4:
                imageSkill = skill4;
                break;
        }

        if (imageSkill != null)
        {
            imageSkill.fillAmount = 0;
        }
    }
}

public enum skillType
{
    SKILL1,
    SKILL2,
    SKILL3,
    SKILL4,
}