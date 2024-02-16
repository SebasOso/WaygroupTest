using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public static InputReader Instance;
    private PlayerInput playerInput;
    private InputAction inventoryOpenCloseAction;
    public bool InventoryOpenCloseInput {get; private set;}
    public Vector2 MovementValue {get; private set;}
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action RuneAttackEvent;
    public event Action HealEvent;
    public event Action EquipEvent;
    public event Action DisarmEvent;
    public event Action CancelTargetEvent;
    public event Action InteractEvent;
    public bool  IsAttacking{get; set;}
    public bool IsInteracting{get;set;}
    public bool IsNPCInteracting{get;set;}
    public bool CanRuneAttack{get;set;}
    [field: SerializeField] public bool CanHeal { get; set;}
    public bool IsRunning{get; private set;}
    public bool IsInRuneAttack{get; set;}
    public bool  IsHeavyAttacking{get; set;}
    public bool IsEquipped { get;  set; }
    public bool IsHealing { get; set;}
    public bool CanDisarm{get;set;}
    public bool IsShop { get;  set; }
    public bool IsDisarming { get; set;}
    public bool IsEquiping { get; set;}

    private Controls controls;
    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        playerInput = GetComponent<PlayerInput>();
        inventoryOpenCloseAction = playerInput.actions["InventoryOpenClose"];
    }
    private void Start() 
    {
        if(controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);

            controls.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Update() 
    {
        InventoryOpenCloseInput = inventoryOpenCloseAction.WasPressedThisFrame();
    }
    private void OnDestroy() 
    {
        controls?.Player.Disable();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(MovementValue == Vector2.zero) return;
        if(IsRunning)
        {
            if(!context.performed){return;}
            JumpEvent?.Invoke();
        }
    }
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (IsHealing) { return; }
        if (!context.performed){return;}
        DodgeEvent?.Invoke();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLookAround(InputAction.CallbackContext context)
    {
       
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        TargetEvent?.Invoke();
    }

    public void OnCancelTarget(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        CancelTargetEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (IsHealing) { return; }
        if (context.performed)
        {
            IsAttacking = true;
        }
        else
        {
            IsAttacking = false;
        }
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        if (MovementValue == Vector2.zero) return;
        if (IsHealing) { return; }
        if (context.performed)
        {
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(IsShop){return;}
        if(IsInteracting || IsNPCInteracting)
        {
            if(!context.performed){return;}
            InteractEvent?.Invoke();
        }
    }

    public void OnRuneAttack(InputAction.CallbackContext context)
    {
        if (IsHealing) { return; }
        if (CanRuneAttack && !IsInRuneAttack)
        {
            if(!context.performed){return;}
            RuneAttackEvent?.Invoke();
        }
    }

    public void OnEquip(InputAction.CallbackContext context)
    {
        if(IsDisarming) { return; }
        if (IsHealing) { return; }
        if (CustomSellerManager.Instance.isOpen){return;}
        if(MenuManager.Instance.isPaused){return;}
        if(!IsEquipped && CanDisarm)
        {
            if(!context.performed){return;}
            EquipEvent?.Invoke();
            IsEquipped = true;
        }
    }

    public void OnDisarm(InputAction.CallbackContext context)
    {
        if (IsEquiping) {  return; }
        if (IsHealing) { return; }
        if (CustomSellerManager.Instance.isOpen){return;}
        if(MenuManager.Instance.isPaused){return;}
        if(IsEquipped && CanDisarm)
        {
            if(!context.performed){return;}
            DisarmEvent?.Invoke();
            IsEquipped = false;
        }
    }

    public void OnInventoryOpenClose(InputAction.CallbackContext context)
    {
        
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (IsHealing) { return; }
        if (IsAttacking) { return; }
        if (CustomSellerManager.Instance.isOpen) { return; }
        if (MenuManager.Instance.isPaused) { return; }
        if (CanHeal && !IsInRuneAttack)
        {
            if (!context.performed) { return; }
            HealEvent?.Invoke();
        }
    }
}
