using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    private int _currentHealth;
    private float _currentSpeed;
    private float _currentJumpForce;

    private Enemy _enemy;
    private Rigidbody2D _rigidbody2D;

    private IPlayerAnimation _playerAnimation;
    private IPlayerInput _playerInput;
    private IPlayerMovement _playerMovement;
    private IPlayerAttack _playerAttack;
    private IPlayerTakeDamage _playerTakeDamage;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerTakeDamage = GetComponent<PlayerTakeDamage>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _currentHealth = _health;
        _currentSpeed = _moveSpeed;
        _currentJumpForce = _jumpForce;
    }

    private void Update()
    {
        Animation();

        if (_rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
        {
            Movement();
            Jump();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _playerAttack.IsOnTopOfEnemy)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
            _playerMovement.Jump(_currentJumpForce);
        }
        else if (collision.gameObject.CompareTag("Enemy") && !_playerAttack.IsOnTopOfEnemy)
        {
            if (_currentHealth > 0)
            {
                _playerTakeDamage.TakeDamage(_currentHealth, 1);
            }
            else
            {
                _playerTakeDamage.Die();
            }
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            //TakeDamage(1);
        }
    }

    public void Movement()
    {
        _playerMovement.Move(_playerInput.Horizontal, _currentSpeed);
    }

    public void Jump()
    {
        if (_playerInput.GetJumpKeyDown)
        {
            _playerMovement.Jump(_currentJumpForce);
        }
    }

    /// <summary>
    /// Handle the player animation based on the player's movement
    /// </summary>
    public void Animation()
    {
        if (_playerInput.Horizontal != 0)
        {
            IPlayerAnimationStrategy runAnimation = new RunAnimationStrategy(true);
            _playerAnimation.PlayAnimation(runAnimation);
        }
        else
        {
            IPlayerAnimationStrategy runAnimation = new RunAnimationStrategy(false);
            _playerAnimation.PlayAnimation(runAnimation);
        }

        if (_rigidbody2D.velocity.y > 0.01f)
        {
            IPlayerAnimationStrategy jumpAnimation = new JumpAnimationStrategy(true);
            _playerAnimation.PlayAnimation(jumpAnimation);
        }
        else
        {
            IPlayerAnimationStrategy jumpAnimation = new JumpAnimationStrategy(false);
            _playerAnimation.PlayAnimation(jumpAnimation);
        }

        if (_rigidbody2D.velocity.y < -0.01f)
        {
            IPlayerAnimationStrategy fallAnimation = new FallAnimationStrategy(true);
            _playerAnimation.PlayAnimation(fallAnimation);
        }
        else
        {
            IPlayerAnimationStrategy fallAnimation = new FallAnimationStrategy(false);
            _playerAnimation.PlayAnimation(fallAnimation);
        }
    }    
}