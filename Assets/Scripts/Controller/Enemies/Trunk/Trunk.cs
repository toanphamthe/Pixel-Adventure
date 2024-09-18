using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : Enemy
{
    enum TrunkState { Idle, Patrol, Attack }

    [SerializeField] private TrunkState _currentState;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isFacingRight;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _idleTime;
    [SerializeField] private Transform _groundCheck;

    protected override void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        _shake = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraShake>();

        _currentState = TrunkState.Patrol;
        _animator.SetBool("Run", true);
    }

    protected override void Update()
    {
        if (_isDead)
        {
            RotateEffect();
        }
        else
        {
            GroundCheck();
            BehaviorHandle();
        }
    }

    private void BehaviorHandle()
    {
        if (_currentState == TrunkState.Patrol && !_isGrounded)
        {
            _currentState = TrunkState.Idle;
            StartCoroutine(Idle(_idleTime));
        }
        else if (_currentState == TrunkState.Patrol && _isGrounded)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (_isFacingRight)
        {
            _rigidbody2D.velocity = new Vector2(_moveSpeed, _rigidbody2D.velocity.y);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(-_moveSpeed, _rigidbody2D.velocity.y);
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
    /// Handle the trunk idle state
    /// </summary>
    /// <param name="time">Idle time</param>
    /// <returns>Idle coroutine</returns>
    private IEnumerator Idle(float time)
    {
        _animator.SetBool("Run", false);
        _currentState = TrunkState.Idle;
        yield return new WaitForSeconds(time);
        _animator.SetBool("Run", true);
        _currentState = TrunkState.Patrol;
        Flip();
    }

    private void GroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckRadius, _groundLayer);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.transform.position, _groundCheckRadius);
    }
#endif
}
