using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int EnemyRunLocomotionBlendTree = Animator.StringToHash("RunBT");
    private readonly int EnemyLocomotionSpeed = Animator.StringToHash("enemySpeed");
    private const float CrossFadeDuration = 0.1f;
    public EnemyChasingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        enemyStateMachine.Animator.CrossFadeInFixedTime(EnemyRunLocomotionBlendTree, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(!IsInChasingRange() && !enemyStateMachine.isAggro)
        {
            if(!enemyStateMachine.FieldOfView.canSeePlayer)
            {
                enemyStateMachine.SwitchState(new EnemySuspiciusState(this.enemyStateMachine));
                return;
            }
        }
        else if(IsInAttackRange())
        {
            enemyStateMachine.SwitchState(new EnemyAttackingState(this.enemyStateMachine));
            return;
        }
        enemyStateMachine.EnemyMover.MoveTo(enemyStateMachine.Player.transform.position, 1.0f);
        FacePlayer();
    }

    public override void Exit()
    {

    }
}
