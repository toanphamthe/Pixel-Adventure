using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public event Action PointChanged;
    public int CurrentPoint { get; set; }

    /// <summary>
    /// Increase the player point
    /// </summary>
    /// <param name="value"></param>
    public void Increment(int value)
    {
        CurrentPoint += value;
        CurrentPoint = Mathf.Clamp(CurrentPoint, 0, 999);
        OnPointChanged();
    }

    /// <summary>
    /// Call the event when the point is changed
    /// </summary>
    public void OnPointChanged()
    {
        PointChanged?.Invoke();
    }
}
