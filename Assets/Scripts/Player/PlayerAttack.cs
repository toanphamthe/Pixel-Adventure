using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAttack
{
    bool IsOnTopOfEnemy { get; }
    void Attack();
}

public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _enemyCheckDistance;

    public bool IsOnTopOfEnemy { get; private set; }

    private void FixedUpdate()
    {
        Attack();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Attack()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, _enemyCheckDistance, _enemyLayer);
        if (ray.collider != null)
        {
            IsOnTopOfEnemy = true;
        }
        else
        {
            IsOnTopOfEnemy = false;
        }
        Debug.DrawRay(transform.position, Vector2.down * _enemyCheckDistance, Color.red);
    }
}
