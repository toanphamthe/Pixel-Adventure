using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bat : Enemy
{
    enum BatState { Idle, Chase }

    [Header("Bat Stats")]
    [SerializeField] private float _attackRange;

    [SerializeField] private BatState _currentState;

    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private LayerMask _playerLayer;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GameObject _attackObject;
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerTakeDamage _playerTakeDamge;
    [SerializeField] private GameObject _batRootObject;

    protected override void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        _shake = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraShake>();
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _playerTakeDamge = _player.GetComponent<PlayerTakeDamage>();
        }

        _currentState = BatState.Idle;
        _startPosition = transform.position;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _moveSpeed;
    }

    protected override void Update()
    {
        if (_isDead)
        {
            RotateEffect();
        }
        else
        {
            BatBehavior();
            IsPlayerOnAttackRange();
        }
    }

    public override void Die()
    {
        base.Die();
        Destroy(_batRootObject, _destroyDelayTime);
        _agent.enabled = false;
    }


    /// <summary>
    /// Handle the enemy behavior
    /// </summary>
    private void BatBehavior()
    {
        if (_currentState == BatState.Chase)
        {
            _agent.SetDestination(_player.transform.position);
            _animator.SetBool("Idle", false);
        }

        if (_playerTakeDamge.IsDead)
        {
            _agent.SetDestination(_startPosition);
            _currentState = BatState.Idle;
        }

        if (Vector2.Distance(transform.position, _startPosition) < 0.1f && _currentState == BatState.Idle)
        {
            _animator.SetBool("Idle", true);
        }

        Flip();
    }

    /// <summary>
    /// Flip the enemy sprite based on the player position
    /// </summary>
    private void Flip()
    {
        if (_player.transform.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    /// <summary>
    /// Check if the player is on the attack range
    /// </summary>
    private void IsPlayerOnAttackRange()
    {
        Collider2D collider = Physics2D.OverlapCircle(_attackObject.transform.position, _attackRange, _playerLayer);
        if (collider)
        {
            _currentState = BatState.Chase;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackObject.transform.position, _attackRange);
    }
#endif
}
