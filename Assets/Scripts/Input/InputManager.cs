using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Waygroup
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        [SerializeField] private PlayerInput playerInput;

        //READ ONLY VARIABLES
        public event Action OnInteraction;
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        [SerializeField] public bool IsRunning;
        [SerializeField] public bool IsInteracting;
        public bool InventoryOpenCloseInput { get; private set; }
        public bool CanRun = false;
        public float throwTime = 0;

        //INPUT ACTIONS
        public InputActionMap _currentMap;
        public InputAction _moveAction;
        public InputAction _lookAction;
        public InputAction _runAction;
        public InputAction _inventoryAction;
        public InputAction _interactAction;
        public InputAction _throwAction;
        public InputAction _throwReleaseAction;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            HideCursor();
            _currentMap = playerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");
            _interactAction = _currentMap.FindAction("Interact");
            _inventoryAction = _currentMap.FindAction("InventoryOpenClose");
            _throwAction = _currentMap.FindAction("Throw");
            _throwReleaseAction = _currentMap.FindAction("ThrowRelease");
            

            _moveAction.performed += OnMove;
            _lookAction.performed += OnLook;
            _runAction.performed += OnRun;
            _interactAction.performed += OnInteract;
            _throwAction.performed += OnThrow;
            _throwReleaseAction.performed += OnThrowRelease;

            _moveAction.canceled += OnMove;
            _lookAction.canceled += OnLook;
            _runAction.canceled += OnWalk;
            _interactAction.canceled += OnNoInteract;
        }
        private void Update()
        {
            InventoryOpenCloseInput = _inventoryAction.WasPressedThisFrame();
            throwTime = _throwAction.GetTimeoutCompletionPercentage();
        }
        private void OnInventory(InputAction.CallbackContext context)
        {
            
        }
        private void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (MenuManager.Instance.isPaused) { return; }
            Move = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            if (MenuManager.Instance.isPaused) { return; }
            Look = context.ReadValue<Vector2>();
        }
        private void OnRun(InputAction.CallbackContext context)
        {
            if (MenuManager.Instance.isPaused) { return; }
            if (CanRun == true)
            {
                IsRunning = true;
            }
        }
        private void OnWalk(InputAction.CallbackContext context)
        {
            if(MenuManager.Instance.isPaused) { return; }
            IsRunning = false;
        }
        private void OnInteract(InputAction.CallbackContext context)
        {
            if(InteractionManager.Instance.canInteract == true)
            {
                IsInteracting = true;
            }
        }
        private void OnNoInteract(InputAction.CallbackContext context)
        {
            if (InteractionManager.Instance.canInteract == true)
            {
                IsInteracting = false;
            }
        }
        private void OnThrow(InputAction.CallbackContext context)
        {
            
        }
        private void OnThrowRelease(InputAction.CallbackContext context)
        {
            ThrowManager.Instance.Throw(throwTime);
        }
        private void OnEnable()
        {
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            _currentMap.Disable();
        }
    }
}

