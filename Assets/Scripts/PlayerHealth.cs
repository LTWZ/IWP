using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private Slider HB_slider;
    private GameObject HB_slider_GO;
    private TextMeshProUGUI HB_valuetext;

    private int maxHealth = 100;
    private int currHealth;

    

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;

        HB_slider_GO = GameObject.FindGameObjectWithTag("HealthBar");
        HB_slider = HB_slider_GO.GetComponentInChildren<Slider>();
        HB_valuetext = HB_slider.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        HB_valuetext.text = currHealth.ToString() + "/" + maxHealth.ToString();

        HB_slider.value = currHealth;
        HB_slider.maxValue = maxHealth;

        //// testing
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    currHealth -= 20;
        //}
        //else if(Input.GetKeyDown(KeyCode.L))
        //{
        //    currHealth += 20;
        //}

    }
}
