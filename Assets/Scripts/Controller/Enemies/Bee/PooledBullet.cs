using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledBullet : MonoBehaviour
{
    [SerializeField] private BulletPool _pool;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isRelease;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;

    public BulletPool Pool { get { return _pool; } set { _pool = value; } }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _groundLayer);
        if (hit.collider != null && !_isRelease)
        {
            _isRelease = true;
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = Vector2.zero;
            _animator.SetTrigger("Break");
        }
    }

    public void Release()
    {
        _rigidbody.gravityScale = 1;
        _pool.ReturnToPool(this);
        _isRelease = false;
    }
}
