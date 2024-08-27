using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    [Header("Bat Stats")]
    [SerializeField] private bool _isChasing;
    [SerializeField] private Vector3 _startPosition;

    //[Header("Bat Components")]

    protected override void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _startPosition = transform.position;
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {

    }
}
