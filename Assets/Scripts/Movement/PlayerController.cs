using UnityEngine;
using Waygroup;

/// <summary>
/// Controls the player's movement and camera.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;
    [SerializeField] private float UpperLimit = -40f;
    [SerializeField] private float BottomLimit = 70f;
    [SerializeField] private float MouseSensitivity = 21.9f;

    [Header("Ground Settings")]
    [SerializeField] private LayerMask groundLayer;

    // Flag to determine if can move or not
    public bool CanMove { get; set; } = true;

    [Header("Position Debug")]
    public Vector3 position;

    // Rerences to manage logic
    private CharacterController _characterController;
    private InputManager _inputManager;
    private Animator _animator;
    private bool _hasAnimator;
    private int _xVelHash;
    private int _yVelHash;

    // Camera Variables
    private float _xRotation;

    // Movement Velocity
    private const float _walkSpeed = 2f;
    private const float _runSpeed = 4f;

    private void Start()
    {
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        _characterController = GetComponent<CharacterController>();
        _inputManager = GetComponent<InputManager>();

        _xVelHash = Animator.StringToHash("XVelocity");
        _yVelHash = Animator.StringToHash("YVelocity");
    }

    private void FixedUpdate()
    {
        position = transform.position;
        if (GetComponent<ForceReceiver>().isGrounded)
        {
            Move();
        }
        else
        {
            CanMove = false;
            _animator.SetBool("IsRunning", false);
            Falling();
        }
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

    /// <summary>
    /// Moves the player character based on input.
    /// </summary>
    private void Move()
    {
        if (MenuManager.Instance.isPaused)
        {
            return;
        }
        if (!_hasAnimator) { return; }
        if (CanMove)
        {
            _inputManager.CanRun = true;
            _animator.SetBool("IsRunning", _inputManager.IsRunning);
            float targetSpeed = _walkSpeed;
            if (_inputManager.IsRunning)
            {
                targetSpeed = _runSpeed;
            }
            if (_inputManager.Move == Vector2.zero)
            {
                _animator.SetFloat("speed", 0f);
                _animator.SetBool("IsRunning", false);
            }

            Vector3 moveDirection = transform.TransformDirection(new Vector3(_inputManager.Move.x, 0, _inputManager.Move.y));
            moveDirection *= targetSpeed;

            _characterController.Move(moveDirection * Time.deltaTime);

            _animator.SetFloat("speed", targetSpeed, 0.1f, Time.fixedDeltaTime);

            _animator.SetFloat(_xVelHash, _inputManager.Move.x, 0.1f, Time.fixedDeltaTime);
            _animator.SetFloat(_yVelHash, _inputManager.Move.y, 0.1f, Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Stops the player's movement.
    /// </summary>
    public void Stop()
    {
        Vector3 moveDirection = transform.TransformDirection(new Vector3(_inputManager.Move.x, 0, _inputManager.Move.y));
        moveDirection *= 0;

        _characterController.Move(moveDirection * Time.deltaTime);

        _animator.SetFloat("speed", 0f);

        _animator.SetFloat(_xVelHash, 0f);
        _animator.SetFloat(_yVelHash, 0f);

        _animator.SetBool("IsRunning", false);
    }

    /// <summary>
    /// Handles camera movement based on input.
    /// </summary>
    private void CameraMovement()
    {
        if (!_hasAnimator) { return; }
        var Mouse_X = _inputManager.Look.x;
        var Mouse_Y = _inputManager.Look.y;

        Camera.position = CameraRoot.position;

        _xRotation -= Mouse_Y * MouseSensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

        Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.Rotate(Vector3.up, Mouse_X * MouseSensitivity * Time.deltaTime);
    }

    /// <summary>
    /// Handles falling when the player is not grounded.
    /// </summary>
    private void Falling()
    {
        _inputManager.CanRun = false;
        Vector3 momentum = _characterController.velocity;
        momentum.y = 0f;
        _characterController.Move(momentum + GetComponent<ForceReceiver>().Movement * Time.deltaTime);
    }
}
