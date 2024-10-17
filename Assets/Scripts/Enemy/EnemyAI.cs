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

    // Detection
    public float detectionRange = 10f;
    public float attackRange = 5f;
    private bool playerInDetectionRange;
    private bool playerInAttackRange;

    // Shooting properties
    public float bulletForce = 20f;
    public float bulletRange = 50f;
    public float spread = 0.1f;
    public float attackCooldown = 1f;
    private bool isAttackOnCooldown;
    
    public GameObject bullet;
    public Transform gunPoint;

    // Animation
    [SerializeField]
    private float speed = 0.0f;
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
            currentState = EnemyState.Patrolling;
        else if (playerInDetectionRange && !playerInAttackRange)
            currentState = EnemyState.Chasing;
        else if (playerInAttackRange && playerInDetectionRange)
            currentState = EnemyState.Attacking;
    }

    private void ExecuteStateAction()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                Debug.Log("Patrolling");
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                Debug.Log("Chasing");
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                Debug.Log("Attacking");
                break;
        }
    }

    private void Patrol()
    {
        if (!isPatrolDestinationSet)
            SetPatrolDestination();

        if (isPatrolDestinationSet)
            agent.destination = patrolDestination; 

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
        agent.destination = player.position;
    }
    private void AttackPlayer()
    {
        // Stop moving while attacking
        agent.destination = transform.position;

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

            // Shooting attack
            Shoot();

            // Melee attack

            isAttackOnCooldown = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void Shoot()
    {
        // Calculate direction towards the player without spread
        Vector3 shootDirection = (player.position - gunPoint.position).normalized;

        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate new direction, considering spread
        Vector3 shootDirectionSpread = shootDirection + new Vector3(x, y, 0);

        // Instantiate the bullet
        GameObject currentBullet = Instantiate(bullet, gunPoint.position, Quaternion.identity);

        // Rotate bullet to correct shooting direction according to direction calculated earlier
        currentBullet.transform.forward = shootDirectionSpread.normalized;

        // Give bullet force
        currentBullet.GetComponent<Rigidbody>().AddForce(shootDirectionSpread.normalized * bulletForce, ForceMode.Impulse);

        // Destroy the projectile after it has traveled its range
        Destroy(currentBullet, bulletRange / bulletForce);
    }

    private void ResetAttack()
    {
        isAttackOnCooldown = false;
    }

    public void ReceiveDamage(float damageAmount)
    {
        healthPoints -= damageAmount;

        if (healthPoints <= 0f)
            Die();
    }

    private void Die()
    {
        // Implement death effects or animations here
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection and attack ranges in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
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
}
