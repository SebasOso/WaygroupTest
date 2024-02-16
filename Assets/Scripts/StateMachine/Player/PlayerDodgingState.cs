using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgingBlendTree = Animator.StringToHash("DodgingBT");
    private readonly int DodgeForwardSpeed = Animator.StringToHash("dodgeForward");
    private readonly int DodgeRightSpeed = Animator.StringToHash("dodgeRight");
    private const float CrossFadeDuration = 0.1f;
    private Vector2 dodgingDirectionInput;
    private float remainingDodgeTime;
    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirection) : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirection;
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;
        stateMachine.Animator.SetFloat(DodgeForwardSpeed, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(DodgeRightSpeed, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgingBlendTree, CrossFadeDuration);
        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeDistance /stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeDistance /stateMachine.DodgeDuration;
        Move(movement, deltaTime);
        FaceTarget();
        remainingDodgeTime -= deltaTime;
        if(remainingDodgeTime <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
}
