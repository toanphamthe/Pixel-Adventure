using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [SerializeField] private int _totalPoint;

    private IPlayerMoveable _playerMovement;
    private IPlayerAttack _playerAttack;
    private IPlayerDamageable _playerTakeDamage;

    public PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerTakeDamage = GetComponent<PlayerTakeDamage>();
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
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
            _playerMovement.Jump();
        }
        else if (collision.gameObject.CompareTag("Enemy") && !_playerAttack.IsOnTopOfEnemy)
        {
            playerStateMachine.TransitionTo(playerStateMachine.dieState);
        }

        // Trap collision
        if (collision.gameObject.CompareTag("Trap"))
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Fruit trigger
        if (collision.gameObject.CompareTag("Fruit"))
        {
            FruitController fruit = collision.gameObject.GetComponent<FruitController>();
            if (fruit != null)
            {
                _totalPoint += fruit.Collect();
            }
        }
    }
}
