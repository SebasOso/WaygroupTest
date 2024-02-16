using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag;
    [SerializeField] private NavMeshAgent navMeshAgent;
    private Vector3 dampingVelocity;

    private Vector3 impact;

    private float verticalVelocity;
    [SerializeField] private bool isGrounded;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] LayerMask obstaclesLayerMask;
    [SerializeField] float groundCheckSphereRad = 0.37f;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;
    void Update()
    {
        HandleGroundCheck();
        GetComponent<Animator>().SetBool("isGrounded", isGrounded);
        if(verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
        if(navMeshAgent != null)
        {
            if(impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                navMeshAgent.enabled = true;
            }
        }
    }
    public void AddForce(Vector3 force)
    {
        impact += force;
        if(navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }
    }
    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
    protected void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckSphereRad, groundLayerMask);
    }
    protected void OnDrawGizmosSelected() 
    {
        Gizmos.DrawSphere(transform.position, groundCheckSphereRad);    
    }
}
