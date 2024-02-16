using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        //Toggle Ragdoll
        for (int i = 0; i < stateMachine.WeaponsLogics.Count; i++)
        {
            stateMachine.WeaponsLogics[i].SetActive(false);
        }
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
}
