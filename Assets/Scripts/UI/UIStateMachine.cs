using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIStateMachine : StateMachine
{
    [field: SerializeField] 
    public PlayerInput PlayerInput {get; private set;}
    [field: SerializeField] 
    public Transform ImageSocket {get; private set;}
    [field: SerializeField] 
    public InteractButtom GamepadInteractButtom {get; private set;}
    [field: SerializeField] 
    public InteractButtom KeyboardInteractButtom {get; private set;}
    private InteractButtom currentInteractButtom;
    private void Awake() 
    {
        SwitchState(new UIGamePadState(this));    
    }
    public void ShowButtom(InteractButtom interactButtom)
    {
        currentInteractButtom = interactButtom;
        interactButtom.Spawn(ImageSocket);
    }
}
