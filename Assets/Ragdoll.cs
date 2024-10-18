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
        if (Input.GetKeyDown(KeyCode.R)) {
            ToggleRagdoll(!IsRagdolling);
        }
    }

    private void ToggleRagdoll(bool isRagdoll)
    {
        IsRagdolling = isRagdoll;
        myCollider.enabled = !isRagdoll;
        myAnimator.enabled = !isRagdoll;
        myNavMeshAgent.enabled = !isRagdoll;

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !isRagdoll;
        }
    }
}
