using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : IState
{
    private Player _player;
    private Rigidbody2D _rigibody;

    private IPlayerAnimation _playerAnimation;
    private IPlayerInput _playerInput;
    private IPlayerMoveable _playerMovement;

    public PlayerFallState(Player player)
    {
        this._player = player;
    }

    public void EnterState()
    {
        _rigibody = _player.GetComponent<Rigidbody2D>();

        _playerAnimation = _player.GetComponent<PlayerAnimation>();
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerMovement = _player.GetComponent<PlayerMovement>();

        // Play fall animation
        IPlayerAnimationStrategy fall = new FallAnimationStrategy(true);
        _playerAnimation.PlayAnimation(fall);

        // Stop jump animation
        IPlayerAnimationStrategy jump = new JumpAnimationStrategy(false);
        _playerAnimation.PlayAnimation(jump);
    }

    public void ExitState()
    {
        // Stop fall animation
        IPlayerAnimationStrategy fall = new FallAnimationStrategy(false);
        _playerAnimation.PlayAnimation(fall);

        _playerMovement.IsDoubleJump = false;
    }

    public void UpdateState()
    {
        // Transition to idle state
        if (_playerMovement.IsGrounded && _playerInput.Horizontal == 0)
        {
            _player.fallPS.Play();
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.idleState);
        }
        // Transition to walk state
        else if (_playerMovement.IsGrounded && _playerInput.Horizontal != 0)
        {
            _player.fallPS.Play();
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.walkState);
        }

        // Transition to double jump state
        if (_playerMovement.IsDoubleJump && _playerInput.GetJumpKeyDown)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.doubleJumpState);

        }
        // Transition to wall slide state
        if (!_playerMovement.IsGrounded && _playerMovement.IsWallSliding)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.wallSlideState);
        }
    }

    public void FixedUpdateState()
    {
        _playerMovement.Move();
    }
}
