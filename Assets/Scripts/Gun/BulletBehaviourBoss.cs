using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviourBoss : MonoBehaviour
{
    // References
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosion;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;

    // Bullet stats
    [SerializeField] private float bounciness = 0.1f;
    [SerializeField] private bool useGravity = true;

    [SerializeField] private float explosionDamage = 40f;
    [SerializeField] private float explosionRange = 15f;
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float explosionDelay = 5f;

    [SerializeField] private float destroyDelay = 0.1f;


    private PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        // Create a new Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        // Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        // Set gravity
        rb.useGravity = useGravity;

        // Randomize destroy delay
        float delayOffset = Random.Range(-2f, 2f);
        explosionDelay += delayOffset;

        // Randomize explosion range
        float rangeOffset = Random.Range(-5f, 5f);
        explosionRange += rangeOffset;
    }

    private void Update()
    {
        
        // Count down lifetime
        explosionDelay -= Time.deltaTime;
        if (explosionDelay <= 0) Explode();
    }

    private void Explode()
    {
        // Instantiate explosion effect
        if (explosion != null)
        {
            // Scale the explosion based on the damage output and elapsed time
            float scaleMultiplier = Random.Range(4f, 8f);

            // Instantiate the explosion
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);

            // Set the scale of the explosion
            explosionInstance.transform.localScale *= scaleMultiplier;
        }

        // Check for the player
        Collider[] players = Physics.OverlapSphere(transform.position, explosionRange, playerLayer);

        // Only one player but collider is array
        foreach (Collider playerCollider in players)
        {
            PlayerHUD playerHUD = playerCollider.GetComponent<PlayerHUD>();
            Rigidbody playerRb = playerCollider.GetComponent<Rigidbody>();

            if (playerHUD != null && playerRb != null)
            {
                // Calculate distance
                float distance = Vector3.Distance(transform.position, playerCollider.transform.position);
                float distanceFactor = Mathf.Clamp01(1 - (distance / explosionRange));

                // Apply damage based on distance
                int finalDamage = Mathf.RoundToInt(explosionDamage * distanceFactor);
                playerHUD.TakeDamage(finalDamage);

                // Apply force based on distance
                float finalForce = explosionForce * distanceFactor;
                playerRb.AddExplosionForce(finalForce, transform.position, explosionRange);
            }
            
        }

        // Destroy bullet
        StartCoroutine(DestroySequence());
    }

    private void OnCollisionEnter(Collision collision)
    {
       Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
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
