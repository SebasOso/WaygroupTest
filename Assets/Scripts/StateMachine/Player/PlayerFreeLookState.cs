using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Waygroup;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookBlendTree = Animator.StringToHash("LocomotionBT");
    private const float CrossFadeDuration = 0.1f;
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTree, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        stateMachine.PlayerController.CanMove = true;
        if (!stateMachine.ForceReceiver.isGrounded)
        {
            stateMachine.PlayerController.CanMove = false;
        }
    }

    public override void Exit()
    {
        
    }
}
