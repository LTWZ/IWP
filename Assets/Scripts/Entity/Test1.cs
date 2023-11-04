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
    public GameObject beamPrefab;

    [Header("Ability 2")]
    public Image Skill_2_Image;
    public float cooldown2 = 5;
    bool isCooldown2 = false;
    public KeyCode ability2;
    public GameObject aoePrefab;

    [Header("Ability 3")]
    public Image Skill_3_Image;
    public float cooldown3 = 5;
    bool isCooldown3 = false;
    public KeyCode ability3;
    public GameObject BHPrefab;

    [Header("Ability 4")]
    public Image Skill_4_Image;
    public float cooldown4 = 5;
    bool isCooldown4 = false;
    public KeyCode ability4;
    public GameObject beamPrefab2;
    public GameObject beamtemp;
    public float beamSpeed; // Adjust the speed of the beam
    public float beamLifetime;
    public float beamoktimer = 0;
    public bool beamok = false;// Adjust the maximum lifetime of the beam

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
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
                Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
                beam.GetComponent<Rigidbody2D>().velocity = direction * beamSpeed;

                // Destroy the beam after a certain amount of time
                Destroy(beam, beamLifetime);
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
                // Instantiate the black hole at the player's position
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject blackHole = Instantiate(BHPrefab, mousePos, Quaternion.identity);

                // Add a script to control the black hole's behavior
                BlackHoleScript blackHoleScript = blackHole.GetComponent<BlackHoleScript>();
                if (blackHoleScript != null)
                {
                    // Customize the black hole's behavior (e.g., damage, pull force)
                    blackHoleScript.SetDamageOverTime(1); // Adjust the damage as needed
                    blackHoleScript.SetPullForce(10); // Adjust the pull force as needed
                }
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
            if (Input.GetKeyDown(ability3) && isCooldown3 == false)
            {
                if (PlayerMovement.GetInstance().Player.currMana >= 15)
                {
                    // Get the mouse position in world coordinates
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    // Instantiate the AoE Prefab at the mouse position
                    GameObject aoeObject = Instantiate(aoePrefab, mousePos, Quaternion.identity);

                    // Set any additional properties (e.g., damage, explosion time) here
                    AreaOfEffect aoeScript = aoeObject.GetComponent<AreaOfEffect>();
                    aoeScript.damage = 15;
                    aoeScript.explosionTime = 2.0f;

                    // Add a debuff to enemies within the AoE
                    aoeScript.ApplyDebuff(0.5f);

                    isCooldown3 = true;
                    UIManager.GetInstance().UpdateCooldownStuff(cooldown3, skillType.SKILL3);
                    currMana -= 15;
                }
                else if (PlayerMovement.GetInstance().Player.currMana <= 15)
                {
                    // Not enough mana, handle accordingly
                }
            }
        }
    }

    public override void Skill4()
    {
        if (Input.GetKeyDown(ability4) && isCooldown4 == false)
        {

            if (PlayerMovement.GetInstance().Player.currMana >= 20)
            {

                //where the beam spawns
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 directionVector = mousePos - (Vector2)gameObject.transform.position;
                Vector2 posToSpawnIn = (mousePos + (Vector2)gameObject.transform.position) / 2;

                GameObject tempbeam = Instantiate(beamtemp, Vector3.zero, Quaternion.identity);
                float widthOfBeam = tempbeam.GetComponentInChildren<SpriteRenderer>().sprite.rect.width;
                float lengthofBeam = directionVector.magnitude * 100;
                float offSetScale = lengthofBeam / widthOfBeam;
                //pls dont change the original scale of the beam object
                tempbeam.transform.localScale = new Vector2(offSetScale, tempbeam.transform.localScale.y);
                Vector2 dir = directionVector.normalized;
                // Calculate the angle in degrees from the Vector2
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                tempbeam.transform.eulerAngles = new Vector3(0, 0, angle);
                tempbeam.transform.position = posToSpawnIn;
                isCooldown4 = true;
                beamok = true;
                UIManager.GetInstance().UpdateCooldownStuff(cooldown4, skillType.SKILL4);
                currMana -= 20;
                //if (beamok == true)
                //{
                //    GameObject beam = Instantiate(beamPrefab2, Vector3.zero, Quaternion.identity);
                //    float widthOfBeam = beam.GetComponentInChildren<SpriteRenderer>().sprite.rect.width;
                //    float lengthofBeam = directionVector.magnitude * 100;
                //    float offSetScale = lengthofBeam / widthOfBeam;
                //    //pls dont change the original scale of the beam object
                //    beam.transform.localScale = new Vector2(offSetScale, beam.transform.localScale.y);
                //    Vector2 dir = directionVector.normalized;
                //    // Calculate the angle in degrees from the Vector2
                //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //    beam.transform.eulerAngles = new Vector3(0, 0, angle);
                //    beam.transform.position = posToSpawnIn;
                //    beamok = false;
                //}


            }
            else if (PlayerMovement.GetInstance().Player.currMana <= 20)
            {

            }
        }
        //u can continue
        if (beamok == true)
        {
            beamoktimer += Time.deltaTime;
            if(beamoktimer >= 1)
            {
                beamoktimer = 0;
                //Destroy(tempbeam);

                //where the beam spawns
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 directionVector = mousePos - (Vector2)gameObject.transform.position;
                Vector2 posToSpawnIn = (mousePos + (Vector2)gameObject.transform.position) / 2;

                GameObject beam = Instantiate(beamPrefab2, Vector3.zero, Quaternion.identity);
                float widthOfBeam = beam.GetComponentInChildren<SpriteRenderer>().sprite.rect.width;
                float lengthofBeam = directionVector.magnitude * 100;
                float offSetScale = lengthofBeam / widthOfBeam;
                //pls dont change the original scale of the beam object
                beam.transform.localScale = new Vector2(offSetScale, beam.transform.localScale.y);
                Vector2 dir = directionVector.normalized;
                // Calculate the angle in degrees from the Vector2
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                beam.transform.eulerAngles = new Vector3(0, 0, angle);
                beam.transform.position = posToSpawnIn;
                beamok = false;
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
