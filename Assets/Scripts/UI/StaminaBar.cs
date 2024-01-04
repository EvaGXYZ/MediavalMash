using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    // create slider
    public Slider slider;
    // create fill
    public Image fill;

    // set max stamina
    public void SetMaxStamina(int stamina)
    {
        // setup stamina for slider
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    // set current stamina
    public void SetStamina(int stamina)
    {
        // update stamina for slider
        slider.value = stamina;
    }
}
