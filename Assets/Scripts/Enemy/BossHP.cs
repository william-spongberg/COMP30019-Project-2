using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private AudioSource damageSound;

    private int maxHealth = 200;
    private int currentHealth;

    private BossHealthSystem bossHealthSystem;

    public void InitializeHealthSystem(BossHealthSystem healthSystem)
    {
        bossHealthSystem = healthSystem;
        currentHealth = maxHealth;
        bossHealthSystem.SetMaxHealth(maxHealth);
        
        Debug.Log("Boss Health System initialised");

    }

    public void TakeDamage(int damage) {
        
        // Decrease health and update the health system
        currentHealth -= damage;

        // Ensure health doesn't drop below zero
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Play damage sound
        damageSound.Play();
        

        // Update health
        bossHealthSystem.SetHealth(currentHealth);

        if(currentHealth <= 0){
            Die();
        }

    }

    private void Die()
    {
        Destroy(gameObject);   
        // Play End screen;
    }
}
