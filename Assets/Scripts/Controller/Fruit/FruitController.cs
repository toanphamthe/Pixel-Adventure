using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour, ICollectable
{
    [SerializeField] private int _fruitPoint;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public int Collect()
    {
        _animator.SetTrigger("Collected");
        return _fruitPoint;
    }

    public void DestroyFruit()
    {
        Destroy(gameObject);
    }
}
