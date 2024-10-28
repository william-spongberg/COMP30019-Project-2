using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    // Start is called before the first frame update
    public Counter tracker;
    [SerializeField]
    private float health;
    [SerializeField]
    private ParticleSystem bloodEffect;
    public EnemyAI enemyAI;
    public Ragdoll ragdoll;
    private bool isDead = false;

    // List to track active blood effects
    private List<ParticleSystem> activeBloodEffects = new List<ParticleSystem>();

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>(); // Finds BulletManager in the scene
        ragdoll = GetComponent<Ragdoll>();
    }

    public void TakeDamage(float damage) {
        
        // No more damage after death
        if (isDead) return;

        health -= damage;

        if(health <= 0){
            tracker.IncreaseSlain(1);         
            Die();
        }
    }
    
    public IEnumerator PlayBloodEffect(Vector3 hitPoint, Vector3 hitDirection) {

        if (isDead) yield break;

        // create new instance of blood effect
        ParticleSystem blood = Instantiate(bloodEffect, transform);
        blood.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
        blood.transform.rotation = Quaternion.LookRotation(hitDirection);
        blood.Play();
        
        // Add to list of active blood effects
        activeBloodEffects.Add(blood);

        // wait for duration of the particle system, then delete
        float duration = blood.main.duration;
        yield return new WaitForSeconds(duration);
        activeBloodEffects.Remove(blood);
        Destroy(blood.gameObject);
    }

    public void setTracker(Counter tracking){
        tracker = tracking;
    }

    private void Die()
    {
        if (isDead) return; // Die() is only called once

        isDead = true;

        // Stop and destroy all active blood effects
        foreach (ParticleSystem blood in activeBloodEffects)
        {
            if (blood != null)
            {
                blood.Stop();
                Destroy(blood.gameObject);
            }
        }

        // Clear the list after all effects are destroyed
        activeBloodEffects.Clear();

        if (enemyAI != null)
        {   
            enemyAI.DestroyCurrentBullet(); // Destroy any existing bullet first
            enemyAI.StopAllCoroutines();    // Stop any ongoing coroutines
            enemyAI.enabled = false;      

        }
        
        // Toggle ragdoll on
        if (ragdoll != null) ragdoll.ToggleRagdoll(true);
        
    }
}
