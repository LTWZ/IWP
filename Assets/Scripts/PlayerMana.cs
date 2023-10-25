using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMana : MonoBehaviour
{
    private Slider Mana_slider;
    private GameObject Mana_slider_GO;
    private TextMeshProUGUI Mana_valuetext;

    public int maxMana = 100;
    public int currMana;

    // Start is called before the first frame update
    void Start()
    {
        currMana = maxMana;

        Mana_slider_GO = GameObject.FindGameObjectWithTag("ManaBar");
        Mana_slider = Mana_slider_GO.GetComponentInChildren<Slider>();
        Mana_valuetext = Mana_slider.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        Mana_valuetext.text = currMana.ToString() + "/" + maxMana.ToString();

        Mana_slider.value = currMana;
        Mana_slider.maxValue = maxMana;

        // testing

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    currMana -= 20;
        //}
        //else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    currMana += 20;
        //}
    }
}
