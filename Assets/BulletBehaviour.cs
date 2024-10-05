using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
   
   [SerializeField]
   private float destroyDelay;

    private void OnCollisionEnter(Collision collision)
    {
        
        
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

