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
    [SerializeField] private float _attackDelayTime;
    [SerializeField] private float _attackCurrentCount;
    [SerializeField] private float _attackCount;

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
    }

    protected override void Update()
    {
        if (_isDead)
        {
            RotateEffect();
        }
        else
        {
            Patrol();
        }
    }

    public override void Die()
    {
        base.Die();
        _rigidbody2D.gravityScale = 1;
    }

    /// <summary>
    /// Moves the object along the X-axis and reverses direction after a cooldown.
    /// </summary>
    private void Patrol()
    {
        _rigidbody2D.velocity = new Vector2(_moveSpeed, _rigidbody2D.velocity.y);
        if (_timeElapsed < _cooldownTimer)
        {
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            _moveSpeed = -_moveSpeed;
            _timeElapsed = 0;
        }
    }

    /// <summary>
    /// Create a bullet from the bullet container position.
    /// </summary>
    public void Shoot()
    {
        Instantiate(_bullet, _bulletContainer.transform.position, Quaternion.identity, _bulletContainer.transform);
    }

    /// <summary>
    /// Control attack animation and reset the attack count.
    /// </summary>
    public void Idle()
    {
        _attackCurrentCount++;
        if (_attackCurrentCount > _attackCount)
        {
            _animator.SetBool("Attack", false);
            _attackCurrentCount = 0;
        }
    }

    /// <summary>
    /// Coroutine to reverse the Bee's movement after a delay.
    /// </summary>
    /// <param name="time">Time to wait before reversing direction.</param>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator Goback(float time)
    {
        _moveSpeed = MathF.Abs(_moveSpeed);

        yield return new WaitForSeconds(time);

        _moveSpeed = -_moveSpeed;
    }

    /// <summary>
    /// Coroutine to repeatedly trigger the attack animation after a specified delay.
    /// </summary>
    /// <param name="time">Delay between each attack.</param>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator AttackCoroutine(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            _animator.SetBool("Attack", true);
        }
    }
}
