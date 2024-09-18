using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondPresenter : MonoBehaviour
{
    [SerializeField] private Diamond _diamond;
    [SerializeField] private TextMeshProUGUI _pointText;

    private void Start()
    {
        _diamond = GameObject.FindGameObjectWithTag("Player").GetComponent<Diamond>();
        if (_diamond != null)
        {
            _diamond.PointChanged += OnPointChanged;
        }
    }

    private void OnDestroy()
    {
        if (_diamond != null)
        {
            _diamond.PointChanged -= OnPointChanged;
        }
    }

    private void UpdateView()
    {
        if (_diamond == null)
        {
            return;
        }

        _pointText.text = _diamond.CurrentPoint.ToString();
    }

    private void OnPointChanged()
    {
        UpdateView();
    }
}
