using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Collider[] colliders;
    private Rigidbody[] rigidbodies;
    [SerializeField] private NavMeshAgent navMeshAgent;
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>(true);
        rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        ToggleRagdoll(false);
    }
    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if(rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }
        navMeshAgent.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
    public void ApplyForce(Vector3 force, Vector3 pointToForce)
    {
        ToggleRagdoll(true);

        Rigidbody hitRb = rigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, pointToForce)).First();

        hitRb.AddForceAtPosition(force, pointToForce, ForceMode.Impulse);
    }
}
