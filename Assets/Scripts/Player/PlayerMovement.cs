using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isFacingRight;

    private Rigidbody2D _rigidbody2D;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        float moveDirection = _playerInput.GetMoveDirection();

        if (moveDirection != 0)
        {
            _rigidbody2D.velocity = new Vector2(_moveSpeed * moveDirection, _rigidbody2D.velocity.y);
        }
    }

    public void Flip()
    {
        float moveDirection = _playerInput.GetMoveDirection();

        if (moveDirection > 0.01f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            _isFacingRight = true;
        }
        else if (moveDirection < -0.01f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            _isFacingRight = false;
        }
    }    
}
