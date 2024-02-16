using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using RPG.Combat;
using RPG.Saving;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField]
    public bool isInteracting { get; set; }
    [field: SerializeField]
    public HealManager HealManager { get; set; }
    [field: SerializeField] 
    public bool IsNearNPC {get;  set;}
    [field: SerializeField] 
    public RuneManager RuneManager {get; private set;}
    [field: SerializeField] 
    public Armory Armory {get; private set;}
    [field: SerializeField] 
    public MusicHandler MusicHandler {get; private set;}
    [field: SerializeField] 
    public float JumpForce {get; private set;}
    [field: SerializeField] 
    public float DodgeDistance {get; private set;}
    [field: SerializeField] 
    public float DodgeDuration {get; private set;}
    [field: SerializeField] 
    public List<GameObject> WeaponsLogics {get; private set;}
    [field: SerializeField] 
    public Health Health {get; private set;}
    
    [field: SerializeField]
    public ForceReceiver ForceReceiver {get; private set;}

    [field: SerializeField]
    public CharacterController CharacterController {get; private set;}

    [field: SerializeField]
    public float TargetingMovementSpeed {get; private set;}

    [field: SerializeField]
    public InputReader InputReader {get; private set;}

    [field: SerializeField]
    public float FreeLookMovementSpeed {get;  set;}

    [field: SerializeField]
    public float RunningMovementSpeed {get; private set;}
    [field: SerializeField]
    public float WalkingMovementSpeed {get; private set;}

    [field: SerializeField]
    public Animator Animator {get; private set;}

    [field: SerializeField]
    public float RotationDamping {get; private set;}

    [field: SerializeField]
    public Targeter Targeter {get; private set;}

    [field: SerializeField]
    public Attack[] Attacks {get; private set;}
    public Transform MainCameraTransform {get; private set;}
    public float PreviousDodgeTime {get; private set;} = Mathf.NegativeInfinity;

    private void Awake() 
    {
        MainCameraTransform = Camera.main.transform;
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
