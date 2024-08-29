using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMoveable
{
    bool IsWallJumping { get; }
    bool IsWallSliding { get; }
    bool IsGround { get; }
    bool IsDoubleJump { get; set; }
    void Move();
    void Jump();
    void WallJump();
    void WallSlide();
    void Flip();
}

public class PlayerMovement : MonoBehaviour, IPlayerMoveable
{
    [Header("Movement Stats")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    [Header("Ground Check")]
    [SerializeField] private bool _isGround;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Wall Slide")]
    [SerializeField] private bool _isWallSliding;
    [SerializeField] private float _wallSlidingSpeed;
    [SerializeField] private GameObject _wallCheck;
    [SerializeField] private float _wallCheckRadius;
    [SerializeField] private LayerMask _wallLayer;

    [Header("Wall Jump")]
    [SerializeField] private bool _isWallJumping;
    [SerializeField] private float _wallJumpDirection;
    [SerializeField] private float _wallJumpForce;
    [SerializeField] private float _forceMultipier;

    [Header("Components")]
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private IPlayerAttack _playerAttack;
    private IPlayerDamageable _playerTakeDamage;
    private IPlayerInput _playerInput;

    public bool IsWallJumping => _isWallJumping;
    public bool IsWallSliding => _isWallSliding;
    public bool IsGround => _isGround;
    public bool IsDoubleJump { get; set; }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _playerTakeDamage = GetComponent<PlayerTakeDamage>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        IsGrounded();
        IsWalled();
    }

    /// <summary>
    /// Move the player horizontally at the certain speed and flip the sprite to left/right
    /// </summary>
    public void Move()
    {
        if (!_playerTakeDamage.IsDead && !_isWallJumping)
        {
            _rigidbody2D.velocity = new Vector2(_playerInput.Horizontal * _moveSpeed, _rigidbody2D.velocity.y);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y);
        }
        Flip();
    }

    /// <summary>
    /// Flip the player left/right based on the input
    /// </summary>
    public void Flip()
    {
        if (_playerInput.Horizontal > 0.01f && _playerInput.Horizontal != 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (_playerInput.Horizontal < -0.01f && _playerInput.Horizontal != 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }

    /// <summary>
    /// Jump the player with a jump force
    /// </summary>
    /// <param name="jumpForce">Jump force</param>
    public void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
    }

    public void WallJump()
    {
        // Wall Jump
        if (_isWallSliding)
        {
            IsDoubleJump = false;
            _isWallJumping = true;
            _wallJumpDirection = -transform.localScale.x;
            _rigidbody2D.velocity = new Vector2(_wallJumpDirection * _wallJumpForce, _wallJumpForce * _forceMultipier);
            Invoke("ResetWallJump", 0.2f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ResetWallJump()
    {
        _isWallJumping = false;
        IsDoubleJump = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="horizontal"></param>
    public void WallSlide()
    {
        if (_isWallSliding && !_isGround)
        {
            _isWallSliding = true;
            IsDoubleJump = false;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Mathf.Clamp(_rigidbody2D.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void IsWalled()
    {
        _isWallSliding = Physics2D.OverlapCircle(_wallCheck.transform.position, _wallCheckRadius, _wallLayer);

    }

    /// <summary>
    /// Check if the player is touching the ground
    /// </summary>
    private void IsGrounded()
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Wall check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_wallCheck.transform.position, _wallCheckRadius);
    }
#endif
}
