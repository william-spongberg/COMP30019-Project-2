using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Melee : MonoBehaviour
{
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private LayerMask enemyLayer; // Define which layers can be hit
    

    // Indicator variables
    private bool canAttack = true;

    // References
    [SerializeField]
    private Transform attackPoint; // Point where the melee attack originates

    [SerializeField]
    private Transform meleeWeaponTransform;

        // Ammo UI
    [SerializeField]
    private TextMeshProUGUI ammoDisplay;

    // Sound reference
    [SerializeField]
    private MeleeAudio meleeAudio;
    // Animation settings
    private Vector3 stabRotation = new Vector3(60f, 0f, 0f);
    private Vector3 stabPosition = new Vector3(-0.5f, -0.5f, -0.5f);

    [SerializeField]
    private float stabSpeed = 0.2f;

    //[SerializeField]
    //private ObjectAudio objectAudio;

    public void HandleInput()
    {
        // Ensure the player cannot attack if the game is paused
        if (PauseMenu.IsPaused) return;
        
        // Check for attack input (e.g., left-click)
        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            StartCoroutine(MeleeAttackAnimation());
            MeleeAttack();
            Debug.Log("Melee Attack");
        }
    }

    private void MeleeAttack()
    {
        canAttack = false; // Prevent further attacks until cooldown is over

        // Perform a raycast from the attack point forward (direction the player is facing)
        RaycastHit hit;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, attackRange))
        {
            // Check if the object hit has the "EnemyHP" component to apply damage
            EnemyHP enemyHealth = hit.collider.GetComponent<EnemyHP>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("Hit enemy: " + hit.collider.name);
                meleeAudio.PlayHitSound();  // Play hit sound
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // Play object hit sound
                //objectAudio.PlayObjectHitSound();
                Debug.Log("Hit environment object: " + hit.collider.name);
            }
            else
            {
                meleeAudio.PlayMissSound(); // Play miss sound for anything else
            }
        }
        else
        {
            // Play miss sound if no hit at all
            meleeAudio.PlayMissSound();
        }

        // Trigger cooldown before another attack can be performed
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private IEnumerator MeleeAttackAnimation()
    {
        
        // Rotate to stab position
        Quaternion targetRotation = Quaternion.Euler(stabRotation);
        Vector3 targetPosition = stabPosition;

        // Move and rotate to stab position
        float elapsedTime = 0f;
        while (elapsedTime < stabSpeed)
        {
            meleeWeaponTransform.localRotation = Quaternion.Slerp(Quaternion.identity, targetRotation, elapsedTime / stabSpeed);
            meleeWeaponTransform.localPosition = Vector3.Lerp(Vector3.zero, targetPosition, elapsedTime / stabSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we reached the target position and rotation
        meleeWeaponTransform.localRotation = targetRotation;
        meleeWeaponTransform.localPosition = targetPosition;

        // Pause briefly at the end of the stab
        yield return new WaitForSeconds(0.1f);

        // Move and rotate back to original position
        elapsedTime = 0f;
        while (elapsedTime < stabSpeed)
        {
            meleeWeaponTransform.localRotation = Quaternion.Slerp(targetRotation, Quaternion.identity, elapsedTime / stabSpeed);
            meleeWeaponTransform.localPosition = Vector3.Lerp(targetPosition, Vector3.zero, elapsedTime / stabSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

         // Snap back to the exact original position and rotation to avoid drift
        meleeWeaponTransform.localRotation = Quaternion.identity;
        meleeWeaponTransform.localPosition = Vector3.zero;
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
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
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

    public void UpdateAmmoDisplay()
    {
        ammoDisplay.text = "";
    }
}
