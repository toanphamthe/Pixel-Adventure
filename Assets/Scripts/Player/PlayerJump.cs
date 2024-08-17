using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _checkDistance;
    [SerializeField] private bool _isGrounded;

    private PlayerInput _playerInput;

    public bool IsGrounded 
    { 
        get { return _isGrounded; }
    }

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    
    }

    private void Update()
    {
        GroundCheck();
        Jump();
    }

    public void Jump()
    {
        if (_playerInput.GetJumpKeyDown() && _isGrounded)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
        }
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _checkDistance, _groundLayer);
        if (hit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _checkDistance);
    }
    #endif
}
