using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : IState
{
    private Player _player;
    private Rigidbody2D _rigidbody;
    private IPlayerAnimation _playerAnimation;
    private IPlayerInput _playerInput;
    private IPlayerMoveable _playerMovement;

    public PlayerWalkState(Player player)
    {
        this._player = player;
    }

    public void EnterState()
    {
        _rigidbody = _player.GetComponent<Rigidbody2D>();
        _playerAnimation = _player.GetComponent<PlayerAnimation>();
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerMovement = _player.GetComponent<PlayerMovement>();

        // Play walk animation
        IPlayerAnimationStrategy walk = new RunAnimationStrategy(true);
        _playerAnimation.PlayAnimation(walk);
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {
        // Transition to idle state
        if (_playerInput.Horizontal == 0 && _playerMovement.IsGround)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.idleState);
        }

        // Transition to jump state
        if (_playerInput.GetJumpKeyDown && _playerMovement.IsGround)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.jumpState);
        }

        // Transition to fall state
        if (!_playerMovement.IsGround && _rigidbody.velocity.y < -0.1f)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.fallState);
        }
    }

    public void FixedUpdateState()
    {
        _playerMovement.Move();
    }
}
