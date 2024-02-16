using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
    private readonly int StunHash = Animator.StringToHash("Stun");
    private const float CrossFadeDuration = 0.1f;
    public EnemyStunState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        enemyStateMachine.snowSystem.Play();
        enemyStateMachine.isStunned = true;
        enemyStateMachine.HealthBar.OnBar();
        enemyStateMachine.Animator.CrossFadeInFixedTime(StunHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        enemyStateMachine.EnemyMover.MoveTo(Vector3.zero, 0f);
    }

    public override void Exit()
    {
        enemyStateMachine.snowSystem.Stop();
    }
}
