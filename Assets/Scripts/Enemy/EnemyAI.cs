using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class EnemyAI : MonoBehaviour
{
    // State machine
    private enum EnemyState {Patrolling, Chasing, Attacking}
    public enum EnemyType { Shooting, Charging }

    [SerializeField]
    private EnemyType enemyType;
    private EnemyState currentState;

    // Components
    public NavMeshAgent agent;
    private Transform player;
    private Animator anim;

    // Layers
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    // Health
    public float healthPoints = 50f;

    // Patrolling
    private Vector3 patrolDestination;
    private bool isPatrolDestinationSet;
    public float patrolRadius = 10f;

    // Chasing
    private Vector3 chaseDestination; // Target point around the player for chasing
    private bool isChaseDestinationSet = false;

    // Attacking
    private bool isCharging = false;
    private int chargeDamage = 10;
     private bool isRetreatDestinationSet = false;
     private Vector3 retreatDestination;

    // Detection
    public float detectionRange = 15f;
    public float minAttackRange = 3f;  // Minimum attack range
    public float maxAttackRange = 7f;  // Maximum attack range
    private float attackRange;
    private bool playerInDetectionRange;
    private bool playerInAttackRange;
    

    // Shooting properties
    public float bulletForce = 20f;
    public float bulletRange = 50f;
    public float spread = 0.1f;

    // Ensure shoot attack cooldown lasts longer than the focus time of 3 Seconds
    public float shootAttackCooldown = 4f;
    public float chargeAttackCooldown = 4f;
    private bool isAttackOnCooldown;
    
    public GameObject bullet;
    private GameObject currentBullet;
    public Transform gunPoint;

    // Animation
    [SerializeField]
    private float shootingEnemySpeed = 1.5f;
    [SerializeField]
    private float chargingEnemySpeed = 3.5f;
    private float defaultSpeed = 0f;

    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Vector3 dest = Vector3.zero;

    // Sound

   [SerializeField]
   private EnemyFootstepsAudio enemyFootstepsAudio;
   private float runThreshold = 2.5f;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();

        // animation multiplier
        anim.SetFloat("MotionSpeed", 0.25f);

        // Randomly set attack range for this enemy
        attackRange = Random.Range(minAttackRange, maxAttackRange);

        // Add more randomness to enemies
        if (enemyType == EnemyType.Shooting)
        {
            float speedOffset = Random.Range(-0.5f, 0.5f);
            defaultSpeed = shootingEnemySpeed + speedOffset;

            float randomScale = Random.Range(1f, 1.8f);
            transform.localScale = new Vector3(randomScale, randomScale, randomScale);


        }
        else if (enemyType == EnemyType.Charging)
        {
            float speedOffset = Random.Range(-1f, 1f);
            defaultSpeed = chargingEnemySpeed + speedOffset;

            float randomScale = Random.Range(0.2f, 0.6f);
            transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }

        // Apply the default speed to the NavMeshAgent
        agent.speed = defaultSpeed;


    }



    private void Update()
    {
        UpdateState();
        ExecuteStateAction();
        HandleFootsteps();
    }

    private void UpdateState()
    {
        // Update detection status
        playerInDetectionRange = Physics.CheckSphere(transform.position, detectionRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        

        if (!playerInDetectionRange && !playerInAttackRange)
        {
            currentState = EnemyState.Patrolling;
            isChaseDestinationSet = false;
        }
        else if (playerInDetectionRange && !playerInAttackRange && !isCharging)
        {
            currentState = EnemyState.Chasing;
        }
        else if (playerInAttackRange || isCharging)
        {
            currentState = EnemyState.Attacking;
            isChaseDestinationSet = false;
        }
    }

    private void ExecuteStateAction()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                Debug.Log("Patrolling State");
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                Debug.Log("Chasing State");
                break;
            case EnemyState.Attacking:
                if (enemyType == EnemyType.Shooting)
                    AttackPlayerShooting();
                else if (enemyType == EnemyType.Charging)
                    AttackPlayerCharging();
                Debug.Log("Attacking State");
                break;
        }
    }

    private void Patrol()
    {
        if (!isPatrolDestinationSet)
            SetPatrolDestination();

        if (isPatrolDestinationSet)
            // Check if NavMeshAgent is enabled before setting destination
            if (agent != null && agent.enabled)
            {
                agent.destination = patrolDestination; 
            }

        float distanceToDestination = Vector3.Distance(transform.position, patrolDestination);

        // Destination reached
        if (distanceToDestination < 1f)
            isPatrolDestinationSet = false;
    }

    private void SetPatrolDestination()
    {
        // Calculate random point within patrol radius
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);

        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y + 3f, transform.position.z + randomZ);

        // Check if the point is on the ground
        if (Physics.Raycast(randomPoint, Vector3.down, 7f, groundLayer))
        {
            patrolDestination = randomPoint;
            isPatrolDestinationSet = true;
        }
    }

    private void ChasePlayer()
    {
        // Set a new destination if one hasn't been set or the current one is reached
        if (!isChaseDestinationSet || Vector3.Distance(transform.position, chaseDestination) < 1f)
        {
            SetChaseDestination();
        }

        // Direct the agent to the chase destination
        if (isChaseDestinationSet)
            
            // Check if NavMeshAgent is enabled before setting destination
            if (agent != null && agent.enabled)
            {
                agent.destination = chaseDestination;
            }
    }

    private void SetChaseDestination()
    {
        float randomX = Random.Range(-attackRange, attackRange);
        float randomZ = Random.Range(-attackRange, attackRange);
        Vector3 randomOffset = new Vector3(randomX, 0, randomZ);
        chaseDestination = player.position + randomOffset;

        // Ensure the destination is on the NavMesh
        if (NavMesh.SamplePosition(chaseDestination, out NavMeshHit hit, attackRange, NavMesh.AllAreas))
        {
            chaseDestination = hit.position;
            isChaseDestinationSet = true;
        }
    }

    private void AttackPlayerShooting()
    {
        // Stop moving while attacking
        // Check if NavMeshAgent is enabled before setting destination
        if (agent != null && agent.enabled)
        {
            agent.destination = transform.position;
        }
        

        // Face the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);


        if (!isAttackOnCooldown)
        {
            // Test Attacking
            //Rigidbody rb = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            //Test
            //Shoot();

            // Shooting attack
            StartCoroutine(FocusAndShoot());

            // Melee attack

            isAttackOnCooldown = true;
            Invoke(nameof(ResetAttack), shootAttackCooldown);
        }

    }
    private void AttackPlayerCharging()
    {
        if (!isAttackOnCooldown)
        {
            //Reset agent speed
            agent.speed = defaultSpeed * 2f;

            // charge
            if (agent != null && agent.enabled)
            {
                agent.destination = player.position;
            }

            // Reset retreat flag to allow new retreat destination next time
            isRetreatDestinationSet = false;
        }
        else
        {

            // Begin backing off only once during cooldown
            if (!isRetreatDestinationSet)
            {
                // Slow down the agent for retreating
                agent.speed = defaultSpeed * 0.5f;

                SetRetreatDestination();
                isRetreatDestinationSet = true;
            }

            
            if (agent != null && agent.enabled)
            {
                agent.destination = retreatDestination;
            }

        }
    }

    private IEnumerator FocusAndShoot()
    {
        Debug.Log("Focus and Shoot");
        isCharging = true;

         // Instantiate the bullet
        currentBullet = Instantiate(bullet, gunPoint.position, Quaternion.identity);

        // Set to trigger so no collisions while charging
        currentBullet.GetComponent<Collider>().isTrigger = true;

        // Get light component of bullet
        Light bulletLight = currentBullet.GetComponent<Light>();
        float maxIntensity = 5f;
        

        // Scale the bullet from small to full size over 3 seconds
        float focusTime = 3f;

        // Bullet's current local scale as the initial scale
        Vector3 initialScale = currentBullet.transform.localScale;
        Vector3 targetScale = initialScale * 5f; // Scale up by a factor for the "charging" effect

        float elapsedTime = 0f;

        while (elapsedTime < focusTime)
        {
            currentBullet.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / focusTime);

            // Increase light intensity gradually
            bulletLight.intensity = Mathf.Lerp(0, maxIntensity, elapsedTime / focusTime); // Adjustable max intensity

            elapsedTime += Time.deltaTime;
            yield return null;
        }   

        // Ensure final scale is reached
        currentBullet.transform.localScale = targetScale;
        bulletLight.intensity = maxIntensity;

        // Calculate direction towards the player without spread
        Vector3 shootDirection = (player.position - gunPoint.position).normalized;

        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate new direction, considering spread
        Vector3 shootDirectionSpread = shootDirection + new Vector3(x, y, 0);

        // Rotate bullet to correct shooting direction according to direction calculated earlier
        currentBullet.transform.forward = shootDirectionSpread.normalized;

        // Give bullet force
        currentBullet.GetComponent<Collider>().isTrigger = false;
        currentBullet.GetComponent<Rigidbody>().AddForce(shootDirectionSpread.normalized * bulletForce, ForceMode.Impulse);
        Debug.DrawRay(gunPoint.position, shootDirectionSpread.normalized * bulletRange, Color.red, 1f);

        // Destroy the projectile after it has traveled its range
        Destroy(currentBullet, bulletRange / bulletForce);

        isCharging = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected!");
        // Check if collided object is the player and attack cooldown is off
        if (!isAttackOnCooldown)
        {
            PlayerHUD player = collision.gameObject.GetComponent<PlayerHUD>();
            if (player != null)
            {
                Debug.Log("Collision with player detected!");
                // Deal damage to the player
                player.TakeDamage(chargeDamage);
                isAttackOnCooldown = true;

                Invoke(nameof(ResetAttack), chargeAttackCooldown);
            }
        }
    }

    private void SetRetreatDestination()
    {
        float randomX = Random.Range(-attackRange, attackRange);
        float randomZ = Random.Range(-attackRange, attackRange);
        Vector3 randomOffset = new Vector3(randomX, 0, randomZ);
        retreatDestination = player.position + randomOffset;
    }

    private void ResetAttack()
    {
        isAttackOnCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualize attack ranges
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minAttackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, maxAttackRange);
    }

    private void HandleFootsteps()
    {
        // Handle footsteps based on velocity
        float speed = agent.velocity.magnitude;
        anim.SetFloat("Speed", speed);

    }

    public void OnFootstep()
    {
        Debug.Log("On footstep being called");

        // Get the agent's current speed (this will help us determine if it's running or walking)
        float currentSpeed = agent.velocity.magnitude;

        // Play running footsteps if above the threshold, otherwise play walking footsteps
        if (currentSpeed > runThreshold)
        {
            enemyFootstepsAudio.PlayFootstepSound(true);  // Play running footsteps
            Debug.Log("Playing running footsteps based on speed");
        }
        else if (currentSpeed > 0)
        {
            enemyFootstepsAudio.PlayFootstepSound(false);  // Play walking footsteps
            Debug.Log("Playing walking footsteps based on speed");
        }

    }   

    public void DestroyCurrentBullet()
    {
        if (currentBullet != null)
        {
            Destroy(currentBullet); // Destroy any bullet if it's still present
            currentBullet = null;
        }
    }


}
