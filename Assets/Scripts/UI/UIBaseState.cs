using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseState : State
{
    protected UIStateMachine uiStateMachine;
    public UIBaseState(UIStateMachine uiStateMachine)
    {
        this.uiStateMachine = uiStateMachine;
    }
}
