
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField]
    public PlayerController PlayerController { get; private set; }
    [field: SerializeField] 
    public Health Health {get; private set;}
    
    [field: SerializeField]
    public ForceReceiver ForceReceiver {get; private set;}

    [field: SerializeField]
    public CharacterController CharacterController {get; private set;}

    [field: SerializeField]
    public Animator Animator {get; private set;}

    [field: SerializeField]
    public float RotationDamping {get; private set;}
    public Transform MainCameraTransform {get; private set;}
    private void Awake() 
    {
        SwitchState(new PlayerFreeLookState(this));    
    }
    private void OnEnable() 
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }
    private void OnDisable() 
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }
    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }
    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }
}
