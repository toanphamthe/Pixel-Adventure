using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour, IPlayerCollectable
{
    [Header("Fruit Stats")]
    [SerializeField] private int _fruitPoint;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Handle the fruit collection
    /// </summary>
    /// <returns>Fruit point</returns>
    public int Collect()
    {
        _animator.SetTrigger("Collected");
        return _fruitPoint;
    }

    /// <summary>
    /// Destroy the fruit
    /// </summary>
    public void DestroyFruit()
    {
        Destroy(gameObject);
    }
}
