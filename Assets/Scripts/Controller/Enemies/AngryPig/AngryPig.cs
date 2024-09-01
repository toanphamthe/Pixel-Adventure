using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryPig : Enemy
{
    enum AngryPigState { Idle, Patrol, Angry }

    [Header("AngryPig Stats")]
    [SerializeField] private float _angrySpeed;
    [SerializeField] private float _idleTime;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isFacingRight;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private AngryPigState _currentState;

    protected override void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        _currentState = AngryPigState.Patrol;
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

    public override void Die()
    {
        if (_currentState == AngryPigState.Angry)
        {
            base.Die();
        }
        else
        {
            Hit();
        }
    }

    /// <summary>
    /// Flip the pig after idle
    /// </summary>
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        transform.Rotate(new Vector3(0, 180f, 0));
    }

    /// <summary>
    /// Handle the pig patroling
    /// </summary>
    private void Patrol()
    {
        if (_isFacingRight && _currentState != AngryPigState.Idle)
        {
            _rigidbody2D.velocity = new Vector2(_moveSpeed, _rigidbody2D.velocity.y);
        }
        else if (!_isFacingRight && _currentState != AngryPigState.Idle)
        {
            _rigidbody2D.velocity = new Vector2(-_moveSpeed, _rigidbody2D.velocity.y);
        }
        if (!_isGrounded && _currentState != AngryPigState.Idle && _currentState != AngryPigState.Angry)
        {
            StartCoroutine(Idle(_idleTime));
        }
        else if (!_isGrounded && _currentState == AngryPigState.Angry)
        {
            Flip();
        }
    }

    /// <summary>
    /// Transite to the angry state when the player hit the pig
    /// </summary>
    private void Hit()
    {
        _moveSpeed = _angrySpeed;
        _animator.SetTrigger("Hit");
        _currentState = AngryPigState.Angry;
    }

    /// <summary>
    /// Check the enemy is grounded or not
    /// </summary>
    private void IsGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckRadius, _groundLayer);
    }


    /// <summary>
    /// Handle the pig idle state
    /// </summary>
    /// <param name="time">Idle time</param>
    /// <returns>Idle coroutine</returns>
    private IEnumerator Idle(float time)
    {
        _animator.SetBool("Idle", true);
        _currentState = AngryPigState.Idle;
        yield return new WaitForSeconds(time);
        _animator.SetBool("Idle", false);
        _currentState = AngryPigState.Patrol;
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
