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
        Debug.Log(healthbarValueText.text);
    }
}

public enum skillType
{
    SKILL1,
    SKILL2,
    SKILL3,
    SKILL4,
}