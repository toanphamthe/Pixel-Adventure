using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput
{
    float Horizontal { get; }
    bool GetJumpKeyDown { get; }
    void EnableInput();
    void DisableInput();
}

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private bool _inputEnabled;

    public float Horizontal { get; private set; }
    public bool GetJumpKeyDown { get; private set; }

    private void Start()
    {
        _inputEnabled = true;
    }

    private void Update()
    {
        InputHandle();
    }

    /// <summary>
    /// Handle the player input
    /// </summary>
    private void InputHandle()
    {
        if (_inputEnabled)
        {
            Horizontal = Input.GetAxisRaw("Horizontal");
            GetJumpKeyDown = Input.GetKeyDown(_jumpKey);
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
