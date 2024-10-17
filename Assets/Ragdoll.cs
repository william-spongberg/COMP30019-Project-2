using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private bool IsRagdoll = false;

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
        SetRagdoll(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !IsRagdoll)
        {
            SetRagdoll(!IsRagdoll);
        }
        else if (Input.GetKeyDown(KeyCode.R) && IsRagdoll)
        {
            SetRagdoll(IsRagdoll);
        }
    }

    private void SetRagdoll(bool bRagdoll)
    {
        IsRagdoll = bRagdoll;
        myCollider.enabled = !bRagdoll;
        myAnimator.enabled = !bRagdoll;
        myNavMeshAgent.enabled = !bRagdoll;

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !bRagdoll;
        }
    }
}
