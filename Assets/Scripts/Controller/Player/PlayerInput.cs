using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerInput
{
    float Horizontal { get; }
    bool GetJumpKeyDown { get; set; }
    void EnableInput();
    void DisableInput();
}

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    [SerializeField] private bool _inputEnabled;

    private PlayerControls _playerControls;

    public float Horizontal { get; private set; }
    public bool GetJumpKeyDown { get; set; }

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();

        _playerControls.Player.Move.performed += ctx => Horizontal = ctx.ReadValue<float>();
        _playerControls.Player.Jump.started += ctx => GetJumpKeyDown = true;
        _playerControls.Player.Jump.canceled += ctx => GetJumpKeyDown = false;
    }

    private void Start()
    {
        _inputEnabled = true;
    }

    private void Update()
    {
        InputHandle();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    /// <summary>
    /// Handle the player input
    /// </summary>
    private void InputHandle()
    {
        if (_inputEnabled)
        {
            
        }
        else
        {
            Horizontal = 0;
            GetJumpKeyDown = false;
        }
    }

    /// <summary>
    /// Enable the player input
    /// </summary>
    public void EnableInput()
    {
        _inputEnabled = true;
    }

    /// <summary>
    /// Disable the player input
    /// </summary>
    public void DisableInput()
    {
        _inputEnabled = false;
    }
}
