using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Class;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReaderClass : ScriptableObject, IPlayerActions
{
    public bool IsRunning;
    public event Action<bool> JumpEvent;
    public event Action TauntEvent;
    public event Action<bool> AttackEvent;
    public event Action<Vector2> MoveEvent;
    private Class classControls;

    private void OnEnable() 
    {
        if(classControls == null)
        {
            classControls = new Class();
            classControls.Player.SetCallbacks(this);
        }
        classControls.Player.Enable();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            AttackEvent?.Invoke(true);
        }
        else if(context.canceled)
        {
            AttackEvent?.Invoke(false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            JumpEvent?.Invoke(true);
        }
        else if(context.canceled)
        {
            JumpEvent?.Invoke(false);
        }
    }

    public void OnMoveAction(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnTaunt(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        TauntEvent?.Invoke();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            IsRunning = true;
        }
        else if(context.canceled)
        {
            IsRunning = false;
        }
    }
}
