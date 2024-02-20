using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Receives external forces and handles character movement and physics interactions.
/// </summary>
public class ForceReceiver : MonoBehaviour
{
    [Header("Variables Needed")]
    [SerializeField] private CharacterController controller; 
    [SerializeField] private float drag;
    [SerializeField] private NavMeshAgent navMeshAgent; 
    private Vector3 dampingVelocity; 

    private Vector3 impact; 

    private float verticalVelocity; 

    [Header("Debug")]
    [SerializeField] public bool isGrounded; 

    [Header("Settings")]
    [SerializeField] LayerMask groundLayerMask; 
    [SerializeField] LayerMask obstaclesLayerMask; 
    [SerializeField] float groundCheckSphereRad = 0.37f; 
    [SerializeField] float forceMagnitude; 

    /// <summary>
    /// Gets the total movement vector of the character including impact and vertical velocity.
    /// </summary>
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    /// <summary>
    /// Updates the character's movement and applies gravity and impact forces.
    /// </summary>
    void Update()
    {
        HandleGroundCheck(); 
        GetComponent<Animator>().SetBool("IsGrounded", isGrounded); 

        // Applies gravity and handles vertical velocity
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        // Smooths out the impact force and enables NavMeshAgent if impact force is low
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
        if (navMeshAgent != null)
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                navMeshAgent.enabled = true;
            }
        }
    }

    /// <summary>
    /// Handles collisions with other colliders and applies force to rigidbodies.
    /// </summary>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rb.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Adds force to the character's impact vector.
    /// </summary>
    public void AddForce(Vector3 force)
    {
        impact += force;
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }
    }

    /// <summary>
    /// Makes the character jump with the given jump force.
    /// </summary>
    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    /// <summary>
    /// Checks if the character is grounded using a spherecast.
    /// </summary>
    protected void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckSphereRad, groundLayerMask);
    }

    /// <summary>
    /// Draws a gizmo to visualize the ground check sphere.
    /// </summary>
    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, groundCheckSphereRad);
    }
}
