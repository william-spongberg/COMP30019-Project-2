using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{

    private int maxHealth = 100;
    private int currentHealth;

    [SerializeField]
    // Reference to the health system
    private HealthSystem healthSystem;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSystem.SetMaxHealth(maxHealth);
        
    }

    public void TakeDamage(int damage)
    {

        // Decrease health and update the health system
        currentHealth -= damage;

        // Ensure health doesn't drop below zero
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update health
        healthSystem.SetHealth(currentHealth);


    }
}
