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

    public void TakeDamage(float damage) {
        health -= damage;
        if(health <= 0){
            tracker.IncreaseSlain(1);
            Destroy(gameObject);
        }
    }
    
    public IEnumerator PlayBloodEffect(Vector3 hitPoint, Vector3 hitDirection) {
        // create new instance of blood effect
        ParticleSystem blood = Instantiate(bloodEffect, transform);
        blood.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
        blood.transform.rotation = Quaternion.LookRotation(hitDirection);
        blood.Play();
    
        // wait for duration of the particle system, then delete
        float duration = blood.main.duration;
        yield return new WaitForSeconds(duration);
        Destroy(blood.gameObject);
    }

    public void setTracker(Counter tracking){
        tracker = tracking;
    }
}
