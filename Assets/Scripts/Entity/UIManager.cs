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
        StartCoroutine(Test());
    }
    
    private IEnumerator Test()
    {
        yield return null;
        LoadSkillImages();
    }
    [SerializeField] Image skill1;
    [SerializeField] Image skill2;
    [SerializeField] Image skill3;
    [SerializeField] Image skill4;
    [SerializeField] Image skill1_alt;
    [SerializeField] Image skill2_alt;
    [SerializeField] Image skill3_alt;
    [SerializeField] Image skill4_alt;
    [SerializeField] Slider healthbarSlider;
    [SerializeField] TextMeshProUGUI healthbarValueText;
    [SerializeField] Slider manaSlider;
    [SerializeField] TextMeshProUGUI manaValueText;
    [SerializeField] Image border1;
    [SerializeField] Image border2;
    [SerializeField] Image border3;
    [SerializeField] Image border4;
    [SerializeField] TextMeshProUGUI coinValueText;
    [SerializeField] TextMeshProUGUI healthpotionNumText;
    [SerializeField] TextMeshProUGUI manapotionNumText;
    [SerializeField] Image CoinImage;
    [SerializeField] TextMeshProUGUI Cointext;
    [SerializeField] Image ManaPotion;
    [SerializeField] Image HealthPotion;
    [SerializeField] Image Skill1ManaImage;
    [SerializeField] TextMeshProUGUI Skill1ManaCost;
    [SerializeField] Image Skill2ManaImage;
    [SerializeField] TextMeshProUGUI Skill2ManaCost;
    [SerializeField] Image Skill3ManaImage;
    [SerializeField] TextMeshProUGUI Skill3ManaCost;
    [SerializeField] Image Skill4ManaImage;
    [SerializeField] TextMeshProUGUI Skill4ManaCost;


    public delegate void OnCooldown(skillType whichSkill);
    public OnCooldown onCooldown;

    public void LoadSkillImages()
    {
        PlayerManager playerManager = PlayerManager.GetInstance();
        if (playerManager != null)
        {
            PlayerEntity player = playerManager.GetCurrentPlayer().GetComponent<PlayerEntity>();
            if (player != null)
            {
                Debug.Log(player);
                Debug.Log(playerManager.GetCurrentPlayer());
                Debug.Log(playerManager);

                skill1.sprite = player.GetAbilitiesSet().ability1.abilitySprite;
                skill2.sprite = player.GetAbilitiesSet().ability2.abilitySprite;
                skill3.sprite = player.GetAbilitiesSet().ability3.abilitySprite;
                skill4.sprite = player.GetAbilitiesSet().ability4.abilitySprite;
                skill1_alt.sprite = player.GetAbilitiesSet().ability1.abilitySprite;
                skill2_alt.sprite = player.GetAbilitiesSet().ability2.abilitySprite;
                skill3_alt.sprite = player.GetAbilitiesSet().ability3.abilitySprite;
                skill4_alt.sprite = player.GetAbilitiesSet().ability4.abilitySprite;
            }

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

    public void UpdateCoinsDisplay(int currCoins)
    {
        coinValueText.text = currCoins.ToString();
    }

    public void UpdateHPPotionDisplay(int currhealthpotionNum)
    {
        healthpotionNumText.text = currhealthpotionNum.ToString();
    }

    public void UpdateManaPotionDisplay(int currmanapotionNum)
    {
        manapotionNumText.text = currmanapotionNum.ToString();
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

    public void ToggleImage(int borderIndex, bool activate)
    {
        Image selectedBorder = GetImageByIndex(borderIndex);

        if (selectedBorder != null)
        {
            selectedBorder.gameObject.SetActive(activate);
        }
        else
        {
            //Debug.LogWarning("Invalid border index: " + borderIndex);
        }
    }

    private Image GetImageByIndex(int index)
    {
        switch (index)
        {
            case 1:
                return border1;
            case 2:
                return border2;
            case 3:
                return border3;
            case 4:
                return border4;
            default:
                return null;
        }
    }

    public void HideAllUIElements()
    {
        HideUIElement(skill1);
        HideUIElement(skill2);
        HideUIElement(skill3);
        HideUIElement(skill4);
        HideUIElement(skill1_alt);
        HideUIElement(skill2_alt);
        HideUIElement(skill3_alt);
        HideUIElement(skill4_alt);

        HideUIElement(healthbarSlider);
        HideUIElement(healthbarValueText);

        HideUIElement(manaSlider);
        HideUIElement(manaValueText);

        HideUIElement(coinValueText);

        HideUIElement(healthpotionNumText);
        HideUIElement(manapotionNumText);

        HideUIElement(border1);
        HideUIElement(border2);
        HideUIElement(border3);
        HideUIElement(border4);
        HideUIElement(CoinImage);
        HideUIElement(Cointext);
        HideUIElement(ManaPotion);
        HideUIElement(HealthPotion);
        HideUIElement(Skill1ManaImage);
        HideUIElement(Skill1ManaCost);
        HideUIElement(Skill2ManaImage);
        HideUIElement(Skill2ManaCost);
        HideUIElement(Skill3ManaImage);
        HideUIElement(Skill3ManaCost);
        HideUIElement(Skill4ManaImage);
        HideUIElement(Skill4ManaCost);
    }

    private void HideUIElement(Component uiElement)
    {
        if (uiElement != null)
        {
            uiElement.gameObject.SetActive(false);
        }
    }

    public void ShowAllUIElements()
    {
        ShowUIElement(skill1);
        ShowUIElement(skill2);
        ShowUIElement(skill3);
        ShowUIElement(skill4);
        ShowUIElement(skill1_alt);
        ShowUIElement(skill2_alt);
        ShowUIElement(skill3_alt);
        ShowUIElement(skill4_alt);

        ShowUIElement(healthbarSlider);
        ShowUIElement(healthbarValueText);

        ShowUIElement(manaSlider);
        ShowUIElement(manaValueText);

        ShowUIElement(coinValueText);

        ShowUIElement(healthpotionNumText);
        ShowUIElement(manapotionNumText);

        ShowUIElement(border1);
        ShowUIElement(border2);
        ShowUIElement(border3);
        ShowUIElement(border4);
        ShowUIElement(CoinImage);
        ShowUIElement(Cointext);
        ShowUIElement(ManaPotion);
        ShowUIElement(HealthPotion);
        ShowUIElement(Skill1ManaImage);
        ShowUIElement(Skill1ManaCost);
        ShowUIElement(Skill2ManaImage);
        ShowUIElement(Skill2ManaCost);
        ShowUIElement(Skill3ManaImage);
        ShowUIElement(Skill3ManaCost);
        ShowUIElement(Skill4ManaImage);
        ShowUIElement(Skill4ManaCost);
    }

    private void ShowUIElement(Component uiElement)
    {
        if (uiElement != null)
        {
            uiElement.gameObject.SetActive(true);
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