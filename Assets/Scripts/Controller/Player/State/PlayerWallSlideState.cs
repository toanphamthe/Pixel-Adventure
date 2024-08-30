using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : IState
{
    private Player _player;
    private Rigidbody2D _rigidbody;
    private IPlayerMoveable _playerMovement;
    private IPlayerAnimation _playerAnimation;
    private IPlayerInput _playerInput;

    public PlayerWallSlideState(Player player)
    {
        this._player = player;
    }

    public void EnterState()
    {
        _rigidbody = _player.GetComponent<Rigidbody2D>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _playerAnimation = _player.GetComponent<PlayerAnimation>();
        _playerInput = _player.GetComponent<PlayerInput>();

        // Play jump animation
        IPlayerAnimationStrategy jump = new JumpAnimationStrategy(false);
        _playerAnimation.PlayAnimation(jump);

        _playerMovement.IsDoubleJump = false;
    }

    public void ExitState()
    {
        // Stop wall slide animation
        IPlayerAnimationStrategy wallSlide = new WallSlidingAnimationStrategy(false);
        _playerAnimation.PlayAnimation(wallSlide);
    }

    public void UpdateState()
    {
        if (!_playerMovement.IsWallJumping)
        {
            _playerMovement.Move();
        }
        else
        {
            _playerMovement.StopMove();
        }

        if (_rigidbody.velocity.y < -0.1f)
        {
            // Play fall animation
            IPlayerAnimationStrategy wallSlide = new WallSlidingAnimationStrategy(true);
            _playerAnimation.PlayAnimation(wallSlide);
        }
        else
        {
            // Stop wall slide animation
            IPlayerAnimationStrategy wallSlide = new WallSlidingAnimationStrategy(false);
            _playerAnimation.PlayAnimation(wallSlide);
        }

        if (_playerMovement.IsWallSliding && _playerInput.GetJumpKeyDown && !_playerMovement.IsWallJumping)
        {
            _playerMovement.WallJump();

            // Play jump animation
            IPlayerAnimationStrategy jump = new JumpAnimationStrategy(true);
            _playerAnimation.PlayAnimation(jump);
        }

        if (_playerMovement.IsWallSliding && !_playerMovement.IsGround)
        {
            _playerMovement.WallSlide();
        }

        if (_playerInput.GetJumpKeyDown && _playerMovement.IsDoubleJump)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.doubleJumpState);
        }

        if (_playerInput.Horizontal != 0 && _playerMovement.IsGround)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.walkState);
        }
        else if (_playerInput.Horizontal == 0 && _playerMovement.IsGround)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.idleState);
        }

        if (!_playerMovement.IsGround && _rigidbody.velocity.y < -0.1f && !_playerMovement.IsWallSliding)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.fallState);
        }
    }

    public void FixedUpdateState()
    {

    }
}
