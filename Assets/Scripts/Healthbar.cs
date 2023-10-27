using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Healthbar : MonoBehaviour
{
    [Header("HP Code")]
    [SerializeField] Slider HB_slider;
    [SerializeField] TextMeshProUGUI HB_valuetext;


    // Update is called once per frame
    void Update()
    {
        HB_valuetext.text = HB_slider.value.ToString() + "/" + HB_slider.maxValue.ToString();
    }

    public void UpdateContent(float val)
    {
        HB_slider.value = val;
    }

    public void SetMinandMax(float min, float max)
    {
        HB_slider.minValue = min;
        HB_slider.maxValue = max;
    }
}
