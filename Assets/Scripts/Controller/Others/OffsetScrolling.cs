using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class OffsetScrolling : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    private Vector3 startPosition;

    [SerializeField] private float _tileHeight; // Use even values to avoid infinite scrolling errors
    [SerializeField] private float _tileWidth; // Use even values to avoid infinite scrolling errors
    [SerializeField] private float _verticalScrollSpeed;
    [SerializeField] private float _horizontalScrollSpeed;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    void Start()
    {
        startPosition = transform.position;
        _tileHeight = _tilemap.size.y / 2f;
        _tileWidth = _tilemap.size.x / 2f;
    }

    void Update()
    {
        Scrolling();
    }

    private void Scrolling()
    {
        transform.position -= new Vector3(_horizontalScrollSpeed * Time.deltaTime, _verticalScrollSpeed * Time.deltaTime, 0);

        // Scrolling down
        if (transform.position.y < startPosition.y - _tileHeight)
        {
            transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        }
        // Scrolling up
        else if (transform.position.y > startPosition.y + _tileHeight)
        {
            transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        }

        // Scrolling left
        if (transform.position.x < startPosition.x - _tileWidth)
        {
            transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        }
        // Scrolling right
        else if (transform.position.x > startPosition.x + _tileWidth)
        {
            transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        }
    }
}
