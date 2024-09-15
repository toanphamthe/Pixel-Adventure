using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : IState
{
    private Player _player;
    private Rigidbody2D _rigidbody;

    private IPlayerAnimation _playerAnimation;
    private IPlayerInput _playerInput;
    private IPlayerMoveable _playerMovement;

    public PlayerDoubleJumpState(Player player)
    {
        this._player = player;
    }

    public void EnterState()
    {
        _rigidbody = _player.GetComponent<Rigidbody2D>();

        _playerAnimation = _player.GetComponent<PlayerAnimation>();
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerMovement = _player.GetComponent<PlayerMovement>();

        _playerMovement.Jump();

        IPlayerAnimationStrategy doubleJump = new DoubleJumpAnimationStrategy();
        _playerAnimation.PlayAnimation(doubleJump);

        _playerMovement.IsDoubleJump = false;
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {
        // Transition to fall state
        if (_rigidbody.velocity.y < -0.1f)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.fallState);
        }

        // Transition to jump state
        if (_rigidbody.velocity.y > 0.1f)
        {
            IPlayerAnimationStrategy jump = new JumpAnimationStrategy(true);
            _playerAnimation.PlayAnimation(jump);
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
