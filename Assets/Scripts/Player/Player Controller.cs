using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerJump _playerJump;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = GetComponent<PlayerInput>();
        _playerJump = GetComponent<PlayerJump>();
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        
    }
}
