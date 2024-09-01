using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : Enemy
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private int _targetPoint;
    [SerializeField] private bool _isFacingRight;
    [SerializeField] private float _minSpeedFactor;
    [SerializeField] private float _maxSpeedFactor;
    [SerializeField] private float _factor;
    [SerializeField] private GameObject _blueBird;

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
            Destroy(_blueBird, _destroyDelayTime);
        }
        else
        {
            Fly();
        }
    }

    public override void Die()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        base.Die();
    }

    /// <summary>
    /// Handle the movement of the blue bird
    /// </summary>
    private void Fly()
    {
        float distanceToTarget = Vector2.Distance(transform.position, _points[_targetPoint].position);

        if (distanceToTarget < 0.1f)
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
            float speedFactor = Mathf.Clamp(distanceToTarget / _factor, _minSpeedFactor, _maxSpeedFactor);
            float adjustedSpeed = _moveSpeed * speedFactor;
            transform.position = Vector2.MoveTowards(transform.position, _points[_targetPoint].position, adjustedSpeed * Time.deltaTime);
        }
    }

    private void Flip()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }
}
