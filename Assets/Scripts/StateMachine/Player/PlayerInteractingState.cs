using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractingState : PlayerBaseState
{
    private readonly int InteractHash = Animator.StringToHash("Interact");
    private readonly int LocomotionSpeed = Animator.StringToHash("speed");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 7.02f;
    public PlayerInteractingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(InteractHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        stateMachine.InputReader.IsInteracting = false;
        Move(deltaTime);
        duration -= deltaTime;
        if(duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }
    public override void Exit()
    {

    }
}
