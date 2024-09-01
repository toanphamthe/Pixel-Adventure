using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    [Header("Bee Components")]
    [SerializeField] private float _cooldownTimer;
    [SerializeField] private float _timeElapsed;
    [SerializeField] private float _amplitude;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _bulletContainer;
    [SerializeField] private GameObject _bulletPosition;
    [SerializeField] private GameObject _bee;
    [SerializeField] private float _attackDelayTime;
    [SerializeField] private float _attackCurrentCount;
    [SerializeField] private float _attackCount;
    [SerializeField] private GameObject _leftPoint;
    [SerializeField] private GameObject _rightPoint;
    [SerializeField] private Vector2 _targetPosition;

    protected override void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        StartCoroutine(AttackCoroutine(_attackDelayTime));

        _targetPosition = GetRandomPosition();
    }

    protected override void Update()
    {
        if (_isDead)
        {
            RotateEffect();
            Destroy(_bee, _destroyDelayTime);
        }
        else
        {
            Patrol();
        }
    }

    public override void Die()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        base.Die();
    }

    /// <summary>
    /// Handle the movement of the bee.
    /// </summary>
    private void Patrol()
    {
        if (Vector2.Distance(transform.position, _targetPosition) < 0.1f)
        {
            _targetPosition = GetRandomPosition();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Get random position between two points
    /// </summary>
    /// <returns>Random position</returns>
    private Vector2 GetRandomPosition()
    {
        float x = UnityEngine.Random.Range(_leftPoint.transform.position.x, _rightPoint.transform.position.x);
        float y = UnityEngine.Random.Range(_leftPoint.transform.position.y, _rightPoint.transform.position.y);
        Vector2 targetPosition = new Vector2(x, y);
        return targetPosition;
    }

    /// <summary>
    /// Create a bullet from the bullet container position.
    /// </summary>
    public void Shoot()
    {
        Instantiate(_bullet, _bulletPosition.transform.position, Quaternion.identity, _bulletContainer.transform);
    }

    /// <summary>
    /// Calculate attack count and set the attack animation to false when the attack count is greater than the attack count.
    /// </summary>
    public void AttackHandle()
    {
        _attackCurrentCount++;
        if (_attackCurrentCount > _attackCount)
        {
            _animator.SetBool("Attack", false);
            _attackCurrentCount = 0;
        }
    }

    /// <summary>
    /// Coroutine to repeatedly trigger the attack animation after a specified delay.
    /// </summary>
    /// <param name="time">Delay between each attack.</param>
    /// <returns>Attack coroutine</returns>
    private IEnumerator AttackCoroutine(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            _animator.SetBool("Attack", true);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Vector3 size = new Vector3(
            Mathf.Abs(_rightPoint.transform.position.x - _leftPoint.transform.position.x),
            Mathf.Abs(_rightPoint.transform.position.y - _leftPoint.transform.position.y),
            0);

        
        Vector3 center = new Vector3(
            (_rightPoint.transform.position.x + _leftPoint.transform.position.x) / 2,
            (_rightPoint.transform.position.y + _leftPoint.transform.position.y) / 2,
            0);

        Gizmos.DrawWireCube(center, size);
    }
#endif
}
