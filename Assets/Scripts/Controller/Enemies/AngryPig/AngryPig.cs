using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryPig : Enemy
{
    [Header("AngryPig Stats")]
    [SerializeField] private bool _isAngry;
    [SerializeField] private float _angrySpeed;
    [SerializeField] private float _idleTime;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isFacingRight;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isIdle;

    protected override void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        if (!_isDead)
        {
            IsGrounded();
            Patrol();
        }
        else
        {
            RotateEffect();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Die()
    {
        if (_isAngry)
        {
            base.Die();
        }
        else
        {
            Hit();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        transform.Rotate(new Vector3(0, 180f, 0));
    }

    /// <summary>
    /// 
    /// </summary>
    private void Patrol()
    {
        if (_isFacingRight && !_isIdle)
        {
            _rigidbody2D.velocity = new Vector2(_moveSpeed, _rigidbody2D.velocity.y);
        }
        else if (!_isFacingRight && !_isIdle)
        {
            _rigidbody2D.velocity = new Vector2(-_moveSpeed, _rigidbody2D.velocity.y);
        }
        if (!_isGrounded && !_isIdle && !_isAngry)
        {
            StartCoroutine(Idle(_idleTime));
        }
        else if (!_isGrounded && _isAngry)
        {
            Flip();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Hit()
    {
        _moveSpeed = _angrySpeed;
        _animator.SetTrigger("Hit");
        _isAngry = true;
    }

    private void IsGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckRadius, _groundLayer);
    }

    private IEnumerator Idle(float time)
    {
        _animator.SetBool("Idle", true);
        _isIdle = true;
        yield return new WaitForSeconds(time);
        _animator.SetBool("Idle", false);
        _isIdle = false;
        Flip();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.transform.position, _groundCheckRadius);
    }
#endif
}
