using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField]
    private float destroyDelay;
    [SerializeField]
    private int damageAmount;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit is an enemy
        EnemyHP enemy = collision.gameObject.GetComponent<EnemyHP>();
        BossHP boss = collision.gameObject.GetComponent<BossHP>();

        // get hit point + direction
        Vector3 hitPoint = collision.contacts[0].point;
        Vector3 hitDirection = collision.contacts[0].normal;

        if (enemy != null)
        {
            // Deal damage to the enemy
            enemy.TakeDamage(damageAmount);
            // Play blood effect
            StartCoroutine(enemy.PlayBloodEffect(hitPoint, hitDirection));
        }

        if (boss != null)
        {
            // Deal damage to the boss
            boss.TakeDamage(damageAmount);
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

