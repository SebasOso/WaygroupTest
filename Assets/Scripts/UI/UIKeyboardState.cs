using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKeyboardState : UIBaseState
{
    public UIKeyboardState(UIStateMachine uiStateMachine) : base(uiStateMachine)
    {
    }

    public override void Enter()
    {
        uiStateMachine.ShowButtom(uiStateMachine.KeyboardInteractButtom);
    }
    public override void Tick(float deltaTime)
    {
        if(uiStateMachine.PlayerInput.currentControlScheme == "GamePad")
        {
            uiStateMachine.SwitchState(new UIGamePadState(uiStateMachine));
        }
    }
    public override void Exit()
    {
        
    }
}
