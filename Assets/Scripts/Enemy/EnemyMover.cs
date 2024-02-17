using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float maxSpeed = 8.5f;
    public NavMeshAgent navMesh;

    Health health;
    public static EnemyMover Instance {get; private set;}
    private void Awake() 
    {
        Instance = this;
        health = GetComponent<Health>();
        navMesh = gameObject.GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        navMesh.enabled = !health.IsDead();
        UpdateAnimator();
    }
    public void StartMoveAction(Vector3 destination, float speedFraction)
    {
        MoveTo(destination, speedFraction);
    }
    public void MoveTo(Vector3 destination, float speedFraction)
    {
        navMesh.SetDestination(destination);
        navMesh.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        navMesh.isStopped = false;
    }
    public void Cancel()
    {
        navMesh.isStopped = true;
    }
    private void UpdateAnimator()
    {
        Vector3 velocity = gameObject.GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        anim.SetFloat("speed", speed);
    }
}
