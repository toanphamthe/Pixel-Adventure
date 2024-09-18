using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float _startPosition;
    private float _length;
    private Camera _camera;
    [SerializeField] private float _parallaxEffect;
    [SerializeField] private float _parallaxSpeed;

    private void Awake()
    {
        _camera = Camera.main;
        _startPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        float distance = Time.deltaTime * _parallaxSpeed * _parallaxEffect;
        transform.position = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);

        if (transform.position.x < _startPosition - _length)
        {
            transform.position = new Vector3(_startPosition, transform.position.y, transform.position.z);
        }
    }
}
