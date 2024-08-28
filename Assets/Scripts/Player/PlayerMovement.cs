using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovement
{
    bool IsWallSliding { get; }
    bool IsDoubleJump { get; set; }
    bool IsGround { get; }
    void Move(float horizontal, float moveSpeed);
    void Jump(float jumpForce);
    void WallJump();
    void WallSlide();
}

public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    [Header("Ground Check")]
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private bool _isGround;
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
    private IPlayerTakeDamage _playerTakeDamage;

    public bool IsWallSliding => _isWallSliding;
    public bool IsGround => _isGround;
    public bool IsDoubleJump { get; set; }

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
        if (!_playerTakeDamage.IsKnockBack && !_isWallJumping)
        {
            _rigidbody2D.velocity = new Vector2(horizontal * moveSpeed, _rigidbody2D.velocity.y);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y);
        }
        Flip(horizontal);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="horizontal"></param>

    private void Flip(float horizontal)
    {
        if (horizontal > 0.01f)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (horizontal < -0.01f)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        
    }

    /// <summary>
    /// Jump the player with a jump force
    /// </summary>
    /// <param name="jumpForce">Jump force</param>
    public void Jump(float jumpForce)
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
    }

    public void WallJump()
    {
        // Wall Jump
        if (IsWallSliding)
        {
            IsDoubleJump = false;
            _isWallJumping = true;
            _wallJumpDirection = -transform.localScale.x;
            _rigidbody2D.velocity = new Vector2(_wallJumpDirection * _wallJumpForce, _wallJumpForce * _forceMultipier);
            Invoke("ResetWallJump", 0.2f);
        }
    }

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
        if (IsWalled() && !_isGround)
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

    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(_wallCheck.transform.position, _wallCheckRadius, _wallLayer);
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Wall check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_wallCheck.transform.position, _wallCheckRadius);
    }
#endif
}
