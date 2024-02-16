using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallingHash = Animator.StringToHash("Falling");

    private Vector3 momentum;
    private const float CrossFadeDuration = 0.1f;
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        momentum = stateMachine.CharacterController.velocity;
        momentum.y = 0f;
        stateMachine.Animator.CrossFadeInFixedTime(FallingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum,deltaTime);
        if(stateMachine.CharacterController.isGrounded)
        {
            ReturnToLocomotion();
        }
        FaceTarget();
    }

    public override void Exit()
    {
        
    }
}
