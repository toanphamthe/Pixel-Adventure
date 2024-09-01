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
    void StopMove();
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
    [SerializeField] private GameObject _leftGroundCheckObject;
    [SerializeField] private GameObject _rightGroundCheckObject;
    [SerializeField] private bool _drawGizmos;

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
        _rigidbody2D.velocity = new Vector2(_playerInput.Horizontal * _moveSpeed, _rigidbody2D.velocity.y);
        Flip();
    }

    /// <summary>
    /// Stop the player movement
    /// </summary>
    public void StopMove()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y);
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

    /// <summary>
    /// Handle the wall jump effect
    /// </summary>
    public void WallJump()
    {
        IsDoubleJump = false;
        _isWallJumping = true;
        _wallJumpDirection = -transform.localScale.x;
        _rigidbody2D.velocity = new Vector2(_wallJumpDirection * _wallJumpForce, _wallJumpForce * _forceMultipier);
        Invoke(nameof(ResetWallJump), 0.2f);
    }

    /// <summary>
    /// Make the player can jump again
    /// </summary>
    private void ResetWallJump()
    {
        _isWallJumping = false;
        IsDoubleJump = true;
    }

    /// <summary>
    /// Handle the wall slide effect
    /// </summary>
    /// <param name="horizontal"></param>
    public void WallSlide()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Mathf.Clamp(_rigidbody2D.velocity.y, -_wallSlidingSpeed, float.MaxValue));
    }

    /// <summary>
    /// Check if the player is touching the wall
    /// </summary>
    private void IsWalled()
    {
        _isWallSliding = Physics2D.OverlapCircle(_wallCheck.transform.position, _wallCheckRadius, _wallLayer);

    }

    /// <summary>
    /// Check if the player is touching the ground
    /// </summary>
    private void IsGrounded()
    {
        RaycastHit2D left = Physics2D.Raycast(_leftGroundCheckObject.transform.position, Vector2.down, _groundCheckDistance, _groundLayer);
        RaycastHit2D right = Physics2D.Raycast(_rightGroundCheckObject.transform.position, Vector2.down, _groundCheckDistance, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * _groundCheckDistance, Color.green);
        if (left.collider != null || right.collider != null) 
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
        if (_drawGizmos)
        {
            // Wall check
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_wallCheck.transform.position, _wallCheckRadius);
            Gizmos.DrawRay(_leftGroundCheckObject.transform.position, Vector3.down * _groundCheckDistance);
            Gizmos.DrawRay(_rightGroundCheckObject.transform.position, Vector3.down * _groundCheckDistance);
        }
    }
#endif
}
