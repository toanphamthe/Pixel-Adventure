using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAreaSetter : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private RectTransform _panelSafeArea;

    private Rect _currentSafeArea;
    private ScreenOrientation _currentOrientation;

    private void Awake()
    {
        _panelSafeArea = GetComponent<RectTransform>();
        _currentOrientation = Screen.orientation;
        _currentSafeArea = Screen.safeArea;
    }

    private void Start()
    {
        _currentOrientation = Screen.orientation;
        _currentSafeArea = Screen.safeArea;

        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        if (_panelSafeArea == null)
            return;

        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= _canvas.pixelRect.width;
        anchorMin.y /= _canvas.pixelRect.height;

        anchorMax.x /= _canvas.pixelRect.width;
        anchorMax.y /= _canvas.pixelRect.height;

        _panelSafeArea.anchorMin = anchorMin;
        _panelSafeArea.anchorMax = anchorMax;
    }

    private void Update()
    {
        if (_currentOrientation != Screen.orientation || _currentSafeArea != Screen.safeArea)
        {
            ApplySafeArea();
        }
    }
}
