using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _radius;
    [SerializeField] private float _destroyDelayTime;
    [SerializeField] private LayerMask _groundLayer;


    [SerializeField] private Sprite _bulletPiecesSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private CircleCollider2D _circleCollider2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        IsGrounded();
    }


    /// <summary>
    /// Destroy the bullet when it hits the ground
    /// </summary>
    private void IsGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.1f), _radius, _groundLayer);

        if (_isGrounded)
        {
            _spriteRenderer.sprite = _bulletPiecesSprite;
            _rigidbody2D.gravityScale = 0;
            Destroy(gameObject, _destroyDelayTime);
            _circleCollider2D.enabled = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y - 0.1f), _radius);
    }
#endif
}
