using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly int EnemyLocomotionSpeed = Animator.StringToHash("enemySpeed");
    private const float CrossFadeDuration = 0.1f;
    public EnemyAttackingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        FacePlayer();
        enemyStateMachine.HealthBar.OnBar();
        enemyStateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer();
        if(GetNormalizedTime(enemyStateMachine.Animator) >= 1)
        {
            enemyStateMachine.SwitchState(new EnemyChasingState(enemyStateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
