using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : Enemy
{
    enum ChameleonState
    {
        Idle,
        Chase,
        Attack
    }
    [SerializeField] private ChameleonState _currentState;
    [SerializeField] private GameObject _chameleon;
    [SerializeField] private GameObject _chaseRange;
    [SerializeField] private GameObject _attackRange;
    [SerializeField] private GameObject _attackTrigger;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Player _player;
    [SerializeField] private float _moveDirection;
    [SerializeField] private bool _isAttack;
    [SerializeField] private float _attackDelayTime;

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
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        _currentState = ChameleonState.Idle;
    }

    protected override void Update()
    {
        if (_isDead)
        {
            RotateEffect();
            Destroy(_chameleon, _destroyDelayTime);
        }
        else
        {
            ChameleonBehaviorHandler();
            PlayerCheck();
        }
    }

    private void AttackTrigger()
    {
        Collider2D attack = Physics2D.OverlapBox(_attackRange.transform.position, _attackRange.transform.localScale, 0, _playerLayer);
        if (attack != null && _isAttack)
        {
            _player.playerStateMachine.TransitionTo(_player.playerStateMachine.takeDamageState);
        }
    }

    /// <summary>
    /// Handle the behavior of the chameleon
    /// </summary>
    private void ChameleonBehaviorHandler()
    {
        switch (_currentState)
        {
            case ChameleonState.Idle:
                Idle();
                break;
            case ChameleonState.Chase:
                Chase();
                break;
            case ChameleonState.Attack:
                Attack();
                break;
        }
    }

    /// <summary>
    /// Stop chase the player
    /// </summary>
    private void Idle()
    {
        _animator.SetBool("Chase", false);
    }

    /// <summary>
    /// Chase the player
    /// </summary>
    private void Chase()
    {
        _animator.SetBool("Chase", true);
        _rigidbody2D.velocity = new Vector2(_moveSpeed * _moveDirection, _rigidbody2D.velocity.y);
        Flip();
    }

    /// <summary>
    /// Attack the player
    /// </summary>
    private void Attack()
    {
        if (!_isAttack)
        {
            _animator.SetBool("Chase", false);
            _animator.SetTrigger("Attack");
            StartCoroutine(AttackDelay(_attackDelayTime));
        }
    }

    /// <summary>
    /// Flip the chameleon to face the player
    /// </summary>
    private void Flip()
    {
        if (_player.gameObject.transform.position.x < transform.position.x)
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

    /// <summary>
    /// Check if the player is in the chase range or attack range and set the current state of the chameleon
    /// </summary>
    private void PlayerCheck()
    {
        Collider2D chase = Physics2D.OverlapBox(_chaseRange.transform.position, _chaseRange.transform.localScale, 0, _playerLayer);
        Collider2D attack = Physics2D.OverlapBox(_attackRange.transform.position, _attackRange.transform.localScale, 0, _playerLayer);
        if (chase != null && attack == null && !_isAttack)
        {
            _currentState = ChameleonState.Chase;
        }
        else if (chase != null && attack != null)
        {
            _currentState = ChameleonState.Attack;
        }
        else
        {
            _currentState = ChameleonState.Idle;
        }
    }

    /// <summary>
    /// The time delay between each attack
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator AttackDelay(float time)
    {
        _isAttack = true;
        yield return new WaitForSeconds(time);
        _isAttack = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_chaseRange.transform.position, _chaseRange.transform.localScale);
        Gizmos.DrawWireCube(_attackRange.transform.position, _attackRange.transform.localScale);
    }
#endif
}
