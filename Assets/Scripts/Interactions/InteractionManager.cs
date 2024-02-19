using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waygroup;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance {  get; private set; }

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = 16;
    [SerializeField] public bool canInteract = true;

    private bool firstGrab = false;
    public event Action OnFirstGrab;

    [Header("For Debugging")]
    [SerializeField] private Interactable currentInteractable;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Update()
    {
        if (ThrowManager.Instance.isBeingCarried) { return; }
        if(canInteract)
        {
            HandleInteractionInput();
            HandleInteractionCheck();
        }
    }
    private void HandleInteractionInput()
    {
        if(InputManager.Instance.IsInteracting && currentInteractable != null && Physics.Raycast(Camera.main.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            if(firstGrab == false)
            {
                firstGrab = true;
                OnFirstGrab.Invoke();
            }
            currentInteractable.OnInteract();
        }
    }
    private void HandleInteractionCheck()
    {
        if(Physics.Raycast(Camera.main.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if(hit.collider.gameObject.layer == 16 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()) )
            {
                hit.collider.TryGetComponent(out currentInteractable);
                if(currentInteractable)
                {
                    currentInteractable.OnFocus();
                }
            }
            else if(currentInteractable)
            {
                currentInteractable.OnLoseFocus();
                currentInteractable = null;
            }
        }
        else if(currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }
}
