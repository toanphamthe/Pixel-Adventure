using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private List<GameObject> _healthList;

    private void Start()
    {
        _health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        if (_health != null)
        {
            _health.HealthChanged += OnHealthChanged;
        }
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.HealthChanged -= OnHealthChanged;
        }
    }

    /// <summary>
    /// Update the player health UI
    /// </summary>
    private void UpdateView()
    {
        if (_health == null)
        {
            return;
        }

        for (int i = 0; i < _healthList.Count; i++)
        {
            if (i < _health.CurrentHealth)
            {
                _healthList[i].SetActive(true);
            }
            else
            {
                _healthList[i].gameObject.GetComponent<Animator>().SetTrigger("Hit");
            }
        }
    }

    /// <summary>
    /// Call the event when the health is changed
    /// </summary>
    private void OnHealthChanged()
    {
        UpdateView();
    }
}
