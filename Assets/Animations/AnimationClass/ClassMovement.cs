using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private InputReaderClass inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private CharacterController characterController;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 0.5f;
    [SerializeField] private float turningRate = 30f;

    private Vector2 previousMovementInput;

    private const float AnimatorDampTime = 0.1f;

    private const float minInputThreshold = 0.1f;

    private enum PlayerState
    {
        Idle,
        IdleBreaker,
        Walk,
        Run
    }
    public void OnEnable()
    {
        inputReader.MoveEvent += HandleMovement;
        inputReader.TauntEvent += Taunt;
    }

    public void OnDisable()
    {
        inputReader.MoveEvent -= HandleMovement;
        inputReader.TauntEvent -= Taunt;
    }

    private PlayerState currentState;
    private void Update()
    {
        if(currentState == PlayerState.IdleBreaker){return;}
        float horizontalInput = previousMovementInput.x;
        float verticalInput = previousMovementInput.y;
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput);
        if (inputDirection.magnitude > minInputThreshold)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection.normalized, Vector3.up);
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, targetRotation, turningRate * Time.deltaTime);
            HandleAnimationServerRPC(0.5f);
            currentState = PlayerState.Walk;
            if(inputReader.IsRunning)
            {
                currentState = PlayerState.Run;
                HandleAnimationServerRPC(2f);
            }
        }
        else
        {
            HandleAnimationServerRPC(0f);
            currentState = PlayerState.Idle;
        }
        if(inputReader.IsRunning)
        {
            currentState = PlayerState.Run;
        }
    }

    private void FixedUpdate()
    {
        if(currentState == PlayerState.IdleBreaker){return;}
        if(inputReader.IsRunning)
        {
            movementSpeed = 3f;
        }
        else
        {
            movementSpeed = 1f;
        }
        Vector3 moveDirection = new Vector3(previousMovementInput.x, 0f, previousMovementInput.y);
        if (moveDirection.magnitude > minInputThreshold)
        {
            characterController.Move(moveDirection * movementSpeed * Time.fixedDeltaTime);
        }
    }
    private void HandleMovement(Vector2 movementInfo)
    {
        previousMovementInput = movementInfo;
    }
    private void HandleAnimationServerRPC(float velocity)
    {
        animator.SetFloat("speed", velocity, AnimatorDampTime, Time.deltaTime);
    }
    private void Taunt()
    {
        animator.SetTrigger("break");
        animator.ResetTrigger("noBreak");
        currentState = PlayerState.IdleBreaker;
        StartCoroutine(TauntTime());
    }
    private IEnumerator TauntTime()
    {
        yield return new WaitForSeconds(4.5f);
        currentState = PlayerState.Idle;
        animator.SetTrigger("noBreak");
        animator.ResetTrigger("break");
    }
}
