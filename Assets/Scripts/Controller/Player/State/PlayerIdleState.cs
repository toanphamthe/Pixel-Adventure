using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState
{
    private Player _player;
    private Rigidbody2D _rigidbody;
    private IPlayerAnimation _playerAnimation;
    private IPlayerInput _playerInput;
    private IPlayerMoveable _playerMovement;

    public PlayerIdleState(Player player)
    {
        this._player = player;
    }

    public void EnterState()
    {
        _rigidbody = _player.GetComponent<Rigidbody2D>();
        _playerAnimation = _player.GetComponent<PlayerAnimation>();
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerMovement = _player.GetComponent<PlayerMovement>();

        // Play idle animation
        IPlayerAnimationStrategy idle = new RunAnimationStrategy(false);
        _playerAnimation.PlayAnimation(idle);
    }

    public void ExitState()
    {
        
    }

    public void UpdateState()
    {
        // Transition to walk state
        if (_playerInput.Horizontal != 0)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.walkState);
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

    }
}
