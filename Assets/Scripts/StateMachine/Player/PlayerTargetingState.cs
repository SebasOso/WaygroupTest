using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTree = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardSpeed = Animator.StringToHash("targetingForward");
    private readonly int TargetingRightSpeed = Animator.StringToHash("targetingRight");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }
    public override void Enter()
    {
        stateMachine.InputReader.HealEvent += OnHeal;
        stateMachine.InputReader.RuneAttackEvent += OnRuneAttack;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.TargetEvent += OnCancelTarget;
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTree, CrossFadeDuration);
    }

    private void OnHeal()
    {
        stateMachine.SwitchState(new PlayerHealingState(stateMachine));
    }

    private void OnRuneAttack()
    {
        if(stateMachine.RuneManager.isCoolDown == false)
        {
            stateMachine.SwitchState(new PlayerRuneAttackState(stateMachine));
        }
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        if(stateMachine.Targeter.currentTarget ==  null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
        


        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
        UpdateAnimator(deltaTime);
        FaceTarget();
    }

    private void UpdateAnimator(float deltaTime)
    {
        if(stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardSpeed, 0, AnimatorDampTime, deltaTime);
        }
        else
        {
            float valueForward = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingForwardSpeed, valueForward, AnimatorDampTime, deltaTime);
        }
        if(stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightSpeed, 0, AnimatorDampTime, deltaTime);
        }
        else
        {
            float valueRight = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightSpeed, valueRight, AnimatorDampTime, deltaTime);
        }
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat("speed", 0f);

            stateMachine.FreeLookMovementSpeed = 0f;
        }
        else
        {
            stateMachine.Animator.SetFloat("speed", 1.5f, AnimatorDampTime, deltaTime);
        }
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.TargetEvent -= OnCancelTarget;
    }

    private void OnDodge()
    {
        if(stateMachine.InputReader.MovementValue == Vector2.zero){return;}
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }
    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }
    private void OnCancelTarget()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        return movement;
    }
}
