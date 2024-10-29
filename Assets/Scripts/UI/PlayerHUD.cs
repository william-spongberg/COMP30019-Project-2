using System.Collections;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth;

    [SerializeField]
    private bool isDead = false;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Canvas mainUI;

    [SerializeField]
    private Canvas hudUI;

    [SerializeField]
    private Canvas dialogueUI;

    [SerializeField]
    private GameObject gameOver;

    [SerializeField]
    private HealthSystem healthSystem;

    [SerializeField]
    private float regenerationDelay = 5f;
    [SerializeField]
    private int regenerationAmount = 10;
    [SerializeField]
    private float regenerationInterval = 6f;
    [SerializeField]
    private AudioSource damageSound;

    private Coroutine regenerationCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        healthSystem.SetMaxHealth(maxHealth);
    }

    //testing
    void Update()
    {
        if (isDead)
        {
            CameraShake();
        }
    }

    public void TakeDamage(int damage)
    {
        // dont take damage if dead
        if (isDead) return;

        // Decrease health and update the health system
        currentHealth -= damage;

        // Ensure health doesn't drop below zero
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Play damage sound
        damageSound.Play();

        // Update health
        healthSystem.SetHealth(currentHealth);

        // Stop any ongoing regeneration and restart the delay
        if (regenerationCoroutine != null)
        {
            StopCoroutine(regenerationCoroutine);
        }

        if (currentHealth <= 0)
        {
            // dead player
            KillPlayer();
        }
        else
        {
            // Start a new delay before health regeneration starts
            regenerationCoroutine = StartCoroutine(StartHealthRegeneration());
        }
    }

    private void KillPlayer()
    {
        isDead = true;

        // pause game
        Time.timeScale = 0;

        // only disable once since disabling the whole parent
        mainUI.enabled = false;
        hudUI.enabled = false;
        dialogueUI.enabled = false;

        // draw game over on screen
        gameOver.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CameraShake()
    {
        // random shake
        player.transform.rotation = Quaternion.Euler(Random.Range(-0.1f, 0.1f),
                                                     Random.Range(-0.1f, 0.1f),
                                                     Random.Range(-0.1f, 0.1f));
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
