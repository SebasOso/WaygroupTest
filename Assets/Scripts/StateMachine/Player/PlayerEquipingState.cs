using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipingState : PlayerBaseState
{
    private readonly int EquipHash = Animator.StringToHash("Equip");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 1.06f;
    public PlayerEquipingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(EquipHash, CrossFadeDuration);
        foreach (GameObject weaponLogic in stateMachine.WeaponsLogics)
        {
            weaponLogic.SetActive(false);
        }
    }
    public override void Tick(float deltaTime)
    {
        stateMachine.InputReader.IsEquiping = true;
        Move(deltaTime);
        duration -= deltaTime;
        if(duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }
    public override void Exit()
    {
        stateMachine.InputReader.IsEquiping = false;
    }
}
