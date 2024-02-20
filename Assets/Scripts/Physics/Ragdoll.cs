
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

/// <summary>
/// Manages the ragdoll behavior of a character, enabling and disabling ragdoll physics.
/// </summary>
public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;        
    private Collider[] colliders;                      
    private Rigidbody[] rigidbodies;                   
    [SerializeField] private NavMeshAgent navMeshAgent; 

    /// <summary>
    /// Initializes colliders and rigidbodies and sets the initial state of the ragdoll.
    /// </summary>
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>(true);    
        rigidbodies = GetComponentsInChildren<Rigidbody>(true); 
        ToggleRagdoll(false);                                  
    }

    /// <summary>
    /// Toggles the ragdoll physics on or off based on the provided flag.
    /// </summary>
    /// <param name="isRagdoll">Flag indicating whether to enable or disable ragdoll physics.</param>
    public void ToggleRagdoll(bool isRagdoll)
    {
        // Enables or disables colliders and rigidbodies based on the provided flag
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if (rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        // Enables or disables the NavMeshAgent and animator components based on the provided flag
        navMeshAgent.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }

    /// <summary>
    /// Applies force to the character's ragdoll body parts.
    /// </summary>
    /// <param name="force">Force vector to be applied.</param>
    /// <param name="pointToForce">Point at which the force should be applied.</param>
    public void ApplyForce(Vector3 force, Vector3 pointToForce)
    {
        ToggleRagdoll(true); 

        
        Rigidbody hitRb = rigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, pointToForce)).First();
        hitRb.AddForceAtPosition(force, pointToForce, ForceMode.Impulse);

        Debug.Log("Force direction: " + force);
    }
}
