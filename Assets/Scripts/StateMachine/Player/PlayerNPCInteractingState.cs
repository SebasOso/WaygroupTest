using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCInteractingState : PlayerBaseState
{
    private readonly int NPCHash = Animator.StringToHash("InteractNPC");
    private const float CrossFadeDuration = 0.1f;
    public PlayerNPCInteractingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(NPCHash, CrossFadeDuration);
        stateMachine.Armory.UnequipWeapon();
        stateMachine.isInteracting = true;
    }

    public override void Exit()
    {
        stateMachine.isInteracting = false;
    }

    public override void Tick(float deltaTime)
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        Move(deltaTime);
        if(!stateMachine.IsNearNPC)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            stateMachine.isInteracting = false;
        }
    }
}
