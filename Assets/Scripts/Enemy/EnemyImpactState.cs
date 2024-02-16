using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("EnemyImpact");
    private readonly int EnemyLocomotionSpeed = Animator.StringToHash("enemySpeed");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 1f;
    public EnemyImpactState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }
    public override void Enter()
    {
        enemyStateMachine.HealthBar.OnBar();
        enemyStateMachine.EnemySoundManager.PlayHit();
        enemyStateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        enemyStateMachine.navMeshAgent.speed = 0f;
        duration -= deltaTime;
        if(duration <= 0f)
        {
            enemyStateMachine.SwitchState(new EnemyIdleState(enemyStateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
