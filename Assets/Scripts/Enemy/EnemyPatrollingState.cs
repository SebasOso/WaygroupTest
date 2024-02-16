using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    private readonly int EnemyLocomotionBlendTree = Animator.StringToHash("LocomotionBT");
    private readonly int EnemyLocomotionSpeed = Animator.StringToHash("enemySpeed");
    private const float CrossFadeDuration = 0.1f;
    int currentWaypointIndex = 0;
    Vector3 guardPosition;
    float timeSinceLastWayPoint = Mathf.Infinity;
    public EnemyPatrollingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        enemyStateMachine.Animator.CrossFadeInFixedTime(EnemyLocomotionBlendTree, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(enemyStateMachine.FieldOfView.canSeePlayer)
        {
            enemyStateMachine.SwitchState(new EnemyChasingState(enemyStateMachine));
        }
        if(IsInChasingRange())
        {
            enemyStateMachine.SwitchState(new EnemyChasingState(enemyStateMachine));
        }
        Vector3 nextPosition = guardPosition;
        if(enemyStateMachine.PatrolPath != null)
        {
            if(AtWaypoint())
            {
                timeSinceLastWayPoint = 0;
                CycleWayPoint();
            }
            nextPosition = GetCurrentWaypoint();
        }
        if(timeSinceLastWayPoint > enemyStateMachine.StayTime)
        {
           enemyStateMachine.EnemyMover.MoveTo(nextPosition, 0.5f);
        }
        UpdateTimers();
    }

    public override void Exit()
    {
        
    }
    private Vector3 GetCurrentWaypoint()
    {
        return enemyStateMachine.PatrolPath.GetMark(currentWaypointIndex);
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(enemyStateMachine.transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < enemyStateMachine.WaypointTolerance;
    }
    private void CycleWayPoint()
    {
        currentWaypointIndex = enemyStateMachine.PatrolPath.GetNextIndex(currentWaypointIndex);
    }
    private void UpdateTimers()
    {
        timeSinceLastWayPoint += Time.deltaTime;
    }
}
