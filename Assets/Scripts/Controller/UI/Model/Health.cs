using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action HealthChanged;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _minHealth;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => _maxHealth;
    public int MinHealth => _minHealth;

    private void Start()
    {
        Restore();
    }

    /// <summary>
    /// Increase the player health
    /// </summary>
    public void Increment()
    {
        CurrentHealth++;
        CurrentHealth = Mathf.Clamp(CurrentHealth, MinHealth, MaxHealth);
        OnHealthChanged();
    }

    /// <summary>
    /// Decrease the player health
    /// </summary>
    public void Decrement()
    {
        CurrentHealth--;
        CurrentHealth = Mathf.Clamp(CurrentHealth, MinHealth, MaxHealth);
        OnHealthChanged();

        if (CurrentHealth == MinHealth)
        {
            GameManager.Instance.LoseGame();
        }
    }

    /// <summary>
    /// Restore the player health
    /// </summary>
    public void Restore()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged();
    }

    /// <summary>
    /// Call the event when the health is changed
    /// </summary>
    public void OnHealthChanged()
    {
        HealthChanged?.Invoke();
    }
}
