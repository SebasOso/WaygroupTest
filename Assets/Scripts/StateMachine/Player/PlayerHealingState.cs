using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealingState : PlayerBaseState
{
    private readonly int HealHash = Animator.StringToHash("Heal");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 2.20f;

    public PlayerHealingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(HealHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.InputReader.IsHealing = true;
        Move(deltaTime);
        duration -= deltaTime;
        if (duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
       stateMachine.InputReader.IsHealing = false;
    }
}
