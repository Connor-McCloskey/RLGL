using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Vars
    private RLGLActions _actions;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private CharacterController _charController;
    private Transform _camTransform;
    private float _playerSpeed = 5.0f;
    private float _cameraSensitivity = 0.5f;
    private float _yVelocity = 0f;
    private bool _movementEnabled = false;
    private float _pitch;
    private const float Gravity = 9.81f;
    private const float MoveMargin = 0.05f;
    
    private GameObject _pauseMenu;
    
    private bool _paused = false;

    public GameObject PauseMenuPrefab;

    public event Action OnPlayerMoved;
    #endregion
    
    #region Methods
    public void EnableMovement()
    {
        _movementEnabled = true;
    }

    public void DisableMovement()
    {
        _movementEnabled = false;
    }
    
    private void Awake()
    {
        _actions = new RLGLActions();
        _charController = GetComponent<CharacterController>();
        if (Camera.main)
        {
            _camTransform = Camera.main.transform;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _moveAction = _actions.Player.Move;
        _moveAction.Enable();
        
        _lookAction = _actions.Player.Look;
        _lookAction.Enable();

        _actions.Player.Pause.performed += OnPaused;
        _actions.Player.Pause.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
    }

    private void OnPaused(InputAction.CallbackContext ctx)
    {
        _paused = !_paused;
        Time.timeScale = _paused ? 0f : 1f;

        if (_paused)
        {
            Cursor.lockState = CursorLockMode.None;
            _pauseMenu = Instantiate(PauseMenuPrefab, transform.position, Quaternion.identity);
            // Add PauseMenu to screen
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Destroy(_pauseMenu);
            // Remove PauseMenu
        }
    }
    
    private bool PlayerMovedEnough(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            return false;
        }
        
        float moveX = Mathf.Abs(input.x - MoveMargin);
        float moveY = Mathf.Abs(input.y - MoveMargin);

        if (moveX > 0 || moveY > 0)
        {
            return true;
        }

        return false;
    }
    
    private void MovePlayer(float dt)
    {
        if (!_charController.isGrounded)
        {
            _yVelocity -= Gravity * dt;
        }
        
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        moveDir = transform.TransformDirection(moveDir);
        moveDir *= _playerSpeed;
        
        moveDir.y = _yVelocity;
        
        _charController.Move(moveDir * dt);

        if (PlayerMovedEnough(moveInput))
        {
            OnPlayerMoved?.Invoke();
        }
    }

    private void RotatePlayer(float dt)
    {
        Vector2 lookDir = _lookAction.ReadValue<Vector2>();
        
        float yaw = lookDir.x * _cameraSensitivity;
        transform.Rotate(0f, yaw, 0f);
        
        _pitch -= lookDir.y * _cameraSensitivity;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        _camTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
    
    private void Update()
    {
        float dt =  Time.deltaTime;

        if (_movementEnabled)
        {
            MovePlayer(dt);
        }
        RotatePlayer(dt);
    }
    #endregion
}
