using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chicken : Enemy
{
    enum ChickenState
    {
        Idle,
        Chase,
    }

    [SerializeField] private ChickenState _currentState;
    [SerializeField] private GameObject _chaseRange;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Player _player;
    [SerializeField] private float _moveDirection;
    [SerializeField] private GameObject _chicken;

    protected override void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _shake = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraShake>();

        _currentState = ChickenState.Idle;
    }

    protected override void Update()
    {
        if (_isDead)
        {
            RotateEffect();
            Destroy(_chicken, _destroyDelayTime);

        }
        else
        {
            ChickenBehaviorHandler();
            CheckPlayer();
        }
    }

    private void ChickenBehaviorHandler()
    {
        switch(_currentState)
        {
            case ChickenState.Idle:
                Idle();
                break;
            case ChickenState.Chase:
                Chase();
                break;
        }
    }

    private void Chase()
    {
        _rigidbody2D.velocity = new Vector2(_moveDirection * _moveSpeed, _rigidbody2D.velocity.y);
        Flip(_player.transform.position.x);
        _animator.SetBool("Run", true);
    }

    private void Idle()
    {
        _animator.SetBool("Run", false);
    }

    /// <summary>
    /// Flip the chameleon to face the player
    /// </summary>
    private void Flip(float moveDirection)
    {
        if (moveDirection < transform.position.x)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            _moveDirection = -1;
        }
        else
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            _moveDirection = 1;
        }
    }

    private void CheckPlayer()
    {
        Collider2D chase = Physics2D.OverlapBox(_chaseRange.transform.position, _chaseRange.transform.localScale, 0, _playerLayer);
        if (chase != null && !_player.GetComponent<PlayerTakeDamage>().IsDead)
        {
            _currentState = ChickenState.Chase;
        }
        else
        {
            _currentState = ChickenState.Idle;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_chaseRange.transform.position, _chaseRange.transform.localScale);
    }
#endif
}
