using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisarmingState : PlayerBaseState
{
    private readonly int DisarmHash = Animator.StringToHash("Disarm");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 1.21f;
    public PlayerDisarmingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DisarmHash, CrossFadeDuration);
        foreach (GameObject weaponLogic in stateMachine.WeaponsLogics)
        {
            weaponLogic.SetActive(false);
        }
    }
    public override void Tick(float deltaTime)
    {
        stateMachine.InputReader.IsDisarming = true;
        Move(deltaTime);
        duration -= deltaTime;
        if(duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }
    public override void Exit()
    {
        stateMachine.InputReader.IsDisarming = false;
    }
}
