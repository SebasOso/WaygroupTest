using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("speed");
    private readonly int FreeLookBlendTree = Animator.StringToHash("LocomotionBT");
    private readonly int RunBlendTree = Animator.StringToHash("RunningBT");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.InputReader.HealEvent += OnHeal;
        stateMachine.InputReader.EquipEvent += OnEquip;
        stateMachine.InputReader.DisarmEvent += OnDisarm;
        stateMachine.InputReader.RuneAttackEvent += OnRuneAttack;
        stateMachine.InputReader.InteractEvent += OnInteract;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTree, CrossFadeDuration);
    }

    private void OnHeal()
    {
        stateMachine.HealManager.Ability();
        stateMachine.SwitchState(new PlayerHealingState(stateMachine));
    }

    private void OnDisarm()
    {
        stateMachine.SwitchState(new PlayerDisarmingState(stateMachine));
    }

    private void OnEquip()
    {
        stateMachine.SwitchState(new PlayerEquipingState(stateMachine));
    }

    private void OnRuneAttack()
    {
        if(stateMachine.RuneManager.isCoolDown == false)
        {
            stateMachine.SwitchState(new PlayerRuneAttackState(stateMachine));
        }
    }

    private void OnInteract()
    {
        if(stateMachine.IsNearNPC)
        {
            stateMachine.SwitchState(new PlayerNPCInteractingState(stateMachine));
            CustomSellerManager.Instance.OpenShop();
        }
        if(stateMachine.InputReader.IsInteracting)
        {
            stateMachine.SwitchState(new PlayerInteractingState(stateMachine));
        }
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
        if(stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetBool("isIdle", true);
            stateMachine.Animator.SetBool("isRun", false);
            stateMachine.Animator.SetFloat("speed", 0f);
            
            stateMachine.FreeLookMovementSpeed = 0f;
            return;
        }
        if(stateMachine.InputReader.IsRunning)
        {
            stateMachine.Animator.SetBool("isIdle", false);
            stateMachine.Animator.SetBool("isRun", true);
            stateMachine.FreeLookMovementSpeed = stateMachine.RunningMovementSpeed;
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, stateMachine.RunningMovementSpeed, AnimatorDampTime, deltaTime);
            FaceMovementDirection(movement, deltaTime);
            return;
        }
        stateMachine.FreeLookMovementSpeed = stateMachine.WalkingMovementSpeed;
        stateMachine.Animator.SetBool("isIdle", false);
        stateMachine.Animator.SetBool("isRun", false);
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, stateMachine.WalkingMovementSpeed, AnimatorDampTime, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }
    private void OnTarget()
    {
        if(!stateMachine.Targeter.SelectTarget()){return;}
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }
    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 cameraForward = stateMachine.MainCameraTransform.forward;
        Vector3 cameraRight = stateMachine.MainCameraTransform.right;

        cameraForward.y = 0f;  
        
        cameraForward.Normalize();
            
        Vector3 movement = cameraForward * stateMachine.InputReader.MovementValue.y + cameraRight * stateMachine.InputReader.MovementValue.x;

        return movement;
    }
    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation, 
            Quaternion.LookRotation(movement), 
            deltaTime * stateMachine.RotationDamping);
    }
}
