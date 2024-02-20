using System;
using UnityEngine;
using Waygroup;

/// <summary>
/// Manages interactions between the player and interactable objects.
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [Header("Interaction Settings")]
    [SerializeField] private Vector3 interactionRayPoint = default; 
    [SerializeField] private float interactionDistance = default; 
    [SerializeField] private LayerMask interactionLayer = 16; 
    [SerializeField] public bool canInteract = true; 

    private bool firstGrab = false; 
    public event Action OnFirstGrab; 

    [Header("Debugging")]
    [SerializeField] private Interactable currentInteractable; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        // Check if the object is being carried; if yes, return to prevent interaction
        if (ThrowManager.Instance.isBeingCarried) { return; }

        // Check if interaction is allowed, then handle interaction input and checks
        if (canInteract)
        {
            HandleInteractionInput();
            HandleInteractionCheck();
        }
    }

    /// <summary>
    /// Handles input for interactions.
    /// </summary>
    private void HandleInteractionInput()
    {
        // Check for interaction input and if there is an interactable object in focus
        if (InputManager.Instance.IsInteracting && currentInteractable != null &&
            Physics.Raycast(Camera.main.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            // If it's the first grab, invoke the event
            if (!firstGrab)
            {
                firstGrab = true;
                OnFirstGrab?.Invoke();
            }
            // Perform interaction with the current interactable object
            currentInteractable.OnInteract();
        }
    }

    /// <summary>
    /// Checks for interactable objects in focus.
    /// </summary>
    private void HandleInteractionCheck()
    {
        // Cast a ray from the camera viewport and check for interactable objects within range
        if (Physics.Raycast(Camera.main.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            // Check if the hit object is on the interaction layer and update the current interactable object
            if (hit.collider.gameObject.layer == 16 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);
                if (currentInteractable)
                {
                    currentInteractable.OnFocus();
                }
            }
            // If there is no interactable object in focus, reset the current interactable
            else if (currentInteractable)
            {
                currentInteractable.OnLoseFocus();
                currentInteractable = null;
            }
        }
        // If there is no hit, reset the current interactable
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }
}
