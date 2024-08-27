using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bat : Enemy
{
    [Header("Bat Stats")]
    [SerializeField] private bool _isChasing;
    [SerializeField] private float _attackRange;

    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private LayerMask _playerLayer;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GameObject _attackObject;
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerTakeDamage _playerTakeDamge;
    [SerializeField] private GameObject _batRootObject;

    //[Header("Bat Components")]

    protected override void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _agent = GetComponent<NavMeshAgent>();

        _startPosition = transform.position;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    protected override void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _playerTakeDamge = _player.GetComponent<PlayerTakeDamage>();
        }
    }

    protected override void Update()
    {
        if (_isDead)
        {
            RotateEffect();
        }
        else
        {
            ChasePlayer();
        }
    }

    public override void Die()
    {
        base.Die();
        _rigidbody2D.gravityScale = 1;
        Destroy(_batRootObject, _destroyDelayTime);
        _agent.enabled = false;
    }


    /// <summary>
    /// The enemy starts chasing the player
    /// if they enter the attack range and returns to the start position 
    /// when the playernis knocked back or the enemy loses the chase. 
    /// </summary>
    private void ChasePlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(_attackObject.transform.position, _attackRange, _playerLayer);
        if (collider)
        {
            _isChasing = true;
        }

        if (_isChasing)
        {
            _agent.SetDestination(_player.transform.position);
            _animator.SetBool("Idle", false);
        }

        if (_playerTakeDamge.IsKnockBack)
        {
            _agent.SetDestination(_startPosition);
            _isChasing = false;
        }

        if (Vector2.Distance(transform.position, _startPosition) < 0.1f)
        {
            _animator.SetBool("Idle", true);
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
