using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBird : Enemy
{
    enum FatBirdState { Idle, Fall }

    [Header("FatBird Stats")]
    [SerializeField] private int _targetPoint;
    [SerializeField] private float _playerCheckDistance;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _isGroundTime;
    [SerializeField] private bool _isShake;

    [SerializeField] private FatBirdState _currentState;

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private GameObject _fatBird;
    [SerializeField] private GameObject[] _points;
    [SerializeField] private GameObject _raycast;

    protected override void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        _shake = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraShake>();

        _currentState = FatBirdState.Idle;
    }

    protected override void Update()
    {
        if (_isDead)
        {
            RotateEffect();
            Destroy(_fatBird, _destroyDelayTime);
        }
        else
        {
            FatBirdBehaviorHandler();
            CheckPlayer();
        }
    }

    /// <summary>
    /// Handle the fat bird behavior
    /// </summary>
    private void FatBirdBehaviorHandler()
    {
        switch (_currentState)
        {
            case FatBirdState.Idle:
                Idle();
                break;
            case FatBirdState.Fall:
                Fall();
                break;
        }
    }

    /// <summary>
    /// Stop the fat bird when it reach the target point
    /// </summary>
    private void Idle()
    {
        if (Vector2.Distance(transform.position, _points[_targetPoint].transform.position) < 0.1f)
        {
            if (_targetPoint < _points.Length - 1)
            {
                _targetPoint++;
            }
            else
            {
                _targetPoint = 0;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _points[_targetPoint].transform.position, _moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Handle the fat bird fall behavior
    /// </summary>
    private void Fall()
    {
        _animator.SetBool("Fall", true);
        _rigidbody2D.gravityScale = 4;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _groundLayer);
        if (ray.collider != null && _currentState == FatBirdState.Fall)
        {
            _animator.SetTrigger("IsGround");
            if (!_isShake)
            {
                _shake.Shake();
                _isShake = true;
            }
            StartCoroutine(IdleDelay(_isGroundTime));
        }
    }

    /// <summary>
    /// Check if the player is in the chase range
    /// </summary>
    private void CheckPlayer()
    {
        RaycastHit2D ray = Physics2D.Raycast(_raycast.transform.position, Vector2.down, _playerCheckDistance, _playerLayer);
        if (ray.collider != null && _currentState == FatBirdState.Idle)
        {
            _currentState = FatBirdState.Fall;
        }
    }

    /// <summary>
    /// Handle the fat bird idle time
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator IdleDelay(float time)
    {
        yield return new WaitForSeconds(time);
        _currentState = FatBirdState.Idle;
        _rigidbody2D.gravityScale = 0;
        _animator.SetBool("Fall", false);
        _isShake = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_raycast.transform.position, Vector2.down * _playerCheckDistance);
        Gizmos.DrawRay(transform.position, Vector2.down * _groundCheckDistance);
    }
#endif
}
