using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    enum MushroomState { Idle, Patrol }

    [Header("Mushroom Stats")]
    [SerializeField] private bool _isFacingRight;
    [SerializeField] private float _idleTime;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckRadius;

    [SerializeField] private MushroomState _currentState;

    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private Transform _groundCheck;

    protected override void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        _shake = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraShake>();
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
            Patrol();
        }
    }

    /// <summary>
    /// Handle the mushroom patroling
    /// </summary>
    private void Patrol()
    {
        if (_isFacingRight && _currentState != MushroomState.Idle)
        {
            _rigidbody2D.velocity = new Vector2(_moveSpeed, _rigidbody2D.velocity.y);
        }
        else if (!_isFacingRight && _currentState != MushroomState.Idle)
        {
            _rigidbody2D.velocity = new Vector2(-_moveSpeed, _rigidbody2D.velocity.y);
        }
        if (!_isGrounded && _currentState != MushroomState.Idle)
        {
            StartCoroutine(Idle(_idleTime));
        }
    }

    /// <summary>
    /// Flip the mushroom after idle
    /// </summary>
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        transform.Rotate(new Vector3(0, 180f, 0));
    }

    /// <summary>
    /// Handle the mushroom idle state
    /// </summary>
    /// <param name="time">Idle time</param>
    /// <returns>Idle coroutine</returns>
    private IEnumerator Idle(float time)
    {
        _animator.SetBool("Run", false);
        _currentState = MushroomState.Idle;
        yield return new WaitForSeconds(time);
        _animator.SetBool("Run", true);
        _currentState = MushroomState.Patrol;
        Flip();
    }

    /// <summary>
    /// Check the mushroom is grounded or not
    /// </summary>
    private void GroundCheck()
    {
        Collider2D hit = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        if (hit != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.transform.position, _groundCheckRadius);
    }
#endif
}
