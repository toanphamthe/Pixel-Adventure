using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovement
{
    void Move(float horizontal, float moveSpeed);
    void Jump(float jumpForce);
}

public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private bool _isGround;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isFacingRight;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private IPlayerAttack _playerAttack;
    private IPlayerTakeDamage _playerTakeDamage;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _playerTakeDamage = GetComponent<PlayerTakeDamage>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void FixedUpdate()
    {
        IsGrounded();
    }

    /// <summary>
    /// Move the player horizontally at the certain speed and flip the sprite to left/right
    /// </summary>
    /// <param name="horizontal">Horizontal input</param>
    /// <param name="moveSpeed">Movement speed</param>
    public void Move(float horizontal, float moveSpeed)
    {
        if (!_playerTakeDamage.IsKnockBack)
        {
            _rigidbody2D.velocity = new Vector2(horizontal * moveSpeed, _rigidbody2D.velocity.y);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y);
        }

        if (horizontal > 0.01f)
        {
            _isFacingRight = true;
        }
        else if (horizontal < -0.01f)
        {
            _isFacingRight = false;
        }
        _spriteRenderer.flipX = !_isFacingRight;
    }

    /// <summary>
    /// Jump the player with a jump force
    /// </summary>
    /// <param name="jumpForce">Jump force</param>
    public void Jump(float jumpForce)
    {
        if (_isGround || _playerAttack.IsOnTopOfEnemy)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        }
    }

    /// <summary>
    /// Check if the player is touching the ground
    /// </summary>
    public void IsGrounded()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * _groundCheckDistance, Color.green);
        if (ray.collider != null) 
        {
            _isGround = true;
        }
        else
        {
            _isGround = false;
        }
    }
}
