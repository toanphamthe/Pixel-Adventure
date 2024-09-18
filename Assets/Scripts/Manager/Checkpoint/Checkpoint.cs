using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, ICheckpoint
{
    [SerializeField] private bool _isActive;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ActiveCheckpoint()
    {
        if (!_isActive)
        {
            _isActive = true;
            _animator.SetBool("Active", true);
            GameManager.Instance.SaveCheckpoint(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActiveCheckpoint();
        }
    }
}
