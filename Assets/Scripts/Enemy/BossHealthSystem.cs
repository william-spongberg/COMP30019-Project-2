using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
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
