using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [SerializeField] private int _coinsCollected;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    public IPlayerAnimation _playerAnimation;
    public IPlayerInput _playerInput;
    public IPlayerMoveable _playerMovement;
    public IPlayerAttack _playerAttack;
    public IPlayerDamageable _playerTakeDamage;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerTakeDamage = GetComponent<PlayerTakeDamage>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        // Old
        AnimationHandler();
        // Movement
        if (_rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
        {
            _playerMovement.Move();

            _playerMovement.WallSlide();

            if (_playerInput.GetJumpKeyDown)
            {
                // Jump and double jump
                if (_playerMovement.IsGround)
                {
                    _playerMovement.Jump();
                    _playerMovement.IsDoubleJump = true;
                }
                else if (_playerMovement.IsDoubleJump && !_playerMovement.IsWallSliding)
                {
                    _playerMovement.Jump();
                    _playerMovement.IsDoubleJump = false;
                    _playerAnimation.PlayAnimation(new DoubleJumpAnimationStrategy());
                }

                // Wall jump
                _playerMovement.WallJump();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy collision
        if (collision.gameObject.CompareTag("Enemy") && _playerAttack.IsOnTopOfEnemy)
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
            if (_playerTakeDamage.CurrentHealth > 0)
            {
                _playerTakeDamage.Damage(1);
            }
            else
            {
                _playerTakeDamage.Die();
            }
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
                _coinsCollected += fruit.coin;
                Destroy(collision.gameObject);
            }
        }
    }

    /// <summary>
    /// Handle the player animation based on the player's movement
    /// </summary>
    public void AnimationHandler()
    {
        // Run
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

        // Jump
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

        // Fall
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

        // Wall Sliding
        if (_playerMovement.IsWallSliding && !_playerMovement.IsGround && _rigidbody2D.velocity.y < -0.01f)
        {
            IPlayerAnimationStrategy wallSlidingAnimation = new WallSlidingAnimationStrategy(true);
            _playerAnimation.PlayAnimation(wallSlidingAnimation);
        }
        else
        {
            IPlayerAnimationStrategy wallSlidingAnimation = new WallSlidingAnimationStrategy(false);
            _playerAnimation.PlayAnimation(wallSlidingAnimation);
        }
    }
}
