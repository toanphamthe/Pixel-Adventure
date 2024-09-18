using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public event Action PointChanged;
    public int CurrentPoint { get; set; }

    public void Increment(int value)
    {
        CurrentPoint += value;
        CurrentPoint = Mathf.Clamp(CurrentPoint, 0, 999);
        OnPointChanged();
    }

    public void OnPointChanged()
    {
        PointChanged?.Invoke();
    }
}
