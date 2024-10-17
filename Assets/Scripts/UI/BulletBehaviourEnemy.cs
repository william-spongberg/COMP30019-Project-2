using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviourEnemy : MonoBehaviour
{
    [SerializeField]
   private float destroyDelay;
   [SerializeField]
    private int damageAmount;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit is an enemy
        PlayerHUD player = collision.gameObject.GetComponent<PlayerHUD>();

        if (player != null)
        {
            // Deal damage to the enemy
            player.TakeDamage(damageAmount);
        }
        
       // Use a coroutine to destroy the bullet after a delay
        StartCoroutine(DestroySequence());
    }

    private IEnumerator DestroySequence()
    {
        // Optional: Play sound, animations, or particle effects

        // Wait for the set delay
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the bullet
        Destroy(gameObject);
    }
}
