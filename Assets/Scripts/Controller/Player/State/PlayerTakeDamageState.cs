using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamageState : IState
{
    private Player _player;
    private Rigidbody2D _rigidbody;

    private IPlayerDamageable _playerTakeDamage;
    private IPlayerAnimation _playerAnimation;

    public PlayerTakeDamageState(Player player)
    {
        this._player = player;
    }

    public void EnterState()
    {
        _rigidbody = _player.GetComponent<Rigidbody2D>();
        _playerTakeDamage = _player.GetComponent<PlayerTakeDamage>();
        _playerAnimation = _player.GetComponent<PlayerAnimation>();

        _rigidbody.velocity = Vector2.zero;
        IPlayerAnimationStrategy die = new TakeDamageAnimationStrategy();
        _playerAnimation.PlayAnimation(die);
        _playerTakeDamage.Damage(1);
    }

    public void ExitState()
    {
        IPlayerAnimationStrategy apper = new AppearAnimationStrategy();
        _playerAnimation.PlayAnimation(apper);
    }

    public void FixedUpdateState()
    {

    }

    public void UpdateState()
    {
        // Transition to idle state
        if (!_playerTakeDamage.IsDead)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.idleState);
        }
    }
}
