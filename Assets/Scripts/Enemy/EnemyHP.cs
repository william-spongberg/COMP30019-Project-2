using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float health;

    public void TakeDamage(float damage) {
        health -= damage;
        if(health <= 0){
            Destroy(gameObject);
        }
    }
}
