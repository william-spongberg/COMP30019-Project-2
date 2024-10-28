using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private bool IsRagdolling = false;

    private Collider myCollider;
    private Rigidbody[] rigidbodies;
    private Animator myAnimator;
    private NavMeshAgent myNavMeshAgent;

    // References to other scripts
    private EnemyAI enemyAI;
    private EnemyHP enemyHP;

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        myCollider = GetComponent<Collider>();
        myAnimator = GetComponent<Animator>();
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<EnemyAI>();
        ToggleRagdoll(false);
    }

    void Update()
    {
        // Q to ragdoll
        //if (Input.GetKeyDown(KeyCode.Q)) {
          //  ToggleRagdoll(!IsRagdolling);
        //}
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        IsRagdolling = isRagdoll;

        Debug.Log("EnemyAI enabled status: " + enemyAI.enabled);

        // turn on/off collider, animator, and navmeshagent if ragdoll
        myCollider.enabled = !isRagdoll;
        myAnimator.enabled = !isRagdoll;
        myNavMeshAgent.enabled = !isRagdoll;

        // Turn on/off ragdoll rigidbodies
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !isRagdoll;
            
            // Reset any velocity when turning ragdoll on to prevent lingering forces
            if (isRagdoll)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
