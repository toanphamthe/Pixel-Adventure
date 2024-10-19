using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delayTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(PlayAnimation());
    }

    /// <summary>
    /// Unactive the health point
    /// </summary>
    public void DisableHealthUI()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handle the animation of the health point
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayAnimation()
    {
        while(true)
        {
            yield return new WaitForSeconds(_delayTime);
            _animator.SetTrigger("Idle");
        }
    }
}
