using UnityEngine;

/// <summary>
/// Manages the state transitions and behavior of the player character.
/// Inherits from the StateMachine base class.
/// </summary>
public class PlayerStateMachine : StateMachine
{
    [field: SerializeField]
    public PlayerController PlayerController { get; private set; } 
    [field: SerializeField]
    public Health Health { get; private set; } 

    [field: SerializeField]
    public ForceReceiver ForceReceiver { get; private set; } 

    [field: SerializeField]
    public CharacterController CharacterController { get; private set; } 

    [field: SerializeField]
    public Animator Animator { get; private set; } 

    [field: SerializeField]
    public float RotationDamping { get; private set; } 
    public Transform MainCameraTransform { get; private set; } 

    /// <summary>
    /// Initializes the player state machine with the initial state.
    /// </summary>
    private void Awake()
    {
        SwitchState(new PlayerFreeLookState(this));    
    }

    /// <summary>
    /// Subscribes to events for taking damage and dying when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage; 
        Health.OnDie += HandleDie; 
    }

    /// <summary>
    /// Unsubscribes from events when the object is disabled.
    /// </summary>
    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage; 
        Health.OnDie -= HandleDie; 
    }

    /// <summary>
    /// Handles the event when the player takes damage by switching to the impact state.
    /// </summary>
    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this)); 
    }

    /// <summary>
    /// Handles the event when the player dies by switching to the dead state.
    /// </summary>
    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this)); 
    }
}
