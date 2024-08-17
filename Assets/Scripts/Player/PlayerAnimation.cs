using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private PlayerInput _playerInput;
    private PlayerJump _playerJump;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _playerJump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        HandleAnimation();
    }

    // Handle the player animations
    private void HandleAnimation()
    {
        if (_playerInput.GetMoveDirection() != 0)
        {
            RunAnimation();
        }
        else
        {
            IdleAnimation();
        }

        if (_rigidbody2D.velocity.y > 0 && !_playerJump.IsGrounded)
        {
            JumpAnimation(true);
        }
        else
        {
           JumpAnimation(false);
        }

        if (_rigidbody2D.velocity.y < 0 && !_playerJump.IsGrounded)
        {
            FallAnimation(true);
        }
        else
        {
            FallAnimation(false);
        }
    }

    // Set animator params to play the player idle animation
    public void IdleAnimation()
    {
        _animator.SetBool("isRun", false);
    }

    // Set animator params to play the player run animation
    public void RunAnimation()
    {
        _animator.SetBool("isRun", true);
    }

    // Set animator params to play the player jump animation
    public void JumpAnimation(bool value)
    {
        _animator.SetBool("isJump", value);
    }

    // Set animator params to play the player fall animation
    public void FallAnimation(bool value)
    {
        _animator.SetBool("isFall", value);
    }
}
