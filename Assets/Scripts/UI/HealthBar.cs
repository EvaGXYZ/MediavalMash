using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // create slider
    public Slider slider;
    // create gradient
    public Gradient gradient;
    // create fill
    public Image fill;

    // set max health
    public void SetMaxHealth(int health)
    {
        // setup health for slider
        slider.maxValue = health;
        slider.value = health;

        // pick color in gradient
        fill.color = gradient.Evaluate(1f);
    }

    // set current health
    public void SetHealth(int health)
    {
        // update health for slider
        slider.value = health;
        // update fill color with silder value(0-1)
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //fill.fillAmount = health / 5.0f;
    }
}
