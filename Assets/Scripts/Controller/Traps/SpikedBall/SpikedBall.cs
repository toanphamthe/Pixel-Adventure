using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpikedBall : MonoBehaviour
{
    [Header("Spiked Ball Stats")]
    [SerializeField] private int _numberOfLinks;

    [SerializeField] private Transform _spikedBall;
    [SerializeField] private GameObject _chainLinkPrefab;
    [SerializeField] private GameObject _chainContainer;
    [SerializeField] private List<GameObject> _chainlinks = new List<GameObject>();

    private void Start()
    {
        CreateChain();
    }

    private void Update()
    {
        Chain();
    }

    /// <summary>
    /// Creates the chain of the spiked ball
    /// </summary>
    private void CreateChain()
    {
        Vector2 direction = (_spikedBall.transform.position - transform.position).normalized;

        float distance = Vector3.Distance(transform.position, _spikedBall.transform.position);

        float segmentLength = distance / (_numberOfLinks + 1);

        for (int i = 0; i < _numberOfLinks; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + direction * segmentLength * (i + 1);

            GameObject newLink = Instantiate(_chainLinkPrefab, spawnPosition, Quaternion.identity, _chainContainer.transform);

            _chainlinks.Add(newLink);

            newLink.transform.position = direction;
        }
    }

    /// <summary>
    /// Aligns the chain links between the spiked ball and the pivot object
    /// </summary>
    private void Chain()
    {
        Vector2 direction = (_spikedBall.transform.position - transform.position).normalized;

        float distance = Vector3.Distance(transform.position, _spikedBall.transform.position);

        float segmentLength = distance / (_numberOfLinks + 1);

        for (int i = 0; i < _numberOfLinks; i++)
        {
            Vector2 position = (Vector2)transform.position + direction * segmentLength * (i + 1);
            _chainlinks[i].transform.position = position;
        }
    }
}
