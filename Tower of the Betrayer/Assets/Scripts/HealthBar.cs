// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages the health bar UI element and updates to reflect player's health status.
public class HealthBar : MonoBehaviour
{
    
    public Slider slider;
    
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
    }
    
    public void AddHealth(float health)
    {
        slider.value += health;
    }
    
    public void RemoveHealth(float health)
    {
        slider.value -= health;
    }
    
    public void SetColor(Color color)
    {
        slider.fillRect.GetComponent<Image>().color = color;
    }
}
