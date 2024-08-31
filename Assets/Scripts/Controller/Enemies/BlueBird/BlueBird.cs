using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : Enemy
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private int _targetPoint;
    [SerializeField] private bool _isFacingRight;

    protected override void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {

    }

    protected override void Update()
    {
        if (_isDead)
        {
            RotateEffect();
        }
        else
        {
            Fly();
        }
    }

    public override void Die()
    {
        base.Die();
        _rigidbody2D.gravityScale = 1;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Fly()
    {
        if (Vector2.Distance(transform.position, _points[_targetPoint].position) < 0.1f)
        {
            _targetPoint++;
            if (_targetPoint == _points.Length)
            {
                _targetPoint = 0;
            }
            Flip();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _points[_targetPoint].position, _moveSpeed * Time.deltaTime);
        }
    }

    private void Flip()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }
}
