using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : IState
{
    private Player _player;
    private Rigidbody2D _rigibody;
    private IPlayerAnimation _playerAnimation;
    private IPlayerInput _playerInput;
    private IPlayerMoveable _playerMovement;

    public PlayerJumpState(Player player)
    {
        this._player = player;
    }

    public void EnterState()
    {
        _rigibody = _player.GetComponent<Rigidbody2D>();
        _playerAnimation = _player.GetComponent<PlayerAnimation>();
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerMovement = _player.GetComponent<PlayerMovement>();

        IPlayerAnimationStrategy jump = new JumpAnimationStrategy(true);
        _playerAnimation.PlayAnimation(jump);
        _playerMovement.Jump();
        _playerMovement.IsDoubleJump = true;
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {
        // Transition to fall state
        if (_rigibody.velocity.y < -0.1f)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.fallState);
        }

        // Transition to double jump state
        if (_playerMovement.IsDoubleJump && !_playerMovement.IsWallSliding && _playerInput.GetJumpKeyDown)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.doubleJumpState);
        }

        // Transition to wall slide state
        if (!_playerMovement.IsGround && _playerMovement.IsWallSliding)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.wallSlideState);
        }
    }

    public void FixedUpdateState()
    {
        _playerMovement.Move();
    }
}
