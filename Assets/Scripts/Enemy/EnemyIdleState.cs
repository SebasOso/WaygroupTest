using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int EnemyLocomotionBlendTree = Animator.StringToHash("LocomotionBT");
    private readonly int EnemyLocomotionSpeed = Animator.StringToHash("enemySpeed");
    private const float CrossFadeDuration = 0.1f;
    public EnemyIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void Enter()
    {
        enemyStateMachine.HealthBar.OffBar();
        enemyStateMachine.Animator.CrossFadeInFixedTime(EnemyLocomotionBlendTree, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        enemyStateMachine.EnemyMover.MoveTo(Vector3.zero, 0f);
        if(enemyStateMachine.FieldOfView.canSeePlayer)
        {
            Debug.Log("In Range");
            enemyStateMachine.SwitchState(new EnemyChasingState(this.enemyStateMachine));
            return;
        }
        if(IsInChasingRange())
        {
            Debug.Log("In Range");
            enemyStateMachine.SwitchState(new EnemyChasingState(this.enemyStateMachine));
            return;
        }
        if(enemyStateMachine.PatrolPath != null)
        {
            enemyStateMachine.SwitchState(new EnemyPatrollingState(enemyStateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
