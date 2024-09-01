using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _coins;
    [SerializeField] protected bool _isDead;
    [SerializeField] protected float _destroyDelayTime;

    [SerializeField] private float _angleInDegrees;
    [SerializeField] private float _knockBackForce;
    [SerializeField] private float _rotateSpeed;

    [Header("Components")]
    [SerializeField] protected Rigidbody2D _rigidbody2D;
    [SerializeField] protected BoxCollider2D _boxCollider2D;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;

    protected abstract void Awake();

    protected abstract void Start();

    protected abstract void Update();

    /// <summary>
    /// Handle the enemy die when it collide with the player
    /// </summary>
    public virtual void Die()
    {
        _animator.SetTrigger("Die");
        _boxCollider2D.enabled = false;
        _moveSpeed = 0f;
        _isDead = true;
        _rigidbody2D.gravityScale = 3;
        KnockBack();
        Destroy(gameObject, _destroyDelayTime);
    }

    /// <summary>
    /// Applie a rotate effect to the enemy after it die.
    /// </summary>
    protected virtual void RotateEffect()
    {
        float currentZRotation = transform.rotation.eulerAngles.z;
        if (Mathf.Abs(currentZRotation) < 90)
        {
            transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Calculate the knockback force and apply it to the enemy
    /// </summary>
    private void KnockBack()
    {
        // Calculate the vector direction of the knockback force by using the angle in degrees
        float x = Mathf.Cos(_angleInDegrees * Mathf.Deg2Rad);
        float y = Mathf.Sin(_angleInDegrees * Mathf.Deg2Rad);
        Vector2 forceDirection = new Vector2(x, y).normalized;

        _rigidbody2D.AddForce(forceDirection * _knockBackForce, ForceMode2D.Impulse);
    }
}
