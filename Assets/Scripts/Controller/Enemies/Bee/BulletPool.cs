using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private uint _initPoolSize;
    [SerializeField] private PooledBullet _objectToPool;
    [SerializeField] private GameObject _parent;

    private Stack<PooledBullet> _stack;

    private void Start()
    {
        SetupPool();
    }

    /// <summary>
    /// Creates the pool (invoke when the lag is not noticeable)
    /// </summary>
    private void SetupPool()
    {
        _stack = new Stack<PooledBullet>();
        PooledBullet instance = null;
        for (int i = 0; i < _initPoolSize; i++)
        {
            instance = Instantiate(_objectToPool, transform.localPosition, Quaternion.identity, _parent.transform);
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            _stack.Push(instance);
        }
    }

    /// <summary>
    /// Returns the first active GameObject from the pool
    /// </summary>
    /// <returns>Pooled object</returns>
    public PooledBullet GetPooledBullet()
    {
        // if the pool is not large enough, instantiate a new PooledObjects
        if (_stack.Count == 0)
        {
            PooledBullet newInstance = Instantiate(_objectToPool, transform.localPosition, Quaternion.identity, _parent.transform);
            newInstance.Pool = this;
            newInstance.gameObject.transform.position = transform.position;
            return newInstance;
        }
        else
        {
            // otherwise, just grab the next one from the list
            PooledBullet nextInstance = _stack.Pop();
            nextInstance.gameObject.transform.position = transform.position;
            nextInstance.gameObject.SetActive(true);
            return nextInstance;
        }
    }

    /// <summary>
    /// Get the active Gameoject to the pool
    /// </summary>
    /// <param name="pooledObject"></param>
    public void ReturnToPool(PooledBullet pooledObject)
    {
        _stack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}
