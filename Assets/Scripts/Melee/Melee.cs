using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField]
    private float attackRange = 2.0f; 
    [SerializeField]
    private float attackDamage = 25f; 
    [SerializeField]
    private float attackCooldown = 1f;
    [SerializeField]
    private LayerMask enemyLayer; // Define which layers can be hit
    

    // Indicator variables
    private bool canAttack = true;

    // References
    [SerializeField]
    private Transform attackPoint; // Point where the melee attack originates

    public void HandleInput()
    {
        // Check for attack input (e.g., left-click)
        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            MeleeAttack();
            Debug.Log("Melee Attack");
        }
    }

    private void MeleeAttack()
    {
        canAttack = false; // Prevent further attacks until cooldown is over

        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        // Damage all enemies hit
        foreach (Collider enemy in hitEnemies)
        {
            // Assume enemy has a component called "EnemyHealth" that handles taking damage
            EnemyHP enemyHealth = enemy.GetComponent<EnemyHP>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }

        // Trigger cooldown before another attack can be performed
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    // Visualize the melee attack range in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Method to indicate if the weapon is busy (for switching logic)
    public bool IsBusy()
    {
        return !canAttack;
    }

    public void ResetState()
    {
        // Reset the attack state
        canAttack = true;

        // Cancel any scheduled invokes (e.g., cooldowns)
        CancelInvoke();
    }
}
