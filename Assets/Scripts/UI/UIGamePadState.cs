using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePadState : UIBaseState
{
    public UIGamePadState(UIStateMachine uiStateMachine) : base(uiStateMachine)
    {
    }

    public override void Enter()
    {
       uiStateMachine.ShowButtom(uiStateMachine.GamepadInteractButtom);
    }
    public override void Tick(float deltaTime)
    {
        if(uiStateMachine.PlayerInput.currentControlScheme == "Keyboard & Mouse")
        {
            uiStateMachine.SwitchState(new UIKeyboardState(uiStateMachine));
        }
    }
    public override void Exit()
    {
       
    }
}
