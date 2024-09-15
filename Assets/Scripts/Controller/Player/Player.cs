using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    private IPlayerMoveable _playerMovement;
    private IPlayerAttack _playerAttack;
    private IPlayerDamageable _playerTakeDamage;
    private IPlayerCollectable _collectable;
    private Diamond _diamond;

    public PlayerStateMachine playerStateMachine;

    public ParticleSystem dustPS;
    public ParticleSystem fallPS;
    public ParticleSystem jumpPS;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerTakeDamage = GetComponent<PlayerTakeDamage>();
        _diamond = GetComponent<Diamond>();
    }

    private void Start()
    {
        playerStateMachine = new PlayerStateMachine(this);

        if (playerStateMachine != null)
        {
            IState state = new PlayerIdleState(this);
            playerStateMachine.Initialize(state);
        }
    }

    private void Update()
    {
        playerStateMachine.Update();
    }

    private void FixedUpdate()
    {
        playerStateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy collision
        if (collision.gameObject.CompareTag("Enemy") && _playerAttack.IsOnTopOfEnemy && !_playerTakeDamage.IsDead)
        {
            IEnemyDie enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
            _playerMovement.Jump();
        }
        else if (collision.gameObject.CompareTag("Enemy") && !_playerAttack.IsOnTopOfEnemy)
        {
            playerStateMachine.TransitionTo(playerStateMachine.takeDamageState);
        }

        // Trap collision
        if (collision.gameObject.CompareTag("Trap"))
        {
            playerStateMachine.TransitionTo(playerStateMachine.takeDamageState);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Fruit trigger
        if (collision.gameObject.CompareTag("Fruit"))
        {
            _collectable = collision.gameObject.GetComponent<FruitController>();
            if (_collectable != null)
            {
                _diamond.Increment(_collectable.Collect());
            }
        }
    }
}
