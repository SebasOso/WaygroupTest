using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine enemyStateMachine;
    public EnemyBaseState(EnemyStateMachine enemyStateMachine)
    {
        this.enemyStateMachine = enemyStateMachine;
    }
    public override void Enter()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
    protected bool IsInChasingRange()
    {
        float distancePlayerMagnitude = (enemyStateMachine.Player.transform.position - enemyStateMachine.transform.position).sqrMagnitude;
        return distancePlayerMagnitude <= enemyStateMachine.PlayerDetectionRange * enemyStateMachine.PlayerDetectionRange;
    }
    protected bool IsInAttackRange()
    {
        float distanceAttackMagnitude = (enemyStateMachine.Player.transform.position - enemyStateMachine.transform.position).sqrMagnitude;
        return distanceAttackMagnitude <= enemyStateMachine.EnemyArmory.currentWeapon.value.GetWeaponRange() * enemyStateMachine.EnemyArmory.currentWeapon.value.GetWeaponRange();
    }
    protected void FacePlayer()
    {
        if(enemyStateMachine.Player == null){return;}
        Vector3 targetDirection = (enemyStateMachine.Player.transform.position - enemyStateMachine.transform.position);
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        float rotationSpeed = 8.4f; 
        enemyStateMachine.transform.rotation = Quaternion.Slerp(enemyStateMachine.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
