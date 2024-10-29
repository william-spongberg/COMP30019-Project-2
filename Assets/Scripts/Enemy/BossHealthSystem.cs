using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthSystem : MonoBehaviour
{
    public Slider healthSlider; // Reference to the slider UI element
    private int maxHealth;

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; // Initialize the slider to full health
    }

    public void SetHealth(int health)
    {
        // Update the slider value to reflect the current health
        healthSlider.value = health;
    }
} 
