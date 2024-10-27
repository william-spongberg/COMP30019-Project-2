using System.Collections;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth;

    [SerializeField]
    // Reference to the health system
    private HealthSystem healthSystem;

    [SerializeField]
    private float regenerationDelay = 5f;
    [SerializeField]
    private int regenerationAmount = 10;
    [SerializeField]
    private float regenerationInterval = 6f;

    private Coroutine regenerationCoroutine;

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

        // Stop any ongoing regeneration and restart the delay
        if (regenerationCoroutine != null)
        {
            StopCoroutine(regenerationCoroutine);
        }

        // Start a new delay before health regeneration starts
        regenerationCoroutine = StartCoroutine(StartHealthRegeneration());
    }

    private IEnumerator StartHealthRegeneration()
    {
        // Wait for the delay period before starting regeneration
        yield return new WaitForSeconds(regenerationDelay);

        // Regenerate health periodically
        while (currentHealth < maxHealth)
        {
            currentHealth += regenerationAmount;

            // Ensure health doesn't exceed max health
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            // Update health system
            healthSystem.SetHealth(currentHealth);

            // Wait for the next regeneration interval
            yield return new WaitForSeconds(regenerationInterval);
        }

        // Set the coroutine to null after finishing
        regenerationCoroutine = null;
    }
}
