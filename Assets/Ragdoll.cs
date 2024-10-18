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

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        myCollider = GetComponent<Collider>();
        myAnimator = GetComponent<Animator>();
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        ToggleRagdoll(false);
    }

    void Update()
    {
        // Q to ragdoll
        if (Input.GetKeyDown(KeyCode.Q)) {
            ToggleRagdoll(!IsRagdolling);
        }
    }

    private void ToggleRagdoll(bool isRagdoll)
    {
        // turn on/off collider, animator, and navmeshagent if ragdoll
        IsRagdolling = isRagdoll;
        myCollider.enabled = !isRagdoll;
        myAnimator.enabled = !isRagdoll;
        myNavMeshAgent.enabled = !isRagdoll;

        // turn on/off ragdoll rigidbodies
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !isRagdoll;
        }
    }
}
