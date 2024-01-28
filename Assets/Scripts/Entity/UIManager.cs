using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private PlayerType selectedPlayerType;
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
        selectedPlayerType = PlayerManager.GetInstance().GetPlayerType();
        if (selectedPlayerType == PlayerType.WIZARD)
        {
            HideUIElement(Skill3HPImage);
            HideUIElement(Skill3HPCost);
        }
        if (selectedPlayerType == PlayerType.ROGUE)
        {
            HideUIElement(Skill3HPImage);
            HideUIElement(Skill3HPCost);
        }
        if (selectedPlayerType == PlayerType.WARRIOR)
        {
            HideUIElement(Skill3ManaImage);
            HideUIElement(Skill3ManaCost);
        }

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
    [SerializeField] Image Skill3HPImage;
    [SerializeField] TextMeshProUGUI Skill3HPCost;
    [SerializeField] Image Skill4ManaImage;
    [SerializeField] TextMeshProUGUI Skill4ManaCost;
    [SerializeField] RawImage Minimap;
    [SerializeField] Image Skill1KeyImage;
    [SerializeField] TextMeshProUGUI Skill1KeyText;
    [SerializeField] Image Skill2KeyImage;
    [SerializeField] TextMeshProUGUI Skill2KeyText;
    [SerializeField] Image Skill3KeyImage;
    [SerializeField] TextMeshProUGUI Skill3KeyText;
    [SerializeField] Image Skill4KeyImage;
    [SerializeField] TextMeshProUGUI Skill4KeyText;
    [SerializeField] Image HealthPotKeyImage;
    [SerializeField] TextMeshProUGUI HealthPotKeyText;
    [SerializeField] Image ManaPotKeyImage;
    [SerializeField] TextMeshProUGUI ManaPotKeyText;
    [SerializeField] Image MapUIImage;
    [SerializeField] Image MapKeyImage;
    [SerializeField] TextMeshProUGUI MapKeyText;
    [SerializeField] Image PauseUIImage;
    [SerializeField] Image PauseKeyImage;
    [SerializeField] TextMeshProUGUI PauseKeyText;



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
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnClickToggleMinimap();
        }
        //Debug.Log(healthbarValueText.text);
    }

    public void OnClickToggleMinimap()
    {
        if (Minimap.gameObject.activeSelf)
        {
            // If the image is currently active, hide it
            HideImage();
        }
        else
        {
            // If the image is not active, show it
            ShowImage();
        }
    }

    private void ShowImage()
    {
        // Set the image to be active
        Minimap.gameObject.SetActive(true);

        // Additional logic if needed when showing the image
    }

    private void HideImage()
    {
        // Set the image to be inactive
        Minimap.gameObject.SetActive(false);

        // Additional logic if needed when hiding the image
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
        if (selectedPlayerType == PlayerType.WARRIOR)
        {
            HideUIElement(Skill3HPImage);
            HideUIElement(Skill3HPCost);
        }
        HideUIElement(Skill1KeyImage);
        HideUIElement(Skill1KeyText);
        HideUIElement(Skill2KeyImage);
        HideUIElement(Skill2KeyText);
        HideUIElement(Skill3KeyImage);
        HideUIElement(Skill3KeyText);
        HideUIElement(Skill4KeyImage);
        HideUIElement(Skill4KeyText);
        HideUIElement(HealthPotKeyImage);
        HideUIElement(HealthPotKeyText);
        HideUIElement(ManaPotKeyImage);
        HideUIElement(ManaPotKeyText);
        HideUIElement(MapUIImage);
        HideUIElement(MapKeyImage);
        HideUIElement(MapKeyText);
        HideUIElement(PauseUIImage);
        HideUIElement(PauseKeyImage);
        HideUIElement(PauseKeyText);
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
        if (selectedPlayerType == PlayerType.WARRIOR)
        {
            ShowUIElement(Skill3HPImage);
            ShowUIElement(Skill3HPCost);
        }
        ShowUIElement(Skill1KeyImage);
        ShowUIElement(Skill1KeyText);
        ShowUIElement(Skill2KeyImage);
        ShowUIElement(Skill2KeyText);
        ShowUIElement(Skill3KeyImage);
        ShowUIElement(Skill3KeyText);
        ShowUIElement(Skill4KeyImage);
        ShowUIElement(Skill4KeyText);
        ShowUIElement(HealthPotKeyImage);
        ShowUIElement(HealthPotKeyText);
        ShowUIElement(ManaPotKeyImage);
        ShowUIElement(ManaPotKeyText);
        ShowUIElement(MapUIImage);
        ShowUIElement(MapKeyImage);
        ShowUIElement(MapKeyText);
        ShowUIElement(PauseUIImage);
        ShowUIElement(PauseKeyImage);
        ShowUIElement(PauseKeyText);
    }

    private void ShowUIElement(Component uiElement)
    {
        if (uiElement != null)
        {
            uiElement.gameObject.SetActive(true);
        }
    }

    public void IsHealth()
    {
        ShowUIElement(Skill3HPImage);
        ShowUIElement(Skill3HPCost);
    }

    public void IsMana()
    {
        ShowUIElement(Skill3ManaImage);
        ShowUIElement(Skill3ManaCost);
    }

}



public enum skillType
{
    SKILL1,
    SKILL2,
    SKILL3,
    SKILL4,
}