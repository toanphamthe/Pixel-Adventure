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

    public void Increment()
    {
        CurrentHealth++;
        CurrentHealth = Mathf.Clamp(CurrentHealth, MinHealth, MaxHealth);
        OnHealthChanged();
    }

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

    public void Restore()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged();
    }

    public void OnHealthChanged()
    {
        HealthChanged?.Invoke();
    }
}
