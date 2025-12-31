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
    private bool _movement_enabled = false;
    private float _pitch;
    private const float Gravity = 9.81f;

    public event Action OnPlayerMoved;
    #endregion
    
    #region Methods
    public void EnableMovement()
    {
        _movement_enabled = true;
    }

    public void DisableMovement()
    {
        _movement_enabled = false;
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
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
    }

    private bool PlayerMovedEnough(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            return false;
        }

        float movemargin = 0.05f;
        
        float move_x = Mathf.Abs(input.x - movemargin);
        float move_y = Mathf.Abs(input.y - movemargin);

        if (move_x > 0 || move_y > 0)
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

        if (_movement_enabled)
        {
            MovePlayer(dt);
        }
        RotatePlayer(dt);
    }
    #endregion
}
