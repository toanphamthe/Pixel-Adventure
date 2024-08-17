using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _moveDirection;

    public float GetMoveDirection()
    {
        _moveDirection = Input.GetAxis("Horizontal");
        return _moveDirection;
    }

    public bool GetJumpKeyDown()
    {
        return Input.GetKeyDown(_jumpKey);
    }
}
